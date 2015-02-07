<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ListarTiposProjeto.aspx.cs" Inherits="ListarTiposProjeto" %>
<%@ Register TagPrefix="Katapoka" TagName="BarraPaginacao" Src="UserControl/BarraPaginacaoUC.ascx" %>
<asp:Content ContentPlaceHolderID="HeaderContent" ID="Content1" runat="server">
    <script type="text/javascript" src="Scripts/Core/Utilitarios.js"></script>
    <script type="text/javascript" src="Scripts/Core/Paginacao.js"></script>
    <script type="text/javascript" src="Scripts/ListarTiposProjeto.js"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BodyContent" ID="Content2" runat="server">
    <div>
        <a href="CadastrarTipoProjeto.aspx" class="btn btn-primary"><span class="glyphicon glyphicon-new-window"></span>&nbsp;Novo projeto</a>
    </div>
    <br />
    <!--
        Por ora irei deixar sem filtro
    <div class="dados-filtro form-group">
        <div class="row">
            <div class="col-md-3">
                <label class="control-label" for="ddlEmpresa">Empresa</label>
                <asp:DropDownList ID="ddlEmpresa" DataValueField="IdEmpresa" DataTextField="DsNomeFantasia" CssClass="form-control" runat="server"></asp:DropDownList>
            </div>
            <div class="col-md-3 col-md-offset-6 text-right">
                <button id="btnFiltrar" type="button" class="btn btn-default botao-filtro"><span class="glyphicon glyphicon-search"></span>&nbsp;Filtrar</button>
            </div>
        </div>
    </div>
    -->
    <asp:Repeater runat="server" ID="rptGrid">
        <HeaderTemplate>
            <Katapoka:BarraPaginacao ID="BarraPaginacao1" runat="server" />
            <div class="table-responsive">
                <table class="table table-condensed table-bordered table-striped table-hover table-responsive">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Tipo de Projeto</th>
                            <th>Ações</th>
                        </tr>
                    </thead>
                    <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%# Eval("IdTipoProjeto") %></td>
                <td><%# Eval("DsTipo") %></td>
                <td>
                    <a href="CadastrarTipoProjeto.aspx?id=<%# Eval("IdTipoProjeto") %>" class="btn btn-primary btn-sm" title="Editar"><span class="glyphicon glyphicon-pencil"></span>&nbsp;Editar</a>
                    <a href="AtividadesTipoProjeto.aspx?pid=<%# Eval("IdTipoProjeto") %>" class="btn btn-primary btn-sm" title="Atividades"><span class="glyphicon glyphicon-download"></span>&nbsp;Atividades</a>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody>
                </table>
                </div>
                <Katapoka:BarraPaginacao ID="BarraPaginacao2" runat="server" />
        </FooterTemplate>
    </asp:Repeater>
</asp:Content>
<asp:Content ContentPlaceHolderID="FooterContent" ID="Content3" runat="server">
</asp:Content>
