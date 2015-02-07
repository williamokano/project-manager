using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CadastrarTipoProjeto : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual == null)
            Response.Redirect("~/Default.aspx");

        int idTipoProjeto = 0;
        if (Request.QueryString["id"] != null)
        {
            if (Int32.TryParse(Request.QueryString["id"].ToString(), out idTipoProjeto))
            {
                Katapoka.BLL.Projeto.TipoProjetoBLL tipoProjetoBLL = new Katapoka.BLL.Projeto.TipoProjetoBLL();

                Katapoka.DAO.TipoProjeto_Tb tipoProjetoTb = tipoProjetoBLL.GetTipoProjeto(idTipoProjeto, null).FirstOrDefault();
                if (tipoProjetoTb != null)
                {
                    txtTipoProjeto.Value = tipoProjetoTb.DsTipo;
                }
            }
        }
        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "IdTipoProjeto", string.Format("var IdTipoProjeto = {0};", idTipoProjeto == 0 ? "null" : idTipoProjeto.ToString()), true);
    }

    [System.Web.Services.WebMethod(true)]
    [System.Web.Script.Services.ScriptMethod(UseHttpGet = false, ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static Katapoka.DAO.JsonResponse Salvar(int? idTipoProjeto, string dsTipoProjeto)
    {
        Katapoka.DAO.JsonResponse response = new Katapoka.DAO.JsonResponse();

        try
        {
            Katapoka.BLL.Projeto.TipoProjetoBLL tipoProjetoBLL = new Katapoka.BLL.Projeto.TipoProjetoBLL();
            tipoProjetoBLL.Save(idTipoProjeto, dsTipoProjeto);
            response.Status = 200;
            response.Data = "OK";
        }
        catch (Exception ex)
        {
            response.Status = 500;
            response.Data = ex.Message;
        }

        return response;
    }
}