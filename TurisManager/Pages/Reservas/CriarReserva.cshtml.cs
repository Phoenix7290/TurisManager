using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TurisManager.Data;
using TurisManager.Models;

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

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // Buscar o pacote turístico associado
            var pacote = await _context.PacotesTuristicos
                .FirstOrDefaultAsync(p => p.Id == Reserva.PacoteTuristicoId);

            if (pacote == null)
            {
                ModelState.AddModelError("", "Pacote turístico não encontrado.");
                return Page();
            }

            // Contar o número de reservas para o pacote
            int numeroReservas = await _context.Reservas
                .CountAsync(r => r.PacoteTuristicoId == Reserva.PacoteTuristicoId);

            // Verificar capacidade
            pacote.VerificarCapacidade(numeroReservas + 1); // +1 para incluir a nova reserva

            // Registrar o evento CapacityReached
            pacote.CapacityReached += (s, e) =>
            {
                ModelState.AddModelError("", $"Capacidade máxima atingida para o pacote {pacote.Titulo}.");
            };

            // Se a capacidade foi atingida, impedir a criação
            if (numeroReservas >= pacote.CapacidadeMaxima)
            {
                return Page();
            }

            // Calcular o valor total
            Reserva.ValorTotal = Reserva.CalcularValorTotal(Diarias, ValorDiaria);

            // Adicionar a reserva
            _context.Reservas.Add(Reserva);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}