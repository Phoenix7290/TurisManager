using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurisManager.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        [Required]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        [Required]
        public int PacoteTuristicoId { get; set; }
        public PacoteTuristico PacoteTuristico { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DataReserva { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorTotal { get; set; }

        public static Func<int, decimal, decimal> CalcularValorTotal = (diarias, valorDiaria) => diarias * valorDiaria;
    }
}