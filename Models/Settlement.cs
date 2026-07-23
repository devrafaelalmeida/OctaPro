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


    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? Note { get; set; }

    public Guid IdPublic { get; set; }

    public long UserId { get; set; }

    public virtual JudicialProcess JudicialProcess { get; set; } = null!;

    public virtual StatusPayment StatusPayment { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
