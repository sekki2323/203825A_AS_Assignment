<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditAccount.aspx.cs" Inherits="_203825A_AS_Assignment.EditAccount" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>Edit Account</title>

    <!-- Font Icon -->
    <link rel="stylesheet" href="fonts/material-icon/css/material-design-iconic-font.min.css">

    <!-- Main css -->
    <link rel="stylesheet" href="css/style.css">
    <script type="text/javascript">
           function validate() {
               var str = document.getElementById('<%=n_password.ClientID %>').value;
               const regex = /^(?=.* [A - Z])(?=.*[a-z])(?=.*[^a-zA-Z0-9])$/;
               if (str.length < 12) {
                   document.getElementById("lbl_pwdchecker").innerHTML = "Password Length Must be at Least 12 Characters";
                   document.getElementById("lbl_pwdchecker").style.color = "Red";
                   return ("too_short");
               }
               // Check at least one Uppercase, lowercase and special characters
               else if (str.search(/[0-9]/) == -1) {
                   document.getElementById("lbl_pwdchecker").innerHTML = "Password requires at least 1 number";
                   document.getElementById("lbl_pwdchecker").style.color = "Red";
                   return ("no_number");
               }

               else if (str.search(/[a-z]/) == -1) {
                   document.getElementById("lbl_pwdchecker").innerHTML = "Password requires at least 1 lowercase";
                   document.getElementById("lbl_pwdchecker").style.color = "Red";
                   return ("no_lowercase");
               }

               else if (str.search(/[A-Z]/) == -1) {
                   document.getElementById("lbl_pwdchecker").innerHTML = "Password requires at least 1 uppercase";
                   document.getElementById("lbl_pwdchecker").style.color = "Red";
                   return ("no_uppercase");
               }

               else if (str.search(/[^A-Za-z0-9]/) == -1) {
                   document.getElementById("lbl_pwdchecker").innerHTML = "Password requires at least 1 special character";
                   document.getElementById("lbl_pwdchecker").style.color = "Red";
                   return ("no_specialcahr");
               }
               document.getElementById("lbl_pwdchecker").innerHTML = "Strong Password!"
               document.getElementById("lbl_pwdchecker").style.color = "Blue";
           }

         
    </script>
</head>
    <body>
    <div class="main">
        <!-- Sign up form -->
       <section class="signup">
            <div class="container">
                <div class="signup-content">
                    <div class="signup-form">
                        <h2 class="form-title">Edit Account</h2>
                        
                        <form class="register-form" id="editform" runat="server">
                            <asp:HiddenField ID="USERID" runat="server"/>
                            <div class="form-group">
                                <label for="pass"><i class="zmdi zmdi-lock"></i></label>
                                <asp:TextBox ID="c_password" textmode="Password" runat="server"  placeholder="Current Password"></asp:TextBox>
                                
                            </div>
                            <div class="form-group">
                                 <asp:Label ID="errormessage" runat="server" EnableViewState="false" ForeColor="Red" Text="" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Current Password needs to be entered" ForeColor="Red" ControlToValidate="c_password"></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group">
                                <label for="re-pass"><i class="zmdi zmdi-lock-outline"></i></label>
                               <asp:TextBox ID="n_password" textmode="Password" runat="server"  placeholder="New Password" onkeyup="javascript:validate()"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="New Password needs to be filled" ForeColor="Red" ControlToValidate="n_password"></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group">
                                <label for="re-pass"><i class="zmdi zmdi-lock-outline"></i></label>
                               <asp:TextBox ID="nr_password" textmode="Password" runat="server"  placeholder="Retype new password"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="CompareValidator" Text="New passwords have to be the same" ControlToCompare="n_password" ControlToValidate="nr_password">New passwords have to be the same</asp:CompareValidator><br />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please enter new password again" ForeColor="Red" ControlToValidate="nr_password"></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblmessage" runat="server" EnableViewState="false" />
                               
                            </div>
                           
                            <div class="form-group form-button">
                                <asp:Button ID="btn_editpw" runat="server" Text="Change Password"  onclick="btn_editpassword" class="form-submit"/>
                                 <asp:Button ID="logoutbtn" runat="server" Text="Logout" OnClick="LogoutUser" Visible="false" class="form-submit" />
                            </div>
                        </form>
                    </div>
                    <div class="signup-image">
                        <figure><img src="images/signup-image.jpg" alt="sing up image"></figure>
                        <a href="HomePage.aspx" class="signup-image-link">Back to Home Page</a>
                    </div>
                </div>
            </div>
        </section>
        </div>
         <!-- JS -->
    <script src="vendor/jquery/jquery.min.js"></script>
    <script src="js/main.js"></script>
</body>
</html>