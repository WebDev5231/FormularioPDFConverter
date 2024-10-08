using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using FormulárioPDFConverter.Data;

namespace FormulárioPDFConverter
{
    public class EmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService()
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
            var dbQuery = new dbQuery();
            bool verificarEmailJaEnviado = dbQuery.VerificarEmailJaEnviado(idEmpresa);

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

                var atualizaEmailEnviado = new dbQuery();
                atualizaEmailEnviado.UpdateEmailEnviado(idEmpresa);

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
            return $@"
                <h3>Prezados responsáveis,</h3>
                <p>Informamos que a empresa <strong>{nomeEmpresa}</strong>, CNPJ <strong>{cnpj}</strong>, concluiu o envio dos documentos requeridos.</p>
                <p>Por favor, verifique no sistema de gestão de documentos.</p>
                <br/>
                <p><b>Atenciosamente,</b></p>
                <p><b>Sistema - Gestão de Documentos.</b></p>";
        }

        private void LogErro(Exception ex)
        {
            string caminhoArquivo = @"C:\inetpub\wwwroot\FormularioPDFConverter\erro_envio_email.txt";
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
