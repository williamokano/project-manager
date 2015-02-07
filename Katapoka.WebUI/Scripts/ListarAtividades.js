$(document).ready(function () {
    $(document).on("click", "#btnFiltrar", btnFiltrar_Click);
    $(document).on("change", "#ddlAtivos", ddlAtivos_Change);
});

function btnFiltrar_Click(evento) {
    evento.preventDefault();
    var idProjeto = $("#ddlProjeto option:selected").val();
    var flProjetosAtivos = $("#ddlAtivos option:selected").val();

    RepopularGrid("pid=" + idProjeto + "&flProjetosAtivos=" + flProjetosAtivos);
}
function ddlAtivos_Change(evento) {
    evento.preventDefault();
    var valor = $(this).find("option:selected").val();
    $("#ddlProjeto option").remove();
    $("#ddlProjeto").append($("<option/>").val("").text("Todos"));
    ExecutaFuncao("API.asmx/GetProjetosFiltroAtivo", { flProjetoAtivo: valor }, function (response) {
        for (i in response) {
            $("#ddlProjeto").append($("<option/>").val(response[i].IdProjeto).text(response[i].DsNome));
        }
    });
}