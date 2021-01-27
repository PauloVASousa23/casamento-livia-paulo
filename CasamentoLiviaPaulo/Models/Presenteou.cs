using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasamentoLiviaPaulo.Models
{
    public class Presenteou
    {
        public int Id { get; set; }
        public string Pessoa { get; set; }
        public DateTime Data { get; set; }
        public string Mensagem { get; set; }
        public int Presente_Id { get; set; }
    }
}
