var tagsInputConf = {
    itemValue: 'IdTag',
    itemText: 'DsTag'
};

var configAutocomplete = {
    source: function (request, theResponse) {
        var palavra = request.term;
        ExecutaFuncao("API.asmx/GetTags", { tag: palavra }, function (resposta) {
            if (resposta.Status == 200) {
                theResponse($.map(resposta.Data, function (item) {
                    return {
                        value: item.DsTag,
                        label: item.DsTag,
                        idUsuario: item.IdTag
                    }
                }));
            }
        }, null, false);
    },
    change: function (evento, ui) {
        /*if (ui.item == null)
            $(this).data("uid", null);
        else
            $(this).data("uid", ui.item.idUsuario); //*/
    },
    minLength: 1,
    select: function (event, ui) {
        //$(this).data("uid", ui.item.idUsuario);
        //$(this).tagsinput('add', { "text": "William", "value": "José" });
        $(this).parent().parent().find(">input").tagsinput('add', { "DsTag": ui.item.label, "IdTag": ui.item.idUsuario });
        $(this).val("");
        return false;
    }
};
$(document).ready(function () {
    aplicarMascaras();
    $(document).on("click", "#btnSalvar", btnSalvar_Click);
    $(document).on("click", "#btnCancelar", btnCancelar_Click);
    $(document).on("change", "#ddlProjeto", ddlProjeto_Change);

    $("#txtTags").tagsinput(tagsInputConf);
    $($("#txtTags").tagsinput('input'))
        .on("keyup", txtTags_KeyUp)
        .on("blur", function () { $(this).val(""); })
        .autocomplete(configAutocomplete);

    if (atividadeTags != undefined && atividadeTags instanceof Array) {
        for (i in atividadeTags)
            $("#txtTags").tagsinput('add', { DsTag: atividadeTags[i].DsTag, IdTag: atividadeTags[i].IdTag });
    }
});
var idNegativoHackTagsInput = -1;
var tempTagsInput = {};
function txtTags_KeyUp(evento) {
    var palavra = this.value;
    if (palavra.endsWith(",")) {
        this.value = "";
        if (palavra != ",") {
            //Removo a vírgula
            palavra = palavra.substring(0, palavra.length - 1);
            //Adiciono ela na lista
            if (tempTagsInput[palavra.toLowerCase()] == undefined) {
                tempTagsInput[palavra.toLowerCase()] =
                    {
                        palavra: palavra,
                        id: idNegativoHackTagsInput--
                    };
            } else {
                text: tempTagsInput[palavra.toLowerCase()]["palavra"] = palavra;
            }
            $("#txtTags").tagsinput('add',
                {
                    DsTag: tempTagsInput[palavra.toLowerCase()]["palavra"],
                    IdTag: tempTagsInput[palavra.toLowerCase()]["id"]
                });
        }
    }
}
function btnCancelar_Click(evento) {
    evento.preventDefault();
    location.href = "ListarAtividades.aspx";
}
function ddlProjeto_Change(evento) {
    evento.preventDefault();
    var valor = $(this).find("option:selected").val();
    $("#ddlAtividadePredecessora option").remove();
    if (valor != "" && isNaN(valor)) {
        $("#ddlAtividadePredecessora").append($("<option/>").val("").text("Selecione um projeto primeiro"));
    } else {
        ExecutaFuncao("API.asmx/GetAtividadesByProjeto", { idProjeto: valor }, function (response) {
            if (response instanceof Array) {
                if (response.length > 0) {
                    $("#ddlAtividadePredecessora").append($("<option/>").val("").text("Selecione uma atividade"));
                    for (i in response) {
                        $("#ddlAtividadePredecessora").append($("<option/>").val(response[i].IdAtividade).text(response[i].DsNomeAtividade));
                    }
                } else {
                    $("#ddlAtividadePredecessora").append($("<option/>").val("").text("Projeto sem atividades"));
                }
            } else {
                $("#ddlAtividadePredecessora").append($("<option/>").val("").text("Projeto sem atividades²"));
            }
        });
    }
}
function btnSalvar_Click(evento) {
    evento.preventDefault();
    obj = GerarObjetoPost();

    if (obj.idProjeto == "") {
        Modal.alert("Atenção", "Campo projeto deve ser preenchido", function () {
            $("#ddlProjeto").focus();
        });
        return;
    }

    if (obj.tituloAtividade == "") {
        Modal.alert("Atenção", "Campo título da atividade deve ser preenchido.", function () {
            $("#txtTituloAtividade").focus();
        });
        return;
    }

    if (obj.porcentagemCompleta == "") {
        Modal.alert("Atenção", "Campo % completo deve ser preenchido.", function () {
            $("#txtPorcentagem").focus();
        });
        return;
    }

    if (!Validacao.DataBR(obj.strDtInicio)) {
        Modal.alert("Atenção", "Campo Dt. Início é obrigatório e não foi informado corretamente.", function () {
            $("#txtDtInicio").focus();
        });
        return;
    }

    if (!Validacao.DataBR(obj.strDtTermino)) {
        Modal.alert("Atenção", "Campo Dt. Término é obrigatório e não foi informado corretamente.", function () {
            $("#txtDtTermino").focus();
        });
        return;
    }

    ExecutaFuncao(PegarEnderecoPagina() + "/Salvar", obj, function (response) {
        if (response.Status == 200) {
            Modal.alert("Informação", "Dados salvos com sucesso.", function () {
                location.href = "ListarAtividades.aspx";
            });
        } else {
            Modal.alert("Atenção", response.Data);
        }
    });
}
function aplicarMascaras() {
    $(".campo-hora").mask("9?99:99");
    $("#txtPorcentagem").mask("9?99");
    $(".campo-data").mask("99/99/9999");
}
function GerarObjetoPost() {
    return {
        idAtividade: IdAtividade,
        idProjeto: $("#ddlProjeto option:selected").val(),
        tituloAtividade: $("#txtTituloAtividade").val(),
        tempoEstimado: HoursToTimeSpan($("#txtTempoEstimado").val()),
        porcentagemCompleta: $("#txtPorcentagem").val(),
        strDtInicio: $("#txtDtInicio").val(),
        strDtTermino: $("#txtDtTermino").val(),
        descricao: $("#txtDescricao").val(),
        idAtividadePredecessora: ($("#ddlAtividadePredecessora option:selected").val() == "" ? null : $("#ddlAtividadePredecessora option:selected").val()),
        idUsuarioAssociado: null,
        tags: $("#txtTags").tagsinput("items")
    };
}