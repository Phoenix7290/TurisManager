using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TurisManager.Data;
using TurisManager.Models;
using System.ComponentModel.DataAnnotations;

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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var cliente = new Cliente
            {
                Nome = Cliente.Nome,
                Email = Cliente.Email,
                IsDeleted = false
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}