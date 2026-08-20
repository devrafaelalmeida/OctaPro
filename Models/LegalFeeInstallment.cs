namespace OctaPro.Models;

public class LegalFeeInstallment : Installment
{
    public virtual LegalFee? LegalFee { get; set; }

    public LegalFeeInstallment()
        : base(LegalFeeTypeId)
    {
    }
}
