using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TurisManager.Data;
using TurisManager.Models;

namespace TurisManager.Pages.Reservas
{
    public class IndexModel : PageModel
    {
        private readonly TurisManagerContext _context;

        public IndexModel(TurisManagerContext context)
        {
            _context = context;
        }

        public IList<Reserva> Reservas { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? StatusFilter { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.PacoteTuristico)
                    .ThenInclude(p => p.Destinos)
                        .ThenInclude(d => d.PaisDestino)
                .AsQueryable();

            // Filtro por nome do cliente
            if (!string.IsNullOrEmpty(SearchString))
            {
                query = query.Where(r => r.Cliente.Nome.Contains(SearchString) ||
                                        r.PacoteTuristico.Titulo.Contains(SearchString));
            }

            // Filtro por status
            if (!string.IsNullOrEmpty(StatusFilter))
            {
                if (StatusFilter == "confirmada")
                {
                    query = query.Where(r => r.IsConfirmada);
                }
                else if (StatusFilter == "pendente")
                {
                    query = query.Where(r => !r.IsConfirmada);
                }
            }

            Reservas = await query
                .OrderByDescending(r => r.DataReserva)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostConfirmarAsync(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            reserva.IsConfirmada = true;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Reserva confirmada com sucesso!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCancelarAsync(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Reserva cancelada com sucesso!";
            return RedirectToPage();
        }
    }
}