<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ListarUsuarios.aspx.cs" Inherits="ListarUsuarios" %>

<%@ Register TagPrefix="Katapoka" TagName="BarraPaginacao" Src="UserControl/BarraPaginacaoUC.ascx" %>
<asp:Content ContentPlaceHolderID="HeaderContent" ID="Content1" runat="server">
    <script type="text/javascript" src="Scripts/Core/Paginacao.js"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BodyContent" ID="Content2" runat="server">
    <div>
        <a href="CadastrarUsuario.aspx" class="btn btn-primary"><span class="glyphicon glyphicon-new-window"></span>Novo usuário</a>
    </div>
    <br />
    <asp:Repeater runat="server" ID="rptGrid">
        <HeaderTemplate>
            <Katapoka:BarraPaginacao ID="BarraPaginacao1" runat="server" />
            <div class="table-responsive">
                <table class="table table-condensed table-bordered table-striped table-hover">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Nome</th>
                            <th>Nível</th>
                            <th>Cargo</th>
                            <th>E-mail</th>
                            <th>Ações</th>
                        </tr>
                    </thead>
                    <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%# Eval("IdUsuario") %></td>
                <td><%# Eval("DsNome") %></td>
                <td><%# Eval("UsuarioNivel_Tb.DsNivel") %></td>
                <td><%# Eval("Cargo_Tb.DsCargo") %></td>
                <td><%# Eval("DsEmail") %></td>
                <td>
                    <a href="CadastrarUsuario.aspx?id=<%# Eval("IdUsuario") %>" class="btn btn-primary btn-sm" title="Editar"><span class="glyphicon glyphicon-pencil"></span>&nbsp;Editar</a>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody>
                </table>
            </div>
            <Katapoka:BarraPaginacao ID="BarraPaginacao1" runat="server" />
        </FooterTemplate>
    </asp:Repeater>
</asp:Content>
<asp:Content ContentPlaceHolderID="FooterContent" ID="Content3" runat="server">
</asp:Content>
