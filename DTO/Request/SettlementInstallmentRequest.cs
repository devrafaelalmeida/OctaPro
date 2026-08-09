namespace OctaPro.DTO.Request
{
    public class SettlementInstallmentRequest
    {
        public decimal ValueInstallment { get; set; }

        public DateOnly DueDate { get; set; }
    }
}
