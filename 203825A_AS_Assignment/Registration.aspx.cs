using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;

namespace _203825A_AS_Assignment
{
    public partial class Registration : System.Web.UI.Page
    {
        int newId = 0;
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        static string line = "\r";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                imageHyper.Visible = false;
            }
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
           
            string pwd = password.Text.ToString().Trim();
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByte = new byte[8];
            rng.GetBytes(saltByte);
            salt = Convert.ToBase64String(saltByte);
            SHA512Managed hashing = new SHA512Managed();

            string pwdWithSalt = pwd + salt;
            byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
            finalHash = Convert.ToBase64String(hashWithSalt);
            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;

            HttpPostedFile postedFile = imageUpload.PostedFile;
            string fileName = Path.GetFileName(postedFile.FileName);
            string fileExtension = Path.GetExtension(fileName);
            int fileSize = postedFile.ContentLength;

            if (fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".bmp" 
                || fileExtension.ToLower() == ".png" || fileExtension.ToLower() == ".gif")
            {
                Stream stream = postedFile.InputStream;
                BinaryReader binaryReader = new BinaryReader(stream);
                byte[] bytes = binaryReader.ReadBytes((int)stream.Length);
                
            }
            else
            {
                lblImage.Visible = true;
                lblImage.Text = "ONLY IMAGES (.JPG, .PNG, .GIF AND .BMP) CAN BE UPLOADED";
                lblImage.ForeColor = System.Drawing.Color.Red;
                imageHyper.Visible = false;
            }

            validations();
            createAccount();         
            //Response.Redirect("SignIn.aspx", false);
            

        }

        protected void createAccount()
        {
           

            HttpPostedFile postedFile = imageUpload.PostedFile;
            Stream stream = postedFile.InputStream;
            BinaryReader binaryReader = new BinaryReader(stream);
            byte[] bytes = binaryReader.ReadBytes((int)stream.Length);


            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {

                    sqlCon.Open();
                    SqlCommand CheckEmail = new SqlCommand("select emailaddress from UserAccounts where emailaddress='" + email.Text + "'", sqlCon);
                    SqlDataAdapter sd = new SqlDataAdapter(CheckEmail);
                    DataTable dt = new DataTable();
                    sd.Fill(dt);
                    if(dt.Rows.Count > 0)
                    {
                        errormessage.Text = "Email already exists.";
                        
                    }
                    else
                    {
                        SqlCommand sqlCmd = new SqlCommand("UserAddOrEdit", sqlCon);
                        SqlCommand sqlCmd2 = new SqlCommand("UserAuditLogEdit", sqlCon);
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd2.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@Id", Convert.ToInt32(USERID.Value == "" ? "0" : USERID.Value));
                        sqlCmd.Parameters.AddWithValue("@firstname", fstb.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@lastname", lntb.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@creditcard", Convert.ToBase64String(encryptData(creditcard.Text.Trim())));
                        sqlCmd.Parameters.AddWithValue("@emailaddress", email.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@passwordhash", finalHash);
                        sqlCmd.Parameters.AddWithValue("@passwordsalt", salt);
                        sqlCmd.Parameters.AddWithValue("@dob", dobtb.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@photoname", Path.GetFileName(postedFile.FileName));
                        sqlCmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                        sqlCmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                        sqlCmd.Parameters.AddWithValue("@photosize", postedFile.ContentLength);
                        sqlCmd.Parameters.AddWithValue("@imagedata", bytes);
                        sqlCmd.Parameters.AddWithValue("@accountstatus", "open");
                        sqlCmd.ExecuteNonQuery();
                        sqlCmd2.Parameters.AddWithValue("@Id", Convert.ToInt32(USERID.Value == "" ? "0" : USERID.Value));
                        sqlCmd2.Parameters.AddWithValue("@email", email.Text.Trim());
                        sqlCmd2.Parameters.AddWithValue("@actiontaken", "Registration of Account");
                        sqlCmd2.Parameters.AddWithValue("@datetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        sqlCmd2.Parameters.AddWithValue("@auditstatus", "Successful");
                        sqlCmd2.ExecuteNonQuery();
                        sqlCon.Close();
                        successmessage.Text = "Registration Completed Successfully!";
                        Response.AddHeader("REFRESH", "3;URL=SignIn.aspx");
                    }
                   
                      
                }
                    
            }

            catch (Exception ex)
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlCommand sqlCmd2 = new SqlCommand("UserAuditLogEdit", sqlCon);
                    sqlCmd2.CommandType = CommandType.StoredProcedure;
                    sqlCmd2.Parameters.AddWithValue("@email", email.Text.Trim());
                    sqlCmd2.Parameters.AddWithValue("@actiontaken", "Registration of Account");
                    sqlCmd2.Parameters.AddWithValue("@datetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    sqlCmd2.Parameters.AddWithValue("@auditstatus", "Unsuccessful");

                    sqlCon.Close();
                    errormessage.Text = "Unsuccessful Registration. Please try again later.";

                }
                    throw new Exception(ex.ToString());
            }
           
        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);


                //Encrypt
                //cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
                //cipherString = Convert.ToBase64String(cipherText);
                //Console.WriteLine("Encrypted Text: " + cipherString);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
            return cipherText;
        }

        protected void validations()
        {

            Regex regex1 = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Regex regex2 = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");

            if (!(Regex.IsMatch(fstb.Text, "^[A-Za-z]+$")))
            {
                Label1.Text = "Can only contain alphabetssss";
            }
            if (!(Regex.IsMatch(lntb.Text, "^[A-Za-z]+$")))
            {
                Label2.Text = "Can only contain alphabets";
            }
            if (!(Regex.IsMatch(creditcard.Text, "^[0-9]{16}$")))
            {
                Label3.Text = "Invalid Credit Card Number";
            }
            if (!(Regex.IsMatch(email.Text, regex1.ToString())))
            {
                Label4.Text = "Invalid Email Address";
            }
            if (!(Regex.IsMatch(password.Text, regex2.ToString())))
            {
                Label5.Text = "Invalid Email Address";
            }
            if (password.Text != rpassword.Text)
            {
                Label6.Text = "Password do not match";
            }
        }

        //protected void checkpasswordstrength(object sender, EventArgs e)
        //{
        //    // implement codes for the button event 
        //    // extract data from textbox
        //    int scores = checkPassword(password.Text);
        //    string status = "";
        //    switch (scores)
        //    {
        //        case 1:
        //            status = "Very Weak";
        //            break;
        //        case 2:
        //            status = "Weak";
        //            break;
        //        case 3:
        //            status = "Medium";
        //            break;
        //        case 4:
        //            status = "Strong";
        //            break;
        //        case 5:
        //            status = "Very Strong";
        //            break;
        //        default:
        //            break;
        //    }
        //    lbl_pwdchecker.Text = "Status: " + status;
        //    if (scores < 4)
        //    {
        //        lbl_pwdchecker.ForeColor = Color.Red;
        //        return;
        //    }
        //    lbl_pwdchecker.ForeColor = Color.Green;
        //}

        //private int checkPassword(string password)
        //{
        //    int score = 0;
        //    if (password.Length < 8)
        //    {
        //        return 1;
        //    }
        //    else
        //    {
        //        score = 1;
        //    }
        //    //score 2 weak
        //    if (Regex.IsMatch(password, "[a-z]"))
        //    {
        //        score++;
        //    }
        //    //score 3 medium 
        //    if (Regex.IsMatch(password, "[A-Z]"))
        //    {
        //        score++;
        //    }
        //    //score 4 strong
        //    if (Regex.IsMatch(password, "[0-9]"))
        //    {
        //        score++;
        //    }
        //    if (Regex.IsMatch(password, "(?=.*[^a-zA-Z0-9])"))
        //    {
        //        score++;
        //    }
        //    return score;
        //}
    }

        //void Clear()
        //{
        //    fstb.Text = lntb.Text = creditcard.Text = email.Text = password.Text = dob.Text = "";
        //    USERID.Value = "";
        //    successmessage.Text = errormessage.Text = "";
        //}


}
