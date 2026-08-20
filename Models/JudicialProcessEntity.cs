using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctaPro.Models;

[Table("judicial_process_entity")]
public partial class JudicialProcessEntity
{
    [Column("judicial_process_id")]
    public long JudicialProcessId { get; set; }

    [ForeignKey(nameof(JudicialProcessId))]
    public JudicialProcess JudicialProcess { get; set; } = null!;

    [Column("entity_id")]
    public long EntityId { get; set; }

    [ForeignKey(nameof(EntityId))]
    public Entity Entity { get; set; } = null!;

    public JudicialProcessEntity()
    {
        
    }
}
