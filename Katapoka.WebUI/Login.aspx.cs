using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual != null)
            Response.Redirect("~/Default.aspx");
    }

    [WebMethod(true)]
    [ScriptMethod(UseHttpGet=false, ResponseFormat=ResponseFormat.Json)]
    public static Katapoka.DAO.JsonResponse Logar(string email, string senha)
    {
        Katapoka.DAO.JsonResponse response = new Katapoka.DAO.JsonResponse();
        try
        {
            Katapoka.BLL.Autenticacao.Usuario.Login(email, senha);
            response.Status = 200;
            response.Data = "";
        }
        catch(Exception ex)
        {
            response.Status = 400;
            response.Data = ex.Message;
        }
        return response;
    }

}