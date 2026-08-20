using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using OctaPro.Enums;

namespace OctaPro.Models;

[Table("installments")]
public abstract class Installment
{
    public const int SettlementTypeId = 1;
    public const int LegalFeeTypeId = 2;

    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Column("reference_id")]
    public long? ReferenceId { get; set; }

    [Column("type_id")]
    public int TypeId { get; protected set; }

    [Column("Document")]
    public string Document { get; set; } = string.Empty;

    [Precision(10, 2)]
    [Column("ValueInstallment")]
    public decimal? ValueInstallment { get; set; }

    [Precision(10, 2)]
    [Column("LateFine")]
    public decimal? LateFine { get; set; }

    [Precision(10, 2)]
    [Column("AdjustedTotal")]
    public decimal? AdjustedTotal { get; set; }

    [Precision(10, 2)]
    [Column("paid_amount")]
    public decimal? PaidAmount { get; set; }

    [Column("StatusPaymentId")]
    public StatusPaymentEnum StatusPaymentId { get; set; }

    [Column("PaymentDate")]
    public DateOnly? PaymentDate { get; set; }

    [Column("DueDate")]
    public DateOnly? DueDate { get; set; }

    [Column("Competence")]
    public string Competence { get; set; } = null!;

    [Column("CreatedAt", TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("UpdatedAt", TypeName = "timestamp with time zone")]
    public DateTime UpdatedAt { get; set; }

    [Column("Note")]
    public string? Note { get; set; }

    [Column("IdPublic")]
    public Guid IdPublic { get; set; }

    [ForeignKey(nameof(TypeId))]
    public virtual TypeInstallment? TypeInstallment { get; set; }

    protected Installment(int typeId)
    {
        TypeId = typeId;
    }

    public void CalculateLateFine()
    {
        if (ValueInstallment is null)
        {
            LateFine = null;
            AdjustedTotal = null;
            return;
        }

        LateFine = Math.Round(ValueInstallment.Value * 0.5m, 2);
        AdjustedTotal = ValueInstallment.Value + LateFine;
    }

    public void Reverse()
    {
        if (StatusPaymentId == StatusPaymentEnum.Reverted)
            throw new InvalidOperationException("Parcela já está estornada.");

        StatusPaymentId = StatusPaymentEnum.Reverted;
        UpdatedAt = DateTime.UtcNow;
    }
}
