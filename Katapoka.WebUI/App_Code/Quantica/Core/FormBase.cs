using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Katapoka.Core
{
    public class FormBase : System.Web.UI.Page
    {
        public bool IgnoreVerifyForm { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            TratarPaginaPesquisa();
        }

        private void TratarPaginaPesquisa()
        {
            IPaginaPesquisa paginaPesquisa = this as IPaginaPesquisa;
            if (paginaPesquisa != null)
            {
                if (!IsPostBack)
                {
                    Repeater rpt = paginaPesquisa.PopularGrid();
                    for (int i = 0; i < rpt.Parent.Controls.Count; i++)
                    {
                        if (rpt.Parent.Controls[i] == rpt)
                        {
                            Literal ltr = new Literal();
                            ltr.Text = "<div id=\"divGrid\">";
                            rpt.Parent.Controls.AddAt(i, ltr);

                            ltr = new Literal();
                            ltr.Text = "</div>";
                            rpt.Parent.Controls.AddAt(i + 2, ltr);
                            break;
                        }
                    }
                }
            }
        }

        protected static FormBase GetCurrentPage()
        {
            if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.Handler == null || !(System.Web.HttpContext.Current.Handler is FormBase))
                throw new Exception("Esse método de busca por Ajax somente deve ser utilizado em páginas que implementem a classe \"FormBase\".");

            FormBase page = (FormBase)System.Web.HttpContext.Current.Handler;
            System.Reflection.MethodInfo setIntrinsics = typeof(System.Web.UI.Page).GetMethod("SetIntrinsics", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, new Type[] { typeof(System.Web.HttpContext) }, null);
            setIntrinsics.Invoke(page, new object[] { System.Web.HttpContext.Current });
            page.FrameworkInitialize();
            System.Reflection.MethodInfo performePreInit = typeof(System.Web.UI.Page).GetMethod("PerformPreInit", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            performePreInit.Invoke(page, null);

            return page;
        }

        [System.Web.Services.WebMethod()]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false, ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static string PopularGridAjax()
        {
            try
            {
                if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.Handler == null || !(System.Web.HttpContext.Current.Handler is IPaginaPesquisa))
                    throw new Exception("Esse método de busca por Ajax somente deve ser utilizado em páginas que implementem a interface \"IPaginaPesquisa\".");

                FormBase page = GetCurrentPage();
                IPaginaPesquisa paginaPesquisa = page as IPaginaPesquisa;

                page.IgnoreVerifyForm = true;
                return RenderControl(paginaPesquisa.PopularGrid());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected static string RenderControl(Control control)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.StringWriter sw = new System.IO.StringWriter(sb);
            System.Web.UI.Html32TextWriter htmlWriter = new Html32TextWriter(sw);

            control.RenderControl(htmlWriter);

            htmlWriter.Close();
            sw.Close();
            return sb.ToString();
        }

        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {
            if (!this.IgnoreVerifyForm)
                base.VerifyRenderingInServerForm(control);
        }

    }
}