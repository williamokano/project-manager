using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ListarProjetos : Katapoka.Core.FormBase, IHttpHandler, Katapoka.Core.IPaginaPesquisa
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual == null)
            Response.Redirect("~/Login.aspx");
        ((Katapoka.Core.QuanticaMasterPage)this.Page.Master).PageTitle = "Projetos";

        populaDDLEmpresa();
    }

    private void populaDDLEmpresa()
    {
        using (Katapoka.BLL.Empresa.EmpresaBLL empresaBLL = new Katapoka.BLL.Empresa.EmpresaBLL())
        {
            ddlEmpresa.Items.Clear();
            ddlEmpresa.DataSource = empresaBLL.GetAll();
            ddlEmpresa.DataBind();
            ddlEmpresa.Items.Insert(0, new ListItem("Todas", ""));

            //Seta o valor que veio pela query string
            if (Request.QueryString["idEmpresa"] != null)
                if (ddlEmpresa.Items.FindByValue(Request.QueryString["idEmpresa"].ToString()) != null)
                    ddlEmpresa.Items.FindByValue(Request.QueryString["idEmpresa"].ToString()).Selected = true;
        }
    }

    private bool PopularDropDownListOrdernacao(System.Web.UI.WebControls.DropDownList dropDownList)
    {
        dropDownList.Items.Clear();

        dropDownList.Items.Add(new ListItem("ID (Crescente)", Katapoka.BLL.Projeto.ProjetoBLL.OrdenacaoProjeto.IdC.ToString()));
        dropDownList.Items.Add(new ListItem("ID (Decrescente)", Katapoka.BLL.Projeto.ProjetoBLL.OrdenacaoProjeto.IdD.ToString()));

        dropDownList.Items.Add(new ListItem("Nome Projeto (Crescente)", Katapoka.BLL.Projeto.ProjetoBLL.OrdenacaoProjeto.NomeProjetoC.ToString()));
        dropDownList.Items.Add(new ListItem("Nome Projeto (Decrescente)", Katapoka.BLL.Projeto.ProjetoBLL.OrdenacaoProjeto.NomeProjetoD.ToString()));

        dropDownList.Items.Add(new ListItem("Nome Empresa (Crescente)", Katapoka.BLL.Projeto.ProjetoBLL.OrdenacaoProjeto.EmpresaC.ToString()));
        dropDownList.Items.Add(new ListItem("Nome Empresa (Decrescente)", Katapoka.BLL.Projeto.ProjetoBLL.OrdenacaoProjeto.EmpresaD.ToString()));

        return true;
    }

    public Repeater PopularGrid()
    {

        //Recupera os parametros
        int? idEmpresa = (Request.QueryString["idEmpresa"] != null && Request.QueryString["idEmpresa"].ToString() != "" ? (int?)Convert.ToInt32(Request.QueryString["idEmpresa"].ToString()) : null);
        Katapoka.BLL.Projeto.ProjetoBLL.OrdenacaoProjeto order = (Request.QueryString["order"] != null ? (Katapoka.BLL.Projeto.ProjetoBLL.OrdenacaoProjeto)Enum.Parse(typeof(Katapoka.BLL.Projeto.ProjetoBLL.OrdenacaoProjeto), Request.QueryString["order"].ToString()) : Katapoka.BLL.Projeto.ProjetoBLL.OrdenacaoProjeto.IdC);

        using (Katapoka.BLL.Projeto.ProjetoBLL projetoBLL = new Katapoka.BLL.Projeto.ProjetoBLL())
        {
            Katapoka.Core.WebControlBind.TotalRegistros = projetoBLL.GetCountProjetos(null, idEmpresa, null, null, null, order);
            Katapoka.Core.WebControlBind.RepeaterBind<Katapoka.DAO.Projeto_Tb>(rptGrid,
                projetoBLL.GetProjetosFiltro(null, idEmpresa, null, null, null, order,
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

}