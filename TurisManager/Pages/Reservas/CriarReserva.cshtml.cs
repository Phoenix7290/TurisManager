using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TurisManager.Data;
using TurisManager.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TurisManager.Pages.Reservas
{
    [Authorize]
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

        public async Task<IActionResult> OnGetAsync(int? clienteId)
        {
            Clientes = new SelectList(await _context.Clientes.Where(c => !c.IsDeleted).ToListAsync(), "Id", "Nome", clienteId);
            Pacotes = new SelectList(await _context.PacotesTuristicos
                .Where(p => p.DataInicio > DateTime.Now)
                .ToListAsync(), "Id", "Titulo");

            if (clienteId.HasValue)
            {
                Reserva = new Reserva { ClienteId = clienteId.Value };
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Clientes = new SelectList(await _context.Clientes.Where(c => !c.IsDeleted).ToListAsync(), "Id", "Nome", Reserva.ClienteId);
                Pacotes = new SelectList(await _context.PacotesTuristicos
                    .Where(p => p.DataInicio > DateTime.Now)
                    .ToListAsync(), "Id", "Titulo");
                return Page();
            }

            var existingReserva = await _context.Reservas
                .AnyAsync(r => r.ClienteId == Reserva.ClienteId &&
                               r.PacoteTuristicoId == Reserva.PacoteTuristicoId &&
                               r.DataReserva.Date == Reserva.DataReserva.Date);
            if (existingReserva)
            {
                ModelState.AddModelError(string.Empty, "O cliente já possui uma reserva para este pacote na mesma data.");
                Clientes = new SelectList(await _context.Clientes.Where(c => !c.IsDeleted).ToListAsync(), "Id", "Nome", Reserva.ClienteId);
                Pacotes = new SelectList(await _context.PacotesTuristicos
                    .Where(p => p.DataInicio > DateTime.Now)
                    .ToListAsync(), "Id", "Titulo");
                return Page();
            }

            var pacote = await _context.PacotesTuristicos
                .Include(p => p.Reservas)
                .FirstOrDefaultAsync(p => p.Id == Reserva.PacoteTuristicoId);
            if (pacote.Reservas.Count >= pacote.CapacidadeMaxima)
            {
                ModelState.AddModelError(string.Empty, "O pacote atingiu a capacidade máxima.");
                Clientes = new SelectList(await _context.Clientes.Where(c => !c.IsDeleted).ToListAsync(), "Id", "Nome", Reserva.ClienteId);
                Pacotes = new SelectList(await _context.PacotesTuristicos
                    .Where(p => p.DataInicio > DateTime.Now)
                    .ToListAsync(), "Id", "Titulo");
                return Page();
            }

            if (pacote.Reservas.Count + 1 >= pacote.CapacidadeMaxima)
            {
                pacote.CapacityReached += (sender, e) =>
                {
                    Console.WriteLine($"Capacidade máxima atingida para o pacote {pacote.Titulo}");
                };
                pacote.CheckCapacity(pacote.Reservas.Count + 1);
            }

            Reserva.ValorTotal = Reserva.CalcularValorTotal(Diarias, ValorDiaria);

            _context.Reservas.Add(Reserva);
            await _context.SaveChangesAsync();
           
            TempData["SuccessMessage"] = "Reserva criada com sucesso!";
            return RedirectToPage("./CriarReserva", new { clienteId = Reserva.ClienteId });
        }
    }
}