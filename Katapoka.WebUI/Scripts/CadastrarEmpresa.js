$(document).ready(function () {
    aplicarMascaras();
    $(document).on('change', '#ddlEstados', DDLEstados_Change_Callback);
    $(document).on('change', '#ddlCidades', DDLCidades_Change_Callback);
    $(document).on('change', '#ddlBairros', DDLBairros_Change_Callback);
    $(document).on('click', '#btnCancelar', BtnCancelar_Click_Callback);
    $(document).on('click', '#btnSalvar', BtnSalvar_Click_Callback);
});

function BtnSalvar_Click_Callback(evento) {
    evento.preventDefault();
    var obj = PreencheObjetoPost();
    //Valida os dados
    ExecutaFuncao(PegarEnderecoPagina() + "/Salvar", obj, function (resposta) {
        if (resposta) {
            if (resposta.Status == 200) {
                Modal.alert("OK", "Dados salvos com sucesso.", function () {
                    if ($.QueryString()["id"] == undefined)
                        location.href = "CadastrarProjeto.aspx?eid=" + resposta.Data;
                    else
                        location.href = "ListarEmpresas.aspx";
                });
            } else {
                Modal.alert("Atenção", resposta.Data);
            }
        } else {
            Modal.alert("Atenção", "Erro desconhecido.");
        }
    });
}

function PreencheObjetoPost() {
    return {
        idEmpresa: IdEmpresaAtual,
        nomeFantasia: $("#txtNomeFantasia").val(),
        razaoSocial: $("#txtRazaoSocial").val(),
        cnpj: $("#txtCnpj").val(),
        idAreaAtuacao: $("#ddlAreaAtuacao option:selected").val(),
        email: $("#txtEmail").val(),
        url: $("#txtUrlSite").val(),
        sumario: $("#txtSumario").val(),
        cep: $("#txtCep").val(),
        endereco: $("#txtEndereco").val(),
        numero: $("#txtNumero").val(),
        complemento: $("#txtComplemento").val(),
        estado: ($("#ddlEstados option:selected").val() == "" ? null : $("#ddlEstados option:selected").val()),
        idCidade: ($("#ddlCidades option:selected").val() == "" ? null : $("#ddlCidades option:selected").val()),
        idBairro: ($("#ddlBairros option:selected").val() == "" || $("#ddlBairros option:selected").val() == "-1" ? null : $("#ddlBairros option:selected").val()),
        bairroNome: $("#txtOutroBairro").val(),
        flAceite: $("#flAceiteTermo").is(":checked"),
        flAprovada: $("#flAprovada").is(":checked"),
        telefoneComercial: $("#txtTelefoneComercial").val(),
        telefoneResidencial: $("#txtTelefoneResidencial").val(),
        telefoneCelular: $("#txtTelefoneCelular").val(),
        telefoneFax: $("#txtFax").val(),
        observacaoContato: $("#txtObservacaoContato").val()
    };
}

function BtnCancelar_Click_Callback(evento) {
    evento.preventDefault();
    LimparFormulario();
}

function LimparFormulario() {
    var controles = [
        '#txtNomeFantasia', '#txtRazaoSocial', '#txtCnpj', '#ddlAreaAtuacao',
        '#txtEmail', '#txtUrlSite', '#txtSumario', '#txtCep', '#txtEndereco',
        '#txtNumero', '#txtComplemento', '#ddlEstados', '#ddlCidades',
        '#ddlBairros', '#txtOutroBairro', '#flAceiteTermo', '#flAprovada'
    ];
    ClearFields(controles);
}

function DDLBairros_Change_Callback(evento) {
    evento.preventDefault();
    var bairroSelecionado = $(this).find("option:selected").val();
    if (bairroSelecionado.length == 0)
        $("#divOutroBairro").slideUp('fast');
    else if (bairroSelecionado == "-1") {
        $("#divOutroBairro").slideDown('fast');
        $("#divOutroBairro").focus();
    }
    else
        $("#divOutroBairro").slideUp('fast');
}

function DDLCidades_Change_Callback(evento) {
    evento.preventDefault();
    var cidadeSelecionada = $(this).find("option:selected").val();
    $("#ddlBairros option").remove();
    if (cidadeSelecionada.length == 0) {
        $("#ddlBairros").append($("<option/>").val("").text("Selecione uma cidade primeiro"));
    } else {
        $("#ddlBairros").append($("<option/>").val("").text("Selecione um bairro"));
        ExecutaFuncao("API.asmx/GetBairros", { IdCidade: cidadeSelecionada }, function (resposta) {
            for (i in resposta)
                $("#ddlBairros").append($("<option/>").val(resposta[i].IdBairro).text(resposta[i].DsNome));
            $("#ddlBairros").append($("<option/>").val("-1").text("Outro"));
            $("#ddlBairros").focus();
        });
    }
}

function DDLEstados_Change_Callback(evento) {
    evento.preventDefault();
    var estadoSelecionado = $(this).find("option:selected").val();
    $("#ddlCidades option").remove();
    if (estadoSelecionado.length == 0) {
        $("#ddlCidades").append($("<option/>").val("").text("Selecione um estado primeiro"));
    } else {
        $("#ddlCidades").append($("<option/>").val("").text("Selecione uma cidade"));
        ExecutaFuncao("API.asmx/GetCidades", { CdUF: estadoSelecionado }, function (resposta) {
            for (i in resposta)
                $("#ddlCidades").append($("<option/>").val(resposta[i].IdCidade).text(resposta[i].DsNome));
            $("#ddlCidades").focus();
        });
    }
}

function aplicarMascaras() {
    $(".campo-cep").mask("99.999-999");
    $(".campo-cnpj").mask("99.999.999/9999-99");
    $(".campo-telefone").brTelMask();
}