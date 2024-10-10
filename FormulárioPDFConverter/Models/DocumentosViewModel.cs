using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FormulárioPDFConverter.Models
{
    public class DocumentosViewModel
    {
        public CadastroViewModel DadosCadastro { get; set; }
        public List<UploadFilesViewModel> Documentos { get; set; }
        public List<string> TiposDocumentos { get; set; }
    }
}