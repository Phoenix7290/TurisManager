namespace TurisManager.Models
{
    public class PacoteTuristico
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime DataInicio { get; set; }
        public int CapacidadeMaxima { get; set; }
        public decimal Preco { get; set; }
        public List<CidadeDestino> Destinos { get; set; }
        //public delegate void CapacityReachedEventHandler(object sender, EventArgs e);
        //public event CapacityReachedEventHandler CapacityReached;

        //public void CheckCapacity()
        //{
        //    if (Reservas?.Count >= CapacidadeMaxima)
        //        CapacityReached?.Invoke(this, EventArgs.Empty);
        //}

        public event EventHandler CapacityReached;
        public void VerificarCapacidade(int numeroReservas)
        {
            if (numeroReservas >= CapacidadeMaxima)
                CapacityReached?.Invoke(this, EventArgs.Empty);
        }
    }
}
