using Microsoft.EntityFrameworkCore;
using OctaPro.Data;
using OctaPro.DTO.Request;
using OctaPro.DTO.Response;
using OctaPro.Enums;
using OctaPro.Models;
using OctaPro.Services.interfaces;

namespace OctaPro.Services
{
    public class LegalFeeService : ILegalFeeService
    {
        private readonly AppDbContext _context;

        public LegalFeeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LegalFeeResponse>> GetAllAsync(SettlementFilterRequest filter)
        {
            var query = _context.LegalFees
                .Include(lf => lf.JudicialProcess)
                .Include(lf => lf.StatusPayment)
                .Include(lf => lf.LegalFeeEntities)
                    .ThenInclude(lfe => lfe.Entity)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.ProcessNumber))
            {
                query = query.Where(lf => lf.JudicialProcess.ProcessNumber == filter.ProcessNumber);
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                var status = int.Parse(filter.Status);
                query = query.Where(lf => lf.StatusPaymentId == status);
            }

            var legalFees = await query.ToListAsync();
            var legalFeeIds = legalFees.Select(lf => lf.Id).ToList();

            var installmentsByLegalFeeId = await _context.LegalFeeInstallments
                .Where(i => i.ReferenceId.HasValue && legalFeeIds.Contains(i.ReferenceId.Value) && i.StatusPaymentId != StatusPaymentEnum.Reverted)
                .OrderBy(i => i.DueDate)
                .Select(i => new
                {
                    LegalFeeId = i.ReferenceId!.Value,
                    Installment = ToInstallmentResponse(i)
                })
                .ToListAsync();

            var installmentsLookup = installmentsByLegalFeeId
                .GroupBy(i => i.LegalFeeId)
                .ToDictionary(g => g.Key, g => g.Select(i => i.Installment).ToList());

            return legalFees
                .Select(lf => ToLegalFeeResponse(lf, installmentsLookup.GetValueOrDefault(lf.Id, new List<InstallmentResponse>())))
                .ToList();
        }

        public async Task<LegalFeeResponse?> GetByIdAsync(Guid idPublic)
        {
            var legalFee = await _context.LegalFees
                .Include(lf => lf.JudicialProcess)
                .Include(lf => lf.StatusPayment)
                .Include(lf => lf.LegalFeeEntities)
                    .ThenInclude(lfe => lfe.Entity)
                .FirstOrDefaultAsync(lf => lf.IdPublic == idPublic);

            if (legalFee == null)
                return null;

            var installments = await _context.LegalFeeInstallments
                .Where(i => i.ReferenceId == legalFee.Id && i.StatusPaymentId != StatusPaymentEnum.Reverted)
                .OrderBy(i => i.DueDate)
                .Select(i => ToInstallmentResponse(i))
                .ToListAsync();

            return ToLegalFeeResponse(legalFee, installments);
        }

        public async Task CreateAsync(LegalFeeRequest request, Guid userLoggedUUID)
        {
            var userLogged = await _context.Users.FirstOrDefaultAsync(user => user.IdPublic == userLoggedUUID)
                ?? throw new Exception("Usuário não encontrado");

            var judicialProcess = await FindJudicialProcessAsync(request)
                ?? throw new Exception("Processo judicial não encontrado");

            if (judicialProcess.IsArchived)
                throw new InvalidOperationException("Não é possível lançar honorários em um processo encerrado.");

            var legalFee = new LegalFee
            {
                IdPublic = Guid.NewGuid(),
                JudicialProcessId = judicialProcess.Id,
                Amount = request.Amount,
                QuantityInstallment = request.QuantityInstallment,
                Note = request.Note,
                UserId = userLogged.Id,
                CorporationId = userLogged.CorporationId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                StatusPaymentEnum = StatusPaymentEnum.Pending,
                FirstDueDate = request.FirstDueDate,
                LegalFeeEntities = judicialProcess.JudicialProcessEntities.Select(c => new LegalFeeEntity
                {
                    EntityId = c.EntityId
                }).ToList()
            };

            _context.LegalFees.Add(legalFee);
            await _context.SaveChangesAsync();

            var legalFeeInstallments = legalFee.CreateInstallments();
            _context.LegalFeeInstallments.AddRange(legalFeeInstallments);

            await _context.SaveChangesAsync();
        }

        public async Task<InstallmentResponse> AddInstallmentAsync(Guid legalFeeId, InstallmentRequest request)
        {
            var legalFee = await _context.LegalFees
                .FirstOrDefaultAsync(lf => lf.IdPublic == legalFeeId)
                ?? throw new Exception("Honorário não encontrado");

            var installment = legalFee.AddInstallment(request.ValueInstallment, request.DueDate);

            _context.LegalFeeInstallments.Add(installment);

            await _context.SaveChangesAsync();

            return ToInstallmentResponse(installment);
        }

        public async Task<bool> DeleteAsync(Guid idPublic)
        {
            var legalFee = await _context.LegalFees
                .FirstOrDefaultAsync(lf => lf.IdPublic == idPublic);

            if (legalFee == null)
                return false;

            _context.LegalFees.Remove(legalFee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Guid legalFeeId, LegalFeeRequest request)
        {
            var legalFee = await _context.LegalFees
                .FirstOrDefaultAsync(lf => lf.IdPublic == legalFeeId);

            if (legalFee == null)
                return false;

            legalFee.Amount = request.Amount;
            legalFee.QuantityInstallment = request.QuantityInstallment;
            legalFee.Note = request.Note;
            legalFee.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<JudicialProcess?> FindJudicialProcessAsync(LegalFeeRequest request)
        {
            var query = _context.JudicialProcesses
                .Include(p => p.JudicialProcessEntities)
                .AsQueryable();

            if (request.ProcessNumberId != Guid.Empty)
                return await query.FirstOrDefaultAsync(p => p.IdPublic == request.ProcessNumberId);

            if (!string.IsNullOrWhiteSpace(request.ProcessNumber))
                return await query.FirstOrDefaultAsync(p => p.ProcessNumber.Contains(request.ProcessNumber));

            return null;
        }

        private static LegalFeeResponse ToLegalFeeResponse(LegalFee legalFee, List<InstallmentResponse> installments)
        {
            return new LegalFeeResponse
            {
                IdPublic = legalFee.IdPublic,
                UserId = legalFee.UserId,
                Amount = legalFee.Amount,
                QuantityInstallment = legalFee.QuantityInstallment,
                JudicialProcessId = legalFee.JudicialProcessId,
                ProcessNumber = legalFee.JudicialProcess.ProcessNumber,
                Payer = legalFee.JudicialProcess.Respondent,
                StatusPaymentId = legalFee.StatusPaymentId,
                StatusPayment = legalFee.StatusPayment.Description,
                Note = legalFee.Note,
                Entities = legalFee.LegalFeeEntities
                    .Select(lfe => new EntityResponse
                    {
                        IdPublic = lfe.Entity.IdPublic,
                        EntityType = lfe.Entity.EntityType
                    })
                    .ToList(),
                LegalFeeInstallments = installments,
                CreatedAt = legalFee.CreatedAt,
                UpdatedAt = legalFee.UpdatedAt
            };
        }

        private static InstallmentResponse ToInstallmentResponse(Installment installment)
        {
            return new InstallmentResponse
            {
                IdPublic = installment.IdPublic,
                Document = installment.Document,
                ValueInstallment = installment.ValueInstallment,
                LateFine = installment.LateFine,
                AdjustedTotal = installment.AdjustedTotal,
                PaidAmount = installment.PaidAmount,
                StatusPayment = installment.StatusPaymentId.ToString(),
                PaymentDate = installment.PaymentDate,
                DueDate = installment.DueDate,
                Competence = installment.Competence,
                Note = installment.Note
            };
        }
    }
}
