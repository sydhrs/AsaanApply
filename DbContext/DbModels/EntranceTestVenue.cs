using System;

namespace WebProject_NetFramework.DbContext.DbModels
{
    public class EntranceTestVenue : BaseDbModels.BaseDbClass
    {
        public string VenueName { get; set; }
        public string VenueAddress { get; set; }
        public DateTime? OpeningTime { get; set; }
        public DateTime? ClosingTime { get; set; }
    }
}