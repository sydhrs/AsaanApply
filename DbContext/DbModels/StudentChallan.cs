using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProject_NetFramework.DbContext.DbModels
{
    public class StudentChallan : BaseDbModels.BaseDbClass
    {
        public string RefNo { get; set; }
        public decimal Amount { get; set; }

        public long? VerifiedBy { get; set; }

        public bool IsPaid { get; set; } = false;
        public DateTime? DatePaid { get; set; }

        public string StatusReason { get; set; }

        public long? EntityStatusTypeId { get; set; }
        [ForeignKey("EntityStatusTypeId")] public virtual EntityStatusType EntityStatusType { get; set; }

        public long AdmissionId { get; set; }
        [ForeignKey("AdmissionId")] public virtual Admission Admission { get; set; }
        public DateTime? LastDate { get; set; }

        public string MimeType { get; set; }
        public byte[] PaidChallan { get; set; }
    }
}