using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using WebProject_NetFramework.DbContext.BaseDbModels;

namespace WebProject_NetFramework.DbContext.DbModels
{

    public class TestSchedule : BaseDbModels.BaseDbClass
    {
        public TestSchedule()
        {
            StudentTestSchedules = new HashSet<StudentTestSchedule>();
        }

        public int TotalSeat { get; set; }
        public int ReservedSeat { get; set; }
        public DateTime ExamDate { get; set; }
        public DateTime StartTime { get; set ; }
        public DateTime EndTime { get; set; }
        public long EntranceTestVenueId { get; set; }
        [ForeignKey("EntranceTestVenueId")] public virtual EntranceTestVenue EntranceTestVenue { get; set; }
        public virtual ICollection<StudentTestSchedule> StudentTestSchedules { get; set; }

    }
}