<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorHandling404.aspx.cs" Inherits="_203825A_AS_Assignment.ErrorHandling404" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="css/errorstyle.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Encode+Sans+Semi+Condensed:100,200,300,400" rel="stylesheet" />
    <title></title>
</head>

<body class="loading">
  <h1>404</h1>
  <h2>Unexpected Error <b>:(</b></h2>
  <div class="gears">
    <div class="gear one">
      <div class="bar"></div>
      <div class="bar"></div>
      <div class="bar"></div>
    </div>
    <div class="gear two">
      <div class="bar"></div>
      <div class="bar"></div>
      <div class="bar"></div>
    </div>
    <div class="gear three">
      <div class="bar"></div>
      <div class="bar"></div>
      <div class="bar"></div>
    </div>
  </div>
  <script src="https://code.jquery.com/jquery-1.10.2.js"></script>
  <script src="js/main.js" type="text/javascript"></script>
    <script>
        $(function () {
            setTimeout(function () {
                $('body').removeClass('loading');
            }, 1000);
        });

    </script>
</body>
</html>
