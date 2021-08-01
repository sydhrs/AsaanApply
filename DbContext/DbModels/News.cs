using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace WebProject_NetFramework.DbContext.DbModels
{
    public class News
    {   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Title { get; set; }   

        public string Detail { get; set; }

        public DateTime? Date { get; set; }

        public string PictureName { get; set; }

    }
} 