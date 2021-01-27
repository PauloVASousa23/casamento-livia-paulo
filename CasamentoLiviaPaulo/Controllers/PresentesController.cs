using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MercadoPago.Resources;
using CasamentoLiviaPaulo.Repository;
using System.Net;
using System.IO;
using System.Drawing;

namespace CasamentoLiviaPaulo.Controllers
{
    public class PresentesController : Controller
    {
        public byte[] GetImgByte(string ftpFilePath)
        {
            WebClient ftpClient = new WebClient();
            ftpClient.Credentials = new NetworkCredential("liviaepaulo", "P@ulo2018");

            byte[] imageByte = ftpClient.DownloadData(ftpFilePath);
            return imageByte;
        }

        public static Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;
        }

        public IActionResult Index()
        {
            var bt = GetImgByte("ftp://ftp.liviaepaulo.com/imagens/presente/img.webp");
            var img = ByteToImage(bt);
            var convert = Convert.ToBase64String(bt);
            PresenteRepository model = HttpContext.RequestServices.GetService(typeof(PresenteRepository)) as PresenteRepository;
            ViewData["Imagem"] = convert;
            return View(model.GetPresentes());
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
