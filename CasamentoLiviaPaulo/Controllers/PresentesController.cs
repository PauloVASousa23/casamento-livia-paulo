using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasamentoLiviaPaulo.Controllers
{
    public class PresentesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
