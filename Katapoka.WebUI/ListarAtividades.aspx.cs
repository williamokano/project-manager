using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ListarAtividades : Katapoka.Core.FormBase, IHttpHandler, Katapoka.Core.IPaginaPesquisa
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual == null)
            Response.Redirect("~/Login.aspx");
        ((Katapoka.Core.QuanticaMasterPage)this.Page.Master).PageTitle = "Atividades";

        popularDDLProjeto();
        popularDDLAtivo();
    }
    public Repeater PopularGrid()
    {
        int? idProjeto = Request.QueryString["pid"] != null && Request.QueryString["pid"].ToString() != "" ? (int?)Int32.Parse(Request.QueryString["pid"].ToString()) : null;
        Katapoka.BLL.Atividade.AtividadeBLL.OrdenacaoAtividade ordem = (Request.QueryString["order"] != null && Request.QueryString["order"].ToString() != "" ? (Katapoka.BLL.Atividade.AtividadeBLL.OrdenacaoAtividade)Enum.Parse(typeof(Katapoka.BLL.Atividade.AtividadeBLL.OrdenacaoAtividade), Request.QueryString["order"].ToString()) : Katapoka.BLL.Atividade.AtividadeBLL.OrdenacaoAtividade.IdC);

        using (Katapoka.BLL.Atividade.AtividadeBLL atividadeBLL = new Katapoka.BLL.Atividade.AtividadeBLL())
        {
            Katapoka.Core.WebControlBind.TotalRegistros = atividadeBLL.GetCountAtividades(null, idProjeto, null, null, null, null, null, ordem);
            Katapoka.Core.WebControlBind.RepeaterBind<Katapoka.DAO.Atividade_Tb>(rptGrid,
                atividadeBLL.GetFiltroQueryAtividades(null, idProjeto, null, null, null, null, null, ordem, null,
                    Katapoka.Core.WebControlBind.PaginaAtual * Katapoka.Core.WebControlBind.QtdRegistrosPagina,
                    Katapoka.Core.WebControlBind.QtdRegistrosPagina),
                Katapoka.Core.WebControlBind.PaginaAtual,
                Katapoka.Core.WebControlBind.QtdRegistrosPagina,
                Katapoka.Core.WebControlBind.TotalRegistros,
                null,
                PopularDropDownListOrdernacao,
                null);
            return rptGrid;
        }
    }
    private bool PopularDropDownListOrdernacao(System.Web.UI.WebControls.DropDownList dropDownList)
    {
        dropDownList.Items.Clear();

        dropDownList.Items.Add(new ListItem("ID (crescente)", Katapoka.BLL.Atividade.AtividadeBLL.OrdenacaoAtividade.IdC.ToString()));
        dropDownList.Items.Add(new ListItem("ID (decrescente)", Katapoka.BLL.Atividade.AtividadeBLL.OrdenacaoAtividade.IdD.ToString()));

        dropDownList.Items.Add(new ListItem("Título (crescente)", Katapoka.BLL.Atividade.AtividadeBLL.OrdenacaoAtividade.TituloC.ToString()));
        dropDownList.Items.Add(new ListItem("Título (decrescente)", Katapoka.BLL.Atividade.AtividadeBLL.OrdenacaoAtividade.TituloD.ToString()));

        dropDownList.Items.Add(new ListItem("Data Início (crescente)", Katapoka.BLL.Atividade.AtividadeBLL.OrdenacaoAtividade.DtInicioC.ToString()));
        dropDownList.Items.Add(new ListItem("Data Início (decrescente)", Katapoka.BLL.Atividade.AtividadeBLL.OrdenacaoAtividade.DtInicioD.ToString()));

        dropDownList.Items.Add(new ListItem("Data Término (crescente)", Katapoka.BLL.Atividade.AtividadeBLL.OrdenacaoAtividade.DtTerminoC.ToString()));
        dropDownList.Items.Add(new ListItem("Data Término (decrescente)", Katapoka.BLL.Atividade.AtividadeBLL.OrdenacaoAtividade.DtTerminoD.ToString()));

        dropDownList.Items.Add(new ListItem("Porcentagem (crescente)", Katapoka.BLL.Atividade.AtividadeBLL.OrdenacaoAtividade.PorcentagemC.ToString()));
        dropDownList.Items.Add(new ListItem("Porcentagem (decrescente)", Katapoka.BLL.Atividade.AtividadeBLL.OrdenacaoAtividade.PorcentagemD.ToString()));

        dropDownList.Items.Add(new ListItem("Tempo Estimado (crescente)", Katapoka.BLL.Atividade.AtividadeBLL.OrdenacaoAtividade.TempoEstimadoC.ToString()));
        dropDownList.Items.Add(new ListItem("Tempo Estimado (decrescente)", Katapoka.BLL.Atividade.AtividadeBLL.OrdenacaoAtividade.TempoEstimadoD.ToString()));

        dropDownList.Items.Add(new ListItem("Tempo Executado (crescente)", Katapoka.BLL.Atividade.AtividadeBLL.OrdenacaoAtividade.TempoExecutadoC.ToString()));
        dropDownList.Items.Add(new ListItem("Tempo Executado (decrescente)", Katapoka.BLL.Atividade.AtividadeBLL.OrdenacaoAtividade.TempoExecutadoD.ToString()));

        return true;
    }
    protected void rptGrid_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            Literal ltrQtTempoEstimado = (Literal)e.Item.FindControl("ltrQtTempoEstimado");
            Literal ltrQtTempoExecutado = (Literal)e.Item.FindControl("ltrQtTempoExecutado");
            //Literal ltrNomePredecessora = (Literal)e.Item.FindControl("ltrNomePredecessora");
            Katapoka.DAO.Atividade_Tb item = (Katapoka.DAO.Atividade_Tb)e.Item.DataItem;

            if (item.QtTempoEstimado != null)
            {
                TimeSpan tempoEstimado = TimeSpan.FromHours((double)item.QtTempoEstimado);
                ltrQtTempoEstimado.Text = string.Format("{0:00}:{1:00}", Math.Floor(tempoEstimado.TotalHours), tempoEstimado.Minutes);
            }

            TimeSpan tempoExecutado = TimeSpan.FromHours((double)item.QtTempoExecutado);
            ltrQtTempoExecutado.Text = string.Format("{0:00}:{1:00}", Math.Floor(tempoExecutado.TotalHours), tempoExecutado.Minutes);
            //if (item.AtividadePredecessora_Tb != null)
            //    ltrNomePredecessora.Text = item.AtividadePredecessora_Tb.DsTituloAtividade;
            //else
            //    ltrNomePredecessora.Text = "Sem pré.";

        }
    }
    private void popularDDLProjeto()
    {
        bool flProjetosAtivos = true;
        if (Request.QueryString["flProjetosAtivos"] != null)
            Boolean.TryParse(Request.QueryString["flProjetosAtivos"].ToString(), out flProjetosAtivos);

        using (Katapoka.BLL.Projeto.ProjetoBLL projetoBLL = new Katapoka.BLL.Projeto.ProjetoBLL())
        {
            ddlProjeto.Items.Clear();
            ddlProjeto.DataSource = projetoBLL.GetAll(flProjetosAtivos);
            ddlProjeto.DataBind();
            ddlProjeto.Items.Insert(0, new ListItem("Todos", ""));

            //Seleciona o campo de projeto correto
            if (Request.QueryString["idProjeto"] != null)
                if (ddlProjeto.Items.FindByValue(Request.QueryString["idProjeto"].ToString()) != null)
                    ddlProjeto.Items.FindByValue(Request.QueryString["idProjeto"].ToString()).Selected = true;
        }
    }
    private void popularDDLAtivo()
    {
        ddlAtivos.Items.Clear();
        ddlAtivos.Items.Insert(0, new ListItem("Apenas ativos", "true"));
        ddlAtivos.Items.Insert(0, new ListItem("Todos", "false"));
        
        //Seta o valor recuperado pela QueryString
        if (Request.QueryString["flProjetosAtivos"] != null)
            if (ddlAtivos.Items.FindByValue(Request.QueryString["flProjetosAtivos"].ToString()) != null)
                ddlAtivos.Items.FindByValue(Request.QueryString["flProjetosAtivos"].ToString()).Selected = true;
    }
}