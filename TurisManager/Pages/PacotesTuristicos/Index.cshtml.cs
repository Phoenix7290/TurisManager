using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TurisManager.Data;
using TurisManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TurisManager.Pages.PacotesTuristicos
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly TurisManagerContext _context;

        public IndexModel(TurisManagerContext context)
        {
            _context = context;
        }

        public IList<PacoteTuristico> PacotesTuristicos { get; set; }

        public async Task OnGetAsync()
        {
            PacotesTuristicos = await _context.PacotesTuristicos
                .Include(p => p.Destinos)
                .ToListAsync();
        }
    }
}