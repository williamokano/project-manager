using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Newtonsoft.Json;

public partial class AtividadesTipoProjeto : System.Web.UI.Page
{

    public static List<string> usuariosOnline = new List<string>();

    IList<Katapoka.DAO.Atividade.TipoAtividade> atividades = null;
    
    Dictionary<int, IList<Katapoka.DAO.Tag.TagCompleta>> tags = new Dictionary<int, IList<Katapoka.DAO.Tag.TagCompleta>>();
    //Dictionary<int, IList<Katapoka.DAO.UsuarioCompleto>> usuarios = new Dictionary<int, IList<Katapoka.DAO.UsuarioCompleto>>();

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

        int idTipoProjeto = 0;

        if (!Int32.TryParse(Request.QueryString["pid"].ToString(), out idTipoProjeto))
            Response.Redirect("~/Default.aspx");

        Katapoka.DAO.TipoProjeto_Tb tipoProjetoTb = new Katapoka.BLL.Projeto.TipoProjetoBLL().GetById(idTipoProjeto);

        Katapoka.BLL.Atividade.AtividadeBLL.StatusAtividade? statusAtividade = null;
        if (Request.QueryString["status"] != null)
            statusAtividade = (Katapoka.BLL.Atividade.AtividadeBLL.StatusAtividade?)Enum.Parse(typeof(Katapoka.BLL.Atividade.AtividadeBLL.StatusAtividade), Request.QueryString["status"]);
        else
            statusAtividade = Katapoka.BLL.Atividade.AtividadeBLL.StatusAtividade.Ativa;

        using (Katapoka.BLL.Atividade.TipoAtividadeBLL tipoAtividadeBLL = new Katapoka.BLL.Atividade.TipoAtividadeBLL())
        {
            atividades = tipoAtividadeBLL.GetTipoAtividadePorTipoProjeto(idTipoProjeto);
            rptAtividades.DataSource = atividades
                .Where(p => p.IdTipoAtividadePredecessora == null).ToList();
            rptAtividades.DataBind();
        }

        ltrNomeProjeto.Text = string.Format("Tipo de Projeto: {0}", tipoProjetoTb.DsTipo);

        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Tags", string.Format("var tags = {0};", JsonConvert.SerializeObject(tags)), true);
        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "IdTipoProjeto", "var IdTipoProjeto = " + idTipoProjeto.ToString() + ";", true);
        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Lid", "var LocalIdCounter = " + IdIncremento.ToString() + ";", true);

    }
    protected void rptAtividades_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            Repeater rptInterno = (Repeater)e.Item.FindControl("rptAtividadesInterna");
            Katapoka.DAO.Atividade.TipoAtividade item = (Katapoka.DAO.Atividade.TipoAtividade)e.Item.DataItem;
            Literal ltrIdLocal = (Literal)e.Item.FindControl("ltrIdLocal");

            //Adiciona as tags
            tags.Add(item.IdTipoAtividade.Value, new Katapoka.BLL.Tag.TipoAtividadeTagBLL()
                .GetTagsTipoAtividade(item.IdTipoAtividade.Value)
                .Select(p => new Katapoka.DAO.Tag.TagCompleta()
            {
                DsTag = p.DsTag,
                IdTag = p.IdTag
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
                    TimeSpan tempoEstimado = TimeSpan.FromHours((double)item.QtTempoEstimado);
                    txtQtTempoEstimado.Value = string.Format("{0:000}:{1:00}", Math.Floor(tempoEstimado.TotalHours), tempoEstimado.Minutes);
                }
            }
            
            HtmlTextArea txtDescricaoAtividade = (HtmlTextArea)e.Item.FindControl("txtDescricaoAtividade");
            txtDescricaoAtividade.InnerText = item.DsAtividade;
            #endregion
            
            rptInterno.ItemTemplate = rptAtividades.ItemTemplate;
            rptInterno.AlternatingItemTemplate = rptAtividades.AlternatingItemTemplate;
            rptInterno.ItemDataBound += rptAtividades_ItemDataBound;
            rptInterno.DataSource = atividades.Where(p => p.IdTipoAtividadePredecessora == item.IdTipoAtividade).ToList();
            rptInterno.DataBind();
        }
    }

    [System.Web.Services.WebMethod(true)]
    [System.Web.Script.Services.ScriptMethod(UseHttpGet = false, ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static Katapoka.DAO.JsonResponse Salvar(int idTipoProjeto, List<Katapoka.DAO.Atividade.AtividadeAjaxPost> atividades)
    {
        Katapoka.DAO.JsonResponse response = new Katapoka.DAO.JsonResponse();
        try
        {
            Katapoka.BLL.Projeto.TipoProjetoBLL tipoProjetoBLL = new Katapoka.BLL.Projeto.TipoProjetoBLL();
            IList<Katapoka.DAO.Atividade.AtividadeAjaxPost> ret = tipoProjetoBLL.InserirTipoAtividades(idTipoProjeto, atividades);
            response.Status = 200;
            response.Data = ret;
        }
        catch (Exception ex)
        {
            response.Status = 500;
            response.Data = ex.Message;
        }
        //try
        //{
        //    Katapoka.BLL.Projeto.ProjetoBLL projetoBLL = new Katapoka.BLL.Projeto.ProjetoBLL();
        //    IList<Katapoka.DAO.Atividade.AtividadeAjaxPost> ret = projetoBLL.SalvarAtividades(idProjeto, atividades, Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual.IdUsuario);
        //    response.Status = 200;
        //    response.Data = ret;
        //
        //    //Tenta mandar um push para todos que estão na página
        //    try
        //    {
        //        lock (usuariosOnline)
        //        {
        //            foreach (string hashLogin in usuariosOnline
        //                .Where(p => p != Katapoka.BLL.Autenticacao.Usuario.UsuarioAtual.DsHashLogin).ToList())
        //            {
        //                Katapoka.AjaxReverso.Message message = new Katapoka.AjaxReverso.Message();
        //                message.RecipientName = hashLogin;
        //                message.MessageContent = JsonConvert.SerializeObject(ret);
        //                Katapoka.AjaxReverso.ClientAdapter.Instance.SendMessage(message);
        //            }
        //        }
        //    }
        //    catch { }
        //}
        //catch (Exception ex)
        //{
        //    response.Status = 500;
        //    response.Data = ex.Message + (ex.InnerException != null ? "\n" + ex.InnerException.Message : "");
        //}
        return response;
    }
}