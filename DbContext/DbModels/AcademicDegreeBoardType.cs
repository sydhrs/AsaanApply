using WebProject_NetFramework.DbContext.BaseDbModels;

namespace WebProject_NetFramework.DbContext.DbModels
{
    public class AcademicDegreeBoardType : BaseTypeValue
    {
        public string PrerequisiteAcademicDegreeTypeIds { get; set; }
    }
}