namespace WebProject_NetFramework.DbContext.DbModels
{
    public class PersonContact : BaseDbModels.BaseDbClass
    {

        public string GuardianName { get; set; }
        public string GuardianAddress { get; set; }
        public string GuardianContact { get; set; }
        public string EmergencyName { get; set; }
        public string EmergencyContact { get; set; }

    }
}