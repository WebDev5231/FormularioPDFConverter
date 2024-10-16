using FormulárioPDFConverter.Models;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using FormulárioPDFConverter.Model;
using FormulárioPDFConverter.Business;

namespace FormulárioPDFConverter.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult FichaIncricao()
        {
            var idEmpresa = (string)Session["ID_Empresa"]?.ToString();

            if (string.IsNullOrEmpty(idEmpresa))
            {
                return new HttpStatusCodeResult(400, "access denied");
            }

            var queryOperacoes = new OperacoesBusiness();

            var cadastro = queryOperacoes.VerificarCadastroPorId(idEmpresa);
            var cidade = queryOperacoes.VerificarMunicipioPorId(cadastro.ID_Cidade);
            cadastro.Cidade = cidade.munMUNICIP;
            cadastro.Estado = cidade.munEST;

            var model = new CadastroViewModel
            {
                ID_Empresa = cadastro.ID_Empresa,
                Razao = cadastro.Razao,
                CNPJ = cadastro.CNPJ,
                Cidade = cadastro.Cidade,
                Estado = cadastro.Estado,
                Endereco = cadastro.Endereco,
                Cep = cadastro.Cep,
                Bairro = cadastro.Bairro,
                Site = cadastro.Site,
                Email = cadastro.Email,
                DataFundacao = cadastro.DataFundacao,
                Telefone = cadastro.Telefone,
                InscricaoJuntaComercial = cadastro.InscricaoJuntaComercial,
                Complemento = cadastro.Complemento,
                PorteEmpresaSelecionado = queryOperacoes.BuscarPorteEmpresaPorId(cadastro.porteempresa),
                PortesEmpresa = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Micro", Value = "micro" },
                    new SelectListItem { Text = "Pequena", Value = "pequena" },
                    new SelectListItem { Text = "Média", Value = "media" },
                    new SelectListItem { Text = "Grande", Value = "grande" },
                    new SelectListItem { Text = "", Value = "" }
                },
                dataDeIngresso = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("pt-BR"))
            };

            return View(model);
        }

        public ActionResult Ficha(CadastroViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FichaIncricao(CadastroViewModel model)
        {
            try
            {
                model.dataDeIngresso = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("pt-BR"));

                var report = new Rotativa.ActionAsPdf("Ficha", model);

                return report;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Content("Erro ao gerar o arquivo PDF");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFile(HttpPostedFileBase fileUpload, string CNPJ, string ID_Empresa, string documentType)
        {
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                try
                {
                    DateTime DataInclusao = DateTime.Now;

                    string cnpjFormatado = Regex.Replace(CNPJ, "[^0-9]", "");
                    documentType = documentType + cnpjFormatado;

                    string path = Path.Combine(Server.MapPath("~/Uploads"), documentType + Path.GetExtension(fileUpload.FileName));

                    fileUpload.SaveAs(path);

                    var uploadFile = new UploadFiles
                    {
                        CNPJ = cnpjFormatado,
                        ID_Empresa = ID_Empresa,
                        NomeArquivo = documentType,
                        DataInclusao = DataInclusao,
                        EmailEnviado = false
                    };

                    var queryOperacoes = new OperacoesBusiness();
                    queryOperacoes.InsertDadosFilesLogs(uploadFile);

                    TempData["AlertMessage"] = "Documento enviado com sucesso!";
                    return RedirectToAction("UploadFile", "Home", new { ID_Empresa });
                }
                catch (Exception ex)
                {
                    TempData["AlertMessage"] = $"Erro ao fazer o upload do arquivo: {ex.Message}";
                    return RedirectToAction("UploadFile", "Home", new { ID_Empresa });
                }
            }
            TempData["AlertMessage"] = "Nenhum arquivo selecionado";
            return RedirectToAction("UploadFile", "Home", new { ID_Empresa });
        }
    }
}
