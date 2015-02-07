$(document).ready(function () {
    $(document).on('click', '#btnLogin', btnLogin_Click);
});

function btnLogin_Click(evento) {
    evento.preventDefault();
    var email = $.trim($("#txtEmail").val());
    var senha = $("#txtSenha").val();
    if (!Validacao.Email(email)) {
        Modal.alert("Atenção", "E-mail fornecido inválido.", function () { $("#txtEmail").focus(); });
        return;
    }

    if (senha.length < 6) {
        Modal.alert("Atenção", "Senha deve conter 6 ou mais caracteres", function () { $("#txtSenha").focus(); });
        return;
    }

    ExecutaFuncao(PegarEnderecoPagina() + "/Logar",
    {
        email: email,
        senha: senha
    }, function (resposta) {
        if (resposta.Status == 200)
            window.location.href = "Default.aspx";
        else
            Modal.alert('Atenção', resposta.Data);
    });

}