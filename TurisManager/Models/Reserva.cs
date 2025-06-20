namespace TurisManager.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public int PacoteTuristicoId { get; set; }
        public PacoteTuristico PacoteTuristico { get; set; }
        public DateTime DataReserva { get; set; }
        public decimal ValorTotal { get; set; } 

        public Func<int, decimal, decimal> CalcularValorTotal { get; } = (diarias, valorDiaria) => diarias * valorDiaria;
    }
}