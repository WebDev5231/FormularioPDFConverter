using Dapper;
using System;
using System.Data.SqlClient;
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
                string destinatario = "responsavel@empresa.com";
                string assunto = "Envio Completo dos Documentos";
                string mensagem = MontarMensagem(cnpj, nomeEmpresa);

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("webmaster@anfir.org.br"),
                    Subject = assunto,
                    Body = mensagem,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(destinatario);

                _smtpClient.Send(mailMessage);

                var atualizaEmailEnviado = new dbQuery();
                atualizaEmailEnviado.UpdateEmailEnviado(idEmpresa);

                return "E-mail enviado com sucesso!";
            }
            catch (Exception ex)
            {
                return $"Erro ao enviar e-mail: {ex.Message}";
            }
        }

        private string MontarMensagem(string cnpj, string nomeEmpresa)
        {
            return $@"
                <h3>Prezado responsável,</h3>
                <p>Informamos que a empresa <strong>{nomeEmpresa}</strong> com o CNPJ <strong>{cnpj}</strong> concluiu o envio de todos os documentos requeridos.</p>
                <p>Por favor, verifique no sistema de gestão de documentos.</p>
                <br/>
                <p>Atenciosamente,</p>
                <p>Sistema de Gestão de Documentos</p>";
        }
    }
}
