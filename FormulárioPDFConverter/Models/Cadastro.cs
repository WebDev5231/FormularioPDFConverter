using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace FormulárioPDFConverter.Models
{
    [Table("cadastro")]
    public class Cadastro
    {
        [Column("[ID_Empresa]")]
        [Key]
        public string ID_Empresa { get; set; }

        [Required]
        [Column("ID_Grupo")]
        public string ID_Grupo { get; set; }
        public string InscricaoJuntaComercial { get; set; }

        [Required]
        [Column("ID_Publico")]
        public string ID_Publico { get; set; }
        public string PorteEmpresaSelecionado { get; set; }

        public List<SelectListItem> PortesEmpresa { get; set; }

        [Required]
        [Column("ID_Sub_Publico")]
        public string ID_Sub_Publico { get; set; }

        public List<SelectListItem> Categorias { get; set; }
        public string CategoriaSelecionada { get; set; }
        public int id_cat_assoc { get; set; }

        [Column("Filial")]
        public bool Filial { get; set; }

        [Column("Homologada")]
        public bool Homologada { get; set; }

        [Required]
        [Column("CNPJ")]
        public string CNPJ { get; set; }

        [Column("Niev")]
        public string Niev { get; set; }

        [Required]
        [Column("Razao")]
        public string Razao { get; set; }

        [Column("Fantasia")]
        public string Fantasia { get; set; }

        [Column("Endereco")]
        public string Endereco { get; set; }
        public string Complemento { get; set; }


        [Column("Numero")]
        public string Numero { get; set; }

        [Column("ID_Cidade")]
        public int ID_Cidade { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        [Column("Bairro")]
        public string Bairro { get; set; }

        [Column("Cep")]
        public string Cep { get; set; }

        [Column("Telefone")]
        public string Telefone { get; set; }

        [Column("Fax")]
        public string Fax { get; set; }

        [Column("Site")]
        public string Site { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("ProdutosHomologados")]
        public string ProdutosHomologados { get; set; }

        [Column("Contato1")]
        public string Contato1 { get; set; }

        [Column("Departamento1")]
        public string Departamento1 { get; set; }

        [Column("Telefone1")]
        public string Telefone1 { get; set; }

        [Column("Email1")]
        public string Email1 { get; set; }

        [Column("Contato2")]
        public string Contato2 { get; set; }

        [Column("Departamento2")]
        public string Departamento2 { get; set; }

        [Column("Telefone2")]
        public string Telefone2 { get; set; }

        [Column("Email2")]
        public string Email2 { get; set; }

        [Column("Taxa_Assoc")]
        public decimal Taxa_Assoc { get; set; }

        [Column("Insert_Grat")]
        public int Insert_Grat { get; set; }

        [Column("Taxa_Extra")]
        public decimal Taxa_Extra { get; set; }

        [Column("Data_Assoc")]
        public string Data_Assoc { get; set; }

        [Column("Saldo")]
        public decimal Saldo { get; set; }

        [Column("Atualizacao")]
        public DateTime Atualizacao { get; set; }

        [Column("porteempresa")]
        public int porteempresa { get; set; }

        [Column("situacao")]
        public int situacao { get; set; }

        [Column("contatorenavam")]
        public string contatorenavam { get; set; }

        [Column("departamentorenavam")]
        public string departamentorenavam { get; set; }

        [Column("telefonerenavam")]
        public string telefonerenavam { get; set; }

        [Column("emailrenavam")]
        public string emailrenavam { get; set; }

        [Column("email_solicitacoes")]
        public string email_solicitacoes { get; set; }

        [Column("cpf")]
        public string cpf { get; set; }

        [Column("emailfinanceiro")]
        public string emailfinanceiro { get; set; }

        [Column("contatofinanceiro")]
        public string contatofinanceiro { get; set; }

        public string status_homologacao { get; set; }
        public DateTime data_cadastro { get; set; }
        public int cod_selo { get; set; }
        public string NomeResponsavelLegal { get; set; }
        public string EmailResponsavelLegal { get; set; }
        public string RGResponsavelLegal { get; set; }
        public string CPFResponsavelLegal { get; set; }
        public int ID_sub_publico_negociacao { get; set; }
        public int diaBoleto { get; set; }
        public string contatoExportador { get; set; }
        public string departamentoExportador { get; set; }
        public string telefoneExportador { get; set; }
        public string emailExportador { get; set; }
        public DateTime DataFundacao { get; set; }
        public string representanteLegal1 { get; set; }
        public string cpfRepresentante1 { get; set; }
        public string rgRepresentante1 { get; set; }
        public string telefoneRepresentante1 { get; set; }
        public string emailRepresentante1 { get; set; }
        public string cargoRepresentante1 { get; set; }
        public string representanteLegal2 { get; set; }
        public string cpfRepresentante2 { get; set; }
        public string rgRepresentante2 { get; set; }
        public string telefoneRepresentante2 { get; set; }
        public string emailRepresentante2 { get; set; }
        public string cargoRepresentante2 { get; set; }
        public string inscricaoEstadual { get; set; }
        public string dataDeIngresso { get; set; }
    }
}
