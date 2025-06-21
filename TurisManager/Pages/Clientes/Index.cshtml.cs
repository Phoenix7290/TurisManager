using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TurisManager.Data;
using TurisManager.Models;

namespace TurisManager.Pages.Clientes
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly TurisManagerContext _context;

        public IndexModel(TurisManagerContext context)
        {
            _context = context;
        }

        public IList<Cliente> Clientes { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Clientes = await _context.Clientes
                .Include(c => c.Reservas)
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.Nome)
                .ToListAsync();

            return Page();
        }
    }
}