using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProject_NetFramework.DbContext.DbModels
{
    public class ApplicationUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Username { get; set; } 
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // Staff,Student
        public byte[] ProfilePic { get; set; } //Storing Picture as Bytes... 
        public long? PersonId { get; set; }
    }
}