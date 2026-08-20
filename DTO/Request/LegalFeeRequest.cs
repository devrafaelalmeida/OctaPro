namespace OctaPro.DTO.Request
{
    public class LegalFeeRequest
    {
        public Guid ProcessNumberId { get; set; }

        public string? ProcessNumber { get; set; }

        public decimal Amount { get; set; }

        public int QuantityInstallment { get; set; }

        public DateOnly FirstDueDate { get; set; }

        public string? Note { get; set; }
    }
}
