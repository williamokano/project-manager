using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CadastrarEmpresa : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual == null)
            Response.Redirect("~/Login.aspx");

        int idEmpresa = 0;

        #region POPULA DDL's
        populaAreasAtuacao();
        populaEstados();
        #endregion

        if (Request.QueryString["id"] == null || !Int32.TryParse(Request.QueryString["id"].ToString(), out idEmpresa))
        {
            ((Katapoka.Core.QuanticaMasterPage)this.Master).PageTitle = "Cadastrar nova empresa";
        }
        else
        {
            using (Katapoka.BLL.Empresa.EmpresaBLL empresaBLL = new Katapoka.BLL.Empresa.EmpresaBLL())
            {
                Katapoka.DAO.Empresa_Tb empresaTb = empresaBLL.GetById(idEmpresa);
                if (empresaTb != null)
                {
                    ((Katapoka.Core.QuanticaMasterPage)this.Master).PageTitle =
                        string.Format("Editar dados da empresa {0}", empresaTb.DsRazaoSocial);
                    preencheCampos(empresaTb);
                }
                else
                {
                    Response.Redirect("~/CadastrarEmpresa.aspx");
                }
            }
        }
        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "IdEmpresaAtual", "var IdEmpresaAtual = " + (idEmpresa == 0 ? "null" : idEmpresa.ToString()) + ";", true);
    }

    #region Métodos de preenchimento de DDL's
    public void populaAreasAtuacao()
    {
        using (Katapoka.BLL.Empresa.AreaAtuacaoBLL areaAtuacaoBLL = new Katapoka.BLL.Empresa.AreaAtuacaoBLL())
        {
            ddlAreaAtuacao.Items.Clear();
            ddlAreaAtuacao.DataSource = areaAtuacaoBLL.GetAll();
            ddlAreaAtuacao.DataValueField = "IdAreaAtuacao";
            ddlAreaAtuacao.DataTextField = "DsAreaAtuacao";
            ddlAreaAtuacao.DataBind();
            ddlAreaAtuacao.Items.Insert(0, new ListItem("Selecione uma área de atuação", ""));
        }
    }
    public void populaEstados()
    {
        using (Katapoka.BLL.Regiao.UFBLL ufBLL = new Katapoka.BLL.Regiao.UFBLL())
        {
            ddlEstados.Items.Clear();
            ddlEstados.DataSource = ufBLL.GetAll();
            ddlEstados.DataValueField = "CdUF";
            ddlEstados.DataTextField = "DsUF";
            ddlEstados.DataBind();
            ddlEstados.Items.Insert(0, new ListItem("Selecione um estado", ""));
        }
    }
    public void populaCidades(string CdUF, int? idCidade = null)
    {
        using (Katapoka.BLL.Regiao.CidadeBLL cidadeBLL = new Katapoka.BLL.Regiao.CidadeBLL())
        {
            ddlCidades.Items.Clear();
            ddlCidades.DataSource = cidadeBLL.GetAll(CdUF);
            ddlCidades.DataValueField = "IdCidade";
            ddlCidades.DataTextField = "DsNome";
            ddlCidades.DataBind();
            ddlCidades.Items.Insert(0, new ListItem("Selecione uma cidade", ""));
            if (idCidade != null && ddlCidades.Items.FindByValue(idCidade.Value.ToString()) != null)
                ddlCidades.Items.FindByValue(idCidade.Value.ToString()).Selected = true;
        }
    }
    public void populaBairros(int idCidade, int? idBairro = null)
    {
        using (Katapoka.BLL.Regiao.BairroBLL bairroBLL = new Katapoka.BLL.Regiao.BairroBLL())
        {
            ddlBairros.Items.Clear();
            ddlBairros.DataSource = bairroBLL.GetAll(idCidade);
            ddlBairros.DataTextField = "DsNome";
            ddlBairros.DataValueField = "IdBairro";
            ddlBairros.DataBind();
            ddlBairros.Items.Insert(0, new ListItem("Selecione um bairro", ""));
            if (idBairro != null)
                if (ddlBairros.Items.FindByValue(idBairro.Value.ToString()) != null)
                    ddlBairros.Items.FindByValue(idBairro.Value.ToString()).Selected = true;
        }
    }
    #endregion
    private void preencheCampos(Katapoka.DAO.Empresa_Tb empresaTb)
    {
        txtNomeFantasia.Value = empresaTb.DsNomeFantasia;
        txtRazaoSocial.Value = empresaTb.DsRazaoSocial;
        txtCnpj.Value = empresaTb.NrCnpj;
        if (ddlAreaAtuacao.Items.FindByValue(empresaTb.IdAreaAtuacao.ToString()) != null)
            ddlAreaAtuacao.Items.FindByValue(empresaTb.IdAreaAtuacao.ToString()).Selected = true;
        txtEmail.Value = empresaTb.DsEmail;
        txtUrlSite.Value = empresaTb.DsSite;
        txtSumario.Value = empresaTb.DsSumarioEmpresa;

        //Dados do enredeço
        txtCep.Value = empresaTb.Endereco_Tb.NrCep;
        txtEndereco.Value = empresaTb.Endereco_Tb.DsEndereco;
        txtNumero.Value = empresaTb.Endereco_Tb.NrEndereco;
        txtComplemento.Value = empresaTb.Endereco_Tb.DsComplemento;
        if (empresaTb.Endereco_Tb.IdCidade != null)
        {
            if (ddlEstados.Items.FindByValue(empresaTb.Endereco_Tb.Cidade_Tb.CdUF) != null)
                ddlEstados.Items.FindByValue(empresaTb.Endereco_Tb.Cidade_Tb.CdUF).Selected = true;
            populaCidades(empresaTb.Endereco_Tb.Cidade_Tb.CdUF, empresaTb.Endereco_Tb.IdCidade);
        }

        if (empresaTb.Endereco_Tb.IdBairro != null)
        {
            populaBairros(empresaTb.Endereco_Tb.Bairro_Tb.IdCidade, empresaTb.Endereco_Tb.Bairro_Tb.IdBairro);
        }
        else
        {
            ddlBairros.Items.Clear();
            ddlBairros.Items.Insert(0, new ListItem("Selecione um bairro", ""));
            ddlBairros.Items.Insert(1, new ListItem("Outro", "-1"));
            ddlBairros.Items[1].Selected = true;

            divOutroBairro.Attributes["style"] = "";
            if(!string.IsNullOrWhiteSpace(empresaTb.Endereco_Tb.DsBairroOutro))
                txtOutroBairro.Value = empresaTb.Endereco_Tb.DsBairroOutro;
        }

        txtTelefoneResidencial.Value = empresaTb.Contato_Tb.DsTelefoneResidencial;
        txtTelefoneCelular.Value = empresaTb.Contato_Tb.DsTelefoneCelular;
        txtTelefoneComercial.Value = empresaTb.Contato_Tb.DsTelefoneComercial;
        txtFax.Value = empresaTb.Contato_Tb.DsFax;
        txtObservacaoContato.Value = empresaTb.Contato_Tb.DsObservacaoContato;

        flAceiteTermo.Checked = empresaTb.FlAceiteTermo;
        flAprovada.Checked = empresaTb.FlAprovada;
    }

    [System.Web.Services.WebMethod(true)]
    [System.Web.Script.Services.ScriptMethod(UseHttpGet=false, ResponseFormat=System.Web.Script.Services.ResponseFormat.Json)]
    public static Katapoka.DAO.JsonResponse Salvar(int? idEmpresa,
        string nomeFantasia, string razaoSocial, string cnpj,
        int idAreaAtuacao, string email, string url, string sumario,
        string cep, string endereco, string numero, string complemento,
        int? idCidade, int? idBairro, string bairroNome, bool flAceite, bool flAprovada,
        string telefoneComercial, string telefoneResidencial, string telefoneCelular, string telefoneFax, string observacaoContato)
    {
        Katapoka.DAO.JsonResponse response = new Katapoka.DAO.JsonResponse(999, null);
        using (Katapoka.BLL.Empresa.EmpresaBLL empresaBLL = new Katapoka.BLL.Empresa.EmpresaBLL())
        {
            try
            {
                int retorno = empresaBLL.Salvar(idEmpresa, nomeFantasia, razaoSocial, cnpj, idAreaAtuacao, email, url, sumario, cep, endereco, numero, complemento, idCidade, idBairro, bairroNome, flAceite, flAprovada, telefoneComercial, telefoneResidencial, telefoneCelular, telefoneFax, observacaoContato);
                response.Status = 200;
                response.Data = retorno;
            }
            catch (Exception ex)
            {
                response.Status = 400;
                response.Data = ex.Message;
            }
        }
        return response;
    }

}