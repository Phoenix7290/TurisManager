using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TurisManager.Data;
using TurisManager.Models;

namespace TurisManager.Pages.Clientes
{
    public class DeletarClienteModel : PageModel
    {
        private readonly TurisManagerContext _context;

        public DeletarClienteModel(TurisManagerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Cliente Cliente { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Cliente = await _context.Clientes
                .Include(c => c.Reservas)
                    .ThenInclude(r => r.PacoteTuristico)
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

            if (Cliente == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Cliente = await _context.Clientes
                .Include(c => c.Reservas)
                    .ThenInclude(r => r.PacoteTuristico)
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

            if (Cliente == null)
            {
                return NotFound();
            }

            try
            {
                Cliente.MarcarComoDeletado();
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Cliente '{Cliente.Nome}' foi excluído com sucesso (exclusão lógica).";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = "Erro ao excluir o cliente. Tente novamente.";
                return Page();
            }
        }
    }
}