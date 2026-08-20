using Microsoft.EntityFrameworkCore;
using OctaPro.Data;
using OctaPro.DTO.Request;
using OctaPro.DTO.Response;
using OctaPro.Enums;
using OctaPro.Models;
using OctaPro.Services.interfaces;

namespace OctaPro.Services
{
    public class InstallmentService : IInstallmentService
    {
        private readonly AppDbContext _context;

        public InstallmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ReverseInstallmentResult> ReverseInstallmentsAsync(int typeId, Guid referencePublicId, List<Guid> installmentPublicIds)
        {
                long referenceId = typeId switch
    {
                    Installment.SettlementTypeId =>
                        (await _context.Settlements
                            .FirstOrDefaultAsync(s => s.IdPublic == referencePublicId))?.Id
                        ?? throw new Exception("Acordo não encontrado."),

                    Installment.LegalFeeTypeId =>
                        (await _context.LegalFees
                            .FirstOrDefaultAsync(l => l.IdPublic == referencePublicId))?.Id
                        ?? throw new Exception("Honorário não encontrado."),

                    _ => throw new ArgumentException("Tipo inválido.")
                };
            var requestedIds = installmentPublicIds.Distinct().ToList();
            
            var installments = await _context.Installments
                .Where(i =>
                    requestedIds.Contains(i.IdPublic) &&
                    i.ReferenceId == referenceId &&
                    i.StatusPaymentId != StatusPaymentEnum.Reverted)
                .ToListAsync();

            foreach (var installment in installments)
            {
                installment.Reverse();
            }

            await _context.SaveChangesAsync();

            var reversedIds = installments
                .Select(i => i.IdPublic)
                .ToList();

            return new ReverseInstallmentResult
            {
                ReversedIds = reversedIds,
                NotFoundIds = requestedIds.Except(reversedIds).ToList()
            };
        }
    }
}
