var Validacao = {
    Email: function (email) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    },
    CNPJ: function (cnpj) {
        str = cnpj.replace('.', '');
        str = str.replace('.', '');
        str = str.replace('.', '');
        str = str.replace('-', '');
        str = str.replace('/', '');
        cnpj = str;
        var numeros, digitos, soma, i, resultado, pos, tamanho, digitos_iguais;
        digitos_iguais = 1;
        if (cnpj.length < 14 && cnpj.length < 15)
            return false;
        for (i = 0; i < cnpj.length - 1; i++)
            if (cnpj.charAt(i) != cnpj.charAt(i + 1)) {
                digitos_iguais = 0;
                break;
            }
        if (!digitos_iguais) {
            tamanho = cnpj.length - 2
            numeros = cnpj.substring(0, tamanho);
            digitos = cnpj.substring(tamanho);
            soma = 0;
            pos = tamanho - 7;
            for (i = tamanho; i >= 1; i--) {
                soma += numeros.charAt(tamanho - i) * pos--;
                if (pos < 2)
                    pos = 9;
            }
            resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
            if (resultado != digitos.charAt(0))
                return false;
            tamanho = tamanho + 1;
            numeros = cnpj.substring(0, tamanho);
            soma = 0;
            pos = tamanho - 7;
            for (i = tamanho; i >= 1; i--) {
                soma += numeros.charAt(tamanho - i) * pos--;
                if (pos < 2)
                    pos = 9;
            }
            resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
            if (resultado != digitos.charAt(1))
                return false;
            return true;
        }
        else
            return false;
    },
    CPF: function (cpf) {
        cpf = cpf.replace(/[^\d]+/g, '');

        if (cpf == '') return false;

        // Elimina CPFs invalidos conhecidos
        if (cpf.length != 11 ||
            cpf == "00000000000" ||
            cpf == "11111111111" ||
            cpf == "22222222222" ||
            cpf == "33333333333" ||
            cpf == "44444444444" ||
            cpf == "55555555555" ||
            cpf == "66666666666" ||
            cpf == "77777777777" ||
            cpf == "88888888888" ||
            cpf == "99999999999")
            return false;

        // Valida 1o digito
        add = 0;
        for (i = 0; i < 9; i++)
            add += parseInt(cpf.charAt(i)) * (10 - i);
        rev = 11 - (add % 11);
        if (rev == 10 || rev == 11)
            rev = 0;
        if (rev != parseInt(cpf.charAt(9)))
            return false;

        // Valida 2o digito
        add = 0;
        for (i = 0; i < 10; i++)
            add += parseInt(cpf.charAt(i)) * (11 - i);
        rev = 11 - (add % 11);
        if (rev == 10 || rev == 11)
            rev = 0;
        if (rev != parseInt(cpf.charAt(10)))
            return false;

        return true;
    },
    URL: function (url) {
        var reg = new RegExp("^((https?|ftp)://|(www|ftp)\.)[a-z0-9-]+(\.[a-z0-9-]+)+([/?].*)?$");
        return reg.test(url);
    },
    DataBR: function (data) {
        var regDate = /^\d{2}\/\d{2}\/\d{4}$/i;
        if (!regDate.test(data))
            return false;
        var dia = data.split("/")[0];
        var mes = data.split("/")[1];
        var ano = data.split("/")[2];
        var diasMes = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        if (ano % 4 == 0 && (ano % 100 != 0 || ano % 400 == 0))
            diasMes[1] = 29;
        if (mes < 1 || mes > 12)
            return false;
        if (dia > diasMes[mes - 1])
            return false;
        if (ano < 0)
            return false;
        return true;
    },
    HoraBR: function (hora) {
        var regHora = /^\d{2}\:\d{2}$/i;
        if (!regHora.test(hora))
            return false;
        var h = parseInt(hora.split(":")[0]);
        var m = parseInt(hora.split(":")[1]);
        return h >= 0 && h < 24 && m >= 0 && m < 60;
    },
    Tempo: function (hora) {
        var reg = /\d{1,}:[0-5]\d/i;
        return reg.test(hora);
    },
    IsNumber: function (str) {
        return !isNaN(parseFloat(str)) && isFinite(str);
    },
    IsEmpty: function (str) {
        return $.trim(str) == "";
    },
    ObjetojQuery: {
        IsEmpty: function ($obj) {

        }
    }
};

var ValidacaoForm = {
    Init: function (config) {

        var defaultOptions = {
            SimboloObrigatorio: "(*)"
        }

        ValidacaoForm.Config = $.fn.extend(defaultOptions, config);

        $(document).on('focusout', '.requer-validacao', ValidacaoForm.Valida);
        $(".requer-validacao.valida-obrigatorio").each(ValidacaoForm.ProcessaObrigatorio);
    },
    Valida: function (evento) {
        evento.preventDefault();
        var $el = $(this);
        ValidacaoForm.ValidaElemento($el);
    },
    ValidaElemento: function ($el) {
        var $pai = $el.parent(".form-group");
        $pai.removeClass("has-error");

        var elName = $el.attr("name") !== undefined ? $el.attr("name") : $el.attr("id");

        if ($el.hasClass("valida-tempo"))
            setTimeout(function () { ValidacaoForm.ValidaTempo($el); }, 1);
        if ($el.hasClass("valida-data"))
            setTimeout(function () { ValidacaoForm.ValidaData($el); }, 1);
        if ($el.hasClass("valida-numero"))
            setTimeout(function () { ValidacaoForm.ValidaNumero($el); }, 1);
        if ($el.hasClass("valida-obrigatorio"))
            setTimeout(function () { ValidacaoForm.ValidaVazio($el); }, 1);
    },
    SetaErro: function ($el, msgErro) {
        var $pai = $el.parent(".form-group");
        if (typeof (msgErro) == "string" && msgErro.length > 0) {
            var $errorBlock = $pai.find(">.error-block");
            if ($errorBlock.size() > 0)
                $errorBlock.first().html(msgErro);
        }
        if (!$pai.hasClass("has-error"))
            $pai.addClass("has-error");
    },
    RemoveErro: function ($el) {
        var $pai = $el.parent(">.form-group");
    },
    ValidaVazio: function ($el) {
        if (Validacao.IsEmpty($el.val()))
            ValidacaoForm.SetaErro($el, "Este campo não pode ser vazio.");
    },
    ValidaData: function ($el) {
        if ($el.val() != "")
            if (!Validacao.DataBR($el.val()))
                ValidacaoForm.SetaErro($el, "Data inválida.");
    },
    ValidaNumero: function ($el) {
        if ($el.val() != "")
            if (!Validacao.IsNumber($el.val())) {
                ValidacaoForm.SetaErro($el, "Este campo deve ser um número.");
            } else {
                if ($el.hasClass("valida-numero-range")) {
                    var range = $el.data("range").split("~");
                    var min = parseInt(range[0], 10);
                    var max = parseInt(range[1], 10);
                    var numero = parseInt($el.val(), 10);
                    if (numero < min || numero > max) {
                        ValidacaoForm.SetaErro($el, "O valor deste campo deve estar entre " + min + " e " + max + ".");
                    }
                }
            }
    },
    ValidaTempo: function ($el) {
        if ($el.val() != "")
            if (!Validacao.Tempo($el.val()))
                ValidacaoForm.SetaErro($el, "Favor inserir horas e minutos. Minutos deve ser de 0 até 59.");
    },
    ProcessaObrigatorio: function () {
        var $pai = $(this).parent(".form-group");
        var $label = $pai.find(">label");
        var oldText = $label.html();
        $label.html(oldText + " " + ValidacaoForm.Config.SimboloObrigatorio);
    },
    Config: null
}