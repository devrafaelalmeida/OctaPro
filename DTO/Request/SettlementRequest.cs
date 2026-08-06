namespace OctaPro.DTO
{
    public class SettlementRequest
    {
        public Guid ProcessNumberId { get; set; }

        public decimal Amount { get; set; }

        public int QuantityInstallment { get; set; }

        public DateOnly FirstDueDate { get; set; }

        public string Note { get; set; } = string.Empty;



    }
}