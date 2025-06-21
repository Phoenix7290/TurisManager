using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TurisManager.Data;
using TurisManager.Models;
using System.ComponentModel.DataAnnotations;

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
        public Reserva Reserva { get; set; } = new();

        [BindProperty]
        [Required(ErrorMessage = "Di�rias � obrigat�rio")]
        [Range(1, int.MaxValue, ErrorMessage = "Di�rias deve ser maior que zero")]
        public int Diarias { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Valor da di�ria � obrigat�rio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor da di�ria deve ser maior que zero")]
        public decimal ValorDiaria { get; set; }

        public SelectList Clientes { get; set; }
        public SelectList Pacotes { get; set; }
        public Reserva? NovaReserva { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await CarregarSelectLists();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Remover valida��es autom�ticas para campos que validaremos manualmente
            ModelState.Remove("Reserva.Cliente");
            ModelState.Remove("Reserva.PacoteTuristico");

            // Valida��es manuais
            if (Reserva.ClienteId <= 0)
            {
                ModelState.AddModelError("Reserva.ClienteId", "Selecione um cliente v�lido");
            }

            if (Reserva.PacoteTuristicoId <= 0)
            {
                ModelState.AddModelError("Reserva.PacoteTuristicoId", "Selecione um pacote tur�stico v�lido");
            }

            if (Diarias <= 0)
            {
                ModelState.AddModelError("Diarias", "Di�rias deve ser maior que zero");
            }

            if (ValorDiaria <= 0)
            {
                ModelState.AddModelError("ValorDiaria", "Valor da di�ria deve ser maior que zero");
            }

            if (Reserva.DataReserva <= DateTime.Today)
            {
                ModelState.AddModelError("Reserva.DataReserva", "Data da reserva deve ser futura");
            }

            // Verificar se cliente existe
            if (Reserva.ClienteId > 0)
            {
                var clienteExiste = await _context.Clientes
                    .AnyAsync(c => c.Id == Reserva.ClienteId && !c.IsDeleted);
                if (!clienteExiste)
                {
                    ModelState.AddModelError("Reserva.ClienteId", "Cliente n�o encontrado");
                }
            }

            // Verificar se pacote existe
            if (Reserva.PacoteTuristicoId > 0)
            {
                var pacoteExiste = await _context.PacotesTuristicos
                    .AnyAsync(p => p.Id == Reserva.PacoteTuristicoId);
                if (!pacoteExiste)
                {
                    ModelState.AddModelError("Reserva.PacoteTuristicoId", "Pacote tur�stico n�o encontrado");
                }
            }

            if (!ModelState.IsValid)
            {
                await CarregarSelectLists();
                return Page();
            }

            try
            {
                Func<int, decimal, decimal> calcularTotal = (diarias, valorDiaria) => diarias * valorDiaria;
                Reserva.ValorTotal = calcularTotal(Diarias, ValorDiaria);

                // Adicionar ao contexto
                _context.Reservas.Add(Reserva);
                await _context.SaveChangesAsync();

                // Carregar dados completos da reserva criada para exibi��o
                NovaReserva = await _context.Reservas
                    .Include(r => r.Cliente)
                    .Include(r => r.PacoteTuristico)
                        .ThenInclude(p => p.Destinos)
                    .FirstOrDefaultAsync(r => r.Id == Reserva.Id);

                TempData["SuccessMessage"] = "Reserva criada com sucesso!";

                // Limpar o formul�rio para nova reserva
                Reserva = new Reserva();
                Diarias = 0;
                ValorDiaria = 0;

                await CarregarSelectLists();
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao criar reserva: {ex.Message}");
                await CarregarSelectLists();
                return Page();
            }
        }

        public async Task<JsonResult> OnGetDestinosAsync(int pacoteId)
        {
            if (pacoteId <= 0)
            {
                return new JsonResult(new object[] { });
            }

            try
            {
                var pacote = await _context.PacotesTuristicos
                    .Include(p => p.Destinos)
                    .FirstOrDefaultAsync(p => p.Id == pacoteId);

                if (pacote?.Destinos == null || !pacote.Destinos.Any())
                {
                    return new JsonResult(new object[] { });
                }

                var destinos = pacote.Destinos.Select(d => new
                {
                    nome = d.Nome,
                    pais = d.Pais
                }).ToArray();

                return new JsonResult(destinos);
            }
            catch (Exception)
            {
                return new JsonResult(new object[] { });
            }
        }

        private async Task CarregarSelectLists()
        {
            Clientes = new SelectList(
                await _context.Clientes
                    .Where(c => !c.IsDeleted)
                    .OrderBy(c => c.Nome)
                    .ToListAsync(),
                "Id", "Nome");

            Pacotes = new SelectList(
                await _context.PacotesTuristicos
                    .OrderBy(p => p.Titulo)
                    .ToListAsync(),
                "Id", "Titulo");
        }
    }
}