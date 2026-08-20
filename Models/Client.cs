using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctaPro.Models;

[Table("clients")]
public partial class Client
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("created_at", TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp with time zone")]
    public DateTime UpdatedAt { get; set; }

    [Column("lawyer_id")]
    public long LawyerId { get; set; }

    [Column("entity_id")]
    public long EntityId { get; set; }

    public virtual Entity Entity { get; set; } = null!;

    [ForeignKey(nameof(LawyerId))]
    public virtual User Lawyer { get; set; } = null!;
}
