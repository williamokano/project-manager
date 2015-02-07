<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AtividadesTipoProjeto.aspx.cs" Inherits="AtividadesTipoProjeto" %>

<asp:Content ContentPlaceHolderID="HeaderContent" ID="Content1" runat="server">
    <link rel="stylesheet" href="Css/Core/jQueryUI/jquery-ui-1.10.3.custom.min.css" />
    <link rel="stylesheet" href="Css/AtividadesProjeto.css" />
    <link rel="stylesheet" href="Css/Core/bootstrap-tagsinput.css">

    <script src="Scripts/Core/Utilitarios.js"></script>
    <script src="Scripts/Core/jquery.mask.js"></script>
    <script src="Scripts/Core/jquery-ui-1.10.3.custom.js"></script>
    <script src="Scripts/Core/jquery.mjs.nestedSortable.js"></script>
    <script src="Scripts/Core/bootstrap-tagsinput.js"></script>
    <script src="Scripts/Core/Validacao.js"></script>
    <script src="Scripts/Core/Modal.js"></script>
    <script src="Scripts/AtividadesTipoProjeto.js"></script>

    <script>
        function waitEvent() {
            API.WaitMessage(hashLogin,
                function (result) {
                    canAutoSave = false; //Desabilita o autosave
                    var response = eval("(" + result + ")");
                    RefreshDisplay(response, false);
                    setTimeout(waitEvent, 0);
                }, function () {
                    setTimeout(waitEvent, 0);
                });
        }
    </script>
</asp:Content>

<asp:Content ContentPlaceHolderID="BodyContent" ID="Content2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="2147483647">
        <Services>
            <asp:ServiceReference Path="~/API.asmx" />
        </Services>
    </asp:ScriptManager>
    <script>        //waitEvent();</script>
    <h1>Atividades</h1>

    <ol id="sortable" class="sortable">
        <li>
            <div>
                <asp:Literal ID="ltrNomeProjeto" runat="server" />
            </div>
            <span class="add-btn primeiro">
                <button class="btn btn-success btn-xs">+</button></span>
            <ol>
                <asp:Repeater ID="rptAtividades" runat="server" OnItemDataBound="rptAtividades_ItemDataBound">
                    <ItemTemplate>
                        <li data-aid="<%# Eval("IdTipoAtividade") %>" data-lid='<asp:Literal ID="ltrIdLocal" runat="server" />'>
                            <div class="titulo-atividade">
                                <span class="titulo">
                                    <asp:Literal ID="ltrTituloAtividade" runat="server" /></span>
                            </div>
                            <div class="form" role="form">
                                <div class="form-group">
                                    <label class="control-label" for="txtTituloAtividade">Título Atividade</label>
                                    <input type="text" class="form-control titulo" id="txtTituloAtividade" runat="server" />
                                    <span class="help-block error-block">O título da atividade é obrigatório.</span>
                                </div>
                                <div class="form-group">
                                    <label class="control-label" for="txtQtTempoEstimado">Tempo Estimado</label>
                                    <input type="text" class="form-control campo-tempo requer-validacao valida-tempo" id="txtQtTempoEstimado" runat="server" />
                                    <span class="help-block error-block">Este campo é obrigatório e requer um valor maior do que 0.</span>
                                </div>
                                <div class="form-group">
                                    <label class="control-label" for="txtDescricaoAtividade">Descrição</label>
                                    <textarea class="form-control" id="txtDescricaoAtividade" rows="5" runat="server"></textarea>
                                    <span class="help-block error-block">Este campo é obrigatório.</span>
                                </div>
                                <div class="form-group">
                                    <label class="control-label" for="txtTags">Tags</label>
                                    <input type="text" class="form-control tags-autocomplete" id="txtTags" runat="server" />
                                </div>
                            </div>
                            <span class="seta">&nbsp;</span>
                            <span class="add-btn">
                                <button class="btn btn-success btn-xs">+</button></span>
                            <span class="remove-btn">
                                <button class="btn btn-danger btn-xs">-</button></span>
                            <asp:Repeater ID="rptAtividadesInterna" runat="server">
                                <HeaderTemplate>
                                    <ol>
                                </HeaderTemplate>
                                <FooterTemplate></ol></FooterTemplate>
                            </asp:Repeater>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ol>
        </li>
    </ol>

    <br />
    <br />
    <div class="form" role="form">
        <div class="form-group">
            <button type="button" class="btn btn-success" id="btnSalvar">Salvar</button>
        </div>
    </div>

</asp:Content>
<asp:Content ContentPlaceHolderID="FooterContent" ID="Content3" runat="server">
</asp:Content>
