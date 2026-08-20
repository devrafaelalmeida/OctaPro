using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctaPro.Models;

[Table("type_installments")]
public class TypeInstallment
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [MaxLength(50)]
    [Column("description")]
    public string Description { get; set; } = null!;

    public virtual ICollection<Installment> Installments { get; set; } = new List<Installment>();
}
