<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BarraPaginacaoUC.ascx.cs"
    Inherits="Includes_BarraPaginacaoUC" %>
<div role="form" class="dados-ordenacao form-horizontal">
    <div class="row">
        <div class="col-md-3">
            <div class="form-group">
                <div class="col-md-12">
                    <label class="control-label" for="ddlOrdenacao">Ordenação</label>
                    <asp:DropDownList ID="ddlOrdenacao" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="col-md-7" style="padding-top: 7px;">
            <asp:Repeater ID="rptPaginas" runat="server" OnItemDataBound="rptPaginas_ItemDataBound">
                <HeaderTemplate>
                    <ul class="pagination">
                        <li class="prev" runat="server" id="prev"><a href="<%# string.Format(URLPaginacao, PaginaAtual-1) %>">&laquo;</a></li>
                </HeaderTemplate>
                <ItemTemplate>
                    <li runat="server" id="pagina"><a href="<%# string.Format(URLPaginacao, Container.ItemIndex) %>"><%# (Container.ItemIndex+1) %></a></li>
                </ItemTemplate>
                <FooterTemplate>
                    <li class="next" runat="server" id="next"><a href="<%# string.Format(URLPaginacao, PaginaAtual+1) %>">&raquo;</a></li>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <div class="col-md-2">
            <div class="form-group">
                <div class="col-md-12">
                    <label class="control-label" for="ddlQtdRegistrosPagina">Itens por página</label>
                    <asp:DropDownList ID="ddlQtdRegistrosPagina" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
        </div>
    </div>
</div>
