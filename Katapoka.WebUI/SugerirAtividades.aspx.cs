using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class SugerirAtividades : System.Web.UI.Page
{
    private Dictionary<int, int> relacoes = new Dictionary<int, int>();

    protected void Page_Load(object sender, EventArgs e)
    {
        int idProjeto = 0;
        if (Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual == null)
            Response.Redirect("~/Login.aspx");
        else
        {
            if (Request.QueryString["pid"] != null)
            {
                Int32.TryParse(Request.QueryString["pid"].ToString(), out idProjeto);
                if (idProjeto > 0)
                {
                    if (new Katapoka.BLL.Atividade.AtividadeBLL().GetCountAtividades(null, idProjeto, null, null, null, null, null, Katapoka.BLL.Atividade.AtividadeBLL.OrdenacaoAtividade.IdC) == 0)
                    {
                        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "IdProjeto", "var IdProjeto = " + idProjeto.ToString() + ";", true);
                        using (Katapoka.BLL.Atividade.TipoAtividadeBLL tipoAtividadeBLL = new Katapoka.BLL.Atividade.TipoAtividadeBLL())
                        {
                            using (Katapoka.BLL.Projeto.ProjetoBLL projetoBLL = tipoAtividadeBLL.CriarObjetoNoMesmoContexto<Katapoka.BLL.Projeto.ProjetoBLL>())
                            {
                                Katapoka.DAO.Projeto_Tb projetoTb = projetoBLL.GetById(idProjeto);
                                if (projetoTb != null)
                                {
                                    IList<Katapoka.DAO.Atividade.TipoAtividade> atividades =
                                        tipoAtividadeBLL.GetTipoAtividadePorTipoProjeto(projetoTb.IdTipoProjeto);

                                    for (int i = 1; i <= atividades.Count; i++)
                                        this.relacoes[atividades[i - 1].IdTipoAtividade.Value] = i;

                                    rptAtividades.DataSource = atividades;
                                    rptAtividades.DataBind();
                                }
                                else
                                {
                                    Response.Redirect("~/ListarProjetos.aspx");
                                }
                            }
                        }
                    }
                    else
                    {
                        Response.Redirect("~/ListarProjetos.aspx");
                    }
                }
                else
                {
                    Response.Redirect("~/ListarProjetos.aspx");
                }
            }
            else
            {
                Response.Redirect("~/ListarProjetos.aspx");
            }
        }
    }
    protected void rptAtividades_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            HtmlInputText txtNomeAtividade = (HtmlInputText)e.Item.FindControl("txtNomeAtividade");
            HtmlInputText txtNomeUsuario = (HtmlInputText)e.Item.FindControl("txtNomeUsuario");
            HtmlInputText txtTempoPrevisto = (HtmlInputText)e.Item.FindControl("txtTempoPrevisto");
            HtmlInputText txtIdAtividadePredecessora = (HtmlInputText)e.Item.FindControl("txtIdAtividadePredecessora");
            Katapoka.DAO.TipoAtividade_Tb item = (Katapoka.DAO.TipoAtividade_Tb)e.Item.DataItem;
            Katapoka.DAO.Usuario_Tb usuarioAssociado = new Katapoka.BLL.Atividade.AtividadeBLL()
                                                            .GetUsuariosPermitidos(item.IdTipoAtividade)
                                                            .FirstOrDefault();

            //Preenche os relacionamentos
            //if (item.TipoProjetoTipoAtividade_Tb != null)
            //    if (this.relacoes.Keys.Where(p => p == item.TipoAtividadePredecessora_Tb.IdTipoAtividade).Count() > 0)
            //        txtIdAtividadePredecessora.Value = this.relacoes[item.TipoAtividadePredecessora_Tb.IdTipoAtividade].ToString();

            if (usuarioAssociado != null)
            {
                //txtNomeUsuario.Value = usuarioAssociado == null ? string.Empty : usuarioAssociado.DsNome;
                txtNomeUsuario.Attributes.Add("data-uid",   usuarioAssociado.IdUsuario.ToString());
                txtNomeUsuario.Attributes.Add("data-uname", usuarioAssociado.DsNome);
            }

            txtNomeAtividade.Value = item.DsTituloAtividade;
            txtTempoPrevisto.Value = item.QtTempoEstimado.ToString(@"hh\:mm").PadLeft(6, '0');
        }
    }

    [System.Web.Services.WebMethod(true)]
    [System.Web.Script.Services.ScriptMethod(UseHttpGet = false, ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static Katapoka.DAO.JsonResponse IncluirAtividades(int idProjeto, Katapoka.DAO.Atividade.AtividadeCompleta[] atividades)
    {
        Katapoka.DAO.JsonResponse response = new Katapoka.DAO.JsonResponse();
        if (Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual == null)
        {
            response.Status = 100;
            response.Data = "Realize o login para continuar.";
            return response;
        }
        try
        {
            using (Katapoka.BLL.Atividade.AtividadeBLL atividadeBLL = new Katapoka.BLL.Atividade.AtividadeBLL())
            {
                atividadeBLL.Salvar(idProjeto, atividades, Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual.IdUsuario);
                response.Status = 200;
                response.Data = "OK";
            }
        }
        catch (Exception ex)
        {
            response.Status = 500;
            response.Data = ex.Message;
        }

        return response;
    }
}