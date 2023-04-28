using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UniWeb.API.Entities
{
    public class AdminToken
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public Admin Admin { get; set; }
        public int AdminId { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime ExpiryTime { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime UpdatedDate { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime CreatedDate { get; set; }
    }
}
