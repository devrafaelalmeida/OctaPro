using Microsoft.EntityFrameworkCore;
using OctaPro.Data;
using OctaPro.DTO;
using OctaPro.DTO.Request;
using OctaPro.DTO.Response;
using OctaPro.Enums;
using OctaPro.Interfaces;
using OctaPro.Models;
using OctaPro.Services.interfaces;

namespace OctaPro.Services
{
    public class SettlementService : ISettlementService
    {
        private readonly AppDbContext _context;
        public SettlementService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SettlementResponse>> GetAllAsync(SettlementFilterRequest filter)
        {
            var query = _context.Settlements
                .Include(s => s.JudicialProcess)
                .Include(s => s.StatusPayment)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.ProcessNumber))
            {
                query = query.Where(s => s.JudicialProcess.ProcessNumber == filter.ProcessNumber);
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                var status = int.Parse(filter.Status);
                query = query.Where(s => s.StatusPaymentId == status);
            }

            return await query
                .Select(s => new SettlementResponse
                {
                    IdPublic = s.IdPublic,
                    ProcessNumber = s.JudicialProcess.ProcessNumber,
                    Payer = s.JudicialProcess.Respondent,
                    Amount = s.Amount,
                    QuantityInstallment = s.QuantityInstallment,
                    CreatedAt = s.CreatedAt,
                    StatusPayment = s.StatusPayment.Description,
                    SettlementInstallments = s.SettlementInstallments
                        .OrderBy(i => i.DueDate)
                        .Select(i => new SettlementInstallmentResponse
                        {
                            IdPublic = i.IdPublic,
                            Document = i.Document,
                            ValueInstallment = i.ValueInstallment,
                            PaidAmount = i.PaidAmount,
                            StatusPayment = i.StatusPaymentId.ToString(),
                            PaymentDate = i.PaymentDate,
                            DueDate = i.DueDate,
                            Competence = i.Competence,
                            Note = i.Note
                        })
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<SettlementResponse?> GetByIdAsync(Guid idPublic)
        {
            var settlement = await _context.Settlements
                .Include(s => s.JudicialProcess)
                .Include(s => s.StatusPayment)
                .Include(s => s.SettlementInstallments)
                .FirstOrDefaultAsync(s => s.IdPublic == idPublic);

            if (settlement == null)
                return null;

            return new SettlementResponse
            {
                IdPublic = settlement.IdPublic,
                ProcessNumber = settlement.JudicialProcess.ProcessNumber,
                Amount = settlement.Amount,
                QuantityInstallment = settlement.QuantityInstallment,
                FirstDayPayment = settlement.FirstDueDate?.Day ?? 0,
                CreatedAt = settlement.CreatedAt,
                StatusPayment = settlement.StatusPayment.Description,
                SettlementInstallments = settlement.SettlementInstallments
                    .OrderBy(i => i.DueDate)
                    .Select(i => new SettlementInstallmentResponse
                    {
                        IdPublic = i.IdPublic,
                        Document = i.Document,
                        ValueInstallment = i.ValueInstallment,
                        PaidAmount = i.PaidAmount,
                        StatusPayment = i.StatusPaymentId.ToString(),
                        PaymentDate = i.PaymentDate,
                        DueDate = i.DueDate,
                        Competence = i.Competence,
                        Note = i.Note
                    })
                    .ToList()
            };
        }

        public async Task CreateAsync(SettlementRequest request, Guid userLoggedUUID)
        {

            var userLogged = await _context.Users.FirstOrDefaultAsync(user => user.IdPublic == userLoggedUUID)
            ?? throw new Exception("Usuário não encontrado");

            var judicialProcess = await _context.JudicialProcesses.FirstOrDefaultAsync(p => p.IdPublic == request.ProcessNumberId)
                ?? throw new Exception("Processo judicial não encontrado");

            var settlement = new Settlement
            {
                IdPublic = Guid.NewGuid(),
                JudicialProcessId = judicialProcess.Id,
                Amount = request.Amount,
                QuantityInstallment = request.QuantityInstallment,
                Note = request.Note,
                UserId = userLogged.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                StatusPaymentEnum = StatusPaymentEnum.Pending,
                FirstDueDate = request.FirstDueDate
            };

            var settlementInstallments = settlement.CreateInstallments();

            _context.Settlements.Add(settlement);
            _context.SettlementInstallments.AddRange(settlementInstallments);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid idPublic)
        {
            var process = await _context.JudicialProcesses
                .FirstOrDefaultAsync(p => p.IdPublic == idPublic);

            if (process == null)
                return false;

            _context.JudicialProcesses.Remove(process);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Guid settlementId, SettlementRequest request)
        {
            var process = await _context.JudicialProcesses
                .FirstOrDefaultAsync(p => p.IdPublic == settlementId);

            if (process == null)
                return false;

            // process.ProcessNumber = request.ProcessNumber;
            // process.InitialDate = request.InitialDate;
            // process.Respondent = request.Respondent;
            // process.Description = request.Description;
            // process.NatureActionId = request.NatureActionId;
            // process.JudicialActionId = request.JudicialActionId;

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
