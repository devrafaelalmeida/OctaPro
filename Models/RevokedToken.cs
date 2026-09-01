using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctaPro.Models;

[Table("revoked_tokens")]
public class RevokedToken
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [MaxLength(64)]
    [Column("token_hash")]
    public string TokenHash { get; set; } = string.Empty;

    [Column("expires_at", TypeName = "timestamp with time zone")]
    public DateTime ExpiresAt { get; set; }

    [Column("revoked_at", TypeName = "timestamp with time zone")]
    public DateTime RevokedAt { get; set; }

    public RevokedToken()
    {
        RevokedAt = DateTime.UtcNow;
    }
}
