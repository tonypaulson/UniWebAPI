namespace UniWeb.API.DTO
{
    public class TenantInputDto
    {
        public string TenantName { get; set; }

        public string TenantSubDomain { get; set; }

        public string TenantPrimaryContact { get; set; }

        public string Logo { get; set; }

        public int CurrencyUnitId { get; set; }

        public int CalenderViewId { get; set; }

        public int WeekStartId { get; set; }
    }
}