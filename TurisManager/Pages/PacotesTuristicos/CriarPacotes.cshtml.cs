using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TurisManager.Data;
using TurisManager.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public PacoteTuristico Pacote { get; set; } = new PacoteTuristico();

        [BindProperty]
        public int SelectedPaisDestinoId { get; set; }

        [BindProperty]
        public int SelectedCidadeDestinoId { get; set; }

        public SelectList Paises { get; set; }
        public SelectList Cidades { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Paises = new SelectList(await _context.PaisesDestinos.ToListAsync(), "Id", "Nome");
            Cidades = new SelectList(await _context.CidadesDestinos.ToListAsync(), "Id", "Nome");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Paises = new SelectList(await _context.PaisesDestinos.ToListAsync(), "Id", "Nome");
                Cidades = new SelectList(await _context.CidadesDestinos.ToListAsync(), "Id", "Nome");
                return Page();
            }

            var cidadeDestino = await _context.CidadesDestinos
                .FirstOrDefaultAsync(c => c.Id == SelectedCidadeDestinoId);

            PaisDestino? paisDestino = null;
            if (cidadeDestino != null)
            {
                paisDestino = await _context.PaisesDestinos
                    .FirstOrDefaultAsync(p => p.Cidades.Any(c => c.Id == cidadeDestino.Id));
            }

            if (cidadeDestino != null)
            {
                Pacote.Destinos = new List<CidadeDestino> { cidadeDestino };
                _context.PacotesTuristicos.Add(Pacote);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}