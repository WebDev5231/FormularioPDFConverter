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

namespace FormulárioPDFConverter.Controllers
{
    public class HomeController : Controller
    {
        private string connectionString;
        public HomeController()
        {
            connectionString = ConfigurationManager.ConnectionStrings["formPDFConverter"].ConnectionString;
        }

        public ActionResult FichaIncricao()
        {
            var ID_Empresa = Session["ID_Empresa"]?.ToString();

            if (string.IsNullOrEmpty(ID_Empresa))
            {
                return new HttpStatusCodeResult(400, "access denied");
            }

            var cadastro = GetCadastroById(ID_Empresa);

            var cidade = GetMunicipioById(cadastro.ID_Cidade);
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
            cadastro.PorteEmpresaSelecionado = GetPorteEmpresa(cadastro.porteempresa);

            cadastro.dataDeIngresso = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("pt-BR"));

            return View(cadastro);
        }

        public ActionResult Ficha(Cadastro model)
        {
            return View(model);
        }

        public Cadastro GetCadastroById(string ID_Empresa)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM cadastro Where ID_Empresa = @ID_Empresa";
                return connection.QueryFirstOrDefault<Cadastro>(sql, new { ID_Empresa = ID_Empresa });
            }
        }

        public Municipio GetMunicipioById(int ID_Cidade)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM municipios WHERE munCOD = @ID_Cidade";
                return connection.QueryFirstOrDefault<Municipio>(sql, new { ID_Cidade = ID_Cidade });
            }
        }

        public void InsertFilesLogs(UploadFiles uploadFile)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO UploadFiles (CNPJ, ID_Empresa, NomeArquivo, DataInclusao)
                       VALUES (@CNPJ, @ID_Empresa, @NomeArquivo, @DataInclusao)";

                connection.Execute(sql, uploadFile);
            }
        }

        private string GetPorteEmpresa(int porteempresa)
        {
            switch (porteempresa)
            {
                case 1: return "Micro";
                case 2: return "Pequena";
                case 3: return "Média";
                case 4: return "Grande";
                default: return "";
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FichaIncricao(Cadastro model)
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

            var dados = GetCadastroById(ID_Empresa);
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
                        DataInclusao = DataInclusao
                    };

                    InsertFilesLogs(uploadFile);

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