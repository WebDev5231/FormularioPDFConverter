using Dapper;
using FormulárioPDFConverter.Data.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace FormulárioPDFConverter.Data
{
    public class dbQuery
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

        public void UpdateEmailEnviado(string idEmpresa)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE UploadFiles SET EmailEnviado = 1 WHERE ID_Empresa = @ID_Empresa";

                connection.Execute(query, new { ID_Empresa = idEmpresa });
            }
        }

        public CadastroData GetCadastroById(string idEmpresa)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM cadastro Where ID_Empresa = @ID_Empresa";
                return connection.QueryFirstOrDefault<CadastroData>(sql, new { ID_Empresa = idEmpresa });
            }
        }

        public List<UploadFilesData> GetDocumentosById(string idEmpresa)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM UploadFiles WHERE ID_Empresa = @ID_Empresa";
                return connection.Query<UploadFilesData>(sql, new { ID_Empresa = idEmpresa }).ToList();
            }
        }

        public MunicipioData GetMunicipioById(int ID_Cidade)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM municipios WHERE munCOD = @ID_Cidade";
                return connection.QueryFirstOrDefault<MunicipioData>(sql, new { ID_Cidade = ID_Cidade });
            }
        }

        public void InsertFilesLogs(UploadFilesData uploadFileData)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO UploadFiles (CNPJ, ID_Empresa, NomeArquivo, DataInclusao, EmailEnviado)
                       VALUES (@CNPJ, @ID_Empresa, @NomeArquivo, @DataInclusao, @EmailEnviado)";

                connection.Execute(sql, uploadFileData);
            }
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
