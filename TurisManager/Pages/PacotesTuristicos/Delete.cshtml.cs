using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TurisManager.Data;
using TurisManager.Models;
using System.Threading.Tasks;

namespace TurisManager.Pages.PacotesTuristicos
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly TurisManagerContext _context;

        public DeleteModel(TurisManagerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PacoteTuristico Pacote { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Pacote = await _context.PacotesTuristicos
                .Include(p => p.Destinos)
                    .ThenInclude(d => d.PaisDestino)
                .Include(p => p.Reservas)
                    .ThenInclude(r => r.Cliente)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Pacote == null)
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

            Pacote = await _context.PacotesTuristicos
                .Include(p => p.Reservas)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Pacote == null)
            {
                return NotFound();
            }

            // Verificar se existem reservas associadas
            if (Pacote.Reservas != null && Pacote.Reservas.Any())
            {
                ErrorMessage = "Não é possível excluir este pacote pois existem reservas associadas a ele.";

                // Recarregar os dados para exibir novamente
                Pacote = await _context.PacotesTuristicos
                    .Include(p => p.Destinos)
                        .ThenInclude(d => d.PaisDestino)
                    .Include(p => p.Reservas)
                        .ThenInclude(r => r.Cliente)
                    .FirstOrDefaultAsync(p => p.Id == id);

                return Page();
            }

            try
            {
                _context.PacotesTuristicos.Remove(Pacote);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Pacote turístico '{Pacote.Titulo}' foi excluído com sucesso.";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = "Ocorreu um erro ao tentar excluir o pacote. Tente novamente.";

                // Recarregar os dados para exibir novamente
                Pacote = await _context.PacotesTuristicos
                    .Include(p => p.Destinos)
                        .ThenInclude(d => d.PaisDestino)
                    .Include(p => p.Reservas)
                        .ThenInclude(r => r.Cliente)
                    .FirstOrDefaultAsync(p => p.Id == id);

                return Page();
            }
        }
    }
}