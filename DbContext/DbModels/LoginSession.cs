using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProject_NetFramework.DbContext.DbModels
{
    public class LoginSession
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string SessionToken { get; set; }

        public string RequestToken { get; set; }

        public long AppUserId { get; set; }

        public string Agent { get; set; }

        public string Browser { get; set; }

        public string OS { get; set; }

        public string Device { get; set; }

        public string IP { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpiryDate { get; set; } = DateTime.UtcNow.AddMinutes(30);

        public bool IsActive { get; set; }
    }
}