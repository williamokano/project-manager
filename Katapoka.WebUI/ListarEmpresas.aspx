<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ListarEmpresas.aspx.cs" Inherits="ListarEmpresas" %>

<%@ Register TagPrefix="Katapoka" TagName="BarraPaginacao" Src="UserControl/BarraPaginacaoUC.ascx" %>
<asp:Content ContentPlaceHolderID="HeaderContent" ID="Content1" runat="server">
    <script type="text/javascript" src="Scripts/Core/Paginacao.js"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BodyContent" ID="Content2" runat="server">
    <div>
        <a href="CadastrarEmpresa.aspx" class="btn btn-primary"><span class="glyphicon glyphicon-new-window"></span>Cadastrar nova empresa</a>
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
                            <th>Razão Social</th>
                            <th>Nome Fantasia</th>
                            <th>CNPJ</th>
                            <th>Ações</th>
                        </tr>
                    </thead>
                    <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%# Eval("IdEmpresa") %></td>
                <td><%# Eval("DsRazaoSocial") %></td>
                <td><%# Eval("DsNomeFantasia") %></td>
                <td><%# Eval("NrCnpj") %></td>
                <td>
                    <a href="CadastrarEmpresa.aspx?id=<%# Eval("IdEmpresa") %>" class="btn btn-primary btn-sm" title="Editar"><span class="glyphicon glyphicon-pencil"></span>&nbsp;Editar</a>
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
