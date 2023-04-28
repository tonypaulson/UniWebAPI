namespace CWL.VirtualCare.API.Entities
{
    public class ProviderSpecialization
    {
        public int Id { get; set; }

        public int ProviderId { get; set; }

        public int SpecializationId { get; set; }

        public Provider Provider { get; set; }

        public Specialization Specialization { get; set; }
    }
}
