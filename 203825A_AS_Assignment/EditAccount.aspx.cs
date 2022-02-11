using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _203825A_AS_Assignment
{
    public partial class EditAccount : System.Web.UI.Page
    {

        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("SignIn.aspx", false);
                }
                else
                {
                    lblmessage.Text = "Welcome" + Session["UserID"];
                    lblmessage.ForeColor = System.Drawing.Color.Green;


                    logoutbtn.Visible = true;
                }
            }
            else
            {
                Response.Redirect("SignIn.aspx", false);
            }
        }

        protected void btn_editpassword(object sender, EventArgs e)
        {
            string pwd = c_password.Text.ToString().Trim();
            string npwd = n_password.Text.ToString().Trim();
            string useremail = Session["UserID"].ToString();
            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(useremail);
            string dbSalt = getDBSalt(useremail);

            try
            {
                if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                {
                    string pwdWithSalt = pwd + dbSalt;
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                    string userHash = Convert.ToBase64String(hashWithSalt);

                    if (userHash.Equals(dbHash))
                    {
                        string finalHash;
                        string salt;
                        byte[] Key;
                        byte[] IV;
                        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                        byte[] saltByte = new byte[8];
                        rng.GetBytes(saltByte);
                        salt = Convert.ToBase64String(saltByte);
                        SHA512Managed hashed = new SHA512Managed();

                        string saltedpw = npwd + salt;
                        byte[] plainHash = hashed.ComputeHash(Encoding.UTF8.GetBytes(npwd));
                        byte[] hashsalt = hashed.ComputeHash(Encoding.UTF8.GetBytes(saltedpw));
                        finalHash = Convert.ToBase64String(hashsalt);
                        RijndaelManaged cipher = new RijndaelManaged();
                        cipher.GenerateKey();
                        Key = cipher.Key;
                        IV = cipher.IV;
                        SqlConnection con = new SqlConnection(connectionString);
                        //String updatedata = "Update UserAccounts set passwordsalt='" + salt + "', passwordhash='" + finalHash + "', Key='" + Convert.ToBase64String(Key) + "', IV='" + Convert.ToBase64String(IV) + "' where emailaddress='" + useremail + "'";

                        using (SqlConnection sqlCon = new SqlConnection(connectionString))
                        {

                            sqlCon.Open();
                            SqlCommand sqlCmd = new SqlCommand("UPDATE UserAccounts SET passwordhash = @passwordhash, passwordsalt = @passwordsalt, [Key] = @Key, IV = @IV where emailaddress='" + useremail + "'", sqlCon);
                            SqlCommand sqlCmd2 = new SqlCommand("UserAuditLogEdit", sqlCon);
                            sqlCmd2.CommandType = CommandType.StoredProcedure;
                            sqlCmd.Parameters.AddWithValue("@passwordhash", finalHash);
                            sqlCmd.Parameters.AddWithValue("@passwordsalt", salt);
                            sqlCmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            sqlCmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            sqlCmd.ExecuteNonQuery();
                            sqlCmd2.Parameters.AddWithValue("@Id", Convert.ToInt32(USERID.Value == "" ? "0" : USERID.Value));
                            sqlCmd2.Parameters.AddWithValue("@email", useremail);
                            sqlCmd2.Parameters.AddWithValue("@actiontaken", "Updating passwords");
                            sqlCmd2.Parameters.AddWithValue("@datetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            sqlCmd2.Parameters.AddWithValue("@auditstatus", "Successful");
                            sqlCmd2.ExecuteNonQuery();
                            sqlCon.Close();
                            lblmessage.Text = "Password has been changed successfully!";
                        }
                    }
                    else
                    {

                        errormessage.Text = "Current Password Incorrect. Please try again.";

                    }  

                }
                else
                {
                    errormessage.Text = "Current Password Incorrect. Please try again.";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }

        }

        protected void LogoutUser(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Response.Redirect("SignIn.aspx", false);
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }

            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
        }

        protected string getDBSalt(string useremail)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(connectionString);
            string sql = "select PASSWORDSALT FROM UserAccounts WHERE emailaddress=@emailaddress";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@emailaddress", useremail);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return s;

        }

        protected string getDBHash(string useremail)
        {

            string h = null;

            SqlConnection connection = new SqlConnection(connectionString);
            string sql = "select PasswordHash FROM UserAccounts WHERE emailaddress=@emailaddress";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@emailaddress", useremail);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return h;
        }

    }
}