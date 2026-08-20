using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctaPro.Models;

[Table("migrations")]
public partial class Migration
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [MaxLength(255)]
    [Column("migration")]
    public string Migration1 { get; set; } = null!;

    [Column("batch")]
    public int Batch { get; set; }
}
