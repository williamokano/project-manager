using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Site : Katapoka.Core.QuanticaMasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ltrTituloMasterPage.Text = ((Katapoka.Core.QuanticaMasterPage)this.Page.Master).PageTitle;
    }
}
