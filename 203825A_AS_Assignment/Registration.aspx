<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="_203825A_AS_Assignment.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration</title>
    <link href="css/Style.css" rel="stylesheet" />
    <link rel="stylesheet" href="fonts/material-icon/css/material-design-iconic-font.min.css" />
       <script type="text/javascript">
           function validate() {
               var str = document.getElementById('<%=password.ClientID %>').value;
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
    <form id="form1" runat="server" class="register-form">
        <asp:HiddenField ID="USERID" runat="server"/>
        <div>
            <!-- Sign up form -->
        <section class="signup">
            <div class="container">
                <div class="signup-content">
                    <div class="signup-form">
                        <h2 class="form-title">Sign up</h2>
                        <div class="form-group">
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="First Name Required" ForeColor="Red" ControlToValidate="fstb"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                        <label for="lastname"><i class="zmdi zmdi-account material-icons-name"></i></label> 
                        <asp:TextBox ID="fstb" runat="server"  placeholder="First Name"></asp:TextBox>  
                        </div>
                        <div class="form-group">
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Can only contain alphabets" ControlToValidate="fstb" ForeColor="Red" ValidationExpression="^[A-Za-z]+$"></asp:RegularExpressionValidator><br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Last Name Required" ControlToValidate="lntb" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <label for="lastname"><i class="zmdi zmdi-account material-icons-name"></i></label>
                            <asp:TextBox ID="lntb" runat="server"  placeholder="Last Name"></asp:TextBox>
                            <asp:Label ID="lnlbl" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="form-group">
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Can only contain alphabets" ControlToValidate="lntb" ForeColor="Red" ValidationExpression="^[A-Za-z]+$"></asp:RegularExpressionValidator><br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Credit Card Info Needed" ControlToValidate="creditcard" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <label for="creditcard"><i class="zmdi zmdi-card"></i></label>
                            <asp:TextBox ID="creditcard" runat="server"  placeholder="Credit Card Info"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="Invalid CreditCard Number" ControlToValidate="creditcard" ForeColor="Red" ValidationExpression="^[0-9]{16}$"></asp:RegularExpressionValidator><br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Email Required" ControlToValidate="email" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <label for="email"><i class="zmdi zmdi-email"></i></label>
                            <asp:TextBox ID="email" textmode="Email" runat="server"  placeholder="Email Address"></asp:TextBox>
                        </div>
                        <div class="form-group">
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Invalid Email Entered" ControlToValidate="email" ForeColor="Red" ValidationExpression="^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$"></asp:RegularExpressionValidator><br />
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Password Required" ControlToValidate="password" ForeColor="Red"></asp:RequiredFieldValidator>          
                        </div>
                        <div class="form-group">
                            <label for="pass"><i class="zmdi zmdi-lock"></i></label>
                            <asp:TextBox textmode="Password" ID="password" runat="server"  placeholder="Password" onkeyup="javascript:validate()"></asp:TextBox>
                            <asp:Label ID="lbl_pwdchecker" runat="server" Text="Password Strength Checker"></asp:Label>  
                        </div>
                        <div class="form-group">
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="&quot;Passwords Mismatch!&quot;" ControlToCompare="password" ControlToValidate="rpassword" ForeColor="Red" ValidationGroup="1"></asp:CompareValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Password Required" ControlToValidate="rpassword" ForeColor="Red" ValidationGroup="1"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <label for="re-pass"><i class="zmdi zmdi-lock-outline"></i></label>
                            <asp:TextBox textmode="Password" ID="rpassword" runat="server"  placeholder="Repeat your password"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Date of Birth Required" ForeColor="Red" ControlToValidate="dobtb"></asp:RequiredFieldValidator>
                        </div>
                            <div class="form-group">
                                <label for="re-pass"><i class="zmdi zmdi-cake"></i></label>
                                <asp:TextBox textmode="Date" ID="dobtb" runat="server"  placeholder="Date of Birth"></asp:TextBox>
                            </div>
                        <div class="form-group">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Image Required" ControlToValidate="imageUpload" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                            <div class="form-group">
                               <label for="re-pass"><i class="zmdi zmdi-image"></i></label>
                                <asp:FileUpload ID="imageUpload" runat="server" />
                                <asp:Label ID="lblImage" runat="server"></asp:Label>
                                <asp:HyperLink ID="imageHyper" runat="server">View Uploaded Image</asp:HyperLink>
                            </div>
                            <div class="form-group form-button">
                                <asp:Button ID="btn_signup" runat="server"  Text="Register"  onclick="btn_Submit_Click" class="form-submit"/>
                            </div>
                    </div>
                    <asp:Label Text="" ID="successmessage" runat="server" ForeColor="Green"/>
                     <asp:Label Text="" ID="errormessage" runat="server" ForeColor="Red"/>
                    <div class="signup-image">
                        <figure><img src="images/signup-image.jpg" alt="sing up image"/></figure>
                        <a href="SignIn.aspx" class="signup-image-link">I am already member</a>
                        <asp:Label ID="Label1" runat="server" Text="" ForeColor="Red"></asp:Label><br />
                        <asp:Label ID="Label2" runat="server" Text="" ForeColor="Red"></asp:Label><br />
                        <asp:Label ID="Label3" runat="server" Text="" ForeColor="Red"></asp:Label><br />
                        <asp:Label ID="Label4" runat="server" Text="" ForeColor="Red"></asp:Label><br />
                        <asp:Label ID="Label5" runat="server" Text="" ForeColor="Red"></asp:Label><br />
                        <asp:Label ID="Label6" runat="server" Text="" ForeColor="Red"></asp:Label><br />
                        <asp:Label ID="Label7" runat="server" Text="" ForeColor="Red"></asp:Label>

                    </div>
                </div>
            </div>
        </section>    
        </div>
    </form>
    
    <script src="vendor/jquery/jquery.min.js"></script>
    <script src="js/main.js"></script>
   
</body>
</html>
