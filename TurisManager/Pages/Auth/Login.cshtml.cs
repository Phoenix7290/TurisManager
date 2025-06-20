using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TurisManager.Pages.Auth
{
    public class LoginModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Usu�rio e senha s�o obrigat�rios.");
                return Page();
            }

            if (username == "admin" && password == "1234")
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync("CookieAuth", principal);
                return RedirectToPage("/Index");
            }

            ModelState.AddModelError("", "Usu�rio ou senha inv�lidos.");
            return Page();
        }
    }
}