using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CadastrarProjeto : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual == null)
            Response.Redirect("~/Login.aspx");

        int idProjeto = 0;
        int idEmpresa = 0;

        populaDDLEmpresa();

        if (Request.QueryString["eid"] != null && Int32.TryParse(Request.QueryString["eid"].ToString(), out idEmpresa))
        {
            ListItem eli = ddlEmpresa.Items.FindByValue(idEmpresa.ToString());
            if (eli != null)
                eli.Selected = true;
        }

        populaDDLTipoProjeto();

        if (Request.QueryString["id"] != null && Int32.TryParse(Request.QueryString["id"].ToString(), out idProjeto))
        {
            //Edição
            using (Katapoka.BLL.Projeto.ProjetoBLL projetoBLL = new Katapoka.BLL.Projeto.ProjetoBLL())
            {
                Katapoka.DAO.Projeto_Tb projetoTb = projetoBLL.GetById(idProjeto);
                preencheDados(projetoTb);
            }
        }
        else
        {
            //Criação
        }
        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "IdProjeto", "var IdProjeto = " + (idProjeto > 0 ? idProjeto.ToString() : "null") + ";", true);
    }

    private void preencheDados(Katapoka.DAO.Projeto_Tb projetoTb)
    {
        txtNomeProjeto.Value = projetoTb.DsNome;
        ListItem li = ddlEmpresa.Items.FindByValue(projetoTb.IdEmpresa.ToString());
        if (li != null)
            li.Selected = true;
        if (projetoTb.DtInicioEstimado != null)
            txtDtInicio.Value = projetoTb.DtInicioEstimado.Value.ToString("dd/MM/yyyy");
        if (projetoTb.DtTerminoEstimado != null)
            txtDtTermino.Value = projetoTb.DtTerminoEstimado.Value.ToString("dd/MM/yyyy");
        if (projetoTb.DsCodigoReferencia != null)
            txtCodigoReferencia.Value = projetoTb.DsCodigoReferencia;
        ListItem liDdlFlStatus = ddlFlStatus.Items.FindByValue(projetoTb.FlStatus);
        if (liDdlFlStatus != null)
            liDdlFlStatus.Selected = true;
        ListItem liDdlTipoProjeto = ddlTipoProjeto.Items.FindByValue(projetoTb.IdTipoProjeto.ToString());
        if (liDdlTipoProjeto != null)
            liDdlTipoProjeto.Selected = true;


    }

    private void populaDDLTipoProjeto()
    {
        using (Katapoka.BLL.Projeto.TipoProjetoBLL tipoProjetoBLL = new Katapoka.BLL.Projeto.TipoProjetoBLL())
        {
            ddlTipoProjeto.Items.Clear();
            ddlTipoProjeto.DataSource = tipoProjetoBLL.GetAll();
            ddlTipoProjeto.DataValueField = "IdTipoProjeto";
            ddlTipoProjeto.DataTextField = "DsTipo";
            ddlTipoProjeto.DataBind();
            ddlTipoProjeto.Items.Insert(0, new ListItem("Selecione um tipo de projeto", ""));
        }
    }

    private void populaDDLEmpresa()
    {
        using (Katapoka.BLL.Empresa.EmpresaBLL empresaBLL = new Katapoka.BLL.Empresa.EmpresaBLL())
        {
            ddlEmpresa.Items.Clear();
            ddlEmpresa.DataSource = empresaBLL.GetAll();
            ddlEmpresa.DataTextField = "DsNomeFantasia";
            ddlEmpresa.DataValueField = "IdEmpresa";
            ddlEmpresa.DataBind();
            ddlEmpresa.Items.Insert(0, new ListItem("Selecione uma empresa", ""));
        }
    }

    [System.Web.Services.WebMethod(true)]
    [System.Web.Script.Services.ScriptMethod(UseHttpGet = false, ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static Katapoka.DAO.JsonResponse Salvar(int? idProjeto, string nome, int idEmpresa, string dtInicioStr, string dtTerminoStr, string codReferencia, string status, int idTipoProjeto)
    {
        Katapoka.DAO.JsonResponse response = new Katapoka.DAO.JsonResponse(999, null);
        if (Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual == null)
        {
            response.Status = 100;
            response.Data = "Você precisa estar conectado para executar esta ação.";
        }
        using (Katapoka.BLL.Projeto.ProjetoBLL projetoBLL = new Katapoka.BLL.Projeto.ProjetoBLL())
        {
            DateTime? dtInicio = null;
            try
            { dtInicio = (DateTime)DateTime.Parse(dtInicioStr); }
            catch
            { dtInicio = null; }

            DateTime? dtTermino = null;
            try
            { dtTermino = (DateTime)DateTime.Parse(dtTerminoStr); }
            catch
            { dtTermino = null; }

            try
            {
                string retorno = projetoBLL.Salvar(idProjeto, nome, idEmpresa, Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual.IdUsuario, dtInicio, dtTermino, codReferencia, status, idTipoProjeto);
                response.Status = 200;
                response.Data = retorno;
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Data = ex.Message;
            }
        }
        return response;
    }

}