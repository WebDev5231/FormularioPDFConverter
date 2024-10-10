using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FormulárioPDFConverter.Model.Models
{
    [Table("municipios")]
    public class Municipio
    {
        public string munMUNICIP { get; set; }
        public string munEST { get; set; }
        public string munREGIAO { get; set; }
    }
}