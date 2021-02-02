﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MercadoPago.Resources;
using CasamentoLiviaPaulo.Repository;
using System.Net;
using System.IO;
using System.Drawing;
using FluentFTP;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text;
using CasamentoLiviaPaulo.Models;

namespace CasamentoLiviaPaulo.Controllers
{
    public class PresentesController : Controller
    {
        public byte[] GetImgByte(string ftpFilePath)
        {
            WebClient ftpClient = new WebClient();
            ftpClient.Credentials = new NetworkCredential("liviaepaulo", "P@ulo2018");

            byte[] imageByte = ftpClient.DownloadData("ftp://ftp.liviaepaulo.com/imagens/presente/" + ftpFilePath);
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

        public IActionResult Index(int pagina = 0)
        {
            //var bt = GetImgByte("ftp://ftp.liviaepaulo.com/imagens/presente/panelas.jpg");
            //var img = ByteToImage(bt);
            //var convert = Convert.ToBase64String(bt);
            //ViewData["Imagem"] = convert;
            PresenteRepository model = HttpContext.RequestServices.GetService(typeof(PresenteRepository)) as PresenteRepository;
            ImagensRepository modelImagens = HttpContext.RequestServices.GetService(typeof(ImagensRepository)) as ImagensRepository;
            List<Presente> presentes = model.GetPresentes(pagina);
            foreach (Presente p in presentes)
            {
                p.imagens = modelImagens.GetImagens(p.Timestamp);
                if (p.imagens.Count > 0)
                {
                    p.base64 = new List<string>();
                }

                foreach(Imagens i in p.imagens)
                {
                    var bytes = GetImgByte(i.Caminho);
                    p.base64.Add(Convert.ToBase64String(bytes));
                }
            }

            ViewData["PaginaAtual"] = pagina;

            return View(presentes);
        }
        public IActionResult Cadastrar()
        {
            ViewData["Timestamp"] = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            return View();
        }

        public string timestampPresente;

        [HttpPost]
        public IActionResult Salvar(string nome, string descricao, string preco, int quantidade, string timestamp)
        {

            Presente p = new Presente();
            p.Nome = nome;
            p.Descricao = descricao;
            p.Preco = float.Parse(preco);
            p.Quantidade = quantidade;
            p.Timestamp = timestamp;

            PresenteRepository model = HttpContext.RequestServices.GetService(typeof(PresenteRepository)) as PresenteRepository;

            ViewData["cadastrou"] = model.CadastrarPresente(p);

            return View("Cadastrar");
        }

        [Route("Presentes/detalhe/{id}")]
        public IActionResult Detalhe(int id)
        {
            PresenteRepository model = HttpContext.RequestServices.GetService(typeof(PresenteRepository)) as PresenteRepository;

            Presente p = model.GetPresenteId(id);

            Preference preference = new Preference();

            // Cria um item na preferência
            preference.Items.Add(
              new MercadoPago.DataStructures.Preference.Item()
              {
                  Id = p.Id.ToString(),
                  Title = p.Nome,
                  Quantity = p.Quantidade,
                  CurrencyId = MercadoPago.Common.CurrencyId.BRL,
                  UnitPrice = (decimal)p.Preco
              }
            );

            //preference.Payer = new MercadoPago.DataStructures.Preference.Payer()
            //{
            //    Email = "paulinho_vtr@live.com"
            //};

            preference.BackUrls = new MercadoPago.DataStructures.Preference.BackUrls()
            {
                Success = "https://liviaepaulo.com/Presentes/sucesso",
                Pending = "",
                Failure = ""
            };

            preference.Save();

            ViewData["preferences"] = preference.Id;
            return View(p);
        }
        public IActionResult Sucesso()
        {
            return View();
        }

        public List<string> GetImagens()
        {
            var i = GetImgByte("panelas.jpg");
            List<string> imagens = new List<string>();
            imagens.Add(Convert.ToBase64String(i));

            return imagens;

        }

        [HttpPost]
        public async Task<List<ImagemParcial>> EnviarImagem(string timestampPresente)
        {

            var uploaded_files = Request.Form.Files;

            List<ImagemParcial> imgs = new List<ImagemParcial>();
            
            ImagensRepository model = HttpContext.RequestServices.GetService(typeof(ImagensRepository)) as ImagensRepository;

            foreach (IFormFile file in uploaded_files)
            {
                ImagemParcial i = new ImagemParcial();

                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");

                var extensao = file.FileName.Split('.')[1]; 
                
                i.timestamp = timestamp + "." + extensao;

                var request = (FtpWebRequest)WebRequest.Create("ftp://ftp.liviaepaulo.com/imagens/presente/"+ timestamp + "." + extensao);

                request.Method = WebRequestMethods.Ftp.UploadFile;


                request.Credentials = new NetworkCredential("liviaepaulo", "P@ulo2018");

                byte[] buffer = new byte[1024];
                var stream = file.OpenReadStream();
                byte[] fileContents;


                using (var ms = new MemoryStream())
                {
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    fileContents = ms.ToArray();
                    i.arquivo = Convert.ToBase64String(fileContents);
                    imgs.Add(i);
                }

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents, 0, fileContents.Length);
                }

                var response = (FtpWebResponse)request.GetResponse();

                Imagens img = new Imagens();
                img.Caminho = timestamp + "." + extensao;
                img.TimestampPresente = timestampPresente;

                model.CadastrarImagem(img);

            }

            return imgs;
        }

        public bool DeletarImagem(string caminho)
        {
            ImagensRepository model = HttpContext.RequestServices.GetService(typeof(ImagensRepository)) as ImagensRepository;
            
            return model.DeletarImagem(caminho);
        }

    }

}
