using System.ComponentModel.DataAnnotations.Schema;
using WebProject_NetFramework.DbContext.BaseDbModels;

namespace WebProject_NetFramework.DbContext.DbModels
{
    public class AcademicDegreeType : BaseTypeValue
    {
        public long? PrerequisiteDegreeTypeId { get; set; }

        [ForeignKey("PrerequisiteDegreeTypeId")]
        public virtual DegreePursuingType DegreePursuingType { get; set; }
    }
}