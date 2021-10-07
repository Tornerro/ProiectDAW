using Build1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Build3.Models
{
    public class Contract
    {
        [Key]
        public int IdContract { get; set; }

        [Required]
        public int IdProiect { get; set; }

        [Required]
        public string IdUser { get; set; }

        public virtual Proiect Proiect { get; set; }

        [ForeignKey("IdUser")]
        public virtual ApplicationUser User { get; set; }
    }
}