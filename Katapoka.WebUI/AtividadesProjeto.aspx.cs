using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Newtonsoft.Json;

public partial class AtividadesProjeto : System.Web.UI.Page
{

    public static List<string> usuariosOnline = new List<string>();

    IList<Katapoka.DAO.Atividade_Tb> atividades = null;
    IList<Katapoka.DAO.Atividade.TipoAtividade> atividadesSugeridas = null;
    Dictionary<int, IList<Katapoka.DAO.Tag.TagCompleta>> tags = new Dictionary<int, IList<Katapoka.DAO.Tag.TagCompleta>>();
    Dictionary<int, IList<Katapoka.DAO.UsuarioCompleto>> usuarios = new Dictionary<int, IList<Katapoka.DAO.UsuarioCompleto>>();
    Dictionary<int, IList<Katapoka.DAO.UsuarioCompleto>> usuariosSugeridos = new Dictionary<int, IList<Katapoka.DAO.UsuarioCompleto>>();
    public int IdIncremento = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual == null)
            Response.Redirect("~/Default.aspx");

        string hashLogin = Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual.DsHashLogin;
        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "HashLogin", string.Format("var hashLogin = '{0}';", hashLogin), true);
        lock (usuariosOnline)
        {
            if (usuariosOnline.Where(p => p == hashLogin).Count() == 0)
            {
                usuariosOnline.Add(hashLogin);
            }
        }
        //Join ajax reverso
        Katapoka.AjaxReverso.ClientAdapter.Instance.Join(hashLogin);

        if (Request.QueryString["pid"] == null)
            Response.Redirect("~/Default.aspx");

        int idProjeto = 0;
        if (!Int32.TryParse(Request.QueryString["pid"].ToString(), out idProjeto))
            Response.Redirect("~/Default.aspx");
        Katapoka.DAO.Projeto_Tb projetoTb = new Katapoka.BLL.Projeto.ProjetoBLL().GetById(idProjeto);

        Katapoka.BLL.Atividade.AtividadeBLL.StatusAtividade? statusAtividade = null;
        if (Request.QueryString["status"] != null)
            statusAtividade = (Katapoka.BLL.Atividade.AtividadeBLL.StatusAtividade?)Enum.Parse(typeof(Katapoka.BLL.Atividade.AtividadeBLL.StatusAtividade), Request.QueryString["status"]);
        else
            statusAtividade = Katapoka.BLL.Atividade.AtividadeBLL.StatusAtividade.Ativa;

        using (Katapoka.BLL.Atividade.AtividadeBLL atividadeBLL = new Katapoka.BLL.Atividade.AtividadeBLL())
        {
            atividades = atividadeBLL.GetFiltroQueryAtividades(null, idProjeto, null, null, null, null, null, Katapoka.BLL.Atividade.AtividadeBLL.OrdenacaoAtividade.IdC, statusAtividade);
            rptAtividades.DataSource = atividades
                .Where(p => p.IdAtividadePredecessora == null).ToList();
            rptAtividades.DataBind();
        }

        bool ExibirSugeridas = atividades.Count == 0 && Request.QueryString["sugeridas"] != null && Convert.ToBoolean(Request.QueryString["sugeridas"].ToString());
        if (ExibirSugeridas)
        {
            using (Katapoka.BLL.Atividade.TipoAtividadeBLL tipoAtividadeBLL = new Katapoka.BLL.Atividade.TipoAtividadeBLL())
            {
                atividadesSugeridas = tipoAtividadeBLL.GetTipoAtividadePorTipoProjeto(projetoTb.IdTipoProjeto);
                rptAtividadesSugeridas.DataSource = atividadesSugeridas.Where(p => p.IdTipoAtividadePredecessora == null).ToList();
                rptAtividadesSugeridas.DataBind();
                rptAtividades.Visible = rptAtividades.Items.Count > 0;
            }
        }

        ltrNomeProjeto.Text = string.Format("Projeto: {0} - Código: #{1}", projetoTb.DsNome, projetoTb.DsCodigoReferencia);

        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Tags", string.Format("var tags = {0};", JsonConvert.SerializeObject(tags)), true);
        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Usuarios", string.Format("var usuarios = {0};", JsonConvert.SerializeObject(usuarios)), true);
        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "UsuariosSugeridos", string.Format("var usuariosSugeridos = {0};", JsonConvert.SerializeObject(usuariosSugeridos)), true);
        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "IdProjeto", "var IdProjeto = " + idProjeto.ToString() + ";", true);
        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Lid", "var LocalIdCounter = " + IdIncremento.ToString() + ";", true);

    }
    protected void rptAtividades_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            Repeater rptInterno = (Repeater)e.Item.FindControl("rptAtividadesInterna");
            Katapoka.DAO.Atividade_Tb item = (Katapoka.DAO.Atividade_Tb)e.Item.DataItem;
            Literal ltrIdLocal = (Literal)e.Item.FindControl("ltrIdLocal");

            //Adiciona as tags
            tags.Add(item.IdAtividade, item.AtividadeTag_Tb.Select(p => new Katapoka.DAO.Tag.TagCompleta()
            {
                DsTag = p.Tag_Tb.DsTag,
                IdTag = p.IdTag
            }).ToList());

            //Adiciona os usuários designados
            usuarios.Add(item.IdAtividade, item.AtividadeUsuario_Tb.Select(p => new Katapoka.DAO.UsuarioCompleto()
            {
                IdUsuario = p.IdUsuario,
                DsNome = p.Usuario_Tb.DsNome
            }).ToList());

            ltrIdLocal.Text = IdIncremento.ToString();
            IdIncremento++;

            #region PREENCHE OS DADOS
            Literal ltrTituloAtividade = (Literal)e.Item.FindControl("ltrTituloAtividade");
            HtmlInputText txtTituloAtividade = (HtmlInputText)e.Item.FindControl("txtTituloAtividade");
            if (txtTituloAtividade != null)
                txtTituloAtividade.Value = !string.IsNullOrWhiteSpace(item.DsTituloAtividade) ? item.DsTituloAtividade : "Título da atividade";
            ltrTituloAtividade.Text = txtTituloAtividade.Value;

            HtmlInputText txtQtTempoEstimado = (HtmlInputText)e.Item.FindControl("txtQtTempoEstimado");
            if (txtQtTempoEstimado != null)
            {
                if (item.QtTempoEstimado != null)
                {
                    TimeSpan tempoEstimado = TimeSpan.FromHours((double)item.QtTempoEstimado.Value);
                    txtQtTempoEstimado.Value = string.Format("{0:000}:{1:00}", Math.Floor(tempoEstimado.TotalHours), tempoEstimado.Minutes);
                }
            }

            HtmlInputText txtQtTempoExecutado = (HtmlInputText)e.Item.FindControl("txtQtTempoExecutado");
            if (txtQtTempoExecutado != null)
            {
                TimeSpan tempoExecutado = TimeSpan.FromHours((double)item.QtTempoExecutado);
                txtQtTempoExecutado.Value = string.Format("{0:000}:{1:00}", Math.Floor(tempoExecutado.TotalHours), tempoExecutado.Minutes);
            }

            HtmlInputText txtVrCompletoPorcentagem = (HtmlInputText)e.Item.FindControl("txtVrCompletoPorcentagem");
            if (txtVrCompletoPorcentagem != null && item.VrCompletoPorcentagem != null)
                txtVrCompletoPorcentagem.Value = item.VrCompletoPorcentagem.ToString();

            HtmlInputText txtDtInicio = (HtmlInputText)e.Item.FindControl("txtDtInicio");
            if (txtDtInicio != null && item.DtInicio != null)
                txtDtInicio.Value = item.DtInicio.Value.ToString("dd/MM/yyyy");

            HtmlInputText txtDtTermino = (HtmlInputText)e.Item.FindControl("txtDtTermino");
            if (txtDtTermino != null && item.DtTermino != null)
                txtDtTermino.Value = item.DtTermino.Value.ToString("dd/MM/yyyy");

            HtmlTextArea txtDescricaoAtividade = (HtmlTextArea)e.Item.FindControl("txtDescricaoAtividade");
            txtDescricaoAtividade.InnerText = item.DsAtividade;
            #endregion

            //rptInterno.HeaderTemplate = rptAtividades.HeaderTemplate;
            //rptInterno.FooterTemplate = rptAtividades.FooterTemplate;
            rptInterno.ItemTemplate = rptAtividades.ItemTemplate;
            rptInterno.AlternatingItemTemplate = rptAtividades.AlternatingItemTemplate;
            rptInterno.ItemDataBound += rptAtividades_ItemDataBound;
            rptInterno.DataSource = atividades.Where(p => p.IdAtividadePredecessora == item.IdAtividade).ToList();
            rptInterno.DataBind();
        }
    }
    protected void rptAtividadesSugeridas_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            Repeater rptAtividadesSugeridasInterna = (Repeater)e.Item.FindControl("rptAtividadesSugeridasInterna");
            Literal ltrIdLocal = (Literal)e.Item.FindControl("ltrIdLocal");
            Katapoka.DAO.Atividade.TipoAtividade item = (Katapoka.DAO.Atividade.TipoAtividade)e.Item.DataItem;

            Katapoka.DAO.Usuario_Tb usuarioAssociado = item.IdTipoAtividade != null ? new Katapoka.BLL.Atividade.AtividadeBLL()
                                                            .GetUsuariosPermitidos(item.IdTipoAtividade.Value)
                                                            .FirstOrDefault() : null;
            if (usuarioAssociado != null)
            {
                IList<Katapoka.DAO.UsuarioCompleto> usuarioCompleto = new List<Katapoka.DAO.UsuarioCompleto>();
                usuarioCompleto.Add(new Katapoka.DAO.UsuarioCompleto()
                {
                    DsNome = usuarioAssociado.DsNome,
                    IdUsuario = usuarioAssociado.IdUsuario
                });
                usuariosSugeridos.Add(IdIncremento, usuarioCompleto);
            }

            ltrIdLocal.Text = IdIncremento.ToString();
            IdIncremento++;

            #region PREENCHE OS DADOS
            HtmlInputText txtTituloAtividade = (HtmlInputText)e.Item.FindControl("txtTituloAtividade");
            if (txtTituloAtividade != null)
                txtTituloAtividade.Value = !string.IsNullOrWhiteSpace(item.DsTituloAtividade) ? item.DsTituloAtividade : "Título da atividade";

            HtmlInputText txtQtTempoEstimado = (HtmlInputText)e.Item.FindControl("txtQtTempoEstimado");
            if (txtQtTempoEstimado != null)
            {
                TimeSpan tempoEstimado = TimeSpan.FromHours((double)item.QtTempoEstimado);
                txtQtTempoEstimado.Value = string.Format("{0:000}:{1:00}", Math.Floor(tempoEstimado.TotalHours), tempoEstimado.Minutes);
            }

            HtmlInputText txtQtTempoExecutado = (HtmlInputText)e.Item.FindControl("txtQtTempoExecutado");
            if (txtQtTempoExecutado != null)
                txtQtTempoExecutado.Value = "000:00";

            HtmlInputText txtVrCompletoPorcentagem = (HtmlInputText)e.Item.FindControl("txtVrCompletoPorcentagem");
            txtVrCompletoPorcentagem.Value = "0";

            HtmlInputText txtDtInicio = (HtmlInputText)e.Item.FindControl("txtDtInicio");
            txtDtInicio.Value = DateTime.Now.ToString("dd/MM/yyyy");

            HtmlInputText txtDtTermino = (HtmlInputText)e.Item.FindControl("txtDtTermino");
            txtDtTermino.Value = DateTime.Now.Add(TimeSpan.FromHours((double)item.QtTempoEstimado)).ToString("dd/MM/yyyy");

            HtmlTextArea txtDescricaoAtividade = (HtmlTextArea)e.Item.FindControl("txtDescricaoAtividade");
            txtDescricaoAtividade.InnerText = item.DsAtividade;
            #endregion

            rptAtividadesSugeridasInterna.ItemTemplate = rptAtividadesSugeridas.ItemTemplate;
            rptAtividadesSugeridasInterna.AlternatingItemTemplate = rptAtividadesSugeridas.AlternatingItemTemplate;
            rptAtividadesSugeridasInterna.ItemDataBound += rptAtividadesSugeridas_ItemDataBound;
            rptAtividadesSugeridasInterna.DataSource = atividadesSugeridas
                                                    .Where(p => p.IdTipoAtividadePredecessora == item.IdTipoAtividade)
                                                    .ToList();
            rptAtividadesSugeridasInterna.DataBind();
        }
    }

    [System.Web.Services.WebMethod(true)]
    [System.Web.Script.Services.ScriptMethod(UseHttpGet = false, ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static Katapoka.DAO.JsonResponse Salvar(int idProjeto, List<Katapoka.DAO.Atividade.AtividadeAjaxPost> atividades)
    {
        Katapoka.DAO.JsonResponse response = new Katapoka.DAO.JsonResponse();
        try
        {
            Katapoka.BLL.Projeto.ProjetoBLL projetoBLL = new Katapoka.BLL.Projeto.ProjetoBLL();
            IList<Katapoka.DAO.Atividade.AtividadeAjaxPost> ret = projetoBLL.SalvarAtividades(idProjeto, atividades, Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual.IdUsuario);
            response.Status = 200;
            response.Data = ret;

            //Tenta mandar um push para todos que estão na página
            try
            {
                lock (usuariosOnline)
                {
                    foreach (string hashLogin in usuariosOnline
                        .Where(p => p != Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual.DsHashLogin).ToList())
                    {
                        Katapoka.AjaxReverso.Message message = new Katapoka.AjaxReverso.Message();
                        message.RecipientName = hashLogin;
                        message.MessageContent = JsonConvert.SerializeObject(ret);
                        Katapoka.AjaxReverso.ClientAdapter.Instance.SendMessage(message);
                    }
                }
            }
            catch { }
        }
        catch (Exception ex)
        {
            response.Status = 500;
            response.Data = ex.Message + (ex.InnerException != null ? "\n" + ex.InnerException.Message : "");
        }
        return response;
    }
}