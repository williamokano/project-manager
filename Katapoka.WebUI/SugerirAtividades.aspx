<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SugerirAtividades.aspx.cs" Inherits="SugerirAtividades" %>

<asp:Content ContentPlaceHolderID="HeaderContent" ID="Content1" runat="server">
    <link rel="stylesheet" href="Css/Core/bootstrap-tagsinput.css" media="screen" />
    <link rel="stylesheet" href="Css/Core/jQueryUI/jquery-ui-1.10.3.custom.css" media="screen" />

    <script type="text/javascript" src="Scripts/Core/jquery.maskedinput.min.js"></script>
    <script type="text/javascript" src="Scripts/Core/jquery-ui-1.10.3.custom.js"></script>
    <script type="text/javascript" src="Scripts/Core/bootstrap-tagsinput.js"></script>
    <script type="text/javascript" src="Scripts/Core/Modal.js"></script>
    <script type="text/javascript" src="Scripts/Core/Validacao.js"></script>
    <script type="text/javascript" src="Scripts/Core/Utilitarios.js"></script>
    <script type="text/javascript" src="Scripts/SugerirAtividades.js"></script>
    <style type="text/css">
        .dados-atividade
        {
            margin-bottom: 10px;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="BodyContent" ID="Content2" runat="server">
    <h1>Atividades sugeridas</h1>

    <h3>Dados das atividades</h3>
    <asp:Repeater ID="rptAtividades" runat="server" OnItemDataBound="rptAtividades_ItemDataBound">
        <ItemTemplate>
            <div role="form" class="dados-atividade form-inline">
                <div class="form-group">
                    <label class="sr-only" for="txtIdFicticioAtividade">ID Atividade</label>
                    <input value="<%# ((RepeaterItem)Container).ItemIndex + 1 %>" style="width: 50px;" readonly="readonly" type="text" class="form-control" id="txtIdFicticioAtividade" placeholder="ID Atividade" />
                </div>
                <div class="form-group">
                    <label class="sr-only" for="txtNomeAtividade">Nome Atividade</label>
                    <input maxlength="200" type="text" class="form-control" id="txtNomeAtividade" runat="server" placeholder="Nome Atividade" />
                </div>
                <div class="form-group">
                    <label class="sr-only" for="txtNomeUsuario">Nome Usuário</label>
                    <input type="text" class="form-control nome-autocomplete" id="txtNomeUsuario" runat="server" placeholder="Nome Usuário" />
                </div>
                <div class="form-group">
                    <label class="sr-only" for="txtTempoPrevisto">Tempo previsto</label>
                    <input type="text" class="form-control campo-tempo" id="txtTempoPrevisto" runat="server" placeholder="Tempo previsto" />
                </div>
                <div class="form-group">
                    <label class="sr-only" for="txtIdAtividadePredecessora">Atividade Predecessora</label>
                    <input type="text" style="width: 50px;" class="form-control campo-predecessor" id="txtIdAtividadePredecessora" runat="server" placeholder="PRÉ" />
                </div>
                <div class="form-group">
                    <button type="button" class="btn btn-danger remover-linha">Remover</button>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div class="form-group">
        <button type="button" id="btnCancelar" class="btn btn-danger">Cancelar</button>
        <button type="button" id="btnAdicionarOutra" class="btn btn-primary">Adicionar atividade</button>
        <button type="button" id="btnSalvar" class="btn btn-success">Salvar</button>
    </div>
</asp:Content>
