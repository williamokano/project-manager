using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ListarUsuarios : Katapoka.Core.FormBase, IHttpHandler, Katapoka.Core.IPaginaPesquisa
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual == null)
            Response.Redirect("~/Login.aspx");
        ((Katapoka.Core.QuanticaMasterPage)this.Page.Master).PageTitle = "Usuários";
    }

    private bool PopularDropDownListOrdernacao(System.Web.UI.WebControls.DropDownList dropDownList)
    {
        dropDownList.Items.Clear();
        dropDownList.Items.Add(new ListItem("Cidade (crescente)", "1"));
        dropDownList.Items.Add(new ListItem("Cidade (decrescente)", "2"));

        return true;
    }

    public Repeater PopularGrid()
    {
        using (Katapoka.BLL.Usuario.UsuarioBLL usuarioBLL = new Katapoka.BLL.Usuario.UsuarioBLL())
        {
            Katapoka.Core.WebControlBind.TotalRegistros = usuarioBLL.GetCountUsuario(null, null, null, null, null);
            Katapoka.Core.WebControlBind.RepeaterBind<Katapoka.DAO.Usuario_Tb>(rptGrid,
                usuarioBLL.GetUsuariosFiltro(null, null, null, null, null,
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