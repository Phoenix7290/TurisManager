using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TurisManager.Data;
using TurisManager.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TurisManager.Pages.PacotesTuristicos
{
    [Authorize]
    public class CriarPacotesModel : PageModel
    {
        private readonly TurisManagerContext _context;

        public CriarPacotesModel(TurisManagerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PacoteTuristico PacoteTuristico { get; set; }
        public SelectList CidadesDestinos { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            CidadesDestinos = new SelectList(await _context.CidadesDestinos.ToListAsync(), "Id", "Nome");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int[] selectedDestinos)
        {
            if (!ModelState.IsValid)
            {
                CidadesDestinos = new SelectList(await _context.CidadesDestinos.ToListAsync(), "Id", "Nome");
                return Page();
            }

            if (selectedDestinos != null)
            {
                PacoteTuristico.Destinos = await _context.CidadesDestinos
                    .Where(d => selectedDestinos.Contains(d.Id))
                    .ToListAsync();
            }

            _context.PacotesTuristicos.Add(PacoteTuristico);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}