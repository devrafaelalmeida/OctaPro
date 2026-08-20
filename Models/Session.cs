using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OctaPro.Models;

[Index(nameof(LastActivity), Name = "sessions_last_activity_index")]
[Index(nameof(UserId), Name = "sessions_user_id_index")]
[Table("sessions")]
public partial class Session
{
    [Key]
    [MaxLength(255)]
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("user_id")]
    public long? UserId { get; set; }

    [MaxLength(45)]
    [Column("ip_address")]
    public string? IpAddress { get; set; }

    [Column("user_agent")]
    public string? UserAgent { get; set; }

    [Column("payload")]
    public string Payload { get; set; } = null!;

    [Column("last_activity")]
    public int LastActivity { get; set; }
}
