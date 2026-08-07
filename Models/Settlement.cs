using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using OctaPro.Enums;

namespace OctaPro.Models;

public partial class Settlement
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public int QuantityInstallment { get; set; }

    public long JudicialProcessId { get; set; }

    public int StatusPaymentId { get; set; }

    [NotMapped]
    public StatusPaymentEnum StatusPaymentEnum
    {
        get => (StatusPaymentEnum)StatusPaymentId;
        set => StatusPaymentId = (int)value;
    }

    [NotMapped]
    public DateOnly? FirstDueDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? Note { get; set; }

    public Guid IdPublic { get; set; }

    public long UserId { get; set; }

    public virtual JudicialProcess JudicialProcess { get; set; } = null!;

    public virtual StatusPayment StatusPayment { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual ICollection<SettlementInstallment> SettlementInstallments { get; set; } = new List<SettlementInstallment>();

        public List<SettlementInstallment> CreateInstallments()
        {
            if (QuantityInstallment <= 0)
                throw new InvalidOperationException("Quantidade de parcelas inválida.");
                
            if (FirstDueDate is null)
                throw new InvalidOperationException("Primeira data de vencimento não informado.");


            var installments = new List<SettlementInstallment>();

            decimal installmentValue = Math.Round(Amount / QuantityInstallment, 2);

            for (int i = 0; i < QuantityInstallment; i++)
            {
                var dueDate = FirstDueDate.Value.AddMonths(i + 1);

                installments.Add(new SettlementInstallment
                {
                    Settlement = this,
                    IdPublic = Guid.NewGuid(),
                    Document = $"{(i + 1):00000}/{QuantityInstallment}",
                    ValueInstallment = installmentValue,
                    DueDate = dueDate,
                    Competence = dueDate.ToString("MM/yyyy"),
                    StatusPaymentId = StatusPaymentEnum.Pending,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            return installments;
        }
}
