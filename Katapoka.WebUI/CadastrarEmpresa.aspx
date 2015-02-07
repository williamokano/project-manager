<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CadastrarEmpresa.aspx.cs" Inherits="CadastrarEmpresa" %>

<asp:Content ContentPlaceHolderID="HeaderContent" ID="Content1" runat="server">
    <script type="text/javascript" src="Scripts/Core/jquery.maskedinput.min.js"></script>
    <script type="text/javascript" src="Scripts/Core/jquery.maskedinput.mask.plugin.nonodigito.js"></script>
    <script type="text/javascript" src="Scripts/Core/Modal.js"></script>
    <script type="text/javascript" src="Scripts/Core/Validacao.js"></script>
    <script type="text/javascript" src="Scripts/CadastrarEmpresa.js"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BodyContent" ID="Content2" runat="server">
    <h1>Cadastrar empresa</h1>

    <hr />
    <h3>Dados da empresa</h3>
    <div role="form" class="dados-empresa form-horizontal">
        <!-- DADOS EMPRESA -->
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtNomeFantasia">Nome Fantasia</label>
            <div class="col-md-8">
                <input maxlength="200" type="text" class="form-control" id="txtNomeFantasia" runat="server" placeholder="Nome fantasia" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtRazaoSocial">Razão Social</label>
            <div class="col-md-8">
                <input maxlength="200" type="text" class="form-control" id="txtRazaoSocial" runat="server" placeholder="Razão Social" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtCnpj">CNPJ</label>
            <div class="col-md-3">
                <input type="text" maxlength="18" class="form-control campo-cnpj" id="txtCnpj" runat="server" placeholder="CNPJ" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="ddlAreaAtuacao">Área de Atuação</label>
            <div class="col-md-4">
                <select size="1" id="ddlAreaAtuacao" class="form-control" runat="server">
                    <option value="">Selecione uma área de atuação</option>
                </select>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtEmail">E-mail</label>
            <div class="col-md-4">
                <input type="text" maxlength="200" required="required" class="form-control" id="txtEmail" runat="server" placeholder="E-mail" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtUrlSite">URL Site</label>
            <div class="col-md-4">
                <input type="text" maxlength="150" required="required" class="form-control" id="txtUrlSite" runat="server" placeholder="URL Site" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtSumario">Sumário empresa</label>
            <div class="col-md-6">
                <textarea class="form-control" maxlength="200" rows="3" id="txtSumario" runat="server" placeholder="Sumário da empresa"></textarea>
            </div>
        </div>
    </div>

    <!-- FIM DADOS EMPRESA -->
    <hr />

    <!-- DADOS TELEFONE -->
    <h3>Telefones</h3>
    <div role="form" class="dados-telefone form-horizontal">
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtTelefoneResidencial">Residencial</label>
            <div class="col-md-3">
                <input type="text" maxlength="16" class="form-control campo-telefone" id="txtTelefoneResidencial" placeholder="Telefone residencial" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtTelefoneCelular">Celular</label>
            <div class="col-md-3">
                <input type="text" maxlength="16" class="form-control campo-telefone" id="txtTelefoneCelular" placeholder="Telefone celular" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtTelefoneComercial">Comercial</label>
            <div class="col-md-3">
                <input type="text" maxlength="16" class="form-control campo-telefone" id="txtTelefoneComercial" placeholder="Telefone comercial" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtFax">Fax</label>
            <div class="col-md-3">
                <input type="text" maxlength="16" class="form-control campo-telefone" id="txtFax" placeholder="FAX" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtObservacaoContato">Observação</label>
            <div class="col-md-6">
                <textarea class="form-control" maxlength="500" rows="3" id="txtObservacaoContato" runat="server" placeholder="Observação"></textarea>
            </div>
        </div>
    </div>
    <!-- FIM DADOS TELEFONE -->
    <hr />

    <!-- ENDEREÇO -->
    <h3>Dados do endereço</h3>
    <div role="form" class="dados-endereco form-horizontal">
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtCep">CEP</label>
            <div class="col-md-2">
                <input type="text" maxlength="10" class="form-control campo-cep" id="txtCep" placeholder="CEP" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtEndereco">Endereço</label>
            <div class="col-md-5">
                <input type="text" maxlength="200" class="form-control" id="txtEndereco" placeholder="Endereço" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtNumero">Número</label>
            <div class="col-md-2">
                <input type="text" maxlength="20" class="form-control" id="txtNumero" placeholder="Número" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="txtComplemento">Complemento</label>
            <div class="col-md-5">
                <input type="text" maxlength="50" class="form-control" id="txtComplemento" placeholder="Complemento" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="ddlEstados">Estado</label>
            <div class="col-md-4">
                <select size="1" id="ddlEstados" class="form-control" runat="server"></select>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label" for="ddlCidades">Cidade</label>
            <div class="col-md-4">
                <select size="1" id="ddlCidades" class="form-control" runat="server">
                    <option value="">Selecione um estado primeiro</option>
                </select>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label">Bairro</label>
            <div class="col-md-4">
                <select size="1" id="ddlBairros" class="form-control" runat="server">
                    <option value="">Selecione uma cidade primeiro</option>
                </select>
            </div>
        </div>
        <div class="form-group" id="divOutroBairro" style="display:none;" runat="server">
            <label class="col-md-2 control-label" for="txtOutroBairro">Nome Bairro</label>
            <div class="col-md-4">
                <input type="text" maxlength="50" class="form-control" id="txtOutroBairro" placeholder="Nome do Bairro" runat="server" />
            </div>
        </div>

        <!-- FIM ENDEREÇO -->
    </div>
    <hr />
    <div role="form" class="dados-aceites form-horizontal">
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <div class="checkbox">
                    <label>
                        <input type="checkbox" id="flAceiteTermo" runat="server" />&nbsp;Termo aceite
                    </label>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <div class="checkbox">
                    <label>
                        <input type="checkbox" id="flAprovada" runat="server" />&nbsp;Aprovada?
                    </label>
                </div>
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
