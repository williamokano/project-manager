using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ListarEmpresas : Katapoka.Core.FormBase, IHttpHandler, Katapoka.Core.IPaginaPesquisa
{

    private bool PopularDropDownListOrdernacao(System.Web.UI.WebControls.DropDownList dropDownList)
    {
        dropDownList.Items.Clear();
        dropDownList.Items.Add(new ListItem("Cidade (crescente)", "1"));
        dropDownList.Items.Add(new ListItem("Cidade (decrescente)", "2"));

        return true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual == null)
            Response.Redirect("~/Login.aspx");
        ((Katapoka.Core.QuanticaMasterPage)this.Page.Master).PageTitle = "Listagem de Empresas";
    }

    public Repeater PopularGrid()
    {
        using (Katapoka.BLL.Empresa.EmpresaBLL empresaBLL = new Katapoka.BLL.Empresa.EmpresaBLL())
        {
            Katapoka.Core.WebControlBind.TotalRegistros = empresaBLL.GetCountFiltro(null, null, null, null, null, null, null, null, null);
            Katapoka.Core.WebControlBind.RepeaterBind<Katapoka.DAO.Empresa_Tb>(rptGrid,
                empresaBLL.GetEmpresasFiltro(null, null, null, null, null, null, null, null, null,
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
