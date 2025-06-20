using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TurisManager.Data;
using TurisManager.Models;
using Microsoft.EntityFrameworkCore;

namespace TurisManager.Pages.Reservas
{
    public class CriarReservaModel : PageModel
    {
        private readonly TurisManagerContext _context;

        public CriarReservaModel(TurisManagerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Reserva Reserva { get; set; }
        [BindProperty]
        public int Diarias { get; set; }
        [BindProperty]
        public decimal ValorDiaria { get; set; }
        public SelectList Clientes { get; set; }
        public SelectList Pacotes { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Clientes = new SelectList(await _context.Clientes.ToListAsync(), "Id", "Nome");
            Pacotes = new SelectList(await _context.PacotesTuristicos.ToListAsync(), "Id", "Titulo");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            Reserva.ValorTotal = Reserva.CalcularValorTotal(Diarias, ValorDiaria);

            _context.Reservas.Add(Reserva);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}