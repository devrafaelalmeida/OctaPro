using System.ComponentModel.DataAnnotations;

namespace OctaPro.DTO.Request
{
    public class ReverseInstallmentRequest
    {
        [Required(ErrorMessage = "É necessário informar ao menos uma parcela.")]
        [MinLength(1, ErrorMessage = "É necessário informar ao menos uma parcela.")]
        public List<Guid> Ids { get; set; } = new();

        
        [Required(ErrorMessage = "É necessário informar o ID da Referência.")]
        public Guid ReferenceId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Tipo de Parcela é obrigatório.")]
        public int TypeId { get; set; }
    }
}
