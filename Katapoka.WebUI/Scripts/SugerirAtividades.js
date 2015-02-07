var configAutocomplete = {
    source: function (request, theResponse) {
        var palavra = request.term;
        ExecutaFuncao("API.asmx/GetUsuarioAutocomplete", { nome: palavra }, function (resposta) {
            if (resposta.Status == 200) {
                theResponse($.map(resposta.Data, function (item) {
                    return {
                        value: item.DsNome,
                        label: item.DsNome,
                        idUsuario: item.IdUsuario
                    }
                }));
            }
        }, null, false);
    },
    change: function (evento, ui) {
        if (ui.item == null)
            $(this).data("uid", null);
        else
            $(this).data("uid", ui.item.idUsuario);
    },
    minLength: 1,
    select: function (event, ui) {
        //$(this).data("uid", ui.item.idUsuario);
        //$(this).tagsinput('add', { "text": "William", "value": "José" });
        $(this).parent().parent().find(">input").tagsinput('add', { "text": ui.item.label, "value": ui.item.idUsuario });
        $(this).val("");
        return false;
    }
};

var tagsInputConf = {
    itemValue: 'value',
    itemText: 'text'
};

$(document).ready(function () {
    aplicarMascaras();
    $(document).on('click', '#btnCancelar', btnCancelar_Click);
    $(document).on('click', '#btnAdicionarOutra', btnAdicionarOutra_Click);
    $(document).on('click', '#btnSalvar', btnSalvar_Click);
    $(document).on('click', ".remover-linha", Class_removerLinha_Click);

    //Inicia o tag-input e o autocomplete
    $(".nome-autocomplete").tagsinput(tagsInputConf);
    $($(".nome-autocomplete").tagsinput('input')).autocomplete(configAutocomplete);

    //Insere o usuário pré-agendado a ele
    $("input[data-uname]").each(function () { $(this).tagsinput('add', { 'text': $(this).data("uname"), 'value': $(this).data("uid") }); });
});

function aplicarMascaras() {
    $(".campo-tempo").unmask();
    $(".campo-tempo").mask("999:99");

    $(".campo-predecessor").unmask();
    $(".campo-predecessor").mask("9?99");
}

function Class_removerLinha_Click(evento) {
    evento.preventDefault();
    $(this).parent().parent().remove();
}

function btnCancelar_Click(evento) {
    evento.preventDefault();
    $("form")[0].reset();
    $("input[type=text][readonly!=readonly]").first().focus();
    location.href = "ListarAtividades.aspx";
}

function btnAdicionarOutra_Click(evento) {
    evento.preventDefault();
    var $lastAti = $(".dados-atividade").last();
    var idNextAti = parseInt($lastAti.find("#txtIdFicticioAtividade").val()) + 1;
    var $divForm = $("<div/>").addClass("dados-atividade form-inline").attr("role", "form");

    var $divIdPre = $("<div/>").addClass("form-group")
            .append($("<label/>").addClass("sr-only").attr("for", "txtIdFicticioAtividade").html("ID Atividade"))
            .append(
                $("<input/>").val(idNextAti)
                    .css("width", "50px").attr("readonly", "readonly")
                    .attr("type", "text")
                    .addClass("form-control")
                    .attr("id", "txtIdFicticioAtividade")
                    .attr("placeholder", "ID Atividade")
            );

    var $divNome = $("<div/>").addClass("form-group")
                .append($("<label/>")
                    .addClass("sr-only")
                    .attr("for", "txtNomeAtividade")
                    .html("Nome atividade")
                )
                .append($("<input/>")
                    .attr("type", "text")
                    .addClass("form-control")
                    .attr("id", "txtNomeAtividade")
                    .attr("placeholder", "Nome da atividade")
                    .attr("maxlength", 200)
                );

    var $inputNomeUsuario = $("<input/>")
                    .attr("type", "text")
                    .addClass("form-control nome-autocomplete")
                    .attr("id", "txtNomeUsuario")
                    .attr("placeholder", "Nome Usuário")
                    .data("uid", null);

    var $divNomeUsuario = $("<div/>").addClass("form-group")
                .append($("<label/>")
                    .addClass("sr-only")
                    .attr("for", "txtNomeUsuario")
                    .html("Nome Usuário")
                )
                .append($inputNomeUsuario);

    var $divTempoPrevisto = $("<div/>").addClass("form-group")
                .append($("<label/>")
                    .addClass("sr-only")
                    .attr("for", "txtTempoPrevisto")
                    .html("Tempo previsto")
                )
                .append($("<input/>")
                    .attr("type", "text")
                    .addClass("form-control campo-tempo")
                    .attr("id", "txtTempoPrevisto")
                    .attr("placeholder", "Tempo previsto")
                    .val("000:00")
                );

    var $divPreAtividade = $("<div/>").addClass("form-group")
                .append($("<label/>")
                    .addClass("sr-only")
                    .attr("for", "txtIdAtividadePredecessora")
                    .html("Id Atividade Predecessora")
                )
                .append($("<input/>")
                    .attr("type", "text")
                    .css("width", "50px")
                    .addClass("form-control campo-predecessor")
                    .attr("id", "txtIdAtividadePredecessora")
                    .attr("placeholder", "PRE")
                );

    var $divBotaoRemover = $("<div/>").addClass("form-group")
        .append(
            $("<button>").attr("type", "button")
                .addClass("btn btn-danger remover-linha")
                .html("Remover")
        );

    $divForm.append($divIdPre);
    $divForm.append($divNome);
    $divForm.append($divNomeUsuario);
    $divForm.append($divTempoPrevisto);
    $divForm.append($divPreAtividade);
    $divForm.append($divBotaoRemover);
    $lastAti.after($divForm);

    //Reaplica máscaras
    aplicarMascaras();

    //Tagsinput
    $inputNomeUsuario.tagsinput(tagsInputConf);
    $inputNomeUsuario.tagsinput('input').autocomplete(configAutocomplete);
}

function btnSalvar_Click(evento) {
    evento.preventDefault();

    $(".nome-autocomplete").each(function () { $(this).parent().removeClass("has-error"); });
    $(".form-group").each(function () { $(this).removeClass("has-error"); });
    var obj = GeraObjetoPost();

    //Valida os campos de nome que não estão vazios e que estão com uid == null
    var validado = true;
    /*
    $(".nome-autocomplete").each(function () {
        if ($(this).val() != "" && $(this).data("uid") == null) {
            if (!$(this).parent().hasClass("has-error"))
                $(this).parent().addClass("has-error");
            validado = false;
        }
    });
    if (!validado) {
        Modal.alert("Atenção", "Existem campos com nome parcialmente digitado.<br/>Corrija os campos em vermelho!");
        return;
    }//*/

    //Valida os campos de nome (todos devem estar preenchidos)
    $(".form-group").each(function () {
        var valor = $(this).find("#txtNomeAtividade").val();
        if (valor == "") {
            if (!$(this).hasClass("has-error"))
                $(this).addClass("has-error");
            validado = false;
        }
    });
    if (!validado) {
        Modal.alert("Atenção", "Campo \"Nome da atividade\" é um campo obrigatório.<br/>Corrija os campos em vermelho.");
        return;
    }

    //Valida se estou apontando para um pré-requisito que não existe
    var idsValidas = new Array();
    $(".dados-atividade").each(function () {
        idsValidas.push($(this).find("#txtIdFicticioAtividade").val());
    });
    var mensagemErro = "";
    $(".form-group #txtIdAtividadePredecessora").each(function () {
        var valor = $(this).val();
        if (valor != "") {
            if (isNaN(valor)) {
                mensagemErro = "Foi um informado um valor não numérico.<br/>Verifique os campos em vermelho.";
                validado = false;
                if (!$(this).parent().hasClass("has-error"))
                    $(this).parent().addClass("has-error");
            } else {
                if (idsValidas.indexOf(valor) == -1) {
                    validado = false;
                    if (!$(this).parent().hasClass("has-error"))
                        $(this).parent().addClass("has-error");
                    mensagemErro = "Você está definindo uma predecessora que não existe ainda.<br/>Verifique os campos campos em vermelho.";
                } else {
                    var myId = $(this).parent().parent().find("#txtIdFicticioAtividade").val();
                    if (myId == valor) {
                        mensagemErro = "Uma atividade não pode ser predecessora dela mesmo.<br/>Corrija os campos em vermelho.";
                        validado = false;
                        if (!$(this).parent().hasClass("has-error"))
                            $(this).parent().addClass("has-error");
                    }
                }
            }
        }
    });

    if (!validado) {
        Modal.alert("Atenção", mensagemErro);
        return;
    }
    console.log(obj);
    //Tudo OK, posta!
    ExecutaFuncao(PegarEnderecoPagina() + "/IncluirAtividades", { idProjeto: IdProjeto, atividades: obj }, function (resposta) {
        if (resposta.Status == 200) {
            if(IdProjeto != undefined && IdProjeto != null)
                location.href = "ListarAtividades.aspx?pid=" + IdProjeto;
            else
                location.href = "ListarAtividades.aspx";
        } else {
            Modal.alert("Atenção", resposta.Data);
        }
    });
}

function GeraObjetoPost() {
    var arrayDados = new Array();
    $(".dados-atividade").each(function () {
        var idLocal = parseInt($(this).find("#txtIdFicticioAtividade").val());
        var nomeAtividade = $(this).find("#txtNomeAtividade").val();
        var idUsuariosResponsaveis = ($(this).find("#txtNomeUsuario").val() != "" ? $(this).find("#txtNomeUsuario").val().split(",") : []);
        var qtdHoras = HoursToTimeSpan($(this).find("#txtTempoPrevisto").val());
        var idLocalPre = ($(this).find("#txtIdAtividadePredecessora").val() == "" ? null : parseInt($(this).find("#txtIdAtividadePredecessora").val()));
        arrayDados.push({
            IdAtividadeLocal: idLocal,
            DsNomeAtividade: nomeAtividade,
            IdUsuariosResponsaveis: idUsuariosResponsaveis,
            QtHoras: qtdHoras,
            IdPreAtividade: idLocalPre
        });
    });
    return arrayDados;
}
