using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teste : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string time1 = "25:00";
        string time2 = "130:59:00";
        Decimal d;
        d = Katapoka.BLL.Utilitarios.Utilitario.ConvertTimeStringToDecimal(time1);
        d = Katapoka.BLL.Utilitarios.Utilitario.ConvertTimeStringToDecimal(time2);
    }
}