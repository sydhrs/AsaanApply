using System.ComponentModel.DataAnnotations.Schema;
using System.Web.DynamicData;

namespace WebProject_NetFramework.DbContext.DbModels
{
    [TableName("Staff")]
    public class Staff : Person
    {
        public string Designation { get; set; }
    }
}