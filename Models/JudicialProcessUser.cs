using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctaPro.Models;

[Table("judicial_process_user")]
public partial class JudicialProcessUser
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("judicial_process_id")]
    public long JudicialProcessId { get; set; }

    [Column("user_id")]
    public long UserId { get; set; }

    [MaxLength(50)]
    [Column("access_level")]
    public string? AccessLevel { get; set; }

    [Column("created_at", TypeName = "timestamp with time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp with time zone")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey(nameof(JudicialProcessId))]
    public virtual JudicialProcess JudicialProcess { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;
}
