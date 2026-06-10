using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using OctaPro.Data;
using OctaPro.DTO;
using OctaPro.DTO.Request;
using OctaPro.DTO.Response;
using OctaPro.Models;
using OctaPro.Services.interfaces;
using OctaPro.Utils;

namespace OctaPro.Services
{
    public class EntityService : IEntityService
    {
        private readonly AppDbContext _context;

        public EntityService(AppDbContext context)
        {
            _context = context;
        }

        // public async Task<IEnumerable<EntityResponse>> GetEntitiesAsync()
        // {
        //     return await _context.Entities
        //         .Include(e => e.EntityIndividual)
        //         .Include(e => e.EntityCompany)
        //         .Select(e => new EntityResponse
        //         {
        //             IdPublic = e.IdPublic,
        //             EntityType = e.EntityType,
        //             CreatedAt = e.CreatedAt,
        //             UpdatedAt = e.UpdatedAt,
        //             // PF
        //             EntityIndividual = e.EntityIndividual == null ? null : new EntityIndividualResponse
        //             {
        //                 Name = e.EntityIndividual.Name,
        //                 CPF = e.EntityIndividual.Cpf,
        //                 RG = e.EntityIndividual.Rg,
        //                 Email = e.EntityIndividual.Email,
        //                 Mobile = e.EntityIndividual.Mobile,
        //                 Phone = e.EntityIndividual.Phone,
        //                 BirthDate = e.EntityIndividual.BirthDate,
        //                 Address = e.EntityIndividual.Address,
        //                 Cep = e.EntityIndividual.Cep,
        //                 HouseNumber = e.EntityIndividual.HouseNumber,
        //                 Complement = e.EntityIndividual.Complement,
        //                 City = e.EntityIndividual.City,
        //                 District = e.EntityIndividual.District,
        //                 Uf = e.EntityIndividual.Uf
        //             },
        //             // PJ
        //             EntityCompany = e.EntityCompany == null ? null : new EntityCompanyResponse
        //             {
        //                 TradeName = e.EntityCompany.TradeName,
        //                 CNPJ = e.EntityCompany.Cnpj,
        //                 CorporateName = e.EntityCompany.CorporateName,
        //                 Email = e.EntityCompany.CorporateEmail,
        //                 Mobile = e.EntityCompany.CorporateMobile,
        //                 Phone = e.EntityCompany.CorporatePhone,
        //                 Address = e.EntityCompany.Address,
        //                 Cep = e.EntityCompany.Cep,
        //                 HouseNumber = e.EntityCompany.HouseNumber,
        //                 Complement = e.EntityCompany.Complement,
        //                 City = e.EntityCompany.City,
        //                 District = e.EntityCompany.District,
        //                 Uf = e.EntityCompany.Uf
        //             }
                 
        //         })
        //         .ToListAsync();
        // }

        public async Task<IEnumerable<EntityResponse>> GetEntitiesAsync(EntityFilterRequest filter)
        {

            var query = _context.Entities
                .Include(e => e.EntityIndividual)
                .Include(e => e.EntityCompany)
                .AsQueryable();

            if (filter.IdPublicEntity.HasValue)
                query = query.Where(e => e.IdPublic == filter.IdPublicEntity.Value);

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(e => e.EntityIndividual != null && e.EntityIndividual.Name.ToUpper().Contains(filter.Name.ToUpper()) ||
                                         e.EntityCompany != null && e.EntityCompany.CorporateName.ToUpper().Contains(filter.Name.ToUpper()));

            if (!string.IsNullOrEmpty(filter.Status))
                query = query.Where(e => e.StatusId == int.Parse(filter.Status));
            
            if (filter.CpfCnpj != null)
                query = query.Where(e =>
                    e.EntityIndividual != null && e.EntityIndividual.Cpf == filter.CpfCnpj ||
                    e.EntityCompany != null && e.EntityCompany.Cnpj == filter.CpfCnpj);

             return await query
                        .Select(e => new EntityResponse
                        {
                            IdPublic = e.IdPublic,
                            EntityType = e.EntityType,
                            CreatedAt = e.CreatedAt,
                            UpdatedAt = e.UpdatedAt,
                            EntityIndividual = e.EntityIndividual == null ? null : new EntityIndividualResponse
                                    {
                                        Name = e.EntityIndividual.Name,
                                        CPF = e.EntityIndividual.Cpf,
                                        RG = e.EntityIndividual.Rg,
                                        Email = e.EntityIndividual.Email,
                                        Mobile = e.EntityIndividual.Mobile,
                                        Phone = e.EntityIndividual.Phone,
                                        BirthDate = e.EntityIndividual.BirthDate,
                                        Address = e.EntityIndividual.Address,
                                        Cep = e.EntityIndividual.Cep,
                                        HouseNumber = e.EntityIndividual.HouseNumber,
                                        Complement = e.EntityIndividual.Complement,
                                        City = e.EntityIndividual.City,
                                        District = e.EntityIndividual.District,
                                        Uf = e.EntityIndividual.Uf
                                    },
                                    // PJ
                                    EntityCompany = e.EntityCompany == null ? null : new EntityCompanyResponse
                                    {
                                        TradeName = e.EntityCompany.TradeName,
                                        CNPJ = e.EntityCompany.Cnpj,
                                        CorporateName = e.EntityCompany.CorporateName,
                                        Email = e.EntityCompany.CorporateEmail,
                                        Mobile = e.EntityCompany.CorporateMobile,
                                        Phone = e.EntityCompany.CorporatePhone,
                                        Address = e.EntityCompany.Address,
                                        Cep = e.EntityCompany.Cep,
                                        HouseNumber = e.EntityCompany.HouseNumber,
                                        Complement = e.EntityCompany.Complement,
                                        City = e.EntityCompany.City,
                                        District = e.EntityCompany.District,
                                        Uf = e.EntityCompany.Uf
                                    }
                        })
                        .ToListAsync();
        }

        public async Task<EntityResponse?> GetByIdAsync(Guid idPublic)
        {
            return await _context.Entities
                .Include(e => e.EntityIndividual)
                .Include(e => e.EntityCompany)
                .Where(e => e.IdPublic == idPublic)
                .Select(e => new EntityResponse
                {
                    IdPublic = e.IdPublic,
                    EntityType = e.EntityType,
                    // PF
                    EntityIndividual = e.EntityIndividual == null ? null : new EntityIndividualResponse
                    {
                        Name = e.EntityIndividual.Name,
                        CPF = e.EntityIndividual.Cpf,
                        RG = e.EntityIndividual.Rg,
                        Email = e.EntityIndividual.Email,
                        Mobile = e.EntityIndividual.Mobile,
                        Phone = e.EntityIndividual.Phone,
                        BirthDate = e.EntityIndividual.BirthDate,
                        Address = e.EntityIndividual.Address,
                        Cep = e.EntityIndividual.Cep,
                        HouseNumber = e.EntityIndividual.HouseNumber,
                        Complement = e.EntityIndividual.Complement,
                        City = e.EntityIndividual.City,
                        District = e.EntityIndividual.District,
                        Uf = e.EntityIndividual.Uf
                    },
                    // PJ
                    EntityCompany = e.EntityCompany == null ? null : new EntityCompanyResponse
                    {
                        TradeName = e.EntityCompany.TradeName,
                        CNPJ = e.EntityCompany.Cnpj,
                        CorporateName = e.EntityCompany.CorporateName,
                        Email = e.EntityCompany.CorporateEmail,
                        Mobile = e.EntityCompany.CorporateMobile,
                        Phone = e.EntityCompany.CorporatePhone,
                        Address = e.EntityCompany.Address,
                        Cep = e.EntityCompany.Cep,
                        HouseNumber = e.EntityCompany.HouseNumber,
                        Complement = e.EntityCompany.Complement,
                        City = e.EntityCompany.City,
                        District = e.EntityCompany.District,
                        Uf = e.EntityCompany.Uf
                    }

                })
                .FirstOrDefaultAsync();
        }

        public async Task CreateEntityIndividualAsync(EntityIndividualRequest request)
        {
            var entity = new Entity
            {
                IdPublic = Guid.NewGuid(),
                EntityType = "PF",
                StatusId = 1
            };

            var entityIndividual = new EntityIndividual
            {
                Entity = entity,
                Name = request.Name,
                Cpf = request.Cpf,
                Rg = request.Rg,
                Email = request.Email,
                Mobile = request.Mobile,
                Phone = request.Phone,
                BirthDate = request.BirthDate,
                Address = request.Address,
                Cep = request.Cep,
                HouseNumber = request.HouseNumber,
                Complement = request.Complement,
                City = request.City,
                District = request.District,
                Uf = request.Uf
            };

            _context.EntitiesIndividuals.Add(entityIndividual);
            await _context.SaveChangesAsync();
        }

        public async Task CreateEntityCompanyAsync(EntityCompanyRequest request)
        {
            var entity = new Entity
            {
                IdPublic = Guid.NewGuid(),
                EntityType = "PJ",
                StatusId = 1
            };

            var entityCompany = new EntityCompany
            {
                Entity = entity,
                Cnpj = request.Cnpj,
                CorporateName = request.CorporateName,
                TradeName = request.TradeName,
                CorporateEmail = request.CorporateEmail,
                CorporateMobile = request.CorporateMobile,
                CorporatePhone = request.CorporatePhone,
                Address = request.Address,
                Cep = request.Cep,
                HouseNumber = request.HouseNumber,
                Complement = request.Complement,
                City = request.City,
                District = request.District,
                Uf = request.Uf?.Trim().ToUpper()
            };

            _context.EntitiesCompanies.Add(entityCompany);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateEntityIndividualAsync(Guid entityId, EntityIndividualRequest request)
        {
            var entityIndividual = await _context.EntitiesIndividuals
                .Include(e => e.Entity)
                .FirstOrDefaultAsync(e => e.Entity.IdPublic == entityId);

            if (entityIndividual == null)
                return false;

            entityIndividual.Name = request.Name;
            entityIndividual.Cpf = request.Cpf;
            entityIndividual.Rg = request.Rg;
            entityIndividual.Email = request.Email;
            entityIndividual.Mobile = request.Mobile;
            entityIndividual.Phone = request.Phone;
            entityIndividual.BirthDate = request.BirthDate;
            entityIndividual.Address = request.Address;
            entityIndividual.Cep = request.Cep;
            entityIndividual.HouseNumber = request.HouseNumber;
            entityIndividual.Complement = request.Complement;
            entityIndividual.City = request.City;
            entityIndividual.District = request.District;
            entityIndividual.Uf = request.Uf?.Trim().ToUpper();

            entityIndividual.Entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateEntityCompanyAsync(Guid entityId, EntityCompanyRequest request)
        {
            var entityCompany = await _context.EntitiesCompanies
                .Include(e => e.Entity)
                .FirstOrDefaultAsync(e => e.Entity.IdPublic == entityId);

            if (entityCompany == null || entityCompany.Entity.EntityType != "PJ")
                return false;

            entityCompany.Cnpj = request.Cnpj;
            entityCompany.CorporateName = request.CorporateName;
            entityCompany.TradeName = request.TradeName;
            entityCompany.CorporateEmail = request.CorporateEmail;
            entityCompany.CorporateMobile = request.CorporateMobile;
            entityCompany.CorporatePhone = request.CorporatePhone;
            entityCompany.Address = request.Address;
            entityCompany.Cep = request.Cep;
            entityCompany.HouseNumber = request.HouseNumber;
            entityCompany.Complement = request.Complement;
            entityCompany.City = request.City;
            entityCompany.District = request.District;
            entityCompany.Uf = request.Uf?.Trim().ToUpper();

            entityCompany.Entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEntityAsync(Guid entityId)
        {
            var entity = await _context.Entities
                .Include(e => e.EntityIndividual)
                .Include(e => e.EntityCompany)
                .FirstOrDefaultAsync(e => e.IdPublic == entityId);

            if (entity == null)
                return false;

            _context.Entities.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<EntitySelectResponse>> SearchClientsAsync(string query)
        {
            query = query.ToLower();

            return await _context.Entities
                .Where(e =>
                    (e.EntityIndividual != null &&
                    e.EntityIndividual.Name.ToLower().Contains(query))
                    ||
                    (e.EntityCompany != null &&
                    e.EntityCompany.CorporateName.ToLower().Contains(query))
                )
                .Select(e => new EntitySelectResponse
                {
                    IdPublic = e.IdPublic,
                    Text = e.EntityIndividual != null
                            ? e.EntityIndividual.Name
                            : e.EntityCompany!.CorporateName
                })
                .ToListAsync();
        }






    }
}
