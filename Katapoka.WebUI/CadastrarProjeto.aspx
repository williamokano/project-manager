<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CadastrarProjeto.aspx.cs" Inherits="CadastrarProjeto" %>
<asp:Content ContentPlaceHolderID="HeaderContent" ID="Content1" runat="server">
    <script type="text/javascript" src="Scripts/Core/jquery.maskedinput.min.js"></script>
    <script type="text/javascript" src="Scripts/Core/Modal.js"></script>
    <script type="text/javascript" src="Scripts/Core/Validacao.js"></script>
    <script type="text/javascript" src="Scripts/CadastrarProjeto.js"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BodyContent" ID="Content2" runat="server">
    <h1>Cadastrar projeto</h1>

    <h3>Dados projeto</h3>
    <!-- DADOS PROJETO -->
    <div role="form" class="dados-empresa form-horizontal">
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtNomeProjeto">Nome Projeto</label>
            <div class="col-md-8">
                <input maxlength="200" type="text" class="form-control" id="txtNomeProjeto" runat="server" placeholder="Nome do projeto" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="ddlEmpresa">Empresa</label>
            <div class="col-md-5">
                <select id="ddlEmpresa" runat="server" class="form-control"></select>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtDtInicio">Dt. Início</label>
            <div class="col-md-2">
                <input type="text" maxlength="10" class="form-control campo-data" id="txtDtInicio" runat="server" placeholder="Dt. Início" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtDtTermino">Dt. Término</label>
            <div class="col-md-2">
                <input type="text" maxlength="10" class="form-control campo-data" id="txtDtTermino" runat="server" placeholder="Dt. Término" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtCodigoReferencia">Cod. Referência</label>
            <div class="col-md-3">
                <input type="text" maxlength="20" class="form-control" id="txtCodigoReferencia" runat="server" placeholder="Código Referência" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="flStatus">Status</label>
            <div class="col-md-3">
                <select id="ddlFlStatus" runat="server" class="form-control">
                    <option value="E">Execução</option>
                    <option value="P">Paralisado</option>
                    <option value="C">Cancelado</option>
                </select>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="ddlTipoProjeto">Tipo do Projeto</label>
            <div class="col-md-4">
                <select id="ddlTipoProjeto" runat="server" class="form-control"></select>
            </div>
        </div>
    </div>
    <!-- FIM DADOS PROJETO -->
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
