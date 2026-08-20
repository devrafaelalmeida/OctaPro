using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OctaPro.Models;

[Index(nameof(Queue), Name = "jobs_queue_index")]
[Table("jobs")]
public partial class Job
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [MaxLength(255)]
    [Column("queue")]
    public string Queue { get; set; } = null!;

    [Column("payload")]
    public string Payload { get; set; } = null!;

    [Column("attempts")]
    public short Attempts { get; set; }

    [Column("reserved_at")]
    public int? ReservedAt { get; set; }

    [Column("available_at")]
    public int AvailableAt { get; set; }

    [Column("created_at")]
    public int CreatedAt { get; set; }
}
