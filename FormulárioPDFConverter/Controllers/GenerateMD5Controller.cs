using FormulárioPDFConverter.Models;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using FormulárioPDFConverter.Data;
using FormulárioPDFConverter.Model;
using FormulárioPDFConverter.Model.Models;
using FormulárioPDFConverter.Business;

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

            var dados = TempData["dados"] as FormulárioPDFConverter.Model.Models.Cadastro;

            var getCadastro = new OperacoesBusiness();
            if (dados == null)
            {
                dados = getCadastro.VerificarCadastroPorId(ID_Empresa);
            }

            ViewBag.MD5Hash = hash;

            var tiposDocumentos = new List<string>
            {
                "Ficha-de-Inscricao-",
                "Contrato-Social-",
                "Cartao-CNPJ-",
                "Procuracao-"
            };

            var documentosData = getCadastro.GetDocumentosPorId(ID_Empresa);

            var documentos = documentosData.Select(MapToUploadFiles).ToList();

            var dadosCompletos = new DocumentosViewModel
            {
                DadosCadastro = MapToCadastro(dados),
                Documentos = documentos,
                TiposDocumentos = tiposDocumentos
            };

            if (documentos.Count >= 4)
            {
                var sendMail = new EmailServiceBusiness();
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

        private UploadFilesViewModel MapToUploadFiles(UploadFiles dados)
        {
            return new UploadFilesViewModel
            {
                ID = dados.ID,
                NomeArquivo = dados.NomeArquivo,
                ID_Empresa = dados.ID_Empresa,
                DataInclusao = dados.DataInclusao,
                EmailEnviado = dados.EmailEnviado,
            };
        }

        private CadastroViewModel MapToCadastro(Cadastro dados)
        {
            return new CadastroViewModel
            {
                CNPJ = dados.CNPJ,
                Razao = dados.Razao,
                ID_Empresa = dados.ID_Empresa,
            };
        }
    }

}
