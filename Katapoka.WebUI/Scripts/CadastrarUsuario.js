$(document).ready(function () {
    $(document).on("click", "#btnSalvar", btnSalvar_Click);
    $(document).on("click", "#btnCancelar", btnCancelar_Click);
});

function btnCancelar_Click(evento) {
    evento.preventDefault();
    location.href = "ListarUsuarios.aspx";
}

function btnSalvar_Click(evento) {
    evento.preventDefault();
    var obj = GeraObjPost();

    if (obj.nome == "") {
        Modal.alert("Atenção", "Campo nome é obrigatório!", function () {
            $("#txtNome").focus();
        });
        return;
    }

    if (!Validacao.Email(obj.email)) {
        Modal.alert("Atenção", "Campo e-mail é obrigatório!<br/>Forneça um e-mail válido.", function () {
            $("#txtEmail").focus();
        });
        return;
    }

    if (obj.senha == "" || obj.senha.length < 6) {
        if (IdUsuario == null) {
            Modal.alert("Atenção", "Campo senha é obrigatório!<br/>Deve conter pelo menos 6 caracteres.", function () {
                $("#txtSenha").focus();
            });
            return;
        }
    }

    if (obj.idNivel == "") {
        Modal.alert("Atenção", "Campo nível é obrigatório.", function () {
            $("#ddlNivel").focus();
        });
        return;
    }

    if (obj.idCargo == "") {
        Modal.alert("Atenção", "Campo cargo é obrigatório.", function () {
            $("#ddlCargo").focus();
        });
        return;
    }

    ExecutaFuncao(PegarEnderecoPagina() + "/Salvar", obj, function (response) {
        if (response.Status == 200) {
            Modal.alert("Informação", "Dados salvos com sucesso", function () {
                location.href = "ListarUsuarios.aspx";
            });
        } else {
            Modal.alert("Atenção", response.Data);
        }
    });

}

function GeraObjPost() {
    return {
        idUsuario: IdUsuario,
        nome: $("#txtNome").val(),
        email: $("#txtEmail").val(),
        senha: $("#txtSenha").val(),
        idNivel: $("#ddlNivel option:selected").val(),
        idCargo: $("#ddlCargo option:selected").val()
    };
}