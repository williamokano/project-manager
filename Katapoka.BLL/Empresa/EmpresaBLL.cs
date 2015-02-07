using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Katapoka.BLL.Empresa
{
    public class EmpresaBLL : AbstractBLLModel<Katapoka.DAO.Empresa_Tb>
    {
        public IList<Katapoka.DAO.Empresa_Tb> GetAll()
        {
            return this.Context.Empresa_Tb
                .OrderBy(p => p.DsNomeFantasia)
                .ToList();
        }
        private IEnumerable<Katapoka.DAO.Empresa_Tb> GetQueryFiltro(int? idEmpresa,
            int? idAreaAtuacao, string nomeFantasia, string razaoSocial, string cnpj,
            string email, bool? flAceite, bool? flAprovada, DateTime? dtCadastro)
        {
            var query = this.Context.Empresa_Tb.AsQueryable();

            if (idEmpresa != null)
                query = query.Where(p => p.IdEmpresa == idEmpresa.Value);

            if (idAreaAtuacao != null)
                query = query.Where(p => p.IdAreaAtuacao == idAreaAtuacao.Value);

            if (!string.IsNullOrEmpty(nomeFantasia))
                query = query.Where(p => p.DsNomeFantasia.Contains(nomeFantasia));

            if (!string.IsNullOrWhiteSpace(razaoSocial))
                query = query.Where(p => p.DsRazaoSocial.Contains(razaoSocial));

            if (!string.IsNullOrWhiteSpace(cnpj))
                query = query.Where(p => p.NrCnpj.Contains(cnpj));

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(p => p.DsEmail.Contains(email));

            if (flAceite != null)
                query = query.Where(p => p.FlAceiteTermo == flAceite.Value);

            if (flAprovada != null)
                query = query.Where(p => p.FlAprovada == flAprovada.Value);

            if (dtCadastro != null)
                query = query.Where(p => p.DtCadastro == dtCadastro.Value);

            return query;
        }
        public int GetCountFiltro(int? idEmpresa, int? idAreaAtuacao,
            string nomeFantasia, string razaoSocial, string cnpj,
            string email, bool? flAceite, bool? flAprovada, DateTime? dtCadastro)
        {
            return GetQueryFiltro(idEmpresa, idAreaAtuacao, nomeFantasia, razaoSocial,
                cnpj, email, flAceite, flAprovada, dtCadastro).Count();
        }
        public IList<Katapoka.DAO.Empresa_Tb> GetEmpresasFiltro(int? idEmpresa, int? idAreaAtuacao,
            string nomeFantasia, string razaoSocial, string cnpj,
            string email, bool? flAceite, bool? flAprovada, DateTime? dtCadastro, int skip, int take)
        {
            return GetQueryFiltro(idEmpresa, idAreaAtuacao, nomeFantasia, razaoSocial,
                cnpj, email, flAceite, flAprovada, dtCadastro).Skip(skip).Take(take)
                .ToList();
        }
        public int Salvar(int? idEmpresa,
        string nomeFantasia, string razaoSocial, string cnpj,
        int idAreaAtuacao, string email, string url, string sumario,
        string cep, string endereco, string numero, string complemento,
        int? idCidade, int? idBairro, string bairroNome, bool flAceite, bool flAprovada,
        string telefoneComercial, string telefoneResidencial, string telefoneCelular, string telefoneFax, string observacaoContato)
        {
            Katapoka.DAO.Empresa_Tb empresaTb = null;
            Katapoka.DAO.Endereco_Tb enderecoTb = null;
            Katapoka.DAO.Contato_Tb contatoTb = null;

            if (idEmpresa != null)
            {
                empresaTb = GetById(idEmpresa.Value);
                if (empresaTb == null)
                    throw new Exception("Empresa não encontrada.");
                enderecoTb = empresaTb.Endereco_Tb;
                contatoTb = empresaTb.Contato_Tb;
            }
            else
            {
                empresaTb = new DAO.Empresa_Tb();
                enderecoTb = new DAO.Endereco_Tb();
                contatoTb = new DAO.Contato_Tb();

                empresaTb.Endereco_Tb = enderecoTb;
                enderecoTb.Empresa_Tb.Add(empresaTb);

                empresaTb.Contato_Tb = contatoTb;
                contatoTb.Empresa_Tb.Add(empresaTb);

                empresaTb.DtCadastro = DateTime.Now;
            }

            if (!Katapoka.BLL.Utilitarios.Validacao.IsValidEmail(email))
                throw new Exception("E-mail inválido.");

            if (!Katapoka.BLL.Utilitarios.Validacao.IsValidCnpj(cnpj))
                throw new Exception("CNPJ inválido.");

            if (!string.IsNullOrWhiteSpace(url) &&
                !Katapoka.BLL.Utilitarios.Validacao.IsValidUrl(url))
                throw new Exception("A URL informada é inválida.");

            empresaTb.DsNomeFantasia = nomeFantasia;
            empresaTb.DsRazaoSocial = razaoSocial;
            empresaTb.NrCnpj = cnpj;
            empresaTb.IdAreaAtuacao = idAreaAtuacao;
            empresaTb.DsEmail = email.Trim();
            empresaTb.DsSite = url.Trim();
            empresaTb.DsSumarioEmpresa = sumario;
            empresaTb.FlAceiteTermo = flAceite;
            empresaTb.FlAprovada = flAprovada;

            enderecoTb.NrCep = cep;
            enderecoTb.DsEndereco = endereco;
            enderecoTb.NrEndereco = numero;
            enderecoTb.DsComplemento = complemento;
            enderecoTb.IdCidade = idCidade;
            enderecoTb.IdBairro = idBairro;
            enderecoTb.DsBairroOutro = bairroNome;

            contatoTb.DsFax = telefoneFax;
            contatoTb.DsObservacaoContato = observacaoContato;
            contatoTb.DsTelefoneCelular = telefoneCelular;
            contatoTb.DsTelefoneComercial = telefoneComercial;
            contatoTb.DsTelefoneResidencial = telefoneResidencial;

            Save(empresaTb);
            return empresaTb.IdEmpresa;
        }
    }
}
