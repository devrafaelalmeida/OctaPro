namespace OctaPro.DTO.Response
{
    public class LegalFeeResponse
    {
        public Guid IdPublic { get; set; }

        public long UserId { get; set; }

        public decimal Amount { get; set; }

        public int QuantityInstallment { get; set; }

        public long JudicialProcessId { get; set; }

        public string ProcessNumber { get; set; } = null!;

        public string Payer { get; set; } = null!;

        public int StatusPaymentId { get; set; }

        public string StatusPayment { get; set; } = null!;

        public string? Note { get; set; }

        public List<EntityResponse> Entities { get; set; } = new();

        public List<InstallmentResponse> LegalFeeInstallments { get; set; } = new();

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
