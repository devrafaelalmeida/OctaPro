using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using OctaPro.Data;
using OctaPro.DTO.Response;

namespace OctaPro.Models;

[Index(nameof(IdPublic), IsUnique = true, Name = "judicial_processes_id_public_key")]
[Index(nameof(ProcessNumber), IsUnique = true, Name = "unique_process_number")]
[Table("judicial_processes")]
public partial class JudicialProcess
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [MaxLength(20)]
    [Column("process_number")]
    public string ProcessNumber { get; set; } = null!;

    [Column("initial_date")]
    public DateOnly InitialDate { get; set; }

    [MaxLength(255)]
    [Column("respondent")]
    public string Respondent { get; set; } = null!;

    [MaxLength(255)]
    [Column("description")]
    public string? Description { get; set; }

    [Column("nature_action_id")]
    public int NatureActionId { get; set; }

    [Column("judicial_action_id")]
    public int JudicialActionId { get; set; }

    [Column("is_archived")]
    public bool IsArchived { get; set; }

    [Column("created_at", TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp with time zone")]
    public DateTime UpdatedAt { get; set; }

    [Column("id_public")]
    public Guid IdPublic { get; set; }

    [Column("user_id")]
    public long UserId { get; set; }

    [Column("corporation_id")]
    public long CorporationId { get; set; }

    public ICollection<JudicialProcessEntity> JudicialProcessEntities { get; set; } = new List<JudicialProcessEntity>();
    public ICollection<LegalFee> LegalFees { get; set; } = new List<LegalFee>();

    [ForeignKey(nameof(NatureActionId))]
    public NatureAction NatureAction { get; set; } = null!;

    [ForeignKey(nameof(JudicialActionId))]
    public JudicialAction JudicialAction { get; set; } = null!;

    public ICollection<Settlement> Settlements { get; set; } = new List<Settlement>();

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
    
    public JudicialProcess()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static List<SelectOptionResponse> forSelect(AppDbContext _context, string searchTerm)
    {
         var processes = _context.JudicialProcesses
                .Where(p => p.ProcessNumber.Contains(searchTerm))
                .Select(p => new SelectOptionResponse
                {
                    IdPublic = p.IdPublic,
                    Text = p.ProcessNumber
                })
                .ToList();
        return processes;
    }
}
