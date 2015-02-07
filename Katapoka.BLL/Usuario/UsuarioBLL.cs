using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Katapoka.BLL.Usuario
{
    public enum EsquemaBuscaNome
    {
        Exatamente,
        ComecandoCom,
        TerminandoCom,
        Contem
    }

    public class UsuarioBLL : AbstractBLLModel<Katapoka.DAO.Usuario_Tb>
    {
        public Katapoka.DAO.Usuario_Tb GetByHashLogin(string hashLogin)
        {
            return this.Context.Usuario_Tb
                .Where(p => p.DsHashLogin == hashLogin)
                .FirstOrDefault();
        }

        public Katapoka.DAO.Usuario_Tb GetByEmail(string email)
        {
            return this.Context.Usuario_Tb
                .Where(p => p.DsEmail == email)
                .FirstOrDefault();
        }
        public IList<Katapoka.DAO.Usuario_Tb> GetByNome(string nome, EsquemaBuscaNome tipoBusca = EsquemaBuscaNome.Exatamente)
        {
            var query = this.Context.Usuario_Tb
                .AsQueryable();
            switch (tipoBusca)
            {
                case EsquemaBuscaNome.ComecandoCom:
                    query = query.Where(p => p.DsNome.StartsWith(nome));
                    break;
                case EsquemaBuscaNome.Contem:
                    query = query.Where(p => p.DsNome.Contains(nome));
                    break;
                case EsquemaBuscaNome.Exatamente:
                    query = query.Where(p => p.DsNome == nome);
                    break;
                case EsquemaBuscaNome.TerminandoCom:
                    query = query.Where(p => p.DsNome.EndsWith(nome));
                    break;
            }
            return query.ToList();
        }

        private IEnumerable<Katapoka.DAO.Usuario_Tb> GetQueryUsuario(int? idUsuario, int? idNivelUsuario, string dsNome, string dsEmail, int? idCargo)
        {
            var query = this.Context.Usuario_Tb.AsQueryable();

            if (idUsuario != null)
                query = query.Where(p => p.IdUsuario == idUsuario.Value);

            if (idNivelUsuario != null)
                query = query.Where(p => p.IdUsuarioNivel == idNivelUsuario.Value);

            if (!string.IsNullOrWhiteSpace(dsNome))
                query = query.Where(p => p.DsNome.Contains(dsNome));

            if (!string.IsNullOrWhiteSpace(dsEmail))
                query = query.Where(p => p.DsEmail.Contains(dsEmail));

            if (idCargo != null)
                query = query.Where(p => p.IdCargo == idCargo.Value);

            return query;
        }
        public int GetCountUsuario(int? idUsuario, int? idNivelUsuario, string dsNome, string dsEmail, int? idCargo)
        {
            return GetQueryUsuario(idUsuario, idNivelUsuario, dsNome, dsEmail, idCargo).Count();
        }
        public IList<Katapoka.DAO.Usuario_Tb> GetUsuariosFiltro(int? idUsuario, int? idNivelUsuario, string dsNome, string dsEmail, int? idCargo, int skip = 0, int? take = null)
        {
            if (take == null)
                return GetQueryUsuario(idUsuario, idNivelUsuario, dsNome, dsEmail, idCargo).Skip(skip).ToList();
            return GetQueryUsuario(idUsuario, idNivelUsuario, dsNome, dsEmail, idCargo).Skip(skip).Take(take.Value).ToList();
        }
        public void Save(int? idUsuario, string nome, string email, string senha, int idNivel, int idCargo)
        {
            Katapoka.DAO.Usuario_Tb usuarioTb = idUsuario == null ? new DAO.Usuario_Tb() : GetById(idUsuario.Value);
            if (usuarioTb == null)
                throw new Exception("Usuário não pode ser criado/salvo");
            usuarioTb.DsNome = nome;
            usuarioTb.DsEmail = email;

            if (!string.IsNullOrWhiteSpace(senha))
                usuarioTb.DsSenha = Utilitarios.Criptografia.GetHash(senha);

            usuarioTb.IdUsuarioNivel = idNivel;
            usuarioTb.IdCargo = idCargo;
            Save(usuarioTb);

        }
    }
}
