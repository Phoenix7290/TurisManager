using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TurisManager.Data;
using TurisManager.Models;

namespace TurisManager.Pages.Clientes
{
    public class EditarClienteModel : PageModel
    {
        private readonly TurisManagerContext _context;

        public EditarClienteModel(TurisManagerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ClienteInputModel Input { get; set; } = new();

        public class ClienteInputModel
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "O nome é obrigatório")]
            [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
            public string Nome { get; set; } = string.Empty;

            [Required(ErrorMessage = "O e-mail é obrigatório")]
            [EmailAddress(ErrorMessage = "E-mail inválido")]
            public string Email { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cliente == null)
            {
                return NotFound();
            }

            Input = new ClienteInputModel
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Email = cliente.Email
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var cliente = await _context.Clientes
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == Input.Id);

            if (cliente == null)
            {
                return NotFound();
            }

            var clienteExistente = await _context.Clientes
                .Where(c => !c.IsDeleted && c.Id != Input.Id && c.Email == Input.Email)
                .FirstOrDefaultAsync();

            if (clienteExistente != null)
            {
                ModelState.AddModelError("Input.Email", "Já existe um cliente cadastrado com este e-mail.");
                return Page();
            }

            cliente.Nome = Input.Nome;
            cliente.Email = Input.Email;

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cliente editado com sucesso!";
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Erro ao salvar as alterações. Tente novamente.");
                return Page();
            }
        }
    }
}