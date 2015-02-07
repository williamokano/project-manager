using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Includes_BarraPaginacaoUC : System.Web.UI.UserControl, Katapoka.Core.IBarraPaginacao
{
    public int PaginaAtual { get; set; }
    public int QtdPaginas { get; set; }
    public int QtdRegistroPagina { get; set; }
    private string urlPaginacao;
    public string URLPaginacao
    {
        get
        {
            if(urlPaginacao == null)
            {
                urlPaginacao = System.Web.HttpContext.Current.Request.Url.Segments.Where(p=>p.Contains(".aspx")).FirstOrDefault();
                if (urlPaginacao.EndsWith("/"))
                    urlPaginacao = urlPaginacao.Remove(urlPaginacao.Length - 1);
                if (System.Web.HttpContext.Current.Request.Url.Query != "" && Request.QueryString.AllKeys.Where(p => !(new string[] { "pg", "qtd" }).Contains(p)).Count() > 0)
                {
                    urlPaginacao += "?" + string.Join("&", Request.QueryString.AllKeys.Where(p => !(new string[] { "pg", "qtd" }).Contains(p)).Select(p => p + "=" + System.Web.HttpContext.Current.Request.QueryString[p]).ToArray())+"&";
                }
                else
                    urlPaginacao += "?";

                urlPaginacao += "pg={0}&qtd=" + QtdRegistroPagina;
            }
            return urlPaginacao;

        }
        set
        {
            urlPaginacao = value;
        }
    }

    public DropDownList DDLOrdenacao
    {
        get
        {
            return ddlOrdenacao;
        }
    }

    public DropDownList DDLQtdRegistrosPagina
    {
        get
        {
            return ddlQtdRegistrosPagina;
        }
    }

    public bool DDLOrdenacaoVisible
    {
        get
        {
            return ddlOrdenacao.Visible;
        }
        set
        {
            ddlOrdenacao.Visible = value;
        }
    }

    public Repeater RPTPaginas
    {
        get
        {
            return rptPaginas;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void rptPaginas_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)
        {
            System.Web.UI.HtmlControls.HtmlGenericControl control = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("prev");
            if (PaginaAtual == 0)
                control.Attributes["class"] = "prev disabled";
                //control.Visible = false;
        }
        else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            if (e.Item.ItemIndex == PaginaAtual)
            {
                System.Web.UI.HtmlControls.HtmlGenericControl control = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("pagina");
                control.Attributes.Add("class", "active");
            }
        }
        else if (e.Item.ItemType == ListItemType.Footer)
        {
            System.Web.UI.HtmlControls.HtmlGenericControl control = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("next");
            if (PaginaAtual == QtdPaginas - 1)
                control.Attributes["class"] = "next disabled";
                //control.Visible = false;
        }
    }
}