using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FormulárioPDFConverter.Model
{
    [Table("UploadFiles")]
    public class UploadFiles
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string CNPJ { get; set; }

        [Required]
        public string ID_Empresa { get; set; }

        [Required]
        public string NomeArquivo { get; set; }

        [Required]
        public DateTime DataInclusao { get; set; }

        public bool EmailEnviado { get; set; } = false;
    }
}