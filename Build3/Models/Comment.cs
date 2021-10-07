using Build3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Build1.Models
{
    public class Comment
    {
        [Key]
        public int IdComment { get; set; }

        [Required(ErrorMessage ="Un comentariu nu poate fi gol")]
        public string ContinutComment { get; set; }

        public DateTime DataComment { get; set; }

        public int IdTask { get; set; }

        public virtual Task Task { get; set; }

        public string IdUser { get; set; }

        [ForeignKey("IdUser")]
        public virtual ApplicationUser User { get; set; }

    }
}