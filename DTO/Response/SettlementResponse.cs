namespace OctaPro.DTO.Response
{
    public class SettlementResponse
    {
        public Guid IdPublic { get; set; }

        public string ProcessNumber { get; set; } = null!;
        public string Payer { get; set; } = null!;

        public decimal Amount { get; set; }

        public int QuantityInstallment { get; set; }

        public int FirstDayPayment { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public string StatusPayment { get; set; } = null!;

        public List<SettlementInstallmentResponse> SettlementInstallments { get; set; } = new();
    }
}
