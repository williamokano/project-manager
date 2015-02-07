<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>
<%@ Register TagPrefix="Katapoka" TagName="Alerta" Src="~/UserControl/ModalAlert.ascx" %>
<!DOCTYPE html>
<html>
<head>

    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <meta name="author" content="Quântica Networks" />
    <link rel="shortcut icon" href="assets/ico/favicon.png" />

    <title>Signin Template for Bootstrap</title>

    <!-- Bootstrap core CSS -->
    <link href="assets/css/bootstrap.css" rel="stylesheet">

    <!-- Custom styles for this template -->
    <link href="assets/css/login.css" rel="stylesheet">

    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="assets/js/html5shiv.js"></script>
      <script src="assets/js/respond.min.js"></script>
    <![endif]-->

    <!-- CORE SCRIPTS -->
    <script type="text/javascript" src="assets/js/jquery.js"></script>
    <script type="text/javascript" src="assets/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="Scripts/Core/Main.js"></script>
    <script type="text/javascript" src="Scripts/Core/Modal.js"></script>
    <script type="text/javascript" src="Scripts/Core/Validacao.js"></script>
    
    <!-- PAGE SCRIPTS -->
    <script type="text/javascript" src="Scripts/Login.js"></script>
</head>
<body>
    <div class="container">
        <form class="form-signin" id="form1" runat="server">
            <h2 class="form-signin-heading">Por favor, faça login</h2>
            <input id="txtEmail" type="text" class="form-control" placeholder="Endereço de e-mail" autofocus>
            <input id="txtSenha" type="password" class="form-control" placeholder="Senha">
            <!--label class="checkbox">
                <input type="checkbox" value="lembrar">
                Lembrar-me
            </label-->
            <button id="btnLogin" class="btn btn-lg btn-primary btn-block" type="submit">Login</button>
        </form>
    </div>
    <!-- /container -->
    <Katapoka:Alerta ID="QNAlert_1" runat="server" />
</body>
</html>
