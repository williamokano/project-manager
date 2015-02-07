<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CadastrarUsuario.aspx.cs" Inherits="CadastrarUsuario" %>

<asp:Content ContentPlaceHolderID="HeaderContent" ID="Content1" runat="server">
    <script type="text/javascript" src="Scripts/Core/jquery.maskedinput.min.js"></script>
    <script type="text/javascript" src="Scripts/Core/jquery.maskedinput.mask.plugin.nonodigito.js"></script>
    <script type="text/javascript" src="Scripts/Core/Modal.js"></script>
    <script type="text/javascript" src="Scripts/Core/Validacao.js"></script>
    <script type="text/javascript" src="Scripts/CadastrarUsuario.js"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BodyContent" ID="Content2" runat="server">
    <h1>Cadastrar usuário</h1>

    <hr />
    <h3>Dados do usuário</h3>
    <div role="form" class="dados-empresa form-horizontal">
        <!-- DADOS EMPRESA -->
        <p>
            Apenas digite a senha em novos cadastros e/ou em caso de intenção de altera-la.<br />
            Enviar a senha em branco, numa edição, não a editará!
        </p>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtNome">Nome</label>
            <div class="col-md-8">
                <input type="text" class="form-control" id="txtNome" runat="server" placeholder="Nome" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtEmail">E-mail</label>
            <div class="col-md-8">
                <input type="text" class="form-control" id="txtEmail" runat="server" placeholder="E-mail" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtSenha">Senha</label>
            <div class="col-md-8">
                <input type="password" class="form-control" id="txtSenha" runat="server" placeholder="Senha" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="ddlNivel">Nível</label>
            <div class="col-md-3">
                <select id="ddlNivel" runat="server" class="form-control"></select>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="ddlCargo">Cargo</label>
            <div class="col-md-3">
                <select id="ddlCargo" runat="server" class="form-control"></select>
            </div>
        </div>
    </div>
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
