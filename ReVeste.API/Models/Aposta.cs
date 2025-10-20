using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ReVeste.API.Models
{
    public class Aposta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        [JsonIgnore]
        public Usuario Usuario { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Valor { get; set; }

        [Required]
        public DateTime DataAposta { get; set; }

        [StringLength(255)]
        public string Descricao { get; set; }
    }
}

