using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulárioPDFConverter.Data.Models
{
    public class DocumentosData
    {
        public string CNPJ { get; set; }
        public string Nome { get; set; }
        public DateTime? DataInclusao { get; set; }
        public bool DocumentosPendentes { get; set; }
    }
}
