using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace TurisManager.Pages.Notas
{
    [Authorize]
    public class VerNotasModel : PageModel
    {
        private readonly IWebHostEnvironment _env;

        public VerNotasModel(IWebHostEnvironment env)
        {
            _env = env;
            Files = new List<string>();
            FileContent = string.Empty;
        }

        [BindProperty]
        public string Anotacao { get; set; }

        public List<string> Files { get; set; }
        public string FileContent { get; set; }

        public void OnGet(string file)
        {
            var path = Path.Combine(_env.WebRootPath, "files");
            Directory.CreateDirectory(path);
            Files = Directory.GetFiles(path).Select(Path.GetFileName).ToList();

            if (!string.IsNullOrEmpty(file))
            {
                FileContent = System.IO.File.ReadAllText(Path.Combine(path, file));
            }
        }

        public IActionResult OnPost()
        {
            var basePath = Path.Combine(_env.WebRootPath, "files", "nota");
            var filePath = basePath + ".txt";
            int counter = 1;

            while (System.IO.File.Exists(filePath))
            {
                filePath = $"{basePath}{counter}.txt";
                counter++;
            }

            System.IO.File.WriteAllText(filePath, Anotacao);
            return RedirectToPage();
        }
    }
}