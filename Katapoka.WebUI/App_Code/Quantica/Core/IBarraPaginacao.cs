using System;

namespace Katapoka.Core
{
    public interface IBarraPaginacao
    {
        global::System.Web.UI.WebControls.DropDownList DDLOrdenacao { get; }
        global::System.Web.UI.WebControls.DropDownList DDLQtdRegistrosPagina { get; }
        int PaginaAtual { get; set; }
        int QtdPaginas { get; set; }
        int QtdRegistroPagina { get; set; }
        global::System.Web.UI.WebControls.Repeater RPTPaginas { get; }
        string URLPaginacao { get; set; }
        bool DDLOrdenacaoVisible { get; set; }
    }
}