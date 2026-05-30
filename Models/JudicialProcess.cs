using System;
using System.Collections.Generic;
using OctaPro.Data;
using OctaPro.DTO.Response;

namespace OctaPro.Models;

public partial class JudicialProcess
{
    public long Id { get; set; }

    public string ProcessNumber { get; set; } = null!;

    public DateOnly InitialDate { get; set; }

    public string Respondent { get; set; } = null!;

    public string? Description { get; set; }

    public int NatureActionId { get; set; }

    public int JudicialActionId { get; set; }

    public bool IsArchived { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid IdPublic { get; set; }

    public long UserId { get; set; }

    public ICollection<JudicialProcessEntity> JudicialProcessEntities { get; set; } = new List<JudicialProcessEntity>();
    public ICollection<LegalFee> LegalFees { get; set; } = new List<LegalFee>();

    public NatureAction NatureAction { get; set; } = null!;
    public JudicialAction JudicialAction { get; set; } = null!;

    public ICollection<Settlement> Settlements { get; set; } = new List<Settlement>();

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
