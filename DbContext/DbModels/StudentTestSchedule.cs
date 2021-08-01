using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebProject_NetFramework.DbContext.DbModels
{
    public class StudentTestSchedule : BaseDbModels.BaseDbClass
    {
        public StudentTestSchedule()
        {
        }
        public long? StudentId { get; set; }
        [ForeignKey("StudentId")] public virtual Student Student { get; set; }

        public long? AdmissionId { get; set; }
        [ForeignKey("AdmissionId")] public virtual Admission Admission { get; set; }

        public long? TestScheduleId { get; set; }
        [NotMapped]
        [ForeignKey("TestScheduleId")] public virtual TestSchedule TestSchedule { get; set; }
       
        public virtual ICollection<TestSchedule> StudentTestSchedules { get; set; }  // i guess i did wrong here 


    }
}