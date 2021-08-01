using System.Collections.Generic;
using WebProject_NetFramework.DbContext.DbModels;

namespace WebProject_NetFramework.Models
{
    public class MeritListDataModel
    {
        public List<MeritListSelectModel> MorningMeritList { get; set; }
        public List<MeritListSelectModel> EveningMeritList { get; set; }
        public bool HasNameInMorning { get; set; }
        public bool HasNameInEveningShift { get; set; }
    }

    public class MeritListSelectModel
    {
        public long? StudentId { get; set; }
        public long? AdmissionId { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string Score { get; set; }
        public string TestGrade { get; set; }
        public decimal Aggregate { get; set; }
        public string DegreePursuingType { get; set; }
        public string StudentType { get; set; }
    }

    public class StudentProfileDataModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public Person Person { get; set; }
        public Admission Admission { get; set; }
        public Student Student { get; set; }
        public MeritList MeritList { get; set; }
    }
}