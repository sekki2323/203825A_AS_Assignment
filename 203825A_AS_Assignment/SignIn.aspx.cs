using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace _203825A_AS_Assignment
{
    public partial class SignIn : System.Web.UI.Page
    {
        static string lockstatus;
        static int attemptcount = 0;

        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        //static string errorMsg = "";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_signin(object sender, EventArgs e)
        {
            string pwd = password.Text.ToString().Trim();
            string useremail = emailtext.Text.ToString().Trim();

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

                    SqlConnection conStatus = new SqlConnection(connectionString);
                    string SQLSTATUS = "Select accountstatus from UserAccounts where emailaddress='" + useremail + "'";
                    SqlCommand statuscommand = new SqlCommand(SQLSTATUS, conStatus);
                    statuscommand.Parameters.AddWithValue("@emailaddress", useremail);
                    conStatus.Open();
                    //conStatus.Open();
                    //cmd.Connection = conStatus;
                    //SqlCommand comparestatus = new SqlCommand("Select accountstatus from UserAccounts where where emailaddress='" + useremail + "'");
                    //comparestatus.Parameters.AddWithValue("@emailaddress", useremail);
                    using (SqlDataReader reader = statuscommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string stat = null;
                            stat = reader["accountstatus"].ToString();

                            if (userHash.Equals(dbHash) && (stat == "open"))
                            {
                                Session["UserID"] = useremail;
                                string guid = Guid.NewGuid().ToString();
                                Session["AuthToken"] = guid;
                                Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                                successlogin();
                                Response.Redirect("HomePage.aspx", false);

                            }
                            else
                            {
                                Invalid_Login_Attempts.Text = "User Email or password is not valid. Please try again. Number of attempts remaining:" + (2 - attemptcount);
                                attemptcount = attemptcount + 1;
                                failurelogin();
                                //Response.Redirect("HomePage.aspx", false);
                                //Response.Redirect("SignIn.aspx", false);
                                if (attemptcount == 3 || (stat == "locked"))
                                {
                                    Invalid_Login_Attempts.Text = "Your Account has been locked due to three or more Invalid Attempts. Please contact your administrator for assistance.";
                                    attemptcount = 0;
                                    String updatedata = "Update UserAccounts set accountstatus='locked' where emailaddress='" + useremail + "'";
                                    SqlConnection con = new SqlConnection(connectionString);
                                    con.Open();
                                    SqlCommand cmd = new SqlCommand();
                                    cmd.CommandText = updatedata;
                                    cmd.Connection = con;
                                    cmd.ExecuteNonQuery();
                                    failurelogin();
                                }

                            }

                            
                        }
                        
                    }
                    
                }
                else
                {
                    Invalid_Login_Attempts.Text = "User Email or password is not valid.";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
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

        protected void successlogin()
        {
            string useremail = emailtext.Text.ToString().Trim();
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {

                    sqlCon.Open();
                    
                    SqlCommand sqlCmd2 = new SqlCommand("UserAuditLogEdit", sqlCon);
                    sqlCmd2.CommandType = CommandType.StoredProcedure;
                    sqlCmd2.Parameters.AddWithValue("@Id", Convert.ToInt32(USERID1.Value == "" ? "0" : USERID1.Value));
                    sqlCmd2.Parameters.AddWithValue("@email", useremail);
                    sqlCmd2.Parameters.AddWithValue("@actiontaken", "Login of Account");
                    sqlCmd2.Parameters.AddWithValue("@datetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    sqlCmd2.Parameters.AddWithValue("@auditstatus", "Successful");
                    sqlCmd2.ExecuteNonQuery();
                    sqlCon.Close();
                    
                }

            }
            catch (Exception ex)
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlCommand sqlCmd2 = new SqlCommand("UserAuditLogEdit", sqlCon);
                    sqlCmd2.CommandType = CommandType.StoredProcedure;
                    sqlCmd2.Parameters.AddWithValue("@email",useremail);
                    sqlCmd2.Parameters.AddWithValue("@actiontaken", "Login of Account");
                    sqlCmd2.Parameters.AddWithValue("@datetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    sqlCmd2.Parameters.AddWithValue("@auditstatus", "Unsuccessful");

                    sqlCon.Close();
                }
                throw new Exception(ex.ToString());
            }
        }

        protected void failurelogin()
        {
            string useremail = emailtext.Text.ToString().Trim();
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {

                    sqlCon.Open();

                    SqlCommand sqlCmd2 = new SqlCommand("UserAuditLogEdit", sqlCon);
                    sqlCmd2.CommandType = CommandType.StoredProcedure;
                    sqlCmd2.Parameters.AddWithValue("@Id", Convert.ToInt32(USERID1.Value == "" ? "0" : USERID1.Value));
                    sqlCmd2.Parameters.AddWithValue("@email", useremail);
                    sqlCmd2.Parameters.AddWithValue("@actiontaken", "Login of Account");
                    sqlCmd2.Parameters.AddWithValue("@datetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    sqlCmd2.Parameters.AddWithValue("@auditstatus", "Unsuccessful");
                    sqlCmd2.ExecuteNonQuery();
                    sqlCon.Close();

                }

            }
            catch (Exception ex)
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlCommand sqlCmd2 = new SqlCommand("UserAuditLogEdit", sqlCon);
                    sqlCmd2.CommandType = CommandType.StoredProcedure;
                    sqlCmd2.Parameters.AddWithValue("@email", useremail);
                    sqlCmd2.Parameters.AddWithValue("@actiontaken", "Login of Account");
                    sqlCmd2.Parameters.AddWithValue("@datetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    sqlCmd2.Parameters.AddWithValue("@auditstatus", "Unsuccessful");

                    sqlCon.Close();
                }
                throw new Exception(ex.ToString());
            }
        }
        



    }

}
