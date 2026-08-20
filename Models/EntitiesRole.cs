using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OctaPro.Models;

[Index(nameof(Name), IsUnique = true, Name = "entities_roles_name_key")]
[Table("entities_roles")]
public partial class EntitiesRole
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(50)]
    [Column("name")]
    public string Name { get; set; } = null!;

    [MaxLength(255)]
    [Column("description")]
    public string? Description { get; set; }

    [Column("created_at", TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp with time zone")]
    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<EntitiesRolesMap> EntitiesRolesMaps { get; set; } = new List<EntitiesRolesMap>();
}
