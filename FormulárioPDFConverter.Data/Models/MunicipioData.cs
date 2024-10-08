using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulárioPDFConverter.Data.Models
{
    [Table("municipios")]
    public class MunicipioData
    {
        public string munMUNICIP { get; set; }
        public string munEST { get; set; }
        public string munREGIAO { get; set; }
    }
}
