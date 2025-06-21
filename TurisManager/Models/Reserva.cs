using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurisManager.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public int PacoteTuristicoId { get; set; }
        public PacoteTuristico PacoteTuristico { get; set; }

        [Required(ErrorMessage = "Data da Reserva é obrigatória")]
        [DataType(DataType.Date)]
        public DateTime DataReserva { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorTotal { get; set; }

        public decimal CalcularValorTotal(int diarias, decimal valorDiaria)
        {
            return diarias * valorDiaria;
        }
        public bool IsConfirmada { get; set; }
    }
}