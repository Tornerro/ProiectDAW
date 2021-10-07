using Build3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Build1.Models
{
    public class Proiect
    {
        [Key]
        public int IdProiect { get; set; }

        [Required(ErrorMessage = "Numele proiectului este obligatoriu")]
        [StringLength(20, ErrorMessage = "Numele proiectului nu poate avea mai mult de 20 de caractere")]
        public string NumeProiect { get; set; }

        public DateTime DataProiect { get; set; }

        public string IdUser { get; set; }

        [ForeignKey("IdUser")]
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }

        public virtual ICollection<Contract> Contracte { get; set; }

    }

    
}