using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctaPro.Models;

[Table("entities_roles_map")]
public partial class EntitiesRolesMap
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("entity_id")]
    public long EntityId { get; set; }

    [Column("role_id")]
    public int RoleId { get; set; }

    [Column("assigned_at", TypeName = "timestamp with time zone")]
    public DateTime AssignedAt { get; set; }

    [Column("assigned_by")]
    public long? AssignedBy { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_at", TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp with time zone")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey(nameof(EntityId))]
    public virtual Entity Entity { get; set; } = null!;

    [ForeignKey(nameof(RoleId))]
    public virtual EntitiesRole Role { get; set; } = null!;
}
