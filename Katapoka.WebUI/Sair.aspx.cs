using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sair : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Katapoka.BLL.Autenticacao.Usuario.Logout();
        Response.Redirect("~/Default.aspx");
    }
}