using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProject_NetFramework.DbContext.DbModels
{
    public class AdmissionCriteria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        [StringLength(int.MaxValue)]
        public string Criteria { get; set; }
        public long DegreePursuingTypeId { get; set; }
    }
}