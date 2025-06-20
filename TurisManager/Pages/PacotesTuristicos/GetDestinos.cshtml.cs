using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TurisManager.Data;
using System.Linq;
using System.Threading.Tasks;

namespace TurisManager.Pages.PacotesTuristicos
{
    public class GetDestinosModel : PageModel
    {
        private readonly TurisManagerContext _context;

        public GetDestinosModel(TurisManagerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int pacoteId)
        {
            if (pacoteId <= 0)
            {
                return new JsonResult(new object[] { });
            }

            var pacote = await _context.PacotesTuristicos
                .Include(p => p.Destinos)
                .FirstOrDefaultAsync(p => p.Id == pacoteId);

            if (pacote?.Destinos == null)
            {
                return new JsonResult(new object[] { });
            }

            var destinos = pacote.Destinos.Select(d => new
            {
                Nome = d.Nome,
                Pais = d.Pais
            }).ToArray();

            return new JsonResult(destinos);
        }
    }
}