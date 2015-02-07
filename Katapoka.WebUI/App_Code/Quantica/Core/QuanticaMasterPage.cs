using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for QuanticaMasterPage
/// </summary>
namespace Katapoka.Core
{
    public class QuanticaMasterPage : System.Web.UI.MasterPage
    {
        private List<string> pageTitle = new List<string>();
        public QuanticaMasterPage()
        {
            pageTitle.Add(Katapoka.BLL.Constantes.NomeSite);
        }
        public string PageTitle
        {
            get
            {
                List<string> pageTitleTempo = new List<string>(pageTitle);
                pageTitleTempo.Reverse();
                return string.Join(" / ", pageTitleTempo.ToArray());
            }

            set
            {
                this.pageTitle.Add(value);
            }
        }
    }
}