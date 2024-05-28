using FormulárioPDFConverter.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace FormulárioPDFConverter.Controllers
{
    public class GenerateMD5Controller : Controller
    {
        private string connectionString;

        public GenerateMD5Controller()
        {
            connectionString = ConfigurationManager.ConnectionStrings["formPDFConverter"].ConnectionString;
        }

        // GET: GenerateMD5/uploadFile
        public ActionResult uploadFile(string ID_Empresa)
        {
            if (string.IsNullOrEmpty(ID_Empresa))
            {
                return new HttpStatusCodeResult(400, "ID_Empresa inválido");
            }

            Session["ID_Empresa"] = ID_Empresa;

            // Gera o hash
            var palavraChave = "ANFIR306";
            var combinacao = ID_Empresa + palavraChave;
            var hash = GenerateMD5(combinacao);
            Session["hash"] = hash;

            return RedirectToAction("displayHash");
        }

        public ActionResult displayHash()
        {
            var hash = Session["hash"] as string;
            var ID_Empresa = Session["ID_Empresa"] as string;

            if (string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(ID_Empresa))
            {
                return new HttpStatusCodeResult(400, "ID_Empresa ou Hash is required");
            }

            var dados = TempData["dados"] as Cadastro;

            if (dados == null)
            {
                dados = GetCadastroById(ID_Empresa);
            }

            ViewBag.MD5Hash = hash;

            var tiposDocumentos = new List<string>
            {
                "Ficha-de-Inscricao-",
                "Contrato-Social-",
                "Cartao-CNPJ-",
                "Procuracao-"
            };

            var documentos = GetDocumentosById(ID_Empresa);

            var dadosCompletos = new DocumentosViewModel
            {
                DadosCadastro = dados,
                Documentos = documentos,
                TiposDocumentos = tiposDocumentos
            };

            // Debugging output
            Console.WriteLine("ID_Empresa: " + ID_Empresa);
            Console.WriteLine("Documentos count: " + documentos.Count);
            foreach (var doc in documentos)
            {
                Console.WriteLine("Documento: " + doc.TipoDocumento);
            }

            return View("~/Views/Home/uploadFile.cshtml", dadosCompletos);
        }

        private string GenerateMD5(string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }

        private Cadastro GetCadastroById(string ID_Empresa)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM cadastro Where ID_Empresa = @ID_Empresa";
                return connection.QueryFirstOrDefault<Cadastro>(sql, new { ID_Empresa = ID_Empresa });
            }
        }

        public List<UploadFiles> GetDocumentosById(string ID_Empresa)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM UploadFiles WHERE ID_Empresa = @ID_Empresa";
                return connection.Query<UploadFiles>(sql, new { ID_Empresa = ID_Empresa }).ToList();
            }
        }

    }
}
