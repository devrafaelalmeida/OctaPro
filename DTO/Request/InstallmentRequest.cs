namespace OctaPro.DTO.Request
{
    public class InstallmentRequest
    {
        public decimal ValueInstallment { get; set; }

        public DateOnly DueDate { get; set; }

        public Guid ReferenceId { get; set; }

        public int TypeId { get; set; }

    }
}
