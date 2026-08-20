namespace OctaPro.DTO.Response;

public class InstallmentResponse
{
    public Guid IdPublic { get; set; }
    public string Document { get; set; } = string.Empty;
    public decimal? ValueInstallment { get; set; }
    public decimal? LateFine { get; set; }
    public decimal? AdjustedTotal { get; set; }
    public decimal? PaidAmount { get; set; }
    public string StatusPayment { get; set; } = string.Empty;
    public DateOnly? PaymentDate { get; set; }
    public DateOnly? DueDate { get; set; }
    public string Competence { get; set; } = string.Empty;
    public string? Note { get; set; }
}
