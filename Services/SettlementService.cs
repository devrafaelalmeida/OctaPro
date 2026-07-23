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
    public class SettlementService : ISettlementService, IInstallmentService<Settlement, SettlementInstallment>
    {
        private readonly AppDbContext _context;
        public SettlementService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SettlementResponse>> GetAllAsync(SettlementFilterRequest filter)
        {
            throw new NotImplementedException();
        }

        public async Task<SettlementResponse?> GetByIdAsync(Guid idPublic)
        {
                      throw new NotImplementedException();


        }

        public async Task CreateAsync(SettlementRequest request, Guid userLoggedUUID)
        {

            var userLogged = await _context.Users.FirstOrDefaultAsync(user => user.IdPublic == userLoggedUUID)
            ?? throw new Exception("Usuário não encontrado");

            Console.WriteLine($"Request: {request.Amount}, {request.ProcessNumberId}, {request.QuantityInstallment}, {request.Note}");

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
                StatusPaymentEnum = StatusPaymentEnum.Pending
            };

            var settlementInstallments = CreateInstallments(settlement);

            _context.Settlements.Add(settlement);
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

        public IEnumerable<SettlementInstallment> CreateInstallments(Settlement settlement)
        {
            var installments = new List<SettlementInstallment>();
            var quantityInstallment = settlement.QuantityInstallment;
            decimal installmentValue = Math.Round(settlement.Amount / settlement.QuantityInstallment, 2);

            for (int i = 0; i < quantityInstallment; i++)
            {
                var installment = new SettlementInstallment
                {
                    IdPublic = Guid.NewGuid(),
                    Document = $"{i.ToString().PadLeft(5, '0')}/{quantityInstallment}",
                    ValueInstallment = installmentValue,
                    StatusPaymentId = StatusPaymentEnum.Pending,
                    DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(i)),
                    Competence = DateTime.UtcNow.AddMonths(i).ToString("MM/yyyy"),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                installments.Add(installment);
            }

            



            throw new NotImplementedException();
        }
    }
}
