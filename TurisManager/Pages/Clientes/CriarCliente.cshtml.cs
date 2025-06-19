using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TurisManager.Data;
using TurisManager.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace TurisManager.Pages.Clientes
{
    [Authorize]
    public class CriarClienteModel : PageModel
    {
        private readonly TurisManagerContext _context;

        public CriarClienteModel(TurisManagerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ClienteInputModel Cliente { get; set; }

        public class ClienteInputModel
        {
            [Required(ErrorMessage = "O nome é obrigatório")]
            [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
            public string Nome { get; set; }

            [Required(ErrorMessage = "O email é obrigatório")]
            [EmailAddress(ErrorMessage = "Email inválido")]
            public string Email { get; set; }
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string redirectTo)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var cliente = new Cliente
                {
                    Nome = Cliente.Nome,
                    Email = Cliente.Email,
                    IsDeleted = false
                };

                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                if (!string.IsNullOrEmpty(redirectTo))
                {
                    return RedirectToPage(redirectTo, new { clienteId = cliente.Id });
                }

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erro ao salvar cliente: {ex.Message}");
                return Page();
            }
        }
    }
}