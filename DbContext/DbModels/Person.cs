using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.DynamicData;

namespace WebProject_NetFramework.DbContext.DbModels
{
    [TableName("Persons")]
    public class Person : BaseDbModels.BaseDbClass
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public byte[] Photo { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string CnicNo { get; set; }
        public long GenderTypeId { get; set; }
        [ForeignKey("GenderTypeId")] public virtual GenderType GenderType { get; set; }
        public long? MaritalStatusTypeId { get; set; }
        [ForeignKey("MaritalStatusTypeId")] public virtual MaritalStatusType MaritalStatusType { get; set; }
        public long? ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")] public virtual ApplicationUser ApplicationUser { get; set; }
        public long? AddressId { get; set; }
        [ForeignKey("AddressId")] public virtual Address Address { get; set; }
        public long? PersonContactId { get; set; }
        [ForeignKey("PersonContactId")] public virtual PersonContact PersonContact { get; set; }
    }
}