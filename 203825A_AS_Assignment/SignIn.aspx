<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="_203825A_AS_Assignment.SignIn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign In </title>
     <link href="css/Style.css" rel="stylesheet" />
    <link rel="stylesheet" href="fonts/material-icon/css/material-design-iconic-font.min.css" />
    <script src="https://www.google.com/recaptcha/api.js?render=6LelS2ceAAAAAOtgq_l29IWmzq-v9itDmh3HMomw"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="USERID1" runat="server"/>
        <div>
            <!-- Sing in  Form -->
        <section class="sign-in">
            <div class="container">
                <div class="signin-content">
                    <div class="signin-image">
                        <figure><img src="images/signin-image.jpg" alt="sing up image"/></figure>
                        <a href="Registration.aspx" class="signup-image-link">Create an account</a>
                    </div>
                    <div class="signin-form">
                        <h2 class="form-title">Sign In</h2>
                            <div class="form-group">
                                <label for="your_name"><i class="zmdi zmdi-email"></i></label>
                                 <asp:TextBox ID="emailtext" runat="server"  placeholder="Your Email"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="your_pass"><i class="zmdi zmdi-lock"></i></label>
                                 <asp:TextBox ID="password" textmode="Password" runat="server"  placeholder="Password"></asp:TextBox>
                            </div>
                            
                        <div class="form-group">
                                <asp:Label Text="" ID="Invalid_Login_Attempts" runat="server" ForeColor="Red"/>
                            </div>
                            <div class="form-group form-button">
                                <asp:Button ID="btn_signup" runat="server"  Text="Sign In"  onclick="btn_signin" class="form-submit"/>  
                            </div>
                        
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
