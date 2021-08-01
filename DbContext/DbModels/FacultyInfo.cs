using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProject_NetFramework.DbContext.DbModels
{
    public class FacultyInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Name { get; set; }
        public string PhoneNo { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string FacebookName { get; set; }
        public string TwitterName { get; set; }
        public string SkypeName { get; set; }
        public string LinkedInName { get; set; }
        public string PicName { get; set; }
        public long? StaffId { get; set; }
        [ForeignKey("StaffId")] public virtual Staff Staff { get; set; }
    }
}