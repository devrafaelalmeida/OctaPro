using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OctaPro.DTO.Response
{
    public class EntitySelectResponse
    {
        public Guid IdPublic { get; set; }
        public string Text { get; set; } = null!;
    }
}