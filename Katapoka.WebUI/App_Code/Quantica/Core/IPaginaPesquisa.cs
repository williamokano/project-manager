using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Katapoka.Core
{
    /// <summary>
    /// Summary description for IPaginaPesquisa
    /// </summary>
    public interface IPaginaPesquisa
    {
        System.Web.UI.WebControls.Repeater PopularGrid();
    }
}