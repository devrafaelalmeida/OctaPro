using System.ComponentModel.DataAnnotations.Schema;

namespace OctaPro.Models;

[Table("legal_fee_entity")]
public partial class LegalFeeEntity
{

    [Column("legal_fee_id")]
    public long LegalFeeId { get; set; }

    [ForeignKey(nameof(LegalFeeId))]
    public LegalFee LegalFee { get; set; } = null!;

    [Column("entity_id")]
    public long EntityId { get; set; }

    [ForeignKey(nameof(EntityId))]
    public Entity Entity { get; set; } = null!;

    public LegalFeeEntity()
    {
        
    }
}
