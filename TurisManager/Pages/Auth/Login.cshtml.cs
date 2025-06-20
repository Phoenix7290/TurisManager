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
                ModelState.AddModelError("", "Usuário e senha são obrigatórios.");
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

            ModelState.AddModelError("", "Usuário ou senha inválidos.");
            return Page();
        }
    }
}