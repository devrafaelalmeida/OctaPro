namespace OctaPro.DTO.Response
{
    public class ReverseInstallmentResult
    {
        public List<Guid> ReversedIds { get; set; } = new();

        public List<Guid> NotFoundIds { get; set; } = new();
    }
}
