$(document).ready(function () {
    $(document).on("click", "#btnFiltrar", btnFiltrar_Click);
});

function btnFiltrar_Click(evento) {
    var idEmpresa = $("#ddlEmpresa option:selected").val();

    //Repopula o grid
    RepopularGrid("idEmpresa=" + idEmpresa);
}