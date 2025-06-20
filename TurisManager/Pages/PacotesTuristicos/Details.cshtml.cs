using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TurisManager.Data;
using TurisManager.Models;
using System.Threading.Tasks;

namespace TurisManager.Pages.PacotesTuristicos
{
    public class DetailsModel : PageModel
    {
        private readonly TurisManagerContext _context;

        public DetailsModel(TurisManagerContext context)
        {
            _context = context;
        }

        public PacoteTuristico Pacote { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Pacote = await _context.PacotesTuristicos
                .Include(p => p.Destinos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Pacote == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}