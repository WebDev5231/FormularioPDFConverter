using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulárioPDFConverter.Data.Models
{
    [Table("UploadFiles")]
    public class UploadFilesData
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

        [Required]
        public bool EmailEnviado { get; set; }
    }
}
