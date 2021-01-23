using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MercadoPago.Resources;

namespace CasamentoLiviaPaulo.Controllers
{
    public class PresentesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Detalhe()
        {
            Preference preference = new Preference();

            // Cria um item na preferência
            preference.Items.Add(
              new MercadoPago.DataStructures.Preference.Item()
              {
                  Id = "123456",
                  Title = "Panela",
                  Quantity = 1,
                  CurrencyId = MercadoPago.Common.CurrencyId.BRL,
                  UnitPrice = (decimal)75.56
              }
            );

            preference.Payer = new MercadoPago.DataStructures.Preference.Payer()
            {
                Email = "brando_armstrong@yahoo.com"
            };

            preference.BackUrls = new MercadoPago.DataStructures.Preference.BackUrls()
            {
                Success = "https://liviaepaulo.com/Presentes/sucesso",
                Pending = "",
                Failure = ""
            };

            preference.Save();

            ViewData["preferences"] = preference.Id;
            return View(preference);
        }
        public IActionResult Sucesso()
        {
            return View();
        }
    }
}
