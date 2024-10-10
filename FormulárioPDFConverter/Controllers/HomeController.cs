using FormulárioPDFConverter.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using Dapper;
using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using FormulárioPDFConverter.Data;
using FormulárioPDFConverter.Model;

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

            var queryOperacoes = new dbQueryData();

            var cadastro = queryOperacoes.GetCadastroById(idEmpresa);

            var cidade = queryOperacoes.GetMunicipioById(cadastro.ID_Cidade);
            cadastro.Cidade = cidade.munMUNICIP;
            cadastro.Estado = cidade.munEST;

            cadastro.PortesEmpresa = new List<SelectListItem>
            {
            new SelectListItem { Text = "Micro", Value = "micro" },
            new SelectListItem { Text = "Pequena", Value = "pequena" },
            new SelectListItem { Text = "Média", Value = "media" },
            new SelectListItem { Text = "Grande", Value = "grande" },
            new SelectListItem {Text = "", Value="" }
            };
            cadastro.PorteEmpresaSelecionado = queryOperacoes.GetPorteEmpresa(cadastro.porteempresa);

            cadastro.dataDeIngresso = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("pt-BR"));

            return View(cadastro);
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

        public ActionResult uploadFile(string ID_Empresa)
        {
            if (Session["ID_Empresa"] == null || Session["ID_Empresa"].ToString() != ID_Empresa)
            {
                Session["ID_Empresa"] = ID_Empresa;
            }

            var queryOperacoes = new dbQueryData();

            var dados = queryOperacoes.GetCadastroById(ID_Empresa);
            TempData["dados"] = dados;

            return RedirectToAction("uploadFile", "GenerateMD5", new { ID_Empresa = ID_Empresa });
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

                    var queryOperacoes = new dbQueryData();
                    queryOperacoes.InsertFilesLogs(uploadFile);

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