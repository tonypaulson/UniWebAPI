using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniWeb.API.DTO
{
    public class CurrencyUnitDto
    {
        public int Id { get; set; }
        public string Currency { get; set; }
        public string Country { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
    }
}
