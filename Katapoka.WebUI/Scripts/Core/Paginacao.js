$(document).ready(function () {
    BindControlesPaginacao();
});
function RepopularGrid(urlBase) {
    //Manter a última ordenação
    if (urlBase.indexOf("order=") == -1) {
        var objQuery = $.QueryString();
        for (prop in objQuery) {
            if (prop == "order") {
                if (urlBase != null && urlBase.length > 0 && !urlBase.endsWith("&"))
                    urlBase += "&";
                urlBase += prop + "=" + objQuery[prop];
            }
        }
    }

    ExecutaFuncao(PegarEnderecoPagina() + "/" + "PopularGridAjax?" + urlBase, { }, function (retorno) {
        $(".pagination a").unbind('click');
        $("#ddlQtdRegistrosPagina").unbind('change');
        $("#ddlOrdenacao").bind('change');

        $("#divGrid").html(retorno);

        BindControlesPaginacao();

        window.history.pushState({ htmlGrid: retorno }, null, PegarEnderecoPagina() + "?" + urlBase);
    });
}

function BindControlesPaginacao() {
    $(".pagination a").bind('click', function (event) {
        var urlLink = $(this).attr("href");
        if (urlLink.indexOf("?") >= 0)
            urlLink = urlLink.substring(urlLink.indexOf("?") + 1);
        RepopularGrid(urlLink);
        event.preventDefault();
        return false;
    });

    $("#ddlQtdRegistrosPagina").bind('change', function (event) {
        var objQuery = $.QueryString();
        var possuiQtd = false;
        var urlBase = "";
        for (prop in objQuery) {
            if (prop == "qtd") {
                urlBase += "qtd=" + $(event.target).find("option:selected").val() + "&";
                possuiQtd = true;
            }
            else
                urlBase += prop + "=" + objQuery[prop] + "&";
        }

        if (!possuiQtd)
            urlBase += "qtd=" + $(event.target).find("option:selected").val();
        else
            urlBase = urlBase.substring(0, urlBase.length - 1);

        RepopularGrid(urlBase);
    });


    $("#ddlOrdenacao").bind('change', function (event) {
        var objQuery = $.QueryString();
        var possuiQtd = false;
        var urlBase = "";
        for (prop in objQuery) {
            if (prop == "order") {
                urlBase += "order=" + $(event.target).find("option:selected").val() + "&";
                possuiQtd = true;
            }
            else
                urlBase += prop + "=" + objQuery[prop] + "&";
        }

        if (!possuiQtd)
            urlBase += "order=" + $(event.target).find("option:selected").val();
        else
            urlBase = urlBase.substring(0, urlBase.length - 1);

        RepopularGrid(urlBase);
    });
}

window.onpopstate = function (event) {
    if (event.state != null && event.state.htmlGrid) {
        $(".pagination a").unbind('click');
        $("#ddlQtdRegistrosPagina").unbind('change');
        $("#ddlOrdenacao").bind('change');

        $("#divGrid").html(event.state.htmlGrid);

        BindControlesPaginacao();
    }
};