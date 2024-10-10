using Dapper;
using FormulárioPDFConverter.Model;
using FormulárioPDFConverter.Model.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace FormulárioPDFConverter.Data
{
    public class dbQueryData
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["formPDFConverter"].ConnectionString;

        public bool VerificarEmailJaEnviado(string idEmpresa)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "SELECT EmailEnviado FROM UploadFiles WHERE ID_Empresa = @ID_Empresa";

                int emailEnviado = connection.ExecuteScalar<int>(query, new { ID_Empresa = idEmpresa });

                return emailEnviado == 1;
            }
        }

        public bool UpdateEmailEnviado(string idEmpresa)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE UploadFiles SET EmailEnviado = 1 WHERE ID_Empresa = @ID_Empresa";

                int rowsAffected = connection.Execute(query, new { ID_Empresa = idEmpresa });

                return rowsAffected > 0;
            }
        }

        public Cadastro GetCadastroById(string idEmpresa)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM cadastro WHERE ID_Empresa = @ID_Empresa";
                return connection.QueryFirstOrDefault<Cadastro>(sql, new { ID_Empresa = idEmpresa });
            }
        }

        public List<UploadFiles> GetDocumentosById(string idEmpresa)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM UploadFiles WHERE ID_Empresa = @ID_Empresa";
                return connection.Query<UploadFiles>(sql, new { ID_Empresa = idEmpresa }).ToList();
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

        public bool InsertFilesLogs(UploadFiles uploadFileData)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO UploadFiles (CNPJ, ID_Empresa, NomeArquivo, DataInclusao, EmailEnviado)
                       VALUES (@CNPJ, @ID_Empresa, @NomeArquivo, @DataInclusao, @EmailEnviado)";

                connection.Execute(sql, uploadFileData);
            }
            return true;
        }

        public string GetPorteEmpresa(int porteempresa)
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
    }
}
