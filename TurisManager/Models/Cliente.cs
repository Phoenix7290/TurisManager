using System.ComponentModel.DataAnnotations;

namespace TurisManager.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        public bool IsDeleted { get; set; }
        public List<Reserva> Reservas { get; set; }

        public void MarcarComoDeletado()
        {
            IsDeleted = true;
        }
    }
}