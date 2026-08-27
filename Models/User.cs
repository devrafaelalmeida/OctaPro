using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace OctaPro.Models;

[Index(nameof(Email), IsUnique = true, Name = "users_email_unique")]
[Index(nameof(IdPublic), IsUnique = true, Name = "users_id_public_key")]
[Table("users")]
public partial class User : IdentityUser<long>
{
    [MaxLength(2048)]
    [Column("profile_photo_path")]
    public string? ProfilePhotoPath { get; set; }

    [Column("created_at", TypeName = "timestamp(0) without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp(0) without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("id_public")]
    public Guid IdPublic { get; set; }

    [Column("corporation_id")]
    public long CorporationId { get; set; }

    [MaxLength(11)]
    [Column("cpf")]
    public string? CPF { get; set; }

    [Column("birth_date")]
    public DateOnly? BirthDate { get; set; }

    [MaxLength(8)]
    [Column("cep")]
    public string? CEP { get; set; }

    [MaxLength(2)]
    [Column("uf")]
    public string? UF { get; set; }

    [MaxLength(255)]
    [Column("city")]
    public string? City { get; set; }

    [MaxLength(255)]
    [Column("address")]
    public string? Address { get; set; }

    [MaxLength(20)]
    [Column("number_house")]
    public string? NumberHouse { get; set; }

    [MaxLength(200)]
    [Column("complement")]
    public string? Complement { get; set; }

    [MaxLength(255)]
    [Column("neithborhood")]
    public string? Neithborhood { get; set; }

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual ICollection<JudicialProcessUser> JudicialProcessUsers { get; set; } = new List<JudicialProcessUser>();

    public virtual ICollection<LegalFee> LegalFees { get; set; } = new List<LegalFee>();

    public virtual ICollection<Settlement> Settlements { get; set; } = new List<Settlement>();

    public User()
    {
        IdPublic = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

}
