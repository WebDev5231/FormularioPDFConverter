using FormulárioPDFConverter.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FormulárioPDFConverter.Model;
using FormulárioPDFConverter.Model.Models;
using FormulárioPDFConverter.Business;
using System;

namespace FormulárioPDFConverter.Controllers
{
    public class UploadFileController : Controller
    {
        private readonly MD5ServiceBusiness _md5ServiceBusiness;
        private readonly OperacoesBusiness _operacoesBusiness;
        private readonly EmailServiceRepresentantesBusiness _emailServiceBusiness;

        public UploadFileController()
        {
            _md5ServiceBusiness = new MD5ServiceBusiness();
            _operacoesBusiness = new OperacoesBusiness();
            _emailServiceBusiness = new EmailServiceRepresentantesBusiness();
        }

        // POST: UploadFile/uploadFile
        [HttpGet]
        public ActionResult UploadFile(string ID_Empresa)
        {
            if (string.IsNullOrEmpty(ID_Empresa))
            {
                return new HttpStatusCodeResult(400, "ID_Empresa inválido");
            }

            Session["ID_Empresa"] = ID_Empresa;

            var palavraChave = "ANFIR306";
            var combinacao = ID_Empresa + palavraChave;
            var hash = _md5ServiceBusiness.GenerateMD5(combinacao);
            Session["hash"] = hash;

            return RedirectToAction("DisplayHash");
        }

        // GET: UploadFile/displayHash
        public ActionResult DisplayHash()
        {
            var hash = Session["hash"] as string;
            var ID_Empresa = Session["ID_Empresa"] as string;

            if (string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(ID_Empresa))
            {
                return new HttpStatusCodeResult(400, "ID_Empresa ou Hash é necessário");
            }

            var dados = TempData["dados"] as Cadastro ?? _operacoesBusiness.VerificarCadastroPorId(ID_Empresa);
            TempData["dados"] = dados;

            ViewBag.MD5Hash = hash;

            var tiposDocumentos = new List<string>
            {
                "Ficha-de-Inscricao-",
                "Contrato-Social-",
                "Cartao-CNPJ-",
                "Procuracao-"
            };

            var documentosData = _operacoesBusiness.GetDocumentosPorId(ID_Empresa);
            var documentos = documentosData.Select(MapToUploadFiles).ToList();

            var dadosCompletos = new DocumentosViewModel
            {
                DadosCadastro = MapToCadastro(dados),
                Documentos = documentos,
                TiposDocumentos = tiposDocumentos
            };

            if (documentos.Count >= 4)
            {
                _emailServiceBusiness.EnviarEmail(dados.CNPJ, dados.Razao, ID_Empresa);
            }

            return View("~/Views/Home/uploadFile.cshtml", dadosCompletos);
        }

        // Map para UploadFilesViewModel
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

        // Map para CadastroViewModel
        private CadastroViewModel MapToCadastro(Cadastro dados)
        {
            return new CadastroViewModel
            {
                CNPJ = dados.CNPJ,
                Razao = dados.Razao,
                ID_Empresa = dados.ID_Empresa,
            };
        }

        // POST: Revisão / Envio de E-mail notificação
        [HttpPost]
        public ActionResult EnviarRevisao(string ID_Empresa, string fileName, string predefinedMessage, string customMessage)
        {
            string mensagem = !string.IsNullOrEmpty(predefinedMessage) ? predefinedMessage : customMessage;

            var envioEmailEmpresas = new EmailServiceEmpresasBusiness();
            string resultado = envioEmailEmpresas.EnviarEmailEmpresasRevisao(ID_Empresa, mensagem, fileName);

            if (!string.IsNullOrEmpty(resultado))
            {
                TempData["AlertMessage"] = resultado;
            }

            return View("~/Views/Home/uploadFile.cshtml");
        }

    }
}
