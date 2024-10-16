using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web.Services.Description;
using FormulárioPDFConverter.Model.Models;

namespace FormulárioPDFConverter.Business
{
    public class EmailServiceEmpresasBusiness
    {
        private readonly SmtpClient _smtpClient;

        public EmailServiceEmpresasBusiness()
        {
            // Desabilitar a validação do certificado SSL
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            _smtpClient = new SmtpClient("smtp.anfir.org.br", 587)
            {
                Credentials = new NetworkCredential("webmaster@anfir.org.br", "anfir306#"),
                EnableSsl = true
            };
        }

        public string EnviarEmailEmpresasRevisao(string idEmpresa, string mensagemCorpo, string mensagemObservacao, string fileName)
        {
            var dbOperacoes = new OperacoesBusiness();
            var dadosEmpresa = dbOperacoes.VerificarCadastroPorId(idEmpresa);

            try
            {
                return Enviar(dadosEmpresa.CNPJ, dadosEmpresa.Razao, dadosEmpresa.Email, idEmpresa, mensagemCorpo, mensagemObservacao, fileName);
            }
            catch (Exception ex)
            {
                LogErro(ex);
            }

            return string.Empty;
        }

        private string Enviar(string cnpj, string nomeEmpresa, string Email, string idEmpresa, string mensagemCorpo, string mensagemObservacao, string fileName)
        {
            try
            {
                var dbOperacoes = new OperacoesBusiness();
                var getEmailEmpresa = dbOperacoes.VerificarCadastroPorId(idEmpresa);

                var emailsArray = getEmailEmpresa.Email.Split(';')
                                        .Select(email => email.Trim())
                                        .Where(email => !string.IsNullOrEmpty(email))
                                        .ToList();

                string[] copiaEmails = { "vinicius@anfir.org.br", "marcio@anfir.org.br", "christian.hiraya@anfir.org.br", "rodrigo@anfir.org.br" };

                string assunto = "Revisão de Documentos - ANFIR";
                string contexto = MontarMensagem(cnpj, nomeEmpresa, mensagemCorpo, mensagemObservacao, fileName);

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("webmaster@anfir.org.br"),
                    Subject = assunto,
                    Body = contexto,
                    IsBodyHtml = true
                };

                foreach (var destinatario in emailsArray)
                {
                    mailMessage.To.Add(destinatario);
                }

                foreach (var ccEmail in copiaEmails)
                {
                    mailMessage.CC.Add(ccEmail);
                }

                _smtpClient.Send(mailMessage);

                return "E-mail enviado com sucesso!";
            }
            catch (Exception ex)
            {
                LogErro(ex);
                return $"Erro ao enviar e-mail: {ex.Message}";
            }
        }

        private string MontarMensagem(string cnpj, string nomeEmpresa, string mensagemCorpo, string mensagemObservacao, string fileName)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"<p>Prezado responsável pela empresa <b>{nomeEmpresa}</b>, CNPJ: <strong>{cnpj}</strong>.</p>");
            sb.AppendLine("<p>Informamos que o documento enviado não está em conformidade para a inclusão no Sistema do Credencia/GOV.</p>");
            sb.AppendLine($"<p><b>Documento:</b> {fileName}</p>");

            if (!string.IsNullOrEmpty(mensagemCorpo))
            {
                sb.AppendLine($"<p><b>Motivo:</b> {mensagemCorpo}</p>");
            }

            if (!string.IsNullOrEmpty(mensagemObservacao))
            {
                var formattedObservacao = mensagemObservacao.Replace("\n", "<br/>");
                sb.AppendLine($"<p><b>Obs.:</b> {formattedObservacao}</p>");
            }

            sb.AppendLine("<br/>");
            sb.AppendLine("<p><b>Atenciosamente,</b></p>");
            sb.AppendLine("<p><b>Sistema - Gestão de Documentos.</b></p>");

            return sb.ToString();
        }

        private void LogErro(Exception ex)
        {
            string caminhoArquivo = @"C:\inetpub\wwwroot\FormularioPDFConverter\Erro_Envio_EmailEmpresas.txt";
            using (StreamWriter writer = new StreamWriter(caminhoArquivo, true))
            {
                writer.WriteLine($"Data: {DateTime.Now}");
                writer.WriteLine($"Mensagem de Erro Envio de Email Para as Empresas: {ex.Message}");
                writer.WriteLine($"StackTrace: {ex.StackTrace}");
                writer.WriteLine(new string('-', 50));
            }
        }

    }
}