<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="_203825A_AS_Assignment.HomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SITCONNECT Home Page </title>
     <link href="css/Style.css" rel="stylesheet" />
    <link rel="stylesheet" href="fonts/material-icon/css/material-design-iconic-font.min.css" />
    <script src="https://www.google.com/recaptcha/api.js?render=6LelS2ceAAAAAOtgq_l29IWmzq-v9itDmh3HMomw"></script>
</head>
<body>
    <form id="LOGOUTFORM" runat="server">
        <asp:HiddenField ID="USERID1" runat="server"/>
        <div>
            <!-- Sing in  Form -->
        <section class="sign-in">
            <div class="container">
                <div class="signin-content">
                    <div class="signin-image">
                        <figure><img src="images/signin-image.jpg" alt="sing up image"/></figure>
                         <div class="form-group form-button">
                               <asp:Button ID="logoutbtn" runat="server" Text="Logout" OnClick="LogoutUser" Visible="false" class="form-submit" />
                            </div>
                        
                    </div>
                    <div class="signin-form">
                        <h2 class="form-title">SITConnect Home Page</h2>
                            <div class="form-group">
                            <p>User Currently Logged In: </p> 
                            <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
                            </div>
                        <asp:Button ID="Button1" runat="server" Text="Edit Account" PostBackUrl="~/EditAccount.aspx" class="form-submit" />
                              
                         <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
                    </div>
                </div>
            </div>
        </section>
        </div>
    </form>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6LelS2ceAAAAAOtgq_l29IWmzq-v9itDmh3HMomw', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
     <script src="vendor/jquery/jquery.min.js"></script>
    <script src="js/main.js"></script>
</body>
</html>
