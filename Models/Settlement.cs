using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using OctaPro.Enums;

namespace OctaPro.Models;

[Index(nameof(IdPublic), IsUnique = true, Name = "settlement_id_public_key")]
[Table("settlement")]
public partial class Settlement
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Precision(10, 2)]
    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("quantity_installment")]
    public int QuantityInstallment { get; set; }

    [Column("judicial_process_id")]
    public long JudicialProcessId { get; set; }

    [Column("status_payment_id")]
    public int StatusPaymentId { get; set; }

    [NotMapped]
    public StatusPaymentEnum StatusPaymentEnum
    {
        get => (StatusPaymentEnum)StatusPaymentId;
        set => StatusPaymentId = (int)value;
    }

    [NotMapped]
    public DateOnly? FirstDueDate { get; set; }

    [Column("created_at", TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp with time zone")]
    public DateTime UpdatedAt { get; set; }

    [MaxLength(255)]
    [Column("note")]
    public string? Note { get; set; }

    [Column("id_public")]
    public Guid IdPublic { get; set; }

    [Column("user_id")]
    public long UserId { get; set; }

    [Column("corporation_id")]
    public long CorporationId { get; set; }

    [ForeignKey(nameof(JudicialProcessId))]
    public virtual JudicialProcess JudicialProcess { get; set; } = null!;

    [ForeignKey(nameof(StatusPaymentId))]
    public virtual StatusPayment StatusPayment { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
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
        
        if (installmentValue <= 0)
            throw new InvalidOperationException("Valor da parcela inválido.");

        for (int i = 0; i < QuantityInstallment; i++)
        {
            var dueDate = FirstDueDate.Value.AddMonths(i + 1);

            var installment = new SettlementInstallment
            {
                ReferenceId = Id,
                IdPublic = Guid.NewGuid(),
                Document = $"{(i + 1):00000}/{QuantityInstallment}",
                ValueInstallment = installmentValue,
                DueDate = dueDate,
                Competence = dueDate.ToString("MM/yyyy"),
                StatusPaymentId = StatusPaymentEnum.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            installment.CalculateLateFine();
            installments.Add(installment);
        }

        return installments;
    }

    public SettlementInstallment AddInstallment(decimal valueInstallment, DateOnly dueDate)
    {
        if (valueInstallment <= 0)
            throw new InvalidOperationException("Valor da parcela inválido.");

        var installmentNumber = QuantityInstallment + 1;
        var now = DateTime.UtcNow;

        var installment = new SettlementInstallment
        {
            ReferenceId = Id,
            IdPublic = Guid.NewGuid(),
            Document = $"{installmentNumber:00000}/1",
            ValueInstallment = valueInstallment,
            DueDate = dueDate,
            Competence = dueDate.ToString("MM/yyyy"),
            StatusPaymentId = StatusPaymentEnum.Pending,
            CreatedAt = now,
            UpdatedAt = now
        };

        installment.CalculateLateFine();
        QuantityInstallment = installmentNumber;

        return installment;
    }
}
