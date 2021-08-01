using System.ComponentModel.DataAnnotations.Schema;

namespace WebProject_NetFramework.DbContext.DbModels
{
    public class StudentAcademic : BaseDbModels.BaseDbClass
    {
        public string RollNo {get;set;}
        public decimal MarksObtained { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal ProjectedPercentage { get; set; } // Will be automatically Calculated on Adding this Record
        public string YearOfPassing { get; set; }
        public long AcademicDegreeTypeId { get; set; }
        [ForeignKey("AcademicDegreeTypeId")] public virtual AcademicDegreeType AcademicDegreeType { get; set; }
        public long? AcademicDegreeBoardTypeId { get; set; }
        [ForeignKey("AcademicDegreeBoardTypeId")]
        public virtual AcademicDegreeBoardType AcademicDegreeBoardType { get; set; }

        public long AdmissionId { get; set; }
        public virtual Admission Admission { get; set; }
    }
}