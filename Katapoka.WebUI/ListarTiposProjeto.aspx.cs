using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ListarTiposProjeto : Katapoka.Core.FormBase, IHttpHandler, Katapoka.Core.IPaginaPesquisa
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual == null)
            Response.Redirect("~/Login.aspx");
    }
    public Repeater PopularGrid()
    {
        int? idTipoProjeto = Request.QueryString["idTipoProjeto"] != null && Request.QueryString["idTipoProjeto"].ToString() != "" ? Convert.ToInt32(Request.QueryString["idTipoProjeto"].ToString()) : (int?)null;
        string dsTipoProjeto = Request.QueryString["dsTipoProjeto"] != null ? Request.QueryString["dsTipoProjeto"].ToString() : null;
        Katapoka.BLL.Projeto.TipoProjetoOrder order = Request.QueryString["order"] != null ? (Katapoka.BLL.Projeto.TipoProjetoOrder)Enum.Parse(typeof(Katapoka.BLL.Projeto.TipoProjetoOrder), Request.QueryString["order"].ToString()) : Katapoka.BLL.Projeto.TipoProjetoOrder.IdC;
        Katapoka.BLL.Projeto.TipoProjetoBLL tipoProjetoBLL = new Katapoka.BLL.Projeto.TipoProjetoBLL();

        Katapoka.Core.WebControlBind.TotalRegistros = tipoProjetoBLL.GetTipoProjetoCount(idTipoProjeto, dsTipoProjeto, order);
        Katapoka.Core.WebControlBind.RepeaterBind<Katapoka.DAO.TipoProjeto_Tb>(rptGrid, tipoProjetoBLL.GetTipoProjeto(idTipoProjeto, dsTipoProjeto, order,
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
    public bool PopularDropDownListOrdernacao(DropDownList list)
    {
        list.Items.Clear();
        
        list.Items.Add(new ListItem("ID (Crescente)", Katapoka.BLL.Projeto.TipoProjetoOrder.IdC.ToString()));
        list.Items.Add(new ListItem("ID (Decrescente)", Katapoka.BLL.Projeto.TipoProjetoOrder.IdC.ToString()));

        list.Items.Add(new ListItem("Tipo (Crescente)", Katapoka.BLL.Projeto.TipoProjetoOrder.DsTipoC.ToString()));
        list.Items.Add(new ListItem("Tipo (Decrescente)", Katapoka.BLL.Projeto.TipoProjetoOrder.DsTipoD.ToString()));

        return true;
    }
}