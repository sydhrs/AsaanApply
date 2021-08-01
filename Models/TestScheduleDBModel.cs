using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using WebProject_NetFramework.DbContext.DbModels;

namespace WebProject_NetFramework.Models
{
    public class TestScheduleDBModel
    {
        public TestSchedule TestSchedule { get; set; }
        public EntranceTestVenue EntranceTestVenues { get; set; }
    }
}