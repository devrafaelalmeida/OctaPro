using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctaPro.Models;

[Table("nature_actions")]
public partial class NatureAction
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(50)]
    [Column("nature")]
    public string Nature { get; set; } = null!;

    [Column("created_at", TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp with time zone")]
    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<JudicialProcess> JudicialProcesses { get; set; } = new List<JudicialProcess>();

    public virtual ICollection<JudicialAction> JudicialAction { get; set; } = new List<JudicialAction>();
}
