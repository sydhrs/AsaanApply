using System.Collections.Generic;

namespace WebProject_NetFramework.DbContext.DbModels
{
    public class Section : BaseDbModels.BaseDbClass
    {
        public Section()
        {
            Students = new HashSet<Student>();
        }

        public string SectionName { get; set; }

        public int Capacity { get; set; }

        public virtual ICollection<Student> Students { get; set; }
        public bool IsMorningShift { get; set; }
    }
}