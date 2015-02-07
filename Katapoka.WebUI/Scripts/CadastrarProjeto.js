$(document).ready(function () {
    AplicarMascaras();
    $(document).on('click', '#btnCancelar', btnCancelar_Click_Callback);
    $(document).on('click', '#btnSalvar', btnSalvar_Click_Callback);
});

function btnSalvar_Click_Callback(evento) {
    evento.preventDefault();
    var obj = PopulaObjeto();
    console.log(obj);
    if (obj.nome.length == 0) {
        Modal.alert("Atenção", "Campo \"Nome do projeto\" é obrigatório.", function () {
            $("#txtNomeProjeto").focus();
        });
        return;
    }

    if (obj.dtInicioStr.length != 0) {
        if (!Validacao.DataBR(obj.dtInicioStr)) {
            Modal.alert("Atenção", "Campo data de início foi preenchido com uma data inválida.", function () {
                $("#txtDtInicio").focus();
            });
            return;
        }
        //var hoje = new Date();
        //hoje.setHours(0, 0, 0, 0);
        //var tmpDt = obj.dtInicioStr.split("/").reverse();
        //tmpDt[1]--;
        //var dtInicio = new Date(tmpDt.join("-"));
        //dtInicio.setHours(0, 0, 0, 0);
    }

    if (obj.idEmpresa.length == 0 || isNaN(obj.idEmpresa)) {
        Modal.alert("Atenção", "Selecione uma empresa para este projeto.", function () {
            $("#ddlEmpresa").show();
        });
        return;
    }

    if (!Validacao.IsNumber(obj.idTipoProjeto)) {
        Modal.alert("Atenção", "Campo tipo de projeto é obrigatório.", function () {
            $("#ddlTipoProjeto").focus();
        });
        return;
    }

    ExecutaFuncao(PegarEnderecoPagina() + "/Salvar", obj, function (resposta) {
        if (resposta.Status == 200) {
            Modal.alert("Sucesso", "Dados salvos com sucesso.", function () {
                console.log(resposta);
                if (IdProjeto != null)
                    location.href = "ListarProjetos.aspx";
                else
                    location.href = "AtividadesProjeto.aspx?pid=" + resposta.Data + "&sugeridas=true";
            });
        } else {
            Modal.alert("Atenção", resposta.Data);
        }
    });

}

function btnCancelar_Click_Callback(evento) {
    evento.preventDefault();
    LimpaFormulario();
    location.href = "ListarProjetos.aspx";
}

function LimpaFormulario() {
    var fields = ['#txtNomeProjeto', '#ddlEmpresa'];
    ClearFields(fields);
}

function PopulaObjeto() {
    return {
        nome: $("#txtNomeProjeto").val(),
        idEmpresa: $("#ddlEmpresa option:selected").val(),
        dtInicioStr: $("#txtDtInicio").val(),
        dtTerminoStr: $("#txtDtTermino").val(),
        codReferencia: $("#txtCodigoReferencia").val(),
        status: $("#ddlFlStatus option:selected").val(),
        idTipoProjeto: $("#ddlTipoProjeto option:selected").val(),
        idProjeto: IdProjeto,
    };
}

function AplicarMascaras() {
    $(".campo-data").mask("99/99/9999");
}