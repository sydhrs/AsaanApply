using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProject_NetFramework.DbContext.DbModels
{
    public class AdmissionChallan : BaseDbModels.BaseDbClass
    {
        public string RefNo { get; set; }
        
        public decimal Amount { get; set; }
        public decimal FeeConcession { get; set; }  // fee concession or Adjustment in fees   
        public decimal NetAmount { get; set; }     // total amount after deducting fee adjustment

        public long? VerifiedBy { get; set; }

        public bool IsPaid { get; set; } = false;
        public DateTime? DatePaid { get; set; }

        public string StatusReason { get; set; }

        public long? EntityStatusTypeId { get; set; }
        [ForeignKey("EntityStatusTypeId")] public virtual EntityStatusType EntityStatusType { get; set; }

        public long AdmissionId { get; set; }

        [ForeignKey("AdmissionId")] public virtual Admission Admission { get; set; }
        
        public DateTime? DueDate { get; set; }

        public string MimeType { get; set; }
        
        public byte[] PaidChallan { get; set; }
    }
}