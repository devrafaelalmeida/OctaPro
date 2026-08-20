namespace OctaPro.Models
{
    public class SettlementInstallment : Installment
    {
        public virtual Settlement? Settlement { get; set; }

        public SettlementInstallment()
            : base(SettlementTypeId)
        {
        }

        
    }
}
