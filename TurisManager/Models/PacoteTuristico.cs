// TurisManager/Models/PacoteTuristico.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TurisManager.Models
{
    public class PacoteTuristico
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatória")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Início")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A capacidade máxima é obrigatória")]
        [Range(1, int.MaxValue, ErrorMessage = "A capacidade deve ser maior que 0")]
        public int CapacidadeMaxima { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que 0")]
        public decimal Preco { get; set; }

        public List<CidadeDestino> Destinos { get; set; } = new List<CidadeDestino>();
        public List<Reserva> Reservas { get; set; } = new List<Reserva>();

        public delegate void CapacityReachedHandler(object sender, EventArgs e);
        public event CapacityReachedHandler CapacityReached;

        public void CheckCapacity(int currentReservations)
        {
            if (currentReservations >= CapacidadeMaxima)
            {
                CapacityReached?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
