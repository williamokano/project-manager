using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CadastrarAtividade : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual == null)
            Response.Redirect("~/Login.aspx");
        populaDDLProjeto();

        int idAtividade = 0;
        if (Request.QueryString["id"] != null &&
            Int32.TryParse(Request.QueryString["id"].ToString(), out idAtividade))
        {
            using (Katapoka.BLL.Atividade.AtividadeBLL atividadeBLL = new Katapoka.BLL.Atividade.AtividadeBLL())
            {
                Katapoka.DAO.Atividade_Tb atividadeTb = atividadeBLL.GetById(idAtividade);
                populaDadosAtividade(atividadeTb);
            }
        }

        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "IdAtividade", "var IdAtividade = " + (idAtividade == 0 ? "null" : idAtividade.ToString()) + ";", true);

    }

    private void populaDadosAtividade(Katapoka.DAO.Atividade_Tb atividadeTb)
    {
        ListItem lip = ddlProjeto.Items.FindByValue(atividadeTb.IdProjeto.ToString());
        if (lip != null)
            lip.Selected = true;
        txtTituloAtividade.Value = atividadeTb.DsTituloAtividade;

        if (atividadeTb.QtTempoEstimado != null)
        {
            TimeSpan tempoEstimado = TimeSpan.FromHours((double)atividadeTb.QtTempoEstimado.Value);
            txtTempoEstimado.Value = string.Format("{0:000}:{1:00}", Math.Floor(tempoEstimado.TotalHours), tempoEstimado.Minutes);
        }
        TimeSpan tempoExecutado = TimeSpan.FromHours((double)atividadeTb.QtTempoExecutado);
        txtTempoExecutado.Value = string.Format("{0:000}:{1:00}", Math.Floor(tempoExecutado.TotalHours), tempoExecutado.Minutes);

        if (atividadeTb.VrCompletoPorcentagem != null)
            txtPorcentagem.Value = atividadeTb.VrCompletoPorcentagem.Value.ToString();

        if (atividadeTb.DtInicio != null)
            txtDtInicio.Value = atividadeTb.DtInicio.Value.ToString("dd/MM/yyyy");

        if (atividadeTb.DtTermino != null)
            txtDtTermino.Value = atividadeTb.DtTermino.Value.ToString("dd/MM/yyyy");
        txtDescricao.Value = atividadeTb.DsAtividade;

        //Popula dados atividade predecessora
        ddlAtividadePredecessora.Items.Clear();
        using (Katapoka.BLL.Atividade.AtividadeBLL atividadeBLL = new Katapoka.BLL.Atividade.AtividadeBLL())
        {
            ddlAtividadePredecessora.DataSource = atividadeBLL.GetFiltroQueryAtividades(null, atividadeTb.IdProjeto, null, null, null, null, null, Katapoka.BLL.Atividade.AtividadeBLL.OrdenacaoAtividade.IdC)
                                                    .Where(p => p.IdAtividade != atividadeTb.IdAtividade &&
                                                        !atividadeTb.AtividadesSucessoras_Tb.Select(p2 => p2.IdAtividade)
                                                        .ToList().Contains(p.IdAtividade)
                                                    ).ToList();
            ddlAtividadePredecessora.DataTextField = "DsTituloAtividade";
            ddlAtividadePredecessora.DataValueField = "IdAtividade";
            ddlAtividadePredecessora.DataBind();
            ddlAtividadePredecessora.Items.Insert(0, new ListItem("Atividades", ""));
            if (atividadeTb.IdAtividadePredecessora != null)
            {
                ListItem liap = ddlAtividadePredecessora.Items.FindByValue(atividadeTb.IdAtividadePredecessora.Value.ToString());
                if (liap != null)
                    liap.Selected = true;
            }
        }

        //Recupera as tags
        List<Katapoka.DAO.Tag.AtividadeTagCompleta> tags = atividadeTb.AtividadeTag_Tb
            .Select(p => new Katapoka.DAO.Tag.AtividadeTagCompleta()
            {
                DsTag = p.Tag_Tb.DsTag,
                IdAtividade = p.IdAtividade,
                IdAtividadeTag = p.IdAtividadeTag,
                IdTag = p.IdTag
            }).ToList();
        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Tags", string.Format("var atividadeTags = {0};", new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(tags)), true);

    }

    private void populaDDLProjeto()
    {
        using (Katapoka.BLL.Projeto.ProjetoBLL projetoBLL = new Katapoka.BLL.Projeto.ProjetoBLL())
        {
            ddlProjeto.Items.Clear();
            ddlProjeto.DataSource = projetoBLL.GetAll();
            ddlProjeto.DataValueField = "IdProjeto";
            ddlProjeto.DataTextField = "DsNome";
            ddlProjeto.DataBind();
            ddlProjeto.Items.Insert(0, new ListItem("Selecione um projeto", ""));
        }
    }

    [System.Web.Services.WebMethod(true)]
    [System.Web.Script.Services.ScriptMethod(UseHttpGet = false, ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static Katapoka.DAO.JsonResponse Salvar(int? idAtividade, int idProjeto, int? idAtividadePredecessora, string tituloAtividade, string tempoEstimado, int porcentagemCompleta, string strDtInicio, string strDtTermino, string descricao, Katapoka.DAO.Tag.TagCompleta[] tags)
    {
        Katapoka.DAO.JsonResponse response = new Katapoka.DAO.JsonResponse();
        if (Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual == null)
        {
            response.Status = 300;
            response.Data = "Por favor, faça o login.";
        }
        else
        {
            DateTime dtInicio = DateTime.Now;
            DateTime dtTermino = DateTime.Now;
            #region CONVERT DTTIME
            try
            {
                dtInicio = Convert.ToDateTime(strDtInicio);
            }
            catch
            {
                response.Status = 500;
                response.Data = "Data Início inválida.";
                return response;
            }

            try
            {
                dtTermino = Convert.ToDateTime(strDtTermino);
            }
            catch
            {
                response.Status = 500;
                response.Data = "Data Término inválida.";
                return response;
            }
            #endregion
            using (Katapoka.BLL.Atividade.AtividadeBLL atividadeBLL = new Katapoka.BLL.Atividade.AtividadeBLL())
            {
                int idUsuario = Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual.IdUsuario;
                try
                {
                    Decimal tmpEstimado = Katapoka.BLL.Utilitarios.Utilitario.ConvertTimeStringToDecimal(tempoEstimado);
                    atividadeBLL.Save(idAtividade, idProjeto, idAtividadePredecessora, tituloAtividade, tmpEstimado, porcentagemCompleta, dtInicio, dtTermino, descricao, idUsuario, tags);
                    response.Status = 200;
                    response.Data = "OK";
                }
                catch (Exception ex)
                {
                    response.Status = 600;
                    response.Data = ex.Message;
                }
            }
        }
        return response;
    }

}