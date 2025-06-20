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
using System.ComponentModel.DataAnnotations;

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
        public Reserva Reserva { get; set; } = new();

        [BindProperty]
        [Required(ErrorMessage = "Número de diárias é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Número de diárias deve ser maior que zero")]
        public int Diarias { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Valor da diária é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor da diária deve ser maior que zero")]
        public decimal ValorDiaria { get; set; }

        public SelectList Clientes { get; set; }
        public SelectList Pacotes { get; set; }
        public Reserva NovaReserva { get; set; }

        public async Task<IActionResult> OnGetDestinosAsync(int pacoteId)
        {
            var destinos = await _context.PacotesTuristicos
                .Where(p => p.Id == pacoteId)
                .SelectMany(p => p.Destinos)
                .Select(d => new {
                    Nome = d.Nome,
                    Pais = d.Pais ?? d.PaisDestino.Nome
                })
                .ToListAsync();

            return new JsonResult(destinos);
        }

        public async Task<IActionResult> OnGetAsync(int? clienteId)
        {
            await CarregarListas(clienteId);

            Reserva = new Reserva
            {
                DataReserva = DateTime.Today.AddDays(1)
            };

            if (clienteId.HasValue)
            {
                Reserva.ClienteId = clienteId.Value;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Debug logging
            System.Diagnostics.Debug.WriteLine("=== DEBUG POST ===");
            System.Diagnostics.Debug.WriteLine($"Reserva.ClienteId: {Reserva.ClienteId}");
            System.Diagnostics.Debug.WriteLine($"Reserva.PacoteTuristicoId: {Reserva.PacoteTuristicoId}");
            System.Diagnostics.Debug.WriteLine($"Diarias: {Diarias}");
            System.Diagnostics.Debug.WriteLine($"ValorDiaria: {ValorDiaria}");
            System.Diagnostics.Debug.WriteLine($"DataReserva: {Reserva.DataReserva}");

            if (!ModelState.IsValid)
            {
                await CarregarListas(Reserva.ClienteId);
                return Page();
            }
            
            if (Reserva.ClienteId <= 0)
            {
                ModelState.AddModelError("Reserva.ClienteId", "Selecione um cliente válido");
                await CarregarListas(Reserva.ClienteId);
                return Page();
            }

            if (Reserva.PacoteTuristicoId <= 0)
            {
                ModelState.AddModelError("Reserva.PacoteTuristicoId", "Selecione um pacote turístico válido");
                await CarregarListas(Reserva.ClienteId);
                return Page();
            }
            

            if (Diarias <= 0)
            {
                ModelState.AddModelError("Diarias", "Número de diárias deve ser maior que zero");
                await CarregarListas(Reserva.ClienteId);
                return Page();
            }

            if (ValorDiaria <= 0)
            {
                ModelState.AddModelError("ValorDiaria", "Valor da diária deve ser maior que zero");
                await CarregarListas(Reserva.ClienteId);
                return Page();
            }

            if (Reserva.DataReserva <= DateTime.Today)
            {
                ModelState.AddModelError("Reserva.DataReserva", "Data da reserva deve ser futura");
                await CarregarListas(Reserva.ClienteId);
                return Page();
            }

            // Calculate ValorTotal
            Reserva.ValorTotal = Diarias * ValorDiaria;

            // Check for existing reservation (optional for testing, can be commented out)
            /*
            var existingReserva = await _context.Reservas
                .AnyAsync(r => r.ClienteId == Reserva.ClienteId &&
                               r.PacoteTuristicoId == Reserva.PacoteTuristicoId &&
                               r.DataReserva.Date == Reserva.DataReserva.Date);

            if (existingReserva)
            {
                ModelState.AddModelError(string.Empty, "O cliente já possui uma reserva para este pacote na mesma data.");
                await CarregarListas(Reserva.ClienteId);
                return Page();
            }
            */

            // Check package capacity (optional for testing, can be commented out)
            /*
            var pacote = await _context.PacotesTuristicos
                .Include(p => p.Reservas)
                .FirstOrDefaultAsync(p => p.Id == Reserva.PacoteTuristicoId);

            if (pacote == null)
            {
                ModelState.AddModelError("Reserva.PacoteTuristicoId", "Pacote turístico não encontrado");
                await CarregarListas(Reserva.ClienteId);
                return Page();
            }

            if (pacote.Reservas.Count >= pacote.CapacidadeMaxima)
            {
                ModelState.AddModelError(string.Empty, "O pacote atingiu a capacidade máxima.");
                await CarregarListas(Reserva.ClienteId);
                return Page();
            }
            */

            try
            {
                _context.Reservas.Add(Reserva);
                await _context.SaveChangesAsync();

                NovaReserva = await _context.Reservas
                    .Include(r => r.Cliente)
                    .Include(r => r.PacoteTuristico)
                        .ThenInclude(p => p.Destinos)
                            .ThenInclude(d => d.PaisDestino)
                    .FirstOrDefaultAsync(r => r.Id == Reserva.Id);

                TempData["SuccessMessage"] = "Reserva criada com sucesso!";

                Reserva = new Reserva { DataReserva = DateTime.Today.AddDays(1) };
                Diarias = 0;
                ValorDiaria = 0;

                await CarregarListas(Reserva.ClienteId);
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erro ao salvar a reserva: {ex.Message}");
                await CarregarListas(Reserva.ClienteId);
                return Page();
            }
        }

        private async Task CarregarListas(int? selectedClienteId)
        {
            var clientesLista = await _context.Clientes
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.Nome)
                .ToListAsync();

            var pacotesLista = await _context.PacotesTuristicos
                .Where(p => p.DataInicio > DateTime.Today)
                .OrderBy(p => p.Titulo)
                .ToListAsync();

            Clientes = new SelectList(clientesLista, "Id", "Nome", selectedClienteId ?? Reserva?.ClienteId);
            Pacotes = new SelectList(pacotesLista, "Id", "Titulo", Reserva?.PacoteTuristicoId);
            System.Diagnostics.Debug.WriteLine($"Clientes count: {clientesLista.Count}");
            System.Diagnostics.Debug.WriteLine($"Pacotes count: {pacotesLista.Count}");
        }
    }
}