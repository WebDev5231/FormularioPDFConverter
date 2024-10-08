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
using FormulárioPDFConverter.Data;
using FormulárioPDFConverter.Data.Models;

namespace FormulárioPDFConverter.Controllers
{
    public class GenerateMD5Controller : Controller
    {

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
                return new HttpStatusCodeResult(400, "ID_Empresa ou Hash é necessário");
            }

            var dados = TempData["dados"] as FormulárioPDFConverter.Data.Models.CadastroData;

            var getValue = new dbQuery();
            if (dados == null)
            {
                dados = getValue.GetCadastroById(ID_Empresa);
            }

            ViewBag.MD5Hash = hash;

            var tiposDocumentos = new List<string>
            {
                "Ficha-de-Inscricao-",
                "Contrato-Social-",
                "Cartao-CNPJ-",
                "Procuracao-"
            };

            var documentosData = getValue.GetDocumentosById(ID_Empresa);

            // Mapeia os documentos usando o método manual
            var documentos = documentosData.Select(MapToUploadFiles).ToList();

            var dadosCompletos = new DocumentosViewModel
            {
                DadosCadastro = MapToCadastro(dados), // Mapeia os dados de CadastroData para Cadastro
                Documentos = documentos,
                TiposDocumentos = tiposDocumentos
            };

            if (documentos.Count >= 4)
            {
                var sendMail = new EmailService();
                sendMail.EnviarEmail(dados.CNPJ, dados.Razao, ID_Empresa);
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

        private UploadFiles MapToUploadFiles(UploadFilesData data)
        {
            return new UploadFiles
            {
                ID = data.ID,
                NomeArquivo = data.NomeArquivo,
                ID_Empresa = data.ID_Empresa,
                DataInclusao = data.DataInclusao,
                EmailEnviado = data.EmailEnviado,

            };
        }

        private Cadastro MapToCadastro(CadastroData data)
        {
            return new Cadastro
            {
                CNPJ = data.CNPJ,
                Razao = data.Razao,
                ID_Empresa = data.ID_Empresa,
            };
        }
    }

}
