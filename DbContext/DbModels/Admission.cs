using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebProject_NetFramework.DbContext.BaseDbModels;

namespace WebProject_NetFramework.DbContext.DbModels
{
    public class Admission : BaseDbClass
    {
        public Admission()
        {
            StudentAcademics = new HashSet<StudentAcademic>();
            StudentTestSchedules = new HashSet<StudentTestSchedule>();
        }

        public long StudentTypeId { get; set; }
        public long DegreePursuingTypeId { get; set; }
        public bool IsMorningShift { get; set; } = true;
        [ForeignKey("StudentTypeId")] public virtual StudentType StudentType { get; set; }
        [ForeignKey("DegreePursuingTypeId")] public virtual DegreePursuingType DegreePursuingType { get; set; }
        public long StudentId { get; set; }
        public virtual Student Student { get; set; }
        public virtual ICollection<StudentAcademic> StudentAcademics { get; set; }
        public long? EntityStatusTypeId { get; set; }
        [ForeignKey("EntityStatusTypeId")] public virtual EntityStatusType EntityStatusType { get; set; }
        public string StatusReason { get; set; }
        public long? TestGradeId { get; set; }
        public virtual ICollection<StudentTestSchedule> StudentTestSchedules { get; set; }
    }
}