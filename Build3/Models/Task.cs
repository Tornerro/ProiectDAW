using Build3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Build1.Models
{
    public class Task
    {
        [Key]
        public int IdTask { get; set; }

        [Required(ErrorMessage ="Numele taskului este obligatoriu")]
        [StringLength(20, ErrorMessage = "Numele taskului nu poate avea mai mult de 20 de caractere")]
        public string NumeTask { get; set; }

        public string DescriereTask { get; set; }

        public DateTime DataTask { get; set; }

        public DateTime? DataSfarsitTask { get; set; }

        [Required]
        public string Status { get; set; }

        public int IdProiect { get; set; }

        public virtual Proiect Proiect { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Repartizare> Rapartizari { get; set; }

    }
}