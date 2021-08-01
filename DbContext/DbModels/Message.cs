using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebProject_NetFramework.DbContext.DbModels
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        [Column("Message")] public string Message1 { get; set; }

        public long? EntityStatusTypeId { get; set; }
        [ForeignKey("EntityStatusTypeId")] public virtual EntityStatusType EntityStatusType { get; set; }
        public string StatusReason { get; set; }
    }
}