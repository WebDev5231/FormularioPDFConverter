using FormulárioPDFConverter.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FormulárioPDFConverter.Models
{
    public class DocumentosViewModel
    {
        public Cadastro DadosCadastro { get; set; }
        public List<UploadFiles> Documentos { get; set; }
        public List<string> TiposDocumentos { get; set; }
    }
}