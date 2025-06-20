using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Cliente = await _context.Clientes.FindAsync(id);
            if (Cliente == null)
                return NotFound();

            Cliente.MarcarComoDeletado();
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}