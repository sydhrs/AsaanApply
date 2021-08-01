using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.DynamicData;

namespace WebProject_NetFramework.DbContext.DbModels
{
    [TableName("Students")]
    public class Student : Person
    {
        public Student()
        {
            Admissions = new HashSet<Admission>();
            StudentTestSchedules = new HashSet<StudentTestSchedule>();
        }

        public DateTime EnrollmentDate { get; set; }

        public long? SectionId { get; set; }
        [ForeignKey("SectionId")] public virtual Section Section { get; set; }

        public virtual ICollection<Admission> Admissions { get; set; }
        
        public virtual ICollection<StudentTestSchedule> StudentTestSchedules { get; set; }

    }
}