using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctaPro.Models;

[Table("cache")]
public partial class Cache
{
    [Key]
    [MaxLength(255)]
    [Column("key")]
    public string Key { get; set; } = null!;

    [Column("value")]
    public string Value { get; set; } = null!;

    [Column("expiration")]
    public int Expiration { get; set; }
}
