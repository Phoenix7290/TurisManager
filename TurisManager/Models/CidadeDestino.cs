using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TurisManager.Models
{
    public class CidadeDestino
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome da cidade é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome da cidade deve ter no máximo 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "País é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome do país deve ter no máximo 100 caracteres")]
        public string Pais { get; set; }

        [Required(ErrorMessage = "País destino é obrigatório")]
        public int PaisDestinoId { get; set; }

        public virtual PaisDestino PaisDestino { get; set; }

        public virtual ICollection<PacoteTuristico> PacotesTuristicos { get; set; } = new List<PacoteTuristico>();
    }
}