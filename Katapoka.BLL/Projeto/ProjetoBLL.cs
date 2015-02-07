using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Katapoka.BLL.Projeto
{
    public enum StatusProjeto
    {
        Execucao = 'E',
        Paralisado = 'P',
        Cancelado = 'C'
    }

    public class ProjetoBLL : AbstractBLLModel<Katapoka.DAO.Projeto_Tb>
    {

        public IList<Katapoka.DAO.Projeto_Tb> GetAll(bool? activesOnly = true)
        {
            // TODO fazer a checagem se o projeto está ativo ou não!
            return this.Context.Projeto_Tb
                .OrderBy(p => p.DsNome)
                .ToList();
        }
        public string Salvar(int? idProjeto, string nome, int idEmpresa, int idUsuarioCadastro, DateTime? dtInicio, DateTime? dtTermino, string codReferencia, string status, int idTipoProjeto)
        {
            Katapoka.DAO.Projeto_Tb projetoTb = null;
            if (idProjeto == null)
            {
                projetoTb = new DAO.Projeto_Tb();
                projetoTb.DtCriacao = DateTime.Now;
                projetoTb.IdUsuarioCriacao = idUsuarioCadastro;
            }
            else
            {
                projetoTb = GetById(idProjeto.Value);
                if (projetoTb == null)
                    return "Projeto não encontrado.";
            }

            projetoTb.DsNome = nome;
            projetoTb.IdEmpresa = idEmpresa;
            projetoTb.DtInicioEstimado = dtInicio;
            projetoTb.DtTerminoEstimado = dtTermino;
            projetoTb.DsCodigoReferencia = codReferencia;
            projetoTb.FlStatus = status;
            projetoTb.IdTipoProjeto = idTipoProjeto;

            Save(projetoTb);
            return projetoTb.IdProjeto.ToString();
        }
        private IEnumerable<Katapoka.DAO.Projeto_Tb> GetQueryProjetos(int? idProjeto, int? idEmpresa, string dsNome, int? idUsuarioCriacao, DateTime? dtCriacao, OrdenacaoProjeto ordem)
        {
            var query = this.Context.Projeto_Tb.AsQueryable();
            if (idProjeto != null)
                query = query.Where(p => p.IdProjeto == idProjeto.Value);

            if (idEmpresa != null)
                query = query.Where(p => p.IdEmpresa == idEmpresa.Value);

            if (!string.IsNullOrWhiteSpace(dsNome))
                query = query.Where(p => p.DsNome.Contains(dsNome));

            if (idUsuarioCriacao != null)
                query = query.Where(p => p.IdUsuarioCriacao == idUsuarioCriacao.Value);

            if (dtCriacao != null)
                query = query.Where(p => p.DtCriacao == dtCriacao.Value);

            Func<Katapoka.DAO.Projeto_Tb, object> paramOrdenacao = null;

            switch (ordem)
            {
                case OrdenacaoProjeto.EmpresaC:
                case OrdenacaoProjeto.EmpresaD:
                    paramOrdenacao = p => p.Empresa_Tb.DsNomeFantasia;
                    break;
                case OrdenacaoProjeto.IdC:
                case OrdenacaoProjeto.IdD:
                    paramOrdenacao = p => p.IdEmpresa;
                    break;
                case OrdenacaoProjeto.NomeProjetoC:
                case OrdenacaoProjeto.NomeProjetoD:
                    paramOrdenacao = p => p.DsNome;
                    break;
            }

            if (paramOrdenacao != null)
            {

                if (new OrdenacaoProjeto[] { OrdenacaoProjeto.EmpresaC, OrdenacaoProjeto.IdC, OrdenacaoProjeto.NomeProjetoC }
                    .Contains(ordem))
                    return query.OrderBy(paramOrdenacao);
                else if (new OrdenacaoProjeto[] { OrdenacaoProjeto.EmpresaD, OrdenacaoProjeto.IdD, OrdenacaoProjeto.NomeProjetoD }
                    .Contains(ordem))
                    return query.OrderByDescending(paramOrdenacao);
                else
                    return query; //Nunca deve chegar nesse return, mas por via das dúvidas, deixa ele aí
            }
            else
            {
                return query;
            }
        }
        public int GetCountProjetos(int? idProjeto, int? idEmpresa, string dsNome, int? idUsuarioCriacao, DateTime? dtCriacao, OrdenacaoProjeto ordem)
        {
            return GetQueryProjetos(idProjeto, idEmpresa, dsNome, idUsuarioCriacao, dtCriacao, ordem).Count();
        }
        public IList<Katapoka.DAO.Projeto_Tb> GetProjetosFiltro(int? idProjeto, int? idEmpresa, string dsNome, int? idUsuarioCriacao, DateTime? dtCriacao, OrdenacaoProjeto ordem, int skip = 0, int? take = null)
        {
            if (take == null)
                return GetQueryProjetos(idProjeto, idEmpresa, dsNome, idUsuarioCriacao, dtCriacao, ordem).Skip(skip).ToList();
            return GetQueryProjetos(idProjeto, idEmpresa, dsNome, idUsuarioCriacao, dtCriacao, ordem).Skip(skip).Take(take.Value).ToList();
        }
        public IList<Katapoka.DAO.Atividade.AtividadeAjaxPost> SalvarAtividades(int idProjeto, List<Katapoka.DAO.Atividade.AtividadeAjaxPost> atividades, int idUsuarioAlteracao)
        {
            Katapoka.DAO.Projeto_Tb projetoTb = GetById(idProjeto);

            Dictionary<int, Katapoka.DAO.Tag_Tb> tagsUtilizadas = new Dictionary<int, DAO.Tag_Tb>();
            Dictionary<int, Katapoka.DAO.Atividade_Tb> atividadesRelacionadas = new Dictionary<int, DAO.Atividade_Tb>();
            Dictionary<int, Katapoka.DAO.Usuario_Tb> usuariosUtilizados = new Dictionary<int, DAO.Usuario_Tb>();

            Katapoka.BLL.Atividade.AtividadeBLL atividadeBLL = CriarObjetoNoMesmoContexto<Katapoka.BLL.Atividade.AtividadeBLL>();
            Katapoka.BLL.Tag.TagBLL tagBLL = CriarObjetoNoMesmoContexto<Katapoka.BLL.Tag.TagBLL>();
            Katapoka.BLL.Tag.AtividadeTagBLL atividadeTagBLL = CriarObjetoNoMesmoContexto<Katapoka.BLL.Tag.AtividadeTagBLL>();
            Katapoka.BLL.Usuario.UsuarioBLL usuarioBLL = CriarObjetoNoMesmoContexto<Usuario.UsuarioBLL>();
            Katapoka.BLL.Atividade.AtividadeUsuarioBLL atividadeUsuarioBLL = CriarObjetoNoMesmoContexto<Atividade.AtividadeUsuarioBLL>();
            foreach (Katapoka.DAO.Atividade.AtividadeAjaxPost atividade in atividades)
            {
                #region Adiciona as atividades na "tabela de relacionamento de atividades"
                Katapoka.DAO.Atividade_Tb atividadeTb = null;
                if (atividade.IdAtividade == null)
                {
                    atividadeTb = new DAO.Atividade_Tb();
                    atividadeTb.DtCriacao = DateTime.Now;
                    atividadeTb.IdUsuarioCriacao = idUsuarioAlteracao;
                    atividadeTb.FlStatus = "A"; //Recém criada, recebe status de ativa!
                    projetoTb.Atividade_Tb.Add(atividadeTb);
                    atividadeTb.Projeto_Tb = projetoTb;
                }
                else
                {
                    atividadeTb = atividadeBLL.GetById(atividade.IdAtividade.Value);
                }
                atividadesRelacionadas.Add(atividade.IdAtividadeLocal, atividadeTb);
                #endregion
                #region Adiciona as tags na "tabela de relacionamento"
                if (atividade.Tags.Count > 0)
                {
                    foreach (Katapoka.DAO.Tag.TagCompleta tag in atividade.Tags)
                    {
                        Katapoka.DAO.Tag_Tb tagTb = null;
                        if (!tagsUtilizadas.ContainsKey(tag.IdTag.Value))
                        {
                            if (tag.IdTag.Value < 1)
                            {
                                tagTb = new DAO.Tag_Tb();
                                tagTb.DsTag = tag.DsTag;
                                Katapoka.DAO.AtividadeTag_Tb atividadeTagTb = new DAO.AtividadeTag_Tb();
                                atividadeTagTb.Tag_Tb = tagTb;
                                atividadeTagTb.Atividade_Tb = atividadeTb;
                                atividadeTb.AtividadeTag_Tb.Add(atividadeTagTb);
                                tagTb.AtividadeTag_Tb.Add(atividadeTagTb);
                            }
                            else
                            {
                                tagTb = tagBLL.GetById(tag.IdTag.Value);
                            }
                            tagsUtilizadas.Add(tag.IdTag.Value, tagTb);
                        }

                    }
                }
                #endregion
                #region Adiciona os usuários utilizados
                foreach (Katapoka.DAO.UsuarioCompleto usuarioCompleto in atividade.Usuarios)
                {
                    if (!usuariosUtilizados.ContainsKey(usuarioCompleto.IdUsuario))
                    {
                        usuariosUtilizados.Add(usuarioCompleto.IdUsuario, usuarioBLL.GetById(usuarioCompleto.IdUsuario));
                    }
                }
                #endregion
            }

            //Processamento geral
            foreach (Katapoka.DAO.Atividade.AtividadeAjaxPost atividade in atividades)
            {
                //Preencho os dados
                if(!string.IsNullOrWhiteSpace(atividade.DsNomeAtividade))
                    atividadesRelacionadas[atividade.IdAtividadeLocal].DsTituloAtividade = atividade.DsNomeAtividade;
                else
                    atividadesRelacionadas[atividade.IdAtividadeLocal].DsTituloAtividade = "Atividade sem título";
                atividadesRelacionadas[atividade.IdAtividadeLocal].DsAtividade = atividade.DsAtividade;
                atividadesRelacionadas[atividade.IdAtividadeLocal].DtInicio = !string.IsNullOrWhiteSpace(atividade.DtInicioStr) ? Convert.ToDateTime(atividade.DtInicioStr) : (DateTime?)null;
                atividadesRelacionadas[atividade.IdAtividadeLocal].DtTermino = !string.IsNullOrWhiteSpace(atividade.DtTerminoStr) ? Convert.ToDateTime(atividade.DtTerminoStr) : (DateTime?)null;
                atividadesRelacionadas[atividade.IdAtividadeLocal].QtTempoEstimado = !string.IsNullOrWhiteSpace(atividade.QtTempoEstimado) ? Katapoka.BLL.Utilitarios.Utilitario.ConvertTimeStringToDecimal(atividade.QtTempoEstimado) : (decimal?)null;
                atividadesRelacionadas[atividade.IdAtividadeLocal].VrCompletoPorcentagem = atividade.VrCompletoPorcentagem;
                //Tempo executado não está aqui pois não pode editado manualmente

                #region Relaciono as atividades com Pre e pos
                if (atividade.IdAtividadePredecessora != null)
                {
                    Katapoka.DAO.Atividade_Tb atividadePre, atividadeTb;
                    atividadeTb = atividadesRelacionadas[atividade.IdAtividadeLocal];
                    atividadePre = atividadesRelacionadas[atividade.IdAtividadePredecessora.Value];
                    atividadeTb.AtividadePredecessora_Tb = atividadePre;
                    atividadePre.AtividadesSucessoras_Tb.Add(atividadeTb);
                }
                else
                {
                    Katapoka.DAO.Atividade_Tb atividadeTb;
                    atividadeTb = atividadesRelacionadas[atividade.IdAtividadeLocal].AtividadePredecessora_Tb = null;
                }
                #endregion

                //Removo as tags que não fazem mais parte da atividade DAS ATIVIDADES PARA EDIÇÃO (IdAtividade != null)
                if (atividade.IdAtividade != null)
                {
                    int[] idsTagsAtividade = atividade.Tags.Where(p => p.IdTag != null && p.IdTag.Value > 0)
                        .Select(p => p.IdTag.Value)
                        .ToArray();
                    IList<Katapoka.DAO.AtividadeTag_Tb> tagsARemover =
                        this.Context.AtividadeTag_Tb
                            .Where(p => p.IdAtividade == atividade.IdAtividade.Value && !idsTagsAtividade.Contains(p.IdTag))
                            .ToList();
                    atividadeTagBLL.DeleteRange(tagsARemover);
                }

                //Gerenciamento dos usuários da atividade
                //Se a atividade é nova, adiciona todos os usuários, caso contrário, valida quem entra, quem fica e quem saí.
                if (atividade.IdAtividade == null)
                {
                    //Adiciona usuários
                    foreach (Katapoka.DAO.UsuarioCompleto usuarioCompleto in atividade.Usuarios)
                    {
                        Katapoka.DAO.AtividadeUsuario_Tb atividadeUsuarioTb = new DAO.AtividadeUsuario_Tb();
                        atividadeUsuarioTb.DtCriacao = DateTime.Now;
                        atividadeUsuarioTb.IdUsuarioCriacao = idUsuarioAlteracao;
                        atividadeUsuarioTb.Atividade_Tb = atividadesRelacionadas[atividade.IdAtividadeLocal];
                        atividadeUsuarioTb.Usuario_Tb = usuariosUtilizados[usuarioCompleto.IdUsuario];
                        usuariosUtilizados[usuarioCompleto.IdUsuario].AtividadeUsuario_Tb.Add(atividadeUsuarioTb);
                        atividadesRelacionadas[atividade.IdAtividadeLocal].AtividadeUsuario_Tb.Add(atividadeUsuarioTb);
                    }

                    //Adiciona tags
                    foreach (Katapoka.DAO.Tag.TagCompleta tag in atividade.Tags)
                    {
                        Katapoka.DAO.AtividadeTag_Tb atividadeTag = new DAO.AtividadeTag_Tb();
                        Katapoka.DAO.Atividade_Tb atividadeTb = atividadesRelacionadas[atividade.IdAtividadeLocal];
                        Katapoka.DAO.Tag_Tb tagTb = tagsUtilizadas[tag.IdTag.Value];

                        atividadeTag.Tag_Tb = tagTb;
                        atividadeTag.Atividade_Tb = atividadeTb;
                        tagTb.AtividadeTag_Tb.Add(atividadeTag);
                        atividadeTb.AtividadeTag_Tb.Add(atividadeTag);
                    }
                }
                else
                {
                    #region Trata usuários
                    int[] idsEnviadas = atividade.Usuarios.Select(p => p.IdUsuario).ToArray();
                    int[] idsUsuariosJaAssociados = atividadesRelacionadas[atividade.IdAtividadeLocal].AtividadeUsuario_Tb
                        .Select(p => p.IdUsuario).ToArray();
                    //Insere novos
                    foreach (int idUsuarioInserir in idsEnviadas.Where(p => !idsUsuariosJaAssociados.Contains(p)).ToArray())
                    {
                        Katapoka.DAO.Usuario_Tb usuarioTb = usuariosUtilizados[idUsuarioInserir];
                        Katapoka.DAO.Atividade_Tb atividadeTb = atividadesRelacionadas[atividade.IdAtividadeLocal];
                        Katapoka.DAO.AtividadeUsuario_Tb atividadeUsuarioTb = new DAO.AtividadeUsuario_Tb();
                        atividadeUsuarioTb.DtCriacao = DateTime.Now;
                        atividadeUsuarioTb.IdUsuarioCriacao = idUsuarioAlteracao;
                        atividadeUsuarioTb.Usuario_Tb = usuarioTb;
                        atividadeUsuarioTb.Atividade_Tb = atividadeTb;
                        atividadeTb.AtividadeUsuario_Tb.Add(atividadeUsuarioTb);
                        usuarioTb.AtividadeUsuario_Tb.Add(atividadeUsuarioTb);
                    }

                    //Remove não associados
                    IList<Katapoka.DAO.AtividadeUsuario_Tb> usuariosRemover = this.Context.AtividadeUsuario_Tb
                        .Where(p => p.IdAtividade == atividade.IdAtividade.Value && !idsEnviadas.Contains(p.IdUsuario))
                        .ToList();
                    atividadeUsuarioBLL.DeleteRange(usuariosRemover);
                    #endregion
                    #region Trata tags
                    int[] idsTagsEnviadas = atividade.Tags.Where(p => p.IdTag != null && p.IdTag.Value > 0)
                        .Select(p => p.IdTag.Value)
                        .ToArray();
                    int[] idsTagsEmUso = atividadesRelacionadas[atividade.IdAtividadeLocal].AtividadeTag_Tb
                        .Select(p => p.IdTag).ToArray();
                    //Insere as novas
                    foreach (int idTagInserir in idsTagsEnviadas.Where(p => !idsTagsEmUso.Contains(p)).ToArray())
                    {
                        Katapoka.DAO.AtividadeTag_Tb atividadeTagTb = new DAO.AtividadeTag_Tb();
                        atividadeTagTb.Tag_Tb = tagsUtilizadas[idTagInserir];
                        atividadeTagTb.Atividade_Tb = atividadesRelacionadas[atividade.IdAtividadeLocal];
                        atividadesRelacionadas[atividade.IdAtividadeLocal].AtividadeTag_Tb.Add(atividadeTagTb);
                        tagsUtilizadas[idTagInserir].AtividadeTag_Tb.Add(atividadeTagTb);
                    }

                    //Deleta as que não existe mais
                    IList<Katapoka.DAO.AtividadeTag_Tb> atividadesARemover = this.Context.AtividadeTag_Tb
                        .Where(p => p.IdAtividade == atividade.IdAtividade.Value && !idsTagsEnviadas.Contains(p.IdTag))
                        .ToList();
                    atividadeTagBLL.DeleteRange(atividadesARemover);

                    //Katapoka.DAO.AtividadeTag_Tb atividadeTagTb = new DAO.AtividadeTag_Tb();
                    //atividadeTagTb.Tag_Tb = tagTb;
                    //atividadeTagTb.Atividade_Tb = atividadeTb;
                    //atividadeTb.AtividadeTag_Tb.Add(atividadeTagTb);
                    //tagTb.AtividadeTag_Tb.Add(atividadeTagTb);
                    #endregion
                }

            }


            //Removo as atividades que não mais fazem parte do projeto
            int[] idsAtividadesProjeto = atividades.Where(p => p.IdAtividade != null)
                .Select(p => p.IdAtividade.Value).ToArray(); //Ids que AINDA FAZEM PARTE do projeto
            IList<Katapoka.DAO.Atividade_Tb> atividadesRemover =
                this.Context.Atividade_Tb.Where(p => p.IdProjeto == idProjeto && !idsAtividadesProjeto.Contains(p.IdAtividade))
                .ToList();
            //atividadeBLL.DeleteRange(atividadesRemover);
            foreach (Katapoka.DAO.Atividade_Tb atividadeRemover in atividadesRemover)
            {
                atividadeRemover.FlStatus = "L";
                atividadeRemover.IdAtividadePredecessora = null; //Se a atividade foi removida eu removo o vínculo dela com qualquer outra para evitar que de problemas futuros de integridade
                //atividadeRemover.AtividadePredecessora_Tb = null;
            }

            //Reza pra não dar pau e tenta salvar tudo
            Save(projetoTb);

            //Atualiza as IDS de atividade e atualiza as IDS de tags e retorna
            for (int i = 0; i < atividades.Count; i++)
            {
                Katapoka.DAO.Atividade_Tb atividadeTb = atividadesRelacionadas[atividades[i].IdAtividadeLocal];
                atividades[i].IdAtividade = atividadeTb.IdAtividade;
                atividades[i].DsNomeAtividade = atividadeTb.DsTituloAtividade;
                TimeSpan tempoExecutado = TimeSpan.FromHours((double)atividadeTb.QtTempoExecutado);
                atividades[i].QtTempoExecutado = string.Format("{0:000}:{1:00}", Math.Floor(tempoExecutado.TotalHours), tempoExecutado.Minutes);

                //Re-gero as tags utilizadas
                //atividades[i].Tags = atividadeTb.AtividadeTag_Tb
                //    .Select(p => new Katapoka.DAO.Tag.TagCompleta()
                //    {
                //        DsTag = p.Tag_Tb.DsTag,
                //        IdTag = p.IdTag
                //    }).ToList();
                //
                ////Re-gero os usuários
                //atividades[i].Usuarios = atividadeTb.AtividadeUsuario_Tb.Select(p => new Katapoka.DAO.UsuarioCompleto()
                //{
                //    DsNome = p.Usuario_Tb.DsNome,
                //    IdUsuario = p.IdUsuario
                //}).ToList();
            }
            return atividades;
        }
        public enum OrdenacaoProjeto
        {
            IdC,
            IdD,
            NomeProjetoC,
            NomeProjetoD,
            EmpresaC,
            EmpresaD
        }

        public enum Status
        {
            Ativa,
            Lixeira,
            Excluida
        }

    }
}
