namespace TurisManager.Models
{
    public class PaisDestino
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<CidadeDestino> Cidades { get; set; }
    }
}
