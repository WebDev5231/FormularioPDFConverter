using System;

namespace FormulárioPDFConverter.Business.Utils
{
    public class UltilMessage
    {
        public static string CreateMessageObsAuto(string mensagemCorpo)
        {
            string mensagemObservacaoAuto;

            switch (mensagemCorpo)
            {
                case "Ficha de Inscrição não assinada e/ou não encontrada.":
                    mensagemObservacaoAuto = "Documento não assinado e/ou não encontrado.\r\n Solicitamos que a regularização seja efetuada em caráter de urgência. Reforçamos a importância de concluir o procedimento com prioridade, uma vez que o não cumprimento poderá resultar no bloqueio do acesso ao sistema por parte da Senatran, impedindo o acesso ao Pré-cadastro no sistema Renavam, a qualquer momento. \r\n";
                    break;

                case "Procuração não encontrada e/ou não assinada ou não reconhecida a firma da assinatura.":
                    mensagemObservacaoAuto = "Portaria Senatran 452 de 2024 - Art. 4º As pessoas jurídicas de direito privado poderão delegar o acesso ao pré-cadastro veicular a entidade representativa de seu ramo de atividade.\r\n § 2º A delegação prevista no caput se dará por procuração assinada pelo representante legal da empresa de direito privado, com reconhecimento de firma.\r\n Solicitamos que a regularização seja efetuada em caráter de urgência. Reforçamos a importância de concluir o procedimento com prioridade, uma vez que o não cumprimento poderá resultar no bloqueio do acesso ao sistema por parte da Senatran, impedindo o acesso ao Pré-cadastro no sistema Renavam, a qualquer momento. \r\n ";
                    break;

                case "Cartão CNPJ não encontrado e/ou CNAE não está em conformidade com o Inciso I do Art. 3º da Portaria Senatran Nº. 452 de 2024.":
                    mensagemObservacaoAuto = "Portaria Senatran 452 de 2024 - Art. 3º Os requerimentos de acesso ao pré-cadastro veicular por pessoas jurídicas de direito privado deverão ser encaminhados à Senatran, por meio do sistema credencia, pelo respectivo representante legal.\r\n § 1º São requisitos para deferimento do requerimento de pessoas jurídicas de direito privado: \r\n I - Ser fabricante, montador ou encarroçador de veículos, tendo como atividade econômica principal um dos códigos da Classificação Nacional de Atividades Econômicas - CNAE elencados no Anexo na Portaria.\r\n Os códigos permitidos para acesso ao pré-cadastro veicular por pessoas jurídicas de direito privado, conforme inciso I, do § 1º do art. 3º da Portaria, são: \r\n\r\n\r\n CNAE - DESCRIÇÃO \r\n 28.22-4/02 Fabricação de máquinas, equipamentos e aparelhos para transporte e elevação de cargas, peças e acessórios \r\n 29.10-7/01 Fabricação de automóveis, camionetas e utilitário \r\n 29.10-7/02 Fabricação de chassis com motor para automóveis, camionetas e utilitários \r\n 29.10-7/03 Fabricação de motores para automóveis, camionetas e utilitários \r\n 29.20-4/01 Fabricação de caminhões e ônibus \r\n 29.30-1/01 Fabricação de cabines, carrocerias e reboques para caminhões \r\n 29.30-1/02 Fabricação de carrocerias para ônibus \r\n 29.30-1/03 Fabricação de cabines, carrocerias e reboques para outros veículos automotores, exceto caminhões e ônibus \r\n 30.50-4/00 Fabricação de veículos militares de combate \r\n 30.91-1/01 Fabricação de motocicletas \r\n\r\n Solicitamos que a regularização seja efetuada em caráter de urgência. Reforçamos a importância de concluir o procedimento com prioridade, uma vez que o não cumprimento poderá resultar no bloqueio do acesso ao sistema por parte da Senatran, impedindo o acesso ao Pré-cadastro no sistema Renavam, a qualquer momento. \r\n ";
                    break;

                case "Contrato Social não encontrado e/ou não está em conformidade com o Inciso II do Art. 3º da Portaria Senatran Nº. 452 de 2024.":
                    mensagemObservacaoAuto = "Contrato Social não encontrado e/ou não está em conformidade com o Inciso II do Art. 3º da Portaria Senatran Nº. 452 de 2024.";
                    break;

                default:
                    mensagemObservacaoAuto = "Por favor, verifique todos os campos";
                    break;
            }

            return mensagemObservacaoAuto;
        }

        public static string CreateMessageFileNameAuto(string fileName)
        {
            string fileNameCorreto;

            fileName = fileName.Trim();

            switch (fileName)
            {
                case "Ficha de Inscricao":
                    fileNameCorreto = "Ficha de Inscrição";
                    break;
                case "Procuracao":
                    fileNameCorreto = "Procuração";
                    break;
                case "Cartao CNPJ":
                    fileNameCorreto = "Cartão CNPJ";
                    break;
                case "Contrato Social":
                    fileNameCorreto = "Contrato Social";
                    break;
                default:
                    fileNameCorreto = "Documento não encontrado";
                    break;
            }

            return fileNameCorreto;
        }
    }
}
