<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ListarAtividades.aspx.cs" Inherits="ListarAtividades" %>

<%@ Register TagPrefix="Katapoka" TagName="BarraPaginacao" Src="UserControl/BarraPaginacaoUC.ascx" %>
<asp:Content ContentPlaceHolderID="HeaderContent" ID="Content1" runat="server">
    <script type="text/javascript" src="Scripts/Core/Paginacao.js"></script>
    <script type="text/javascript" src="Scripts/ListarAtividades.js"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BodyContent" ID="Content2" runat="server">
    <div>
        <a href="CadastrarAtividade.aspx" class="btn btn-primary"><span class="glyphicon glyphicon-new-window"></span>Nova atividade</a>
    </div>
    <br />
    <div class="dados-filtro form-group">
        <div class="row">
            <div class="col-md-3">
                <label class="control-label" for="ddlAtivos">Projetos ativos</label>
                <asp:DropDownList ID="ddlAtivos" CssClass="form-control" runat="server"></asp:DropDownList>
            </div>
            <div class="col-md-3">
                <label class="control-label" for="ddlProjeto">Projeto</label>
                <asp:DropDownList ID="ddlProjeto" DataValueField="IdProjeto" DataTextField="DsNome" CssClass="form-control" runat="server"></asp:DropDownList>
            </div>
            <div class="col-md-3 col-md-offset-3 text-right">
                <button id="btnFiltrar" type="button" class="btn btn-default botao-filtro"><span class="glyphicon glyphicon-search"></span>&nbsp;Filtrar</button>
            </div>
        </div>
    </div>
    <asp:Repeater runat="server" ID="rptGrid" OnItemDataBound="rptGrid_ItemDataBound">
        <HeaderTemplate>
            <Katapoka:BarraPaginacao ID="BarraPaginacao1" runat="server" />
            <div class="table-responsive">
                <table class="table table-condensed table-bordered table-striped table-hover">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Título</th>
                            <th title="Tempo estimado">TE</th>
                            <th title="Tempo executado">TX</th>
                            <th title="Porcentagem de conclusão">%</th>
                            <th>Dt. Início</th>
                            <th>Dt. Término</th>
                            <!--<th title="Título pré-atividade">Pré</th>-->
                            <th>Ações</th>
                        </tr>
                    </thead>
                    <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%# Eval("IdAtividade") %></td>
                <td><%# Eval("DsTituloAtividade") %></td>
                <td>
                    <asp:Literal ID="ltrQtTempoEstimado" runat="server" /></td>
                <td>
                    <asp:Literal ID="ltrQtTempoExecutado" runat="server" /></td>
                <td><%# Eval("VrCompletoPorcentagem") %></td>
                <td><%# Eval("DtInicio", "{0:dd/MM/yyyy}") %></td>
                <td><%# Eval("DtTermino", "{0:dd/MM/yyyy}") %></td>
                <!--<td>
                    <asp:Literal ID="ltrNomePredecessora" runat="server" /></td>-->
                <td>
                    <a href="CadastrarAtividade.aspx?id=<%# Eval("IdAtividade") %>" class="btn btn-primary btn-sm" title="Editar"><span class="glyphicon glyphicon-pencil"></span>&nbsp;Editar</a>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody>
                </table>
                <Katapoka:BarraPaginacao ID="BarraPaginacao1" runat="server" />
            </div>
        </FooterTemplate>
    </asp:Repeater>
</asp:Content>
<asp:Content ContentPlaceHolderID="FooterContent" ID="Content3" runat="server">
</asp:Content>
