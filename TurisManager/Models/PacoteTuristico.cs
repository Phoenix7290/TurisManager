using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurisManager.Models
{
    public class PacoteTuristico
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Título é obrigatório")]
        [StringLength(200, ErrorMessage = "Título deve ter no máximo 200 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Data de início é obrigatória")]
        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "Capacidade máxima é obrigatória")]
        [Range(1, int.MaxValue, ErrorMessage = "Capacidade máxima deve ser maior que zero")]
        public int CapacidadeMaxima { get; set; }

        [Required(ErrorMessage = "Preço é obrigatório")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
        public decimal Preco { get; set; }

        public virtual ICollection<CidadeDestino> Destinos { get; set; } = new List<CidadeDestino>();

        public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

        public event EventHandler CapacityReached;

        public void CheckCapacity(int currentReservations)
        {
            if (currentReservations >= CapacidadeMaxima)
            {
                CapacityReached?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}