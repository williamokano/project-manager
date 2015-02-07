$(document).ready(function () {
    $(document).on('click', '#btnCancelar', btnCancelar_Click);
    $(document).on('click', '#btnSalvar', btnSalvar_Click);
});

function btnCancelar_Click(evento) {
    location.href = "ListarTiposProjeto.aspx";
}

function btnSalvar_Click(evento) {
    var obj = GeraObjPost();

    if (Validacao.IsEmpty(obj.dsTipoProjeto)) {
        Modal.alert("Atenção", "Campo Tipo de Projeto é obrigatório.", function () {
            $("#dsTipoProjeto").focus();
        });
        return;
    }

    ExecutaFuncao(PegarEnderecoPagina() + "/Salvar", obj, function (response) {
        if (response.Status == 200) {
            Modal.alert("Informação", "Dados salvos com sucesso.", function () {
                location.href = "ListarTiposProjeto.aspx";
            });
        }else{
            Modal.alert("Atenção", response.Data);
        }
    });

}

function GeraObjPost() {
    return {
        idTipoProjeto: IdTipoProjeto,
        dsTipoProjeto: $("#txtTipoProjeto").val()
    };
}