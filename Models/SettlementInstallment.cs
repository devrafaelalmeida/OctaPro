using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using OctaPro.Enums;

namespace OctaPro.Models
{
    [Table("settlement_installments")]
    public class SettlementInstallment
    {
        public int Id { get; set; }

        public string Document { get; set; } = string.Empty;
        
        public decimal? ValueInstallment { get; set; }

        public StatusPaymentEnum StatusPaymentId { get; set; }

        public DateOnly? PaymentDate { get; set; }

        public DateOnly? DueDate { get; set; }

        public string Competence { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string? Note { get; set; }

        public Guid IdPublic { get; set; }

        // public virtual StatusPayment StatusPayment { get; set; } = null!;

     

    }
}