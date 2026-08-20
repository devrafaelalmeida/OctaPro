using Microsoft.EntityFrameworkCore;
using OctaPro.Data;
using OctaPro.DTO;
using OctaPro.DTO.Request;
using OctaPro.DTO.Response;
using OctaPro.Enums;
using OctaPro.Models;
using OctaPro.Services.interfaces;

namespace OctaPro.Services
{
    public class JudicialProcessService : IJudicialProcessService
    {
        private readonly AppDbContext _context;
        private readonly int IS_ARCHIVED = 2;
        public JudicialProcessService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JudicialProcessResponse>> GetAllAsync(ProcessFilterRequest filter)
        {
            var query = _context.JudicialProcesses
                .Include(p => p.NatureAction)
                .Include(p => p.JudicialAction)
                .Include(p => p.User)
                .Include(p => p.JudicialProcessEntities)
                    .ThenInclude(jpe => jpe.Entity)
                        .ThenInclude(e => e.EntityIndividual)
                .Include(p => p.JudicialProcessEntities)
                    .ThenInclude(jpe => jpe.Entity)
                        .ThenInclude(e => e.EntityCompany)
                .AsQueryable();


            if (!string.IsNullOrEmpty(filter.ProcessNumber))
            {
                query = query.Where(p => p.ProcessNumber == filter.ProcessNumber);
            }

            if (filter.IdPublicEntity.HasValue)
            {
                query = query.Where(p =>
                    p.JudicialProcessEntities.Any(jpe => jpe.Entity.IdPublic == filter.IdPublicEntity.Value)
                );
            }

            if (!string.IsNullOrEmpty(filter.Status))
            {
                var status = int.Parse(filter.Status) == IS_ARCHIVED ? true : false;
                query = query.Where(p => p.IsArchived == status);
            }

            if (filter.InitialDate.HasValue)
            {
                query = query.Where(p => p.InitialDate == filter.InitialDate);
            }

            return await query
                .Select(p => new JudicialProcessResponse
                {
                    IdPublic = p.IdPublic,
                    ProcessNumber = p.ProcessNumber,
                    InitialDate = p.InitialDate,
                    Respondent = p.Respondent,
                    Description = p.Description,
                    IsArchived = p.IsArchived,
                    CreatedAt = p.CreatedAt,

                    NatureAction = new SelectOptionResponse
                    {
                        Id = p.NatureAction.Id,
                        Text = p.NatureAction.Nature
                    },

                    JudicialAction = new SelectOptionResponse
                    {
                        Id = p.JudicialAction.Id,
                        Text = p.JudicialAction.Action
                    },

                    User = p.User.Id,

                    Entities = p.JudicialProcessEntities
                        .Select(jpe => new EntityResponse
                        {
                            IdPublic = jpe.Entity.IdPublic,
                            EntityType = jpe.Entity.EntityType,

                            EntityIndividual = jpe.Entity.EntityIndividual != null
                                ? new EntityIndividualResponse
                                {
                                    Name = jpe.Entity.EntityIndividual.Name
                                }
                                : null,

                            EntityCompany = jpe.Entity.EntityCompany != null
                                ? new EntityCompanyResponse
                                {
                                    TradeName = jpe.Entity.EntityCompany.TradeName
                                }
                                : null
                        })
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<JudicialProcessResponse?> GetByIdAsync(Guid idPublic)
        {
            var process = await _context.JudicialProcesses
                .Include(p => p.NatureAction)
                .Include(p => p.JudicialAction)
                .Include(p => p.User)
                .Include(p => p.JudicialProcessEntities)
                    .ThenInclude(jpe => jpe.Entity)
                        .ThenInclude(e => e.EntityIndividual)
                .Include(p => p.JudicialProcessEntities)
                    .ThenInclude(jpe => jpe.Entity)
                        .ThenInclude(e => e.EntityCompany)
                .FirstOrDefaultAsync(p => p.IdPublic == idPublic);

            if (process == null)
                return null;

            return new JudicialProcessResponse
            {
                IdPublic = process.IdPublic,
                ProcessNumber = process.ProcessNumber,
                InitialDate = process.InitialDate,
                Respondent = process.Respondent,
                Description = process.Description,
                IsArchived = process.IsArchived,
                CreatedAt = process.CreatedAt,

                NatureAction = new SelectOptionResponse
                {
                    Id = process.NatureAction.Id,
                    Text = process.NatureAction.Nature
                },

                JudicialAction = new SelectOptionResponse
                {
                    Id = process.JudicialAction.Id,
                    Text = process.JudicialAction.Action
                },

                User = process.User.Id,
                
                Entities = process.JudicialProcessEntities
                    .Select(jpe => new EntityResponse
                    {
                        IdPublic = jpe.Entity.IdPublic,
                        EntityType = jpe.Entity.EntityType,

                        EntityIndividual = jpe.Entity.EntityIndividual != null
                            ? new EntityIndividualResponse
                            {
                                Name = jpe.Entity.EntityIndividual.Name,
                                CPF = jpe.Entity.EntityIndividual.Cpf,
                                Email = jpe.Entity.EntityIndividual.Email,
                                Mobile = jpe.Entity.EntityIndividual.Mobile,
                            }
                            : null,

                        EntityCompany = jpe.Entity.EntityCompany != null
                            ? new EntityCompanyResponse
                            {
                                TradeName = jpe.Entity.EntityCompany.TradeName,
                                CNPJ = jpe.Entity.EntityCompany.Cnpj,
                                Email = jpe.Entity.EntityCompany.CorporateEmail,
                                Mobile = jpe.Entity.EntityCompany.CorporateMobile,

                            }
                            : null
                    })
                    .ToList()
            };
        }

        public async Task CreateAsync(JudicialProcessRequest request, Guid userLoggedUUID)
        {

            var userLogged = await _context.Users.FirstOrDefaultAsync(user => user.IdPublic == userLoggedUUID)
            ?? throw new Exception("Usuário não encontrado");

            var entities = await _context.Entities
                .Where(e => request.EntityIds.Contains(e.IdPublic))
                .ToListAsync();

            if (entities.Count != request.EntityIds.Count)
                throw new Exception("Uma ou mais entidades não foram encontradas");

            var process = new JudicialProcess
            {
                ProcessNumber = request.ProcessNumber,
                InitialDate = request.InitialDate,
                Respondent = request.Respondent,
                Description = request.Description,
                NatureActionId = request.NatureActionId,
                JudicialActionId = request.JudicialActionId,
                UserId = userLogged.Id,
                EmpresaId = userLogged.EmpresaId,
                IdPublic = Guid.NewGuid()
            };

            foreach (var entity in entities)
            {
                process.JudicialProcessEntities.Add(new JudicialProcessEntity
                {
                    Entity = entity
                });
            }

            _context.JudicialProcesses.Add(process);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ArchiveAsync(Guid idPublic)
        {
            var process = await _context.JudicialProcesses
                .FirstOrDefaultAsync(p => p.IdPublic == idPublic);

            if (process == null)
                return false;

            var settlementIds = await _context.Settlements
                .Where(s => s.JudicialProcessId == process.Id)
                .Select(s => s.Id)
                .ToListAsync();

            var hasUnpaidSettlementInstallments = await _context.SettlementInstallments
                .AnyAsync(i =>
                    i.ReferenceId.HasValue &&
                    settlementIds.Contains(i.ReferenceId.Value) &&
                    i.StatusPaymentId != StatusPaymentEnum.Paid);

            if (hasUnpaidSettlementInstallments)
                throw new InvalidOperationException("Não é possível arquivar o processo. Existem parcelas de acordo pendentes ou atrasadas.");

            var legalFeeIds = await _context.LegalFees
                .Where(lf => lf.JudicialProcessId == process.Id)
                .Select(lf => lf.Id)
                .ToListAsync();

            var hasUnpaidLegalFeeInstallments = await _context.LegalFeeInstallments
                .AnyAsync(i =>
                    i.ReferenceId.HasValue &&
                    legalFeeIds.Contains(i.ReferenceId.Value) &&
                    i.StatusPaymentId != StatusPaymentEnum.Paid);

            if (hasUnpaidLegalFeeInstallments)
                throw new InvalidOperationException("Não é possível arquivar o processo. Existem parcelas de honorários pendentes ou atrasadas.");

            process.IsArchived = true;
            process.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid idPublic)
        {
            var process = await _context.JudicialProcesses
                .FirstOrDefaultAsync(p => p.IdPublic == idPublic);

            if (process == null)
                return false;

            _context.JudicialProcesses.Remove(process);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SelectOptionResponse>> GetAllNatureAsync()
        {
            var natures = await _context.NatureActions
                .Select(n => new SelectOptionResponse
                {
                    Id = n.Id,
                    Text = n.Nature
                })
                .ToListAsync();

            return natures;
        }

        public async Task<IEnumerable<SelectOptionResponse>> GetActionsAsync(int natureId)
        {
            var actions = await _context.JudicialActions
                .Where(a => a.NatureActionId == natureId)
                .Select(n => new SelectOptionResponse
                {
                    Id = n.Id,
                    Text = n.Action
                })
                .ToListAsync();

            return actions;
        }

        public Task<IEnumerable<SelectOptionResponse>> searchProcessAsync(string searchTerm)
        {
            var processes = JudicialProcess.forSelect(_context, searchTerm);
            return Task.FromResult(processes.AsEnumerable());
        }
    }
}
