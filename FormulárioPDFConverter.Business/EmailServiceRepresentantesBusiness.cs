using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

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
                LogErro(ex);
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

        private void LogErro(Exception ex)
        {
            string caminhoArquivo = @"C:\inetpub\wwwroot\FormularioPDFConverter\Erro_Envio_EmailRepresentantes.txt";
            using (StreamWriter writer = new StreamWriter(caminhoArquivo, true))
            {
                writer.WriteLine($"Data: {DateTime.Now}");
                writer.WriteLine($"Mensagem de Erro: {ex.Message}");
                writer.WriteLine($"StackTrace: {ex.StackTrace}");
                writer.WriteLine(new string('-', 50));
            }
        }
    }
}