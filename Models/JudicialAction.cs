using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctaPro.Models;

[Table("judicials_actions")]
public partial class JudicialAction
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(150)]
    [Column("judicial_action")]
    public string Action { get; set; } = null!;

    [Column("nature_action_id")]
    public int NatureActionId { get; set; }

    [Column("created_at", TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp with time zone")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey(nameof(NatureActionId))]
    public virtual NatureAction NatureAction { get; set; } = null!;
}
