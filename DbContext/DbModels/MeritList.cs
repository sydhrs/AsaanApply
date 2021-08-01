using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProject_NetFramework.DbContext.DbModels
{
    public class MeritList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long? DegreePursuingTypeId { get; set; }
        [ForeignKey("DegreePursuingTypeId")] public virtual DegreePursuingType DegreePursuingType { get; set; }
        public long? StudentId { get; set; }
        [ForeignKey("StudentId")] public virtual Student Student { get; set; }
        public long? TestGradeId { get; set; }
        [ForeignKey("TestGradeId")] public virtual TestGrade TestGrade { get; set; }
        public decimal Aggregate { get; set; }
    }
}