using FormulárioPDFConverter.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using Dapper;
using System;
using System.Collections.Generic;

namespace FormulárioPDFConverter.Controllers
{
    public class HomeController : Controller
    {
        private string connectionString;
        public HomeController()
        {
            connectionString = ConfigurationManager.ConnectionStrings["formPDFConverter"].ConnectionString;
        }

        public ActionResult Index()
        {
            var ID_Empresa = "10";
            var cadastro = GetCadastroById(ID_Empresa);

            var cidade = GetMunicipioById(cadastro.ID_Cidade);
            cadastro.Cidade = cidade.munMUNICIP;
            cadastro.Estado = cidade.munEST;

            cadastro.Categorias = new List<SelectListItem>
            {
            new SelectListItem { Text = "FABRICANTE", Value = "FABRICANTE" },
            new SelectListItem { Text = "SISTEMISTA", Value = "SISTEMISTA" },
            new SelectListItem { Text = "COMPONENTES", Value = "COMPONENTES" },
            new SelectListItem { Text = "", Value = "" }
            };
            cadastro.CategoriaSelecionada = GetCategoriaById(cadastro.id_cat_assoc);


            cadastro.PortesEmpresa = new List<SelectListItem>
            {
            new SelectListItem { Text = "Micro", Value = "micro" },
            new SelectListItem { Text = "Pequena", Value = "pequena" },
            new SelectListItem { Text = "Média", Value = "media" },
            new SelectListItem { Text = "Grande", Value = "grande" },
            new SelectListItem {Text = "", Value="" }
            };
            cadastro.PorteEmpresaSelecionado = GetPorteEmpresa(cadastro.porteempresa);

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

        private string GetCategoriaById(int categoria)
        {
            switch (categoria)
            {
                case 1: return "FABRICANTE";
                case 2: return "SISTEMISTA";
                case 3: return "COMPONENTES";
                default: return "";
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Cadastro model)
        {
            try
            {
                var report = new Rotativa.ActionAsPdf("Ficha", model);

                return report;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Content("Erro ao gerar o arquivo PDF");
            }
        }

    }
}