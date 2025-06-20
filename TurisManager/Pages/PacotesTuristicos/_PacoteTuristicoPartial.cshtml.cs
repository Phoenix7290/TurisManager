using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using TurisManager.Models;

namespace TurisManager.Pages.PacotesTuristicos
{
    public class PacoteTuristicoPartialModel : PageModel
    {
        public string Titulo { get; set; }
        public DateTime DataInicio { get; set; }
        public int CapacidadeMaxima { get; set; }
        public decimal Preco { get; set; }
        public List<CidadeDestino> Destinos { get; set; } = new();

        public void OnGet(
            string titulo,
            DateTime dataInicio,
            int capacidadeMaxima,
            decimal preco,
            List<CidadeDestino> destinos)
        {
            Titulo = titulo;
            DataInicio = dataInicio;
            CapacidadeMaxima = capacidadeMaxima;
            Preco = preco;
            Destinos = destinos;
        }
    }
}