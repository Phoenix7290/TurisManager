using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TurisManager.Data;
using TurisManager.Models;

namespace TurisManager.Pages.Clientes.Detalhes
{
    public class DetalhesModel : PageModel
    {
        private readonly TurisManagerContext _context;

        public DetalhesModel(TurisManagerContext context)
        {
            _context = context;
        }

        public Cliente Cliente { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            Cliente = await _context.Clientes.FirstOrDefaultAsync(m => m.Id == id);
            if (Cliente == null) return NotFound();
            return Page();
        }
    }
}