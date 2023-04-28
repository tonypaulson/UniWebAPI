using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using UniWeb.API.Entities;

namespace UniWeb.API.Entities
{
    public class UserToken
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime ExpiryTime { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime UpdatedDate { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime CreatedDate { get; set; }
    }
}
