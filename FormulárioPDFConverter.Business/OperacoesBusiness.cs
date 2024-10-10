using FormulárioPDFConverter.Data;
using FormulárioPDFConverter.Model;
using FormulárioPDFConverter.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FormulárioPDFConverter.Business
{
    public class OperacoesBusiness
    {
        public bool VerificarEnvioDeEmail(string idEmpresa)
        {
            var dbQueryData = new dbQueryData();
            bool emailEnviado = dbQueryData.VerificarEmailJaEnviado(idEmpresa);

            return emailEnviado;
        }

        public bool AtualizarEmailEnviado(string idEmpresa)
        {
            var dbQueryData = new dbQueryData();
            bool emailAtualizado = dbQueryData.UpdateEmailEnviado(idEmpresa);

            return emailAtualizado;
        }

        public Cadastro VerificarCadastroPorId(string idEmpresa)
        {
            var dbQueryData = new dbQueryData();
            return dbQueryData.GetCadastroById(idEmpresa);
        }

        public List<UploadFiles> GetDocumentosPorId(string idEmpresa)
        {
            var dbQueryData = new dbQueryData();
            var getDocumentos = dbQueryData.GetDocumentosById(idEmpresa);

            return getDocumentos;
        }

        public Municipio VerificarMunicipioPorId(int IdCidade)
        {
            var dbQueryData = new dbQueryData();
            return dbQueryData.GetMunicipioById(IdCidade);
        }

        public string BuscarPorteEmpresaPorId(int porteempresa)
        {
            var dbQueryData = new dbQueryData();
            var getPorteEmpresa = dbQueryData.GetPorteEmpresa(porteempresa);

            return getPorteEmpresa;
        }

        public bool InsertDadosFilesLogs(UploadFiles uploadFileData)
        {
            var dbQueryData = new dbQueryData();
            dbQueryData.InsertFilesLogs(uploadFileData);

            return true;
        }
    }
}