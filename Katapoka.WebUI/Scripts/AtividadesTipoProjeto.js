var unsaveChanges = false;
var canAutoSave = true;

/* Configuração tagsinput e autocomplete para tags */
var configAutocompleteTags = {
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

    },
    minLength: 1,
    select: function (event, ui) {
        $(this).parent().parent().find(">input").tagsinput('add', { "DsTag": ui.item.label, "IdTag": ui.item.idUsuario });
        $(this).val("");
        return false;
    }
};
var tagsInputConfTags = {
    itemValue: 'IdTag',
    itemText: 'DsTag'
};
/* FIM Configuração tagsinput e autocomplete para tags */

function titulo_Blur() {
    var valor = $(this).val();
    if (valor == "") {
        if (!$(this).parent().hasClass("has-error")) {
            $(this).parent().addClass("has-error");
        }
    } else {
        $(this).parent().removeClass("has-error");
    }
}
function removeBtn_Click(evento) {
    evento.preventDefault();
    var qtdFilhos = $(this).parent().find("> ol").find(">li").size();
    if (qtdFilhos == 0) {
        var $elRemove = $(this).parent();
        Modal.confirm("Atenção", "Deseja remover esta atividade?", function () {
            $elRemove.remove();
            SalvarSemAviso();
        });
    } else {
        var $elRemove = $(this).parent();
        Modal.confirm("ATENÇÃO!", "Remover esta atividade também removerá todos as atividades que dependem dela!\nRealmente deseja continuar?", function () {
            $elRemove.remove();
            SalvarSemAviso();
        });
    }
    AtualizaPais();
}
function AdicionaTagTemp(tag) {
    var dsTag = tag.DsTag;
    var idTag = tag.IdTag;

    if (tempTagsInput[dsTag.toLowerCase()] === undefined) {
        tempTagsInput[dsTag.toLowerCase()] = {
            palavra: dsTag,
            id: idTag
        };
    }

}
function GetTagTemp(palavra) {
    return {
        DsTag: tempTagsInput[palavra.toLowerCase()]["palavra"],
        IdTag: tempTagsInput[palavra.toLowerCase()]["id"]
    }
}
$(document).ready(function () {
    ValidacaoForm.Init({
        SimboloObrigatorio: "<em>( * )</em>"
    });

    $(document).on('click', '.remove-btn', removeBtn_Click);
    $(document).on('blur', 'input, select, textarea', SalvarSemAviso);

    /* TAGS INPUT E AUTOCOMPLETE PARA TAGS */
    $(".tags-autocomplete").tagsinput(tagsInputConfTags);
    var tagsInput = $(".tags-autocomplete").tagsinput('input');
    if (tagsInput !== undefined) {
        if (tagsInput.length == 1)
            $(".tags-autocomplete").tagsinput('input')
            .keyup(txtTags_KeyUp)
            .blur(function () { $(this).val(""); })
            .autocomplete(configAutocompleteTags);
        else
            for (i in tagsInput) {
                $(tagsInput[i])
                .keyup(txtTags_KeyUp)
                .blur(function () { $(this).val(""); })
                .autocomplete(configAutocompleteTags);
            }
    }
    /* FIM DO TAGS INPUT E AUTOCOMPLETE PARA TAGS */

    $(document).on("click", ".add-btn", addBtn_Click);

    $('.sortable').nestedSortable({
        handle: 'div',
        items: 'li',
        toleranceElement: '> div',
        forcePlaceholderSize: true,
        placeholder: 'placeholder',
        isTree: true,
        protectRoot: true,
        update: function (event, ui) { AtualizaPais(); SalvarSemAviso(); }
    });

    $(document).on('click', '.sortable .seta', function (evento) {
        evento.preventDefault();
        var $li = $(this).parent();
        $li.toggleClass("oculta-form");
        $li.find(">.form").slideToggle('fast');
    });

    $(".sortable .seta").trigger("click");

    $(document).on('keyup', 'input.titulo', function (evento) {
        var valor = $.trim($(this).val());
        valor = valor == "" ? "&nbsp;" : valor;
        $(this).parent().parent().parent().find("span.titulo").html(valor);
    });

    AplicarMascaras();
    AtualizaPais();
    $(document).on('click', '#btnSalvar', btnSalvar_Click);

    //Adiciona os usuários sugeridos das atividades sugeridas
    if (typeof usuariosSugeridos != "undefined") {
        for (i in usuariosSugeridos) {
            if (usuariosSugeridos[i] instanceof Array) {
                if (usuariosSugeridos[i].length > 0) {
                    for (j in usuariosSugeridos[i]) {
                        $("li[data-lid=" + i + "]").find("#txtUsuarios").tagsinput('add', {
                            IdUsuario: usuariosSugeridos[i][j].IdUsuario,
                            DsNome: usuariosSugeridos[i][j].DsNome
                        });
                    }
                }
            }
        }
    }

    //Adiciona os usuários
    if (typeof (usuarios) != "undefined")
        for (i in usuarios)
            for (j in usuarios[i])
                $("li[data-aid=" + i + "]").find("#txtUsuarios").tagsinput('add', {
                    IdUsuario: usuarios[i][j].IdUsuario,
                    DsNome: usuarios[i][j].DsNome
                });

    //Adiciona as tags
    if (typeof (tags) != "undefined")
        for (i in tags) {
            if (tags[i] instanceof Array && tags[i].length > 0) {
                for (j in tags[i]) {
                    $("li[data-aid=" + i + "]").find("#txtTags").tagsinput('add', {
                        IdTag: tags[i][j].IdTag,
                        DsTag: tags[i][j].DsTag
                    });

                    //Hack to prevent bug "duplicate tag"
                    tempTagsInput[tags[i][j].DsTag.toLowerCase()] = { palavra: tags[i][j].DsTag, id: tags[i][j].IdTag };

                }
            }
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
            var idTag = idNegativoHackTagsInput--;
            AdicionaTagTemp({ DsTag: palavra, IdTag: idTag });

            $(this).parent().parent().find(".tags-autocomplete").tagsinput('add', GetTagTemp(palavra));
        }
    }
}
function btnSalvar_Click(evento) {
    evento.preventDefault();
    Salvar();
}
function AtualizaPais() {
    $("ol.sortable li").each(function () {
        var pai = $(this).parent().parent().data("lid") == undefined ? null : $(this).parent().parent().data("lid");
        $(this).attr("data-pai", pai)
        .data("pai", pai);
        //console.log({ id: $(this).data("aid"), pai: pai });
    });
}
function GeraObjPost() {
    AtualizaPais();
    var dados = new Array();

    $("li[data-lid]").each(function () {
        var tagsAtividade = $(this).find(">.form #txtTags").tagsinput('items');
        if (tagsAtividade.length > 0 && tagsAtividade[0] instanceof Array)
            tagsAtividade = [];

        dados.push({
            IdAtividade: $(this).data("aid"),
            IdAtividadeLocal: $(this).data("lid"),
            DsNomeAtividade: $(this).find("#txtTituloAtividade").val(),
            IdAtividadePredecessora: $(this).data("pai"),
            QtTempoEstimado: $(this).find("#txtQtTempoEstimado").val(),
            DsAtividade: $(this).find("#txtDescricaoAtividade").val(),
            Tags: tagsAtividade
        });
    });

    return dados;
}
function AplicarMascaras() {
    //Utilizando o plugin de máscaras do igor escobar
    //Site: http://igorescobar.github.io/jQuery-Mask-Plugin/
    //Github: https://github.com/igorescobar/jQuery-Mask-Plugin

    //Tempo
    $(".campo-tempo").unmask();
    $(".campo-tempo").mask("##0:00", { reverse: true });

    //Data
    $(".campo-data").unmask();
    $(".campo-data").mask("99/99/9999");

    //Numero
    $(".campo-numero").unmask();
    $(".campo-numero").mask("990");
}
function addBtn_Click(evento) {
    evento.preventDefault();
    var IdLocal = ++LocalIdCounter;

    var $li = CriaLiAtividade(IdLocal);
    var $liPai = $(this).parent();

    InsereAtividade($li, $liPai);
}
function Salvar() {
    var qtErr = $(".has-error").size();
    if (qtErr > 0) {
        if (qtErr == 1) {
            Modal.alert("Atenção!", "Existe um campo que não foi validado.\nCorrija o campo com destaque em vermelho.");
        } else {
            Modal.alert("Atenção!", "Existem campos que não foram validados.\nCorrija os campos com destaque em vermelho.");
        }
        var $el = $(".has-error").first();
        if ($el.parent().parent().hasClass("oculta-form")) {
            $el.parent().parent().removeClass("oculta-form");
            $el.parent().parent().find(">.form").slideDown('fast');
        }

        var offset = $el.offset();
        $("html,body").animate({ scrollTop: offset.top - 50 });
    } else {
        var objPost = GeraObjPost();
        ExecutaFuncao(PegarEnderecoPagina() + "/Salvar", { atividades: objPost, idTipoProjeto: IdTipoProjeto }, function (response) {
            if (response.Status == 200) {
                unsaveChanges = false;
                var atividades = response.Data;
                RefreshDisplay(atividades, true);
            } else {
                Modal.alert("Erro", response.Data);
            }
        }, function () { canAutoSave = true; }, false);
    }
}
function SalvarSemAviso() {
    return false;
    setTimeout(function () {
        if (canAutoSave) {
            var qtErr = $(".has-error").size();
            if (qtErr == 0) {
                var objPost = GeraObjPost();
                ExecutaFuncao(PegarEnderecoPagina() + "/Salvar", { atividades: objPost, idTipoProjeto: IdTipoProjeto }, function (response) {
                    if (response.Status == 200) {
                        unsaveChanges = false;
                        var atividades = response.Data;
                        RefreshDisplay(atividades, true);
                    } else {
                        Modal.alert("Erro", response.Data);
                    }
                }, function () { canAutoSave = true; }, false);
            }
        }
    }, 10);
}
function SetFilho(pai, filho) {
    var $pai = null;
    if (pai !== undefined && isNaN(pai)) {
        console.warn("PAI: não é um número nem undefined.");
        return;
    }

    if (isNaN(filho)) {
        console.warn("FILHO: não é um número.");
        return;
    }

    if (pai == filho) {
        console.warn("WHAT? item não pode ser filho dele próprio.");
        return;
    }

    //Encontro o pai
    if (pai === undefined) {//Se o pai não estiver definido, o pai é a "raiz"
        $pai = $("ol.sortable > li").last();
    } else {
        //Caso contrário, tento encontrar o pai
        $pai = $("li[data-lid=" + pai + "]");
        if ($pai.size() < 1) { //Não foi encontrado, emito aviso
            console.warn(["Atenção", "Não consegui realizar o realocamento de pai e filho entre " + pai + " e " + filho + "."]);
            return;
        } else { //Encontrado 1 ou mais "pais" (teóricamente é impossível encontrar mais que um, mas deixarei assim por ora.
            $pai = $pai.last(); //Pego o último, pois o realoc deve ser feito sempre no final
        }
    }

    //Tento encontrar a LI filha
    $filho = $("li[data-lid=" + filho + "]");
    if ($filho.size() < 1) { //Não encontrei o filho para realocar
        console.warn(["Atenção", "Não foi encontrado o filho " + filho + " para realocar com o pai " + pai + "."]);
    } else {

        //Verifico se estou tentando passar para filho de alguém que depende de mim, isso da bug, deve ser tratado.
        //Item só pode ser filho de alguém que não é sub-filho dele em qualquer nível!!!!!!!!!!!!onze!!!!111!!!
        var isChildNode = false;

        //Get all child nodes
        AtualizaPais();
        var childs = new Array();
        $filho.find("li[data-lid]").each(function () {
            childs.push(parseInt($(this).data("lid"), 10));
        });

        var myLid = parseInt($filho.data("lid"), 10);

        if (pai === undefined || childs.indexOf(myLid) == -1) { //É possível realocar
            //Verifico se já não sou pai e filho
            if ($pai.data("lid") != $filho.data("pai")) {
                //Tudo ok, tenta realoc
                $ol = $pai.find(">ol");

                if ($ol.size() == 0) {
                    //OL not found, create one
                    $ol = $("<ol/>");
                    $pai.append($ol);
                }

                $ol.append($filho);
            } else {
                console.warn("MISERROR: Já são pai e filho diretamente.");
            }
        } else {
            console.warn("FILHO: can't realloc to a child. " + $pai.data("lid") + " -> " + $filho.data("lid"));
            return;
        }

    }

}
function CriaLiAtividade(IdLocal) {
    var $li = $("<li/>").data("lid", IdLocal).attr("data-lid", IdLocal);

    //Filhos diretos de LI
    var $titulo = $("<div/>").addClass("titulo-atividade")
            .append($("<span/>").addClass("titulo").html("Atividade sem título"));

    var $form = $("<div/>").addClass("form").attr("role", "form");
    //Campos do form
    var $txtTituloAtividade = $("<div/>").addClass("form-group")
                                        .append($("<label/>")
                                                    .attr("for", "txtTituloAtividade")
                                                    .html("Título Atividade")
                                                    .addClass("control-label"))
                                        .append($("<input/>")
                                                    .attr("type", "text")
                                                    .val("Atividade sem título")
                                                    .addClass("form-control titulo")
                                                    .attr("id", "txtTituloAtividade"))
                                        .append('<span class="help-block error-block">O título da atividade é obrigatório.</span>');
    $form.append($txtTituloAtividade);

    var $txtQtTempoEstimado = $("<div/>").addClass("form-group")
                                        .append($("<label/>")
                                                    .attr("for", "txtQtTempoEstimado")
                                                    .html("Tempo Estimado")
                                                    .addClass("control-label"))
                                        .append($("<input/>")
                                                    .attr("type", "text")
                                                    .addClass("form-control campo-tempo requer-validacao valida-tempo")
                                                    .attr("id", "txtQtTempoEstimado"))
                                        .append('<span class="help-block error-block">Este campo é obrigatório e requer um valor maior do que 0.</span>');
    $form.append($txtQtTempoEstimado);

    var $txtDescricaoAtividade = $("<div/>").addClass("form-group")
                                        .append($("<label/>")
                                                    .attr("for", "txtDescricaoAtividade")
                                                    .html("Descrição")
                                                    .addClass("control-label"))
                                        .append($("<textarea/>")
                                                    .attr("rows", "5")
                                                    .addClass("form-control")
                                                    .attr("id", "txtDescricaoAtividade"))
                                        .append('<span class="help-block error-block">Este campo é obrigatório.</span>');
    $form.append($txtDescricaoAtividade);

    $inputTags = $("<input/>")
                    .attr("type", "text")
                    .addClass("form-control tags-autocomplete")
                    .attr("id", "txtTags");

    var $txtTags = $("<div/>").addClass("form-group")
                                        .append($("<label/>")
                                                    .attr("for", "txtTags")
                                                    .html("Tags")
                                                    .addClass("control-label"))
                                        .append($inputTags);
    $form.append($txtTags);

    var $seta = $("<span/>").addClass("seta").html("&nbsp;");
    var $botaoAdd = $("<span/>").addClass("add-btn").append($("<button/>").addClass("btn btn-success btn-xs").html("+"));
    var $botaoRemove = $("<span/>").addClass("remove-btn").append($("<button/>").addClass("btn btn-danger btn-xs").html("-"));

    $li
        .append($titulo)
        .append($form)
        .append($seta)
        .append($botaoAdd)
        .append($botaoRemove)
        .append($("<ol/>"));

    return $li;
}
function AplicaPluginsJs($li) {
    var $inputTags = $li.find("#txtTags");
    var $inputNome = $li.find("#txtUsuarios");
    var $form = $li.find(".form");

    $inputTags.tagsinput(tagsInputConfTags);
    $($inputTags.tagsinput('input'))
        .keyup(txtTags_KeyUp)
        .blur(function () { $(this).val(""); })
        .autocomplete(configAutocompleteTags);

    //$form.find(".requer-validacao").parent(".form-group").addClass("has-error");
    $form.find(".requer-validacao").each(function () {
        ValidacaoForm.ValidaElemento($(this));
    });
    $form.find(".requer-validacao.valida-obrigatorio").each(ValidacaoForm.ProcessaObrigatorio);
}
function InsereAtividade($li, $liPai, oculta) {
    var ocultar = typeof oculta == "boolean" ? oculta : true;
    //Cria a OL
    var $ol = $liPai.find("> ol");
    if ($ol.size() == 0) { //Não tem OL
        $ol = $("<ol/>");
        $liPai.append($ol);
    }

    if (ocultar) {
        //fecho todos os campos abertos e depois dou o foco no novo que foi criado
        $("li[data-lid]").each(function () {
            var $el = $(this);
            if (!$el.hasClass("oculta-form")) {
                $el.addClass("oculta-form");
                $el.find(">.form").hide();
            }
        });
    } else {
        $li.addClass("oculta-form");
        $li.find(">.form").css("display", "none");
    }

    $ol.append($li);

    if (ocultar) {
        //find my top
        var liTop = $li.offset().top - 50;
        PageFuns.ScrollTo(liTop);
    }

    AplicarMascaras();
    AplicaPluginsJs($li);
    SalvarSemAviso();
}
function RefreshDisplay(atividades, local) {
    
    var idsExistentes = new Array();
    $("ol.sortable li[data-aid]").each(function () {
        idsExistentes.push(parseInt($(this).data("aid"), 10));
    });

    var idsNovas = new Array();
    var maiorIdLocal = 0;

    if (atividades instanceof Array && atividades.length > 0) {
        for (i in atividades) {

            if (atividades[i].IdAtividadeLocal > maiorIdLocal)
                maiorIdLocal = atividades[i].IdAtividadeLocal;

           //Processa a remoção
            idsNovas.push(parseInt(atividades[i].IdAtividade, 10));

            var $li = $("li[data-lid=" + atividades[i].IdAtividadeLocal + "]");

            //Caso ela não exista na tela ainda (vulgo, é nova de outra tela), adiciono-a
            if ($li.size() == 0) {
                $li = CriaLiAtividade(atividades[i].IdAtividadeLocal);
                $liPai = $("li[data-lid=" + atividades[i].IdAtividadePredecessora + "]");
                if ($liPai.size() == 0)
                    $liPai = $("ol.sortable > li"); //Se não achar um pai, insere na raiz
                InsereAtividade($li, $liPai, false);
            }

            //Atualiza com as IDS reais
            $li.attr("data-aid", atividades[i].IdAtividade)
               .data("aid", parseInt(atividades[i].IdAtividade, 10));

            //Removo todas as tags da temp de tags
            tempTagsInput = {};

            //Deleto todas as tags e as insiro novamente
            $li.find("#txtTags").tagsinput('removeAll');
            for (j in atividades[i].Tags) {
                AdicionaTagTemp({
                    DsTag: atividades[i].Tags[j].DsTag,
                    IdTag: atividades[i].Tags[j].IdTag
                });

                $li.find("#txtTags").tagsinput('add', GetTagTemp(atividades[i].Tags[j].DsTag));
            }

            //Deleto os usuários e insiro novamente.
            $li.find("#txtUsuarios").tagsinput('removeAll');
            for (j in atividades[i].Usuarios) {
                $li.find("#txtUsuarios").tagsinput('add', {
                    IdUsuario: atividades[i].Usuarios[j].IdUsuario,
                    DsNome: atividades[i].Usuarios[j].DsNome
                });
            }

            if (typeof local == "boolean" && !local) {

                //Altero o valor dos campos
                if ($li.find("#txtTituloAtividade").val() != atividades[i].DsNomeAtividade)
                    $li.find("#txtTituloAtividade").val(atividades[i].DsNomeAtividade);

                if ($li.find(".titulo-atividade .titulo").html() != atividades[i].DsNomeAtividade)
                    $li.find(".titulo-atividade .titulo").html(atividades[i].DsNomeAtividade);

                if ($li.find("#txtQtTempoEstimado").val() != atividades[i].QtTempoEstimado)
                    $li.find("#txtQtTempoEstimado").val(atividades[i].QtTempoEstimado);

                if ($li.find("#txtQtTempoExecutado").val() != atividades[i].QtTempoExecutado)
                    $li.find("#txtQtTempoExecutado").val(atividades[i].QtTempoExecutado);

                if ($li.find("#txtVrCompletoPorcentagem").val() != atividades[i].VrCompletoPorcentagem)
                    $li.find("#txtVrCompletoPorcentagem").val(atividades[i].VrCompletoPorcentagem);

                if ($li.find("#txtDtInicio").val() != atividades[i].DtInicioStr)
                    $li.find("#txtDtInicio").val(atividades[i].DtInicioStr);

                if ($li.find("#txtDtTermino").val() != atividades[i].DtTerminoStr)
                    $li.find("#txtDtTermino").val(atividades[i].DtTerminoStr);

                if ($li.find("#txtDescricaoAtividade").val() != atividades[i].DsAtividade)
                    $li.find("#txtDescricaoAtividade").val(atividades[i].DsAtividade);
            }
        }

        if (typeof local == "boolean" && !local) {
            //Processa as remoções
            for (i in idsExistentes) {
                if (idsNovas.indexOf(idsExistentes[i]) == -1)
                    $("li[data-aid=" + idsExistentes[i] + "]").remove();
            }

            for (i in atividades) {
                //Corrige hierarquia
                var pai = atividades[i].IdAtividadePredecessora == null ? undefined : atividades[i].IdAtividadePredecessora;
                SetFilho(pai, atividades[i].IdAtividadeLocal);
            }
        }
        unsaveChanges = false;
        canAutoSave = true;

        //Correção das ID's locais para não dar bug
        LocalIdCounter = ++maiorIdLocal;
    }
}