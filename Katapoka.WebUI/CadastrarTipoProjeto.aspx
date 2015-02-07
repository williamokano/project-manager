<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CadastrarTipoProjeto.aspx.cs" Inherits="CadastrarTipoProjeto" %>

<asp:Content ContentPlaceHolderID="HeaderContent" ID="Content1" runat="server">
    <script type="text/javascript" src="Scripts/Core/Modal.js"></script>
    <script type="text/javascript" src="Scripts/Core/Validacao.js"></script>
    <script type="text/javascript" src="Scripts/CadastrarTipoProjeto.js"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BodyContent" ID="Content2" runat="server">
    <h1>Cadastrar Tipo de Projeto</h1>

    <hr />
    <h3>Dados do Tipo de Atividade</h3>
    <div role="form" class="dados-tipo-projeto form-horizontal">
        <!-- DADOS DO TIPO DE ATIVIDADE -->
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtTipoProjeto">Tipo de Projeto</label>
            <div class="col-md-8">
                <input maxlength="50" type="text" class="form-control" id="txtTipoProjeto" runat="server" placeholder="Tipo de Projeto" />
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
