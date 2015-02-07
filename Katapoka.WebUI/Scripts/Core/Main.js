$(document).ready(function () {
    //Remove o comportamento padrão dos forms
    $("form").submit(function (evento) { evento.preventDefault(); return false; });
});
var Katapoka = { };
$.QueryString = function () {
    var a = window.location.search.substr(1).split('&');
    if (a == "") return {};
    var b = {};
    for (var i = 0; i < a.length; i++) {
        var p = a[i].split('=');
        if (p.length != 2) continue;
        b[p[0]] = decodeURIComponent(p[1].replace(/\+/g, " "));
    }
    return b;
};
function ClearFields(fieldsArray) {
    for (i in fieldsArray)
        $(fieldsArray[i]).val("")
            .removeAttr("checked")
            .trigger("change");
    $(fieldsArray[0]).focus();
}
function ExecutaFuncao(endereco, argumentos, funcRetorno, funcPosExec, showLoader) {
    var mostrarLoader = (typeof (showLoader) == "boolean" ? showLoader : true);
    var strUrl = endereco;
    var data = JSON.stringify(argumentos, null, 2);
    var bReturn = null;
    jQuery.ajax({
        type: "POST",
        url: strUrl,
        async: true,
        beforeSend: function () { if (mostrarLoader) { $("#ajaxLoaderFullScreen").fadeIn('fast'); } },
        data: data,
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: ((funcRetorno != null) ? function (result) { funcRetorno(result.d); } : function (result) { bReturn = result.d; }),
        complete: function () {
            if (funcPosExec != null && typeof (funcPosExec) == "function")
                funcPosExec();
            if (mostrarLoader)
                $("#ajaxLoaderFullScreen").fadeOut('fast');
        }
    });

    if (funcRetorno == null)
        return bReturn;
}
function PegarEnderecoPagina() {
    return window.location.pathname;

    if (window.location.href.indexOf('?') >= 0)
        return window.location.href.split('?')[0];
    else if (window.location.href.indexOf('#') >= 0)
        return window.location.href.split('#')[0];
    else
        return window.location.href;
}