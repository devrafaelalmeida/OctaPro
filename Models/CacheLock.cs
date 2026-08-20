using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctaPro.Models;

[Table("cache_locks")]
public partial class CacheLock
{
    [Key]
    [MaxLength(255)]
    [Column("key")]
    public string Key { get; set; } = null!;

    [MaxLength(255)]
    [Column("owner")]
    public string Owner { get; set; } = null!;

    [Column("expiration")]
    public int Expiration { get; set; }
}
