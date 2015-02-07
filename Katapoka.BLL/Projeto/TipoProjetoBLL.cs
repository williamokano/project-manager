using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Katapoka.DAO;

namespace Katapoka.BLL.Projeto
{
    public class TipoProjetoBLL : AbstractBLLModel<Katapoka.DAO.TipoProjeto_Tb>
    {
        public IList<Katapoka.DAO.TipoProjeto_Tb> GetAll()
        {
            return this.Context.TipoProjeto_Tb
                .OrderBy(p => p.DsTipo)
                .ToList();
        }
        private IEnumerable<Katapoka.DAO.TipoProjeto_Tb> GetTipoProjetoQuery(int? idTipoProjeto, string dsTipoProjeto, TipoProjetoOrder order = TipoProjetoOrder.IdC)
        {
            var query = this.Context.TipoProjeto_Tb.AsQueryable();

            if (idTipoProjeto != null)
                query = query.Where(p => p.IdTipoProjeto == idTipoProjeto.Value);

            if (!string.IsNullOrWhiteSpace(dsTipoProjeto))
                query = query.Where(p => p.DsTipo.Trim().Contains(dsTipoProjeto.Trim()));

            Func<Katapoka.DAO.TipoProjeto_Tb, object> paramOrdenacao = p => p.IdTipoProjeto;

            switch (order)
            {
                case TipoProjetoOrder.IdC:
                case TipoProjetoOrder.IdD:
                    paramOrdenacao = p => p.IdTipoProjeto;
                    break;
                case TipoProjetoOrder.DsTipoC:
                case TipoProjetoOrder.DsTipoD:
                    paramOrdenacao = p => p.DsTipo;
                    break;
            }

            if (new TipoProjetoOrder[] { TipoProjetoOrder.DsTipoC, TipoProjetoOrder.IdC }.Contains(order))
                return query.OrderBy(paramOrdenacao);
            else
                return query.OrderByDescending(paramOrdenacao);
        }
        public int GetTipoProjetoCount(int? idTipoProjeto, string dsTipoProjeto, TipoProjetoOrder order = TipoProjetoOrder.IdC)
        {
            return GetTipoProjetoQuery(idTipoProjeto, dsTipoProjeto, order).Count();
        }
        public IList<Katapoka.DAO.TipoProjeto_Tb> GetTipoProjeto(int? idTipoProjeto, string dsTipoProjeto, TipoProjetoOrder order = TipoProjetoOrder.IdC, int skip = 0, int? take = null)
        {
            var query = GetTipoProjetoQuery(idTipoProjeto, dsTipoProjeto, order).Skip(skip);

            if (take != null)
                query = query.Take(take.Value);

            return query.ToList();
        }
        public void Save(int? idTipoProjeto, string dsTipoProjeto)
        {
            Katapoka.DAO.TipoProjeto_Tb tipoProjetoTb = idTipoProjeto == null ? new Katapoka.DAO.TipoProjeto_Tb() : GetById(idTipoProjeto.Value);
            tipoProjetoTb.DsTipo = dsTipoProjeto;
            Save(tipoProjetoTb);
        }
        public IList<Katapoka.DAO.Atividade.AtividadeAjaxPost> InserirTipoAtividades(int idTipoProjeto, List<Katapoka.DAO.Atividade.AtividadeAjaxPost> atividades)
        {
            Dictionary<int, Katapoka.DAO.TipoAtividade_Tb> atividadesRelacionadas = new Dictionary<int, Katapoka.DAO.TipoAtividade_Tb>();
            Dictionary<int, Katapoka.DAO.Tag_Tb> tagsUtilizadas = new Dictionary<int, DAO.Tag_Tb>();

            Atividade.TipoAtividadeBLL tipoAtividadeBLL = CriarObjetoNoMesmoContexto<Atividade.TipoAtividadeBLL>();
            Tag.TagBLL tagBLL = CriarObjetoNoMesmoContexto<Tag.TagBLL>();
            TipoProjetoTipoAtividadeBLL tipoProjetoTipoAtividadeBLL = CriarObjetoNoMesmoContexto<TipoProjetoTipoAtividadeBLL>();
            Tag.TipoAtividadeTagBLL tipoAtividadeTagBLL = CriarObjetoNoMesmoContexto<Tag.TipoAtividadeTagBLL>();

            Katapoka.DAO.TipoProjeto_Tb tipoProjetoTb = GetById(idTipoProjeto);

            foreach (Katapoka.DAO.Atividade.AtividadeAjaxPost atividade in atividades)
            {
                Katapoka.DAO.TipoAtividade_Tb tipoAtividadeTb = null;
                TipoProjetoTipoAtividade_Tb tipoProjetoTipoAtividadeTb = null;

                //Inclui os TipoAtividade
                if (atividade.IdAtividade != null)
                {
                    tipoAtividadeTb = tipoAtividadeBLL.GetById(atividade.IdAtividade.Value);
                    tipoProjetoTipoAtividadeTb = tipoProjetoTipoAtividadeBLL.GetTipoProjetoTipoAtividade(idTipoProjeto, atividade.IdAtividade.Value);
                    if (tipoProjetoTipoAtividadeTb == null)
                    {
                        tipoProjetoTipoAtividadeTb = new TipoProjetoTipoAtividade_Tb();

                        //Vincula
                        tipoProjetoTipoAtividadeTb.TipoAtividade_Tb = tipoAtividadeTb;
                        tipoAtividadeTb.TipoProjetoTipoAtividade_Tb.Add(tipoProjetoTipoAtividadeTb);

                        tipoProjetoTipoAtividadeTb.TipoProjeto_Tb = tipoProjetoTb;
                        tipoProjetoTb.TipoProjetoTipoAtividade_Tb.Add(tipoProjetoTipoAtividadeTb);
                    }
                }
                else
                {
                    tipoAtividadeTb = new TipoAtividade_Tb();
                    tipoProjetoTipoAtividadeTb = new TipoProjetoTipoAtividade_Tb();

                    //Vincula
                    tipoProjetoTipoAtividadeTb.TipoAtividade_Tb = tipoAtividadeTb;
                    tipoAtividadeTb.TipoProjetoTipoAtividade_Tb.Add(tipoProjetoTipoAtividadeTb);

                    tipoProjetoTipoAtividadeTb.TipoProjeto_Tb = tipoProjetoTb;
                    tipoProjetoTb.TipoProjetoTipoAtividade_Tb.Add(tipoProjetoTipoAtividadeTb);
                }

                tipoAtividadeTb.DsAtividade = atividade.DsAtividade;
                tipoAtividadeTb.DsTituloAtividade = atividade.DsNomeAtividade;
                tipoAtividadeTb.QtTempoEstimado = Utilitarios.Utilitario.ConvertTimeStringToDecimal(atividade.QtTempoEstimado);
                atividadesRelacionadas[atividade.IdAtividadeLocal] = tipoAtividadeTb;

                //Inclui as tags
                foreach (Katapoka.DAO.Tag.TagCompleta tagCompleta in atividade.Tags)
                {
                    Katapoka.DAO.Tag_Tb tagTb = null;
                    if (!tagsUtilizadas.ContainsKey(tagCompleta.IdTag.Value))
                    {
                        if (tagCompleta.IdTag < 1)
                        {
                            tagTb = new Tag_Tb();
                        }
                        else
                        {
                            tagTb = tagBLL.GetById(tagCompleta.IdTag.Value);
                        }
                        tagTb.DsTag = tagCompleta.DsTag;
                        tagsUtilizadas[tagCompleta.IdTag.Value] = tagTb;
                    }
                }
            }

            //Configura as pre atividades
            foreach (Katapoka.DAO.Atividade.AtividadeAjaxPost atividade in atividades)
            {
                DAO.TipoAtividade_Tb tipoAtividadeTb = atividadesRelacionadas[atividade.IdAtividadeLocal];

                #region VINCULA AS PRE_ATIVIDADES
                DAO.TipoProjetoTipoAtividadePredecessora_Tb configPredecessora = null;
                //Se é um novo registro, ele ainda não tem sua configuração de predecessora
                if (atividade.IdAtividade == null)
                {
                    configPredecessora = CriaConfigTipoAtividadePredecessora(tipoProjetoTb, tipoAtividadeTb);
                }
                else
                {
                    configPredecessora = tipoAtividadeBLL.GetTipoProjetoTipoAtividadePredecessora(idTipoProjeto, atividade.IdAtividade.Value);
                    if (configPredecessora == null)
                        configPredecessora = CriaConfigTipoAtividadePredecessora(tipoProjetoTb, tipoAtividadeTb);
                }

                //Configura a pre
                if (atividade.IdAtividadePredecessora == null)
                {
                    configPredecessora.TipoAtividadePredecessora_Tb = null;
                }
                else
                {
                    TipoAtividade_Tb tipoAtividadePre = atividadesRelacionadas[atividade.IdAtividadePredecessora.Value];
                    configPredecessora.TipoAtividadePredecessora_Tb = tipoAtividadePre;
                }

                #endregion

                #region PROCESSA AS TAGS
                if (atividade.IdAtividade == null)
                {
                    //Adiciona as novas tags
                    foreach (DAO.Tag.TagCompleta tagCompleta in atividade.Tags.Where(p => p.IdTag < 1))
                    {
                        DAO.TipoAtividadeTag_Tb tipoAtividadeTag = new TipoAtividadeTag_Tb();
                        DAO.Tag_Tb tagTb = tagsUtilizadas[tagCompleta.IdTag.Value];

                        //Vincula tag com a atividade
                        tipoAtividadeTb.TipoAtividadeTag_Tb.Add(tipoAtividadeTag);
                        tipoAtividadeTag.TipoAtividade_Tb = tipoAtividadeTb;

                        tagTb.TipoAtividadeTag_Tb.Add(tipoAtividadeTag);
                        tipoAtividadeTag.Tag_Tb = tagTb;
                    }
                }
                else
                {
                    int[] idsTagsEnviadas = atividade.Tags.Where(p => p.IdTag != null)
                        .Select(p => p.IdTag.Value)
                        .ToArray();

                    int[] idsTagsEmUso = atividadesRelacionadas[atividade.IdAtividadeLocal].TipoAtividadeTag_Tb
                        .Select(p => p.IdTag).ToArray();

                    //Insere as novas
                    foreach (int idTagInserir in idsTagsEnviadas.Where(p => !idsTagsEmUso.Contains(p)).ToArray())
                    {
                        Katapoka.DAO.TipoAtividadeTag_Tb tipoAtividadeTagTb = new DAO.TipoAtividadeTag_Tb();
                        tipoAtividadeTagTb.Tag_Tb = tagsUtilizadas[idTagInserir];
                        tipoAtividadeTagTb.TipoAtividade_Tb = atividadesRelacionadas[atividade.IdAtividadeLocal];
                        atividadesRelacionadas[atividade.IdAtividadeLocal].TipoAtividadeTag_Tb.Add(tipoAtividadeTagTb);
                        tagsUtilizadas[idTagInserir].TipoAtividadeTag_Tb.Add(tipoAtividadeTagTb);
                    }

                    //Deleta as que não existe mais
                    IList<Katapoka.DAO.TipoAtividadeTag_Tb> atividadesARemover = this.Context.TipoAtividadeTag_Tb
                        .Where(p => p.IdTipoAtividade == atividade.IdAtividade.Value && !idsTagsEnviadas.Contains(p.IdTag))
                        .ToList();
                    tipoAtividadeTagBLL.DeleteRange(atividadesARemover);
                }
                #endregion
            }

            //Removo as atividades que não mais fazem parte do tipo de projeto
            int[] idsAtividadesProjeto = atividades.Where(p => p.IdAtividade != null)
                .Select(p => p.IdAtividade.Value).ToArray(); //Ids que AINDA FAZEM PARTE do tipo projeto
            IList<Katapoka.DAO.TipoProjetoTipoAtividade_Tb> atividadesRemover =
                this.Context.TipoProjetoTipoAtividade_Tb.Where(p => p.IdTipoProjeto == idTipoProjeto && !idsAtividadesProjeto.Contains(p.IdTipoAtividade))
                .ToList();
            tipoProjetoTipoAtividadeBLL.DeleteRange(atividadesRemover);
            Save(tipoProjetoTb);

            //Reprocessa a lista atualizando as ids tags tags
            foreach (Katapoka.DAO.Atividade.AtividadeAjaxPost atividade in atividades)
            {
                atividade.IdAtividade = atividadesRelacionadas[atividade.IdAtividadeLocal].IdTipoAtividade;
                foreach (Katapoka.DAO.Tag.TagCompleta tagCompleta in atividade.Tags)
                {
                    int oldTagId = tagCompleta.IdTag.Value;
                    tagCompleta.IdTag = tagsUtilizadas[oldTagId].IdTag;
                }
            }

            return atividades;
        }

        private TipoProjetoTipoAtividadePredecessora_Tb CriaConfigTipoAtividadePredecessora(DAO.TipoProjeto_Tb tipoProjetoTb, DAO.TipoAtividade_Tb tipoAtividadeTb)
        {
            TipoProjetoTipoAtividadePredecessora_Tb configPredecessora = new TipoProjetoTipoAtividadePredecessora_Tb();

            //Vincula com o projeto
            configPredecessora.TipoProjeto_Tb = tipoProjetoTb;
            tipoProjetoTb.TipoProjetoTipoAtividadePredecessora_Tb.Add(configPredecessora);

            //Vincula com a atividade
            configPredecessora.TipoAtividade_Tb = tipoAtividadeTb;
            tipoAtividadeTb.TipoProjetoTipoAtividadePredecessoraPredecessora_Tb.Add(configPredecessora);

            return configPredecessora;
        }
    }

    public enum TipoProjetoOrder
    {
        IdC,
        IdD,
        DsTipoD,
        DsTipoC
    }

}
