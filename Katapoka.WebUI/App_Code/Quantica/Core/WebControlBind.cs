using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Katapoka.Core
{
    /// <summary>
    /// Summary description for WebControlBind
    /// </summary>
    public static class WebControlBind
    {
        #region Bind Repeater

        private static CarregaDadosDatabound DadosDatabound
        {
            get
            {
                return System.Web.HttpContext.Current.Items["RepeaterBindParameter"] as CarregaDadosDatabound;
            }
            set
            {
                if (value != null)
                    System.Web.HttpContext.Current.Items["RepeaterBindParameter"] = value;
                else
                    System.Web.HttpContext.Current.Items.Remove("RepeaterBindParameter");
            }
        }

        public static int PaginaAtual
        {
            get
            {
                int paginaAtual = -1;
                System.Web.HttpContext current = System.Web.HttpContext.Current;
                if (current.Items.Contains(typeof(WebControlBind).FullName + "_pg"))
                    paginaAtual = (int)current.Items[typeof(WebControlBind).FullName + "_pg"];
                if (paginaAtual == -1 && current.Request.QueryString["pg"] != null)
                {
                    int.TryParse(current.Request.QueryString["pg"], out paginaAtual);
                    current.Items[typeof(WebControlBind).FullName + "_pg"] = paginaAtual;
                }
                return paginaAtual;
            }
            private set
            {
                System.Web.HttpContext current = System.Web.HttpContext.Current;
                if (value != -1)
                    current.Items[typeof(WebControlBind).FullName + "_pg"] = value;
                else
                    current.Items.Remove(typeof(WebControlBind).FullName + "_pg");
            }
        }

        public static int QtdRegistrosPagina
        {
            get
            {
                System.Web.HttpContext current = System.Web.HttpContext.Current;
                int qtdRegistrosPagina = 20;
                if (current.Request.QueryString["qtd"] != null)
                    int.TryParse(current.Request.QueryString["qtd"], out qtdRegistrosPagina);
                return qtdRegistrosPagina;
            }
        }

        public static int TotalRegistros
        {
            get
            {
                int totalRegistros = -1;
                System.Web.HttpContext current = System.Web.HttpContext.Current;
                if (current.Items.Contains(typeof(WebControlBind).FullName + "_total_registros"))
                    totalRegistros = (int)current.Items[typeof(WebControlBind).FullName + "_total_registros"];
                return totalRegistros;
            }
            set
            {
                int totalRegistros = value;

                System.Web.HttpContext current = System.Web.HttpContext.Current;
                if (value != -1)
                    current.Items[typeof(WebControlBind).FullName + "_total_registros"] = value;
                else
                    current.Items.Remove(typeof(WebControlBind).FullName + "_total_registros");

                int nroPaginas = (int)Math.Ceiling((decimal)totalRegistros / QtdRegistrosPagina);
                if (nroPaginas <= PaginaAtual) PaginaAtual = nroPaginas - 1;
                if (PaginaAtual < 0) PaginaAtual = 0;
            }
        }

        public static void RepeaterBind<T>(System.Web.UI.WebControls.Repeater repeater, IList<T> dataSource)
        {
            RepeaterBind<T>(repeater, dataSource, PaginaAtual, QtdRegistrosPagina, TotalRegistros, null, null, null);
            PaginaAtual = -1;
            TotalRegistros = -1;
        }

        public static void RepeaterBind<T>(System.Web.UI.WebControls.Repeater repeater, IList<T> dataSource, int paginaAtual, int qtdRegistrosPagina, int totalRegistros, int[] opcoesRegistroPagina, PopularDropDownListOrdernacao popularDropDownListOrdernacao, RepeaterItemEventHandler onItemDataBound)
        {
            int nroPaginas = (int)Math.Ceiling((decimal)totalRegistros / qtdRegistrosPagina);
            if (nroPaginas <= paginaAtual) paginaAtual = nroPaginas - 1;
            if (paginaAtual < 0) paginaAtual = 0;

            if (opcoesRegistroPagina == null)
                opcoesRegistroPagina = new int[] { 20, 40, 60, 80, 100, 160, 200 };

            DadosDatabound = new CarregaDadosDatabound() { PaginaAtual = paginaAtual, TotalPaginas = nroPaginas, QtdRegistrosPagina = qtdRegistrosPagina, TotalRegistros = totalRegistros, OpcoesRegistroPagina = opcoesRegistroPagina, PopularDropDownListOrdernacao = popularDropDownListOrdernacao, OnItemDataBound = onItemDataBound };

            repeater.ItemDataBound += new RepeaterItemEventHandler(repeater_ItemDataBound);
            repeater.DataSource = dataSource;
            repeater.DataBind();

            DadosDatabound = null;
        }

        private static void repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            CarregaDadosDatabound dadosDatabound = DadosDatabound;
            if (dadosDatabound != null)
            {
                if (e.Item.ItemType == ListItemType.Header || e.Item.ItemType == ListItemType.Footer)
                {
                    IBarraPaginacao barraPaginacao = FindControlByTypeIBarraPaginacao(e.Item.Controls);
                    if (barraPaginacao != null)
                    {
                        barraPaginacao.PaginaAtual = dadosDatabound.PaginaAtual;
                        barraPaginacao.QtdPaginas = dadosDatabound.TotalPaginas;
                        barraPaginacao.QtdRegistroPagina = dadosDatabound.QtdRegistrosPagina;

                        barraPaginacao.DDLQtdRegistrosPagina.DataSource = dadosDatabound.OpcoesRegistroPagina;
                        barraPaginacao.DDLQtdRegistrosPagina.DataBind();
                        barraPaginacao.DDLQtdRegistrosPagina.SelectedValue = dadosDatabound.QtdRegistrosPagina.ToString();

                        if (dadosDatabound.TotalPaginas > 0)
                        {
                            barraPaginacao.RPTPaginas.DataSource = new int[dadosDatabound.TotalPaginas];
                            barraPaginacao.RPTPaginas.DataBind();
                        }
                        else
                            barraPaginacao.RPTPaginas.Visible = false;

                        if (dadosDatabound.PopularDropDownListOrdernacao != null)
                        {
                            barraPaginacao.DDLOrdenacaoVisible = dadosDatabound.PopularDropDownListOrdernacao(barraPaginacao.DDLOrdenacao);
                            if (!string.IsNullOrWhiteSpace(System.Web.HttpContext.Current.Request.QueryString["order"]))
                                barraPaginacao.DDLOrdenacao.SelectedValue = System.Web.HttpContext.Current.Request.QueryString["order"];
                        }
                        else
                            barraPaginacao.DDLOrdenacaoVisible = false;
                    }
                }

                if (dadosDatabound.OnItemDataBound != null)
                    dadosDatabound.OnItemDataBound(sender, e);
            }
        }

        private static IBarraPaginacao FindControlByTypeIBarraPaginacao(ControlCollection controles)
        {
            if (controles != null)
            {
                foreach (Control controle in controles)
                {
                    if (controle is IBarraPaginacao || controles.GetType().IsSubclassOf(typeof(IBarraPaginacao)))
                        return (IBarraPaginacao)controle;
                    else
                    {
                        IBarraPaginacao filho = FindControlByTypeIBarraPaginacao(controle.Controls);
                        if (filho != null)
                            return filho;
                    }
                }
            }
            return null;
        }

        private class CarregaDadosDatabound
        {
            public int PaginaAtual { get; set; }
            public int TotalPaginas { get; set; }
            public int QtdRegistrosPagina { get; set; }
            public int TotalRegistros { get; set; }
            public int[] OpcoesRegistroPagina { get; set; }
            public PopularDropDownListOrdernacao PopularDropDownListOrdernacao { get; set; }
            public RepeaterItemEventHandler OnItemDataBound { get; set; }
        }

        /// <summary>
        /// Realiza o bind em um determinado DropDownList
        /// </summary>
        /// <typeparam name="T">Tipo de objeto do datasource</typeparam>
        /// <param name="dropDown">dropDownList</param>
        /// <param name="dataSource">datasource</param>
        /// <param name="dataValueField">dataSourceId</param>
        /// <param name="dataTextField">dataTextField</param>
        /// Criado por Alan Lopes Melo em 20/07/2012
        internal static void DropDownListBind<T>(System.Web.UI.WebControls.DropDownList dropDown,
                                                    IList<T> dataSource,
                                                    string dataValueField,
                                                    string dataTextField,
                                                    string textoPadrao)
        {
            dropDown.Items.Clear();
            dropDown.DataValueField = dataValueField;
            dropDown.DataTextField = dataTextField;
            dropDown.DataSource = dataSource;
            dropDown.DataBind();
            dropDown.Items.Insert(0, new System.Web.UI.WebControls.ListItem(textoPadrao, "-1"));
        }

        public delegate bool PopularDropDownListOrdernacao(System.Web.UI.WebControls.DropDownList dropDownList);

        #endregion
    }
}