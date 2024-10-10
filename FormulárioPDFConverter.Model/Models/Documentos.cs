using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FormulárioPDFConverter.Model.Models
{
    public class Documentos
    {
        public string CNPJ { get; set; }
        public string Nome { get; set; }
        public DateTime? DataInclusao { get; set; }
        public bool DocumentosPendentes { get; set; }
    }
}