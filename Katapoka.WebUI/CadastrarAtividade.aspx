<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CadastrarAtividade.aspx.cs" Inherits="CadastrarAtividade" %>

<asp:Content ContentPlaceHolderID="HeaderContent" ID="Content1" runat="server">
    <link rel="stylesheet" href="Css/Core/bootstrap-tagsinput.css" />
    <link rel="stylesheet" href="Css/Core/jQueryUI/jquery-ui-1.10.3.custom.css" />

    <script type="text/javascript" src="Scripts/Core/jquery-ui-1.10.3.custom.js"></script>
    <script type="text/javascript" src="Scripts/Core/jquery.maskedinput.min.js"></script>
    <script type="text/javascript" src="Scripts/Core/jquery.maskedinput.mask.plugin.nonodigito.js"></script>
    <script type="text/javascript" src="Scripts/Core/bootstrap-tagsinput.js"></script>
    
    <script type="text/javascript" src="Scripts/Core/Modal.js"></script>
    <script type="text/javascript" src="Scripts/Core/Validacao.js"></script>
    <script type="text/javascript" src="Scripts/Core/Utilitarios.js"></script>
    <script type="text/javascript" src="Scripts/CadastrarAtividade.js"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BodyContent" ID="Content2" runat="server">
    <h1>Cadastrar atividade</h1>
    <h3>Dados da atividade</h3>
    <div role="form" class="dados-empresa form-horizontal">
        <!-- DADOS ATIVIDADE -->
        <div class="form-group">
            <label class="col-md-2 control-label" for="ddlProjeto">Projeto</label>
            <div class="col-md-6">
                <select class="form-control" id="ddlProjeto" runat="server"></select>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="ddlAtividadePredecessora">Atividade Predecessora</label>
            <div class="col-md-6">
                <select class="form-control" id="ddlAtividadePredecessora" runat="server">
                    <option value="">Selecione um projeto antes</option>
                </select>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtTags">Tags</label>
            <div class="col-md-6">
                <input type="text" class="form-control" id="txtTags" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtTituloAtividade">Título atividade</label>
            <div class="col-md-6">
                <input type="text" class="form-control" id="txtTituloAtividade" runat="server" placeholder="Título atividade" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtTempoEstimado">Tempo estimado</label>
            <div class="col-md-2">
                <input type="text" class="form-control campo-hora" id="txtTempoEstimado" runat="server" placeholder="00:00" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtTempoExecutado">Tempo executado</label>
            <div class="col-md-2">
                <input type="text" class="form-control campo-hora" readonly="true" id="txtTempoExecutado" runat="server" placeholder="00:00" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtPorcentagem">% Completo</label>
            <div class="col-md-1">
                <input type="text" class="form-control" id="txtPorcentagem" runat="server" placeholder="0" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtDtInicio">Dt. Início</label>
            <div class="col-md-2">
                <input type="text" class="form-control campo-data" id="txtDtInicio" runat="server" placeholder="dd/MM/yyyy" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtDtTermino">Dt. Término</label>
            <div class="col-md-2">
                <input type="text" class="form-control campo-data" id="txtDtTermino" runat="server" placeholder="dd/MM/yyyy" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtDescricao">Descrição</label>
            <div class="col-md-8">
                <textarea rows="5" class="form-control" id="txtDescricao" runat="server" placeholder="Descrição da atividade"></textarea>
            </div>
        </div>
    </div>
    <!-- FIM DADOS ATIVIDADE -->
    <hr />
    <div role="form" class="botoes-acao form-horizontal">
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <button type="button" class="btn btn-danger" id="btnCancelar">Cancelar</button>
                <button type="button" class="btn btn-success" id="btnSalvar">Salvar</button>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ContentPlaceHolderID="FooterContent" ID="Content3" runat="server">
</asp:Content>
