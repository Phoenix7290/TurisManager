using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TurisManager.Data;
using TurisManager.Models;
using Microsoft.EntityFrameworkCore;

namespace TurisManager.Pages.PacotesTuristicos
{
    public class IndexModel : PageModel
    {
        private readonly TurisManagerContext _context;

        public IndexModel(TurisManagerContext context)
        {
            _context = context;
        }

        public PacoteTuristico PacoteTuristico { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            PacoteTuristico = await _context.PacotesTuristicos
                .Include(p => p.Destinos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (PacoteTuristico == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}