using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CadastrarUsuario : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual == null)
            Response.Redirect("~/Login.aspx");
        populaDDLCargo();
        populaDDLNivel();

        int idUsuario = 0;

        if (Request.QueryString["id"] != null && Int32.TryParse(Request.QueryString["id"].ToString(), out idUsuario))
        {
            using (Katapoka.BLL.Usuario.UsuarioBLL usuarioBLL = new Katapoka.BLL.Usuario.UsuarioBLL())
            {
                Katapoka.DAO.Usuario_Tb usuarioTb = usuarioBLL.GetById(idUsuario);
                populaDados(usuarioTb);
            }
        }

        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "IdUsuario", "var IdUsuario = " + (idUsuario > 0 ? idUsuario.ToString() : "null") + ";", true);

    }

    private void populaDados(Katapoka.DAO.Usuario_Tb usuarioTb)
    {
        txtNome.Value = usuarioTb.DsNome;
        txtEmail.Value = usuarioTb.DsEmail;

        ListItem lin = ddlNivel.Items.FindByValue(usuarioTb.IdUsuarioNivel.ToString());
        if (lin != null)
            lin.Selected = true;
        
        ListItem lic = ddlCargo.Items.FindByValue(usuarioTb.IdCargo.ToString());
        if (lic != null)
            lic.Selected = true;
    }

    private void populaDDLCargo()
    {
        ddlCargo.Items.Clear();
        using (Katapoka.BLL.Usuario.CargoBLL cargoBLL = new Katapoka.BLL.Usuario.CargoBLL())
        {
            ddlCargo.DataSource = cargoBLL.GetAll();
            ddlCargo.DataValueField = "IdCargo";
            ddlCargo.DataTextField = "DsCargo";
            ddlCargo.DataBind();
            ddlCargo.Items.Insert(0, new ListItem("Selecione um cargo", ""));
        }
    }

    private void populaDDLNivel()
    {
        ddlNivel.Items.Clear();
        using (Katapoka.BLL.Usuario.UsuarioNivelBLL usuarioNivelBLL = new Katapoka.BLL.Usuario.UsuarioNivelBLL())
        {
            ddlNivel.DataSource = usuarioNivelBLL.GetAll();
            ddlNivel.DataValueField = "IdUsuarioNivel";
            ddlNivel.DataTextField = "DsNivel";
            ddlNivel.DataBind();
            ddlNivel.Items.Insert(0, new ListItem("Selecione um nível", ""));
        }
    }


    [System.Web.Services.WebMethod(true)]
    [System.Web.Script.Services.ScriptMethod(UseHttpGet = false, ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static Katapoka.DAO.JsonResponse Salvar(int? idUsuario, string nome, string email, string senha, int idNivel, int idCargo)
    {
        Katapoka.DAO.JsonResponse response = new Katapoka.DAO.JsonResponse();

        if (Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual == null)
        {
            response.Status = 300;
            response.Data = "Por favor, faça login!";
        }
        else
        {
            using (Katapoka.BLL.Usuario.UsuarioBLL usuarioBLL = new Katapoka.BLL.Usuario.UsuarioBLL())
            {
                try
                {
                    usuarioBLL.Save(idUsuario, nome, email, senha, idNivel, idCargo);
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