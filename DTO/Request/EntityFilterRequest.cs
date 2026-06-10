using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OctaPro.DTO.Request
{
    public class EntityFilterRequest
    {
        public Guid? IdPublicEntity { get; set; }
      
        public string? Status { get; set; }

        public string? CpfCnpj { get; set; }

        public string? Name { get; set; }

    }
}