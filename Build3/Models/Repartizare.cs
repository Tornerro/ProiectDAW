using Build1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Build3.Models
{
    public class Repartizare
    {
        [Key]
        public int IdRepartizare { get; set; }

        [Required]
        public int IdTask { get; set; }

        [Required]
        public string IdUser { get; set; }

        public virtual Task Task { get; set; }

        [ForeignKey("IdUser")]
        public virtual ApplicationUser User { get; set; }

    }
}