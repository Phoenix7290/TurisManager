using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TurisManager.Models;

namespace TurisManager.Pages.PacoteTuristico
{
    public class IndexModel : PageModel
    {
        public Models.PacoteTuristico PacoteTuristico { get; set; }
        public void OnGet()
        {
        }
    }
}