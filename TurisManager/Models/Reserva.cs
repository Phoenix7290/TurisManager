using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurisManager.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Cliente é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um cliente válido")]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [Required(ErrorMessage = "Pacote Turístico é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um pacote turístico válido")]
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
    }
}