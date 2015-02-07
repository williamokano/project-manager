using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Katapoka.BLL.Atividade
{
    public class AtividadeBLL : AbstractBLLModel<Katapoka.DAO.Atividade_Tb>
    {
        private IEnumerable<Katapoka.DAO.Atividade_Tb> GetQueryAtividades(int? idProjetoAtividade,
            int? idProjeto, string dsTitulo, DateTime? dtTermino, string dsAtividade, int? idUsuarioCriacao, DateTime? dtCriacao, OrdenacaoAtividade ordem, StatusAtividade? status = null)
        {
            var query = this.Context.Atividade_Tb.AsQueryable();

            if (idProjetoAtividade != null)
                query = query.Where(p => p.IdAtividade == idProjetoAtividade.Value);

            if (idProjeto != null)
                query = query.Where(p => p.IdProjeto == idProjeto.Value);

            if (!string.IsNullOrWhiteSpace(dsTitulo))
                query = query.Where(p => p.DsTituloAtividade.Contains(dsTitulo));

            if (dtTermino != null)
                query = query.Where(p => p.DtTermino == dtTermino.Value);

            if (!string.IsNullOrWhiteSpace(dsAtividade))
                query = query.Where(p => p.DsAtividade.Contains(dsAtividade));

            if (idUsuarioCriacao != null)
                query = query.Where(p => p.IdUsuarioCriacao == idUsuarioCriacao.Value);

            if (dtCriacao != null)
                query = query.Where(p => p.DtCriacao == dtCriacao.Value);

            if (status != null)
            {
                switch (status.Value)
                {
                    case StatusAtividade.Ativa: query = query.Where(p => p.FlStatus == "A"); break;
                    case StatusAtividade.Lixeira: query = query.Where(p => p.FlStatus == "L"); break;
                    case StatusAtividade.Excluida: query = query.Where(p => p.FlStatus == "E"); break;
                }
            }

            Func<Katapoka.DAO.Atividade_Tb, object> paramOrdenacao = null;

            switch (ordem)
            {
                case OrdenacaoAtividade.DtInicioC:
                case OrdenacaoAtividade.DtInicioD:
                    paramOrdenacao = p => p.DtInicio;
                    break;
                case OrdenacaoAtividade.DtTerminoC:
                case OrdenacaoAtividade.DtTerminoD:
                    paramOrdenacao = p => p.DtTermino;
                    break;
                case OrdenacaoAtividade.IdC:
                case OrdenacaoAtividade.IdD:
                    paramOrdenacao = p => p.IdAtividade;
                    break;
                case OrdenacaoAtividade.PorcentagemC:
                case OrdenacaoAtividade.PorcentagemD:
                    paramOrdenacao = p => p.VrCompletoPorcentagem;
                    break;
                case OrdenacaoAtividade.TempoEstimadoC:
                case OrdenacaoAtividade.TempoEstimadoD:
                    paramOrdenacao = p => p.QtTempoEstimado;
                    break;
                case OrdenacaoAtividade.TempoExecutadoC:
                case OrdenacaoAtividade.TempoExecutadoD:
                    paramOrdenacao = p => p.QtTempoExecutado;
                    break;
                case OrdenacaoAtividade.TituloC:
                case OrdenacaoAtividade.TituloD:
                    paramOrdenacao = p => p.DsTituloAtividade;
                    break;
            }

            if (paramOrdenacao == null)
                return query;
            else
                if (new OrdenacaoAtividade[] { OrdenacaoAtividade.DtInicioC, OrdenacaoAtividade.DtTerminoC, OrdenacaoAtividade.IdC, OrdenacaoAtividade.PorcentagemC, OrdenacaoAtividade.TempoEstimadoC, OrdenacaoAtividade.TempoExecutadoC, OrdenacaoAtividade.TituloC }
                        .Contains(ordem))
                    return query.OrderBy(paramOrdenacao);
                else
                    return query.OrderByDescending(paramOrdenacao);
        }
        public int GetCountAtividades(int? idProjetoAtividade,
            int? idProjeto, string dsTitulo, DateTime? dtTermino, string dsAtividade, int? idUsuarioCriacao, DateTime? dtCriacao, OrdenacaoAtividade ordem, StatusAtividade? status = null)
        {
            return GetQueryAtividades(idProjetoAtividade, idProjeto, dsTitulo, dtTermino, dsAtividade, idUsuarioCriacao, dtCriacao, ordem, status).Count();
        }
        public IList<Katapoka.DAO.Atividade_Tb> GetFiltroQueryAtividades(int? idProjetoAtividade,
            int? idProjeto, string dsTitulo, DateTime? dtTermino, string dsAtividade, int? idUsuarioCriacao, DateTime? dtCriacao, OrdenacaoAtividade ordem, StatusAtividade? status = null, int skip = 0, int? take = null)
        {
            if (take == null)
                return GetQueryAtividades(idProjetoAtividade, idProjeto, dsTitulo, dtTermino, dsAtividade, idUsuarioCriacao, dtCriacao, ordem, status).Skip(skip).ToList();
            return GetQueryAtividades(idProjetoAtividade, idProjeto, dsTitulo, dtTermino, dsAtividade, idUsuarioCriacao, dtCriacao, ordem, status).Skip(skip).Take(take.Value).ToList();
        }
        public IList<Katapoka.DAO.Usuario_Tb> GetUsuariosPermitidos(int idTipoAtividade)
        {
            return this.Context.Usuario_Tb
                .Join(this.Context.Cargo_Tb, p => p.IdCargo, p => p.IdCargo, (tb1, tb2) => new { Usuario_Tb = tb1, Cargo_Tb = tb2 })
                .Join(this.Context.CargoTipoAtividade_Tb, p => p.Cargo_Tb.IdCargo, p => p.IdCargo, (tb1, tb2) => new { Usuario_Tb = tb1.Usuario_Tb, Cargo_Tb = tb1.Cargo_Tb, CargoTipoAtividade_Tb = tb2 })
                .Where(p => p.CargoTipoAtividade_Tb.IdTipoAtividade == idTipoAtividade)
                .Select(p => p.Usuario_Tb)
                .Distinct()
                .ToList();
        }
        public void Salvar(int idProjeto, Katapoka.DAO.Atividade.AtividadeCompleta[] atividades, int idUsuarioCriacao)
        {
            using (TransactionScope scopeTran = new TransactionScope())
            {

                //Monta o mapa para relacionamentos!
                Dictionary<int, Katapoka.DAO.Atividade_Tb> relacoes = new Dictionary<int, DAO.Atividade_Tb>();
                foreach (Katapoka.DAO.Atividade.AtividadeCompleta atividade in atividades)
                    if (atividade.IdAtividade == null)
                        relacoes[atividade.IdAtividadeLocal] = new DAO.Atividade_Tb();
                    else
                        relacoes[atividade.IdAtividadeLocal] = GetById(atividade.IdAtividade.Value);

                //Cria os relacionamentos (ou atualiza) e remove (ou adiciona) os usuários da task
                Atividade.AtividadeUsuarioBLL atividadeUsuarioBLL = CriarObjetoNoMesmoContexto<Atividade.AtividadeUsuarioBLL>();
                foreach (Katapoka.DAO.Atividade.AtividadeCompleta atividade in atividades)
                {
                    relacoes[atividade.IdAtividadeLocal].DsAtividade = string.Empty;
                    relacoes[atividade.IdAtividadeLocal].DsTituloAtividade = atividade.DsNomeAtividade;
                    relacoes[atividade.IdAtividadeLocal].DtCriacao = DateTime.Now;
                    relacoes[atividade.IdAtividadeLocal].DtInicio = DateTime.Now;
                    relacoes[atividade.IdAtividadeLocal].DtTermino = DateTime.Now.Add(TimeSpan.FromHours((double)atividade.QtHoras));
                    relacoes[atividade.IdAtividadeLocal].IdProjeto = idProjeto;
                    relacoes[atividade.IdAtividadeLocal].IdUsuarioCriacao = idUsuarioCriacao;

                    // TODO: Converter o TipoAtividade para Decimal tbm
                    relacoes[atividade.IdAtividadeLocal].QtTempoEstimado = atividade.QtHoras;
                    relacoes[atividade.IdAtividadeLocal].QtTempoExecutado = new Decimal(0);
                    relacoes[atividade.IdAtividadeLocal].VrCompletoPorcentagem = 0;

                    //Save(relacoes[atividade.IdAtividadeLocal]);
                    //Insere os novos usuários da atividade (Selecione apenas os que já não estão na lista
                    foreach (int idUsuario in atividade.IdUsuariosResponsaveis
                        .Where(p => !relacoes[atividade.IdAtividadeLocal].AtividadeUsuario_Tb
                            .Select(p2 => p2.IdUsuario).Contains(p)))
                    {
                        Katapoka.DAO.AtividadeUsuario_Tb atividadeUsuarioTb = new DAO.AtividadeUsuario_Tb();
                        atividadeUsuarioTb.DtCriacao = DateTime.Now;
                        atividadeUsuarioTb.IdUsuario = idUsuario;
                        atividadeUsuarioTb.IdUsuarioCriacao = idUsuarioCriacao;
                        relacoes[atividade.IdAtividadeLocal].AtividadeUsuario_Tb.Add(atividadeUsuarioTb);
                    }

                }

                foreach (Katapoka.DAO.Atividade.AtividadeCompleta atividade in atividades)
                {
                    if (atividade.IdPreAtividade != null)
                        relacoes[atividade.IdAtividadeLocal].AtividadePredecessora_Tb = relacoes[atividade.IdPreAtividade.Value];
                    Save(relacoes[atividade.IdAtividadeLocal]);
                }

                this.Context.SaveChanges();
                scopeTran.Complete();
            }
        }
        public void Save(int? idAtividade, int idProjeto, int? idAtividadePredecessora, string tituloAtividade, Decimal tempoEstimado, int porcentagemCompleta, DateTime dtInicio, DateTime dtTermino, string descricao, int idUsuario, Katapoka.DAO.Tag.TagCompleta[] tags)
        {
            Katapoka.DAO.Atividade_Tb atividadeTb = null;
            if (idAtividade == null)
            {
                atividadeTb = new DAO.Atividade_Tb();
                atividadeTb.IdUsuarioCriacao = idUsuario;
                atividadeTb.DtCriacao = DateTime.Now;
            }
            else
            {
                atividadeTb = GetById(idAtividade.Value);
            }
            if (atividadeTb == null)
                throw new Exception("Atividade não pode ser criada.");
            atividadeTb.IdProjeto = idProjeto;
            atividadeTb.IdAtividadePredecessora = idAtividadePredecessora;
            atividadeTb.DsTituloAtividade = tituloAtividade;
            atividadeTb.QtTempoEstimado = tempoEstimado;
            atividadeTb.VrCompletoPorcentagem = porcentagemCompleta;
            atividadeTb.DtInicio = dtInicio;
            atividadeTb.DtTermino = dtTermino;
            atividadeTb.DsAtividade = descricao;
            Save(atividadeTb);

            //Verifico quais tags eu possuo associadas para associar (apenas ID's positivas)
            List<Katapoka.DAO.Tag.TagCompleta> tagsReais = tags.Where(p => p.IdTag > 0).ToList();
            List<Katapoka.DAO.Tag.TagCompleta> tagsNovas = tags.Where(p => p.IdTag < 0).ToList();
            BLL.Tag.TagBLL tagBLL = CriarObjetoNoMesmoContexto<Tag.TagBLL>();
            BLL.Tag.AtividadeTagBLL atiTagBLL = CriarObjetoNoMesmoContexto<Tag.AtividadeTagBLL>();

            //Removo as que não foram postadas
            int[] idsPostadas = tags.Where(p => p.IdTag != null).Select(p => p.IdTag.Value).ToArray();
            foreach (Katapoka.DAO.AtividadeTag_Tb ati in atividadeTb.AtividadeTag_Tb
                                                            .Where(p => !idsPostadas.Contains(p.IdTag))
                                                            .ToList())
            {
                atiTagBLL.Delete(ati);
            }

            //Cadastro as novas tags e associo à atividade
            foreach (Katapoka.DAO.Tag.TagCompleta tag in tagsNovas)
            {
                Katapoka.DAO.Tag_Tb newTag = new DAO.Tag_Tb() { DsTag = tag.DsTag };
                tagBLL.Save(newTag);

                Katapoka.DAO.AtividadeTag_Tb atiTag = new DAO.AtividadeTag_Tb() { IdTag = newTag.IdTag, IdAtividade = atividadeTb.IdAtividade };
                atiTagBLL.Save(atiTag);
            }

            //Adiciono as novas tags
            foreach (int idTag in tagsReais.Select(p => p.IdTag).ToList()
                .Where(p => !atividadeTb.AtividadeTag_Tb.Select(p2 => p2.IdTag).ToList().Contains(p.Value)))
            {
                Katapoka.DAO.AtividadeTag_Tb atiTag = new DAO.AtividadeTag_Tb() { IdTag = idTag, IdAtividade = atividadeTb.IdAtividade };
                atiTagBLL.Save(atiTag);
            }

            Save(atividadeTb);
        }

        public enum OrdenacaoAtividade
        {
            IdC,
            IdD,
            TituloC,
            TituloD,
            TempoEstimadoC,
            TempoEstimadoD,
            TempoExecutadoC,
            TempoExecutadoD,
            PorcentagemC,
            PorcentagemD,
            DtInicioC,
            DtInicioD,
            DtTerminoC,
            DtTerminoD
        }
        public enum StatusAtividade
        {
            Ativa,
            Lixeira,
            Excluida
        }

    }
}
