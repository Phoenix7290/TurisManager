using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TurisManager.Data;
using TurisManager.Models;

namespace TurisManager.Pages.Reservas
{
    [Authorize]
    public class CriarClienteModel : PageModel
    {
        private readonly TurisManagerContext _context;

        public CriarClienteModel(TurisManagerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Cliente Cliente { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Clientes.Add(Cliente);
            await _context.SaveChangesAsync();
            return RedirectToPage("/Clientes/Index");
        }
    }
}