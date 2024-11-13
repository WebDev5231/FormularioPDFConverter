using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Services.Description;

namespace FormulárioPDFConverter.Business
{
    public class EmailServiceRepresentantesBusiness
    {
        private readonly SmtpClient _smtpClient;

        public EmailServiceRepresentantesBusiness()
        {
            // Desabilitar a validação do certificado SSL
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            _smtpClient = new SmtpClient("smtp.anfir.org.br", 587)
            {
                Credentials = new NetworkCredential("webmaster@anfir.org.br", "anfir306#"),
                EnableSsl = true
            };
        }

        public string EnviarEmail(string cnpj, string nomeEmpresa, string idEmpresa)
        {
            var dbOperacoes = new OperacoesBusiness();
            bool verificarEmailJaEnviado = dbOperacoes.VerificarEnvioDeEmail(idEmpresa);

            if (!verificarEmailJaEnviado)
            {
                return Enviar(cnpj, nomeEmpresa, idEmpresa);
            }

            return string.Empty;
        }

        private string Enviar(string cnpj, string nomeEmpresa, string idEmpresa)
        {
            try
            {
                string[] destinatarios = { "vinicius@anfir.org.br", "marcio@anfir.org.br", "christian.hiraya@anfir.org.br", "rodrigo@anfir.org.br" };
                string assunto = "Envio Completo dos Documentos";
                string mensagem = MontarMensagem(cnpj, nomeEmpresa);

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("webmaster@anfir.org.br"),
                    Subject = assunto,
                    Body = mensagem,
                    IsBodyHtml = true
                };

                foreach (var destinatario in destinatarios)
                {
                    mailMessage.To.Add(destinatario);
                }

                _smtpClient.Send(mailMessage);

                var atualizaEmailEnviado = new OperacoesBusiness();
                atualizaEmailEnviado.AtualizarEmailEnviado(idEmpresa);

                return "E-mail enviado com sucesso!";
            }
            catch (Exception ex)
            {
                LogErro(ex, cnpj, nomeEmpresa);
                return $"Erro ao enviar e-mail: {ex.Message}";
            }
        }

        private string MontarMensagem(string cnpj, string nomeEmpresa)
        {
            var sb = new StringBuilder();

            sb.Append("<h3>Prezados responsáveis,</h3>");
            sb.Append("<p>Informamos que a empresa <strong>");
            sb.Append(nomeEmpresa);
            sb.Append("</strong>, CNPJ <strong>");
            sb.Append(cnpj);
            sb.Append("</strong>, concluiu o envio dos documentos requeridos.</p>");
            sb.Append("<p>Por favor, verifique no sistema de gestão de documentos.</p>");
            sb.Append("<br/>");
            sb.Append("<p><b>Atenciosamente,</b></p>");
            sb.Append("<p><b>Sistema - Gestão de Documentos.</b></p>");

            return sb.ToString();
        }

        private void LogErro(Exception ex, string cnpj, string nomeEmpresa)
        {
            StringBuilder logContent = new StringBuilder();
            logContent.AppendLine($"[Data] {DateTime.Now}");
            logContent.AppendLine($"[CNPJ] {cnpj}");
            logContent.AppendLine($"[Empresa] {nomeEmpresa}");
            logContent.AppendLine($"[Mensagem de Erro] {ex.Message}");
            logContent.AppendLine($"[StackTrace] {ex.StackTrace}");

            if (ex.InnerException != null)
            {
                logContent.AppendLine($"[InnerException] {ex.InnerException.Message}");
                logContent.AppendLine($"[InnerStackTrace] {ex.InnerException.StackTrace}");
            }

            logContent.AppendLine(new string('-', 50));

            // Escrever no arquivo
            string caminhoArquivo = @"C:\inetpub\wwwroot\FormularioPDFConverter\Erro_Envio_EmailRepresentantes.txt";
            using (StreamWriter writer = new StreamWriter(caminhoArquivo, true))
            {
                writer.WriteLine(logContent.ToString());
            }
        }
    }
}
