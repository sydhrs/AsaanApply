using System.ComponentModel.DataAnnotations.Schema;

namespace WebProject_NetFramework.DbContext.DbModels
{
    public class TestGrade : BaseDbModels.BaseDbClass
    {
        public string Score { get; set; }
        public string Grade { get; set; }
        public string Comment { get; set; }

        public long StudentId { get; set; }
        [ForeignKey("StudentId")] public virtual Student Student { get; set; }


        public long? AdmissionId { get; set; }
        [ForeignKey("AdmissionId")] public virtual Admission Admission { get; set; }
    }
}