using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReVeste.API.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(150)]
        [EmailAddress]
        public string Email { get; set; }

        public ICollection<Aposta>? Apostas { get; set; }
    }
}

