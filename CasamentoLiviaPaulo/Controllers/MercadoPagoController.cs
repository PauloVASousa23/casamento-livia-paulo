using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MercadoPago;
using MercadoPago.Resources;
using MercadoPago.DataStructures.Preference;
using MercadoPago.DataStructures.PaymentMethod;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CasamentoLiviaPaulo.Controllers
{

    public class MercadoPagoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Pagar()
        {

            Preference preference = new Preference();

            // Cria um item na preferência
            preference.Items.Add(
              new MercadoPago.DataStructures.Preference.Item()
              {
                  Id = "123456",
                  Title = "Meu produto",
                  Quantity = 1,
                  CurrencyId = MercadoPago.Common.CurrencyId.BRL,
                  UnitPrice = (decimal)75.56
              }
            );

            preference.Payer = new MercadoPago.DataStructures.Preference.Payer()
            {
                Email = "brando_armstrong@yahoo.com"
            };

            preference.Save();

            ViewData["preferences"] = preference.Id;

            return View(preference);
        }
    }
}
