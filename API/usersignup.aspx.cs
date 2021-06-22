using System;
using System.Collections.Generic;
using System.Configuration; // this is imported in order to link the aspx page to our MSSQL
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace API
{
    public partial class usersignup : System.Web.UI.Page
    {
        // a string is created using this syntax and the connection string variable is linked
        // this means that the value this variable corresponds to our MSSQL
        string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString; 
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        // signup botton click event
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (checkMemberExists())
            {
                Response.Write("<script>alert('ID exists');</script>");
            }
            else
            {
                signUpNewMember();
            }            
        }

        // function to check if user exist
        bool checkMemberExists()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT * FROM member_master_tbl WHERE member_id = '"+TextBox8.Text.Trim()+"';", con);
                // above is the query to select all data from member_master_tbl where value is equal to what user enters in "TextBox8"
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);   

                if(dt.Rows.Count >= 1)
                {
                    return true;    
                }
                else
                {
                    return false;
                }

                // closes the connection with the DB
                con.Close();

            }
            catch (Exeception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
                return false;
            }
        }

        // function to insert new user data
        void signUpNewMember()
        {

            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("INSERT INTO member_master_tbl (full_name, dob, contact_no, email, state, city, pincode, full_address, member_id, password, account_status) values(@full_name, @dob, @contact_no, @email, @state, @city, @pincode, @full_address, @member_id, @password, @account_status)", con);
                // above is the query to insert data. INSERT DATA table_name (column_names) values(@column_names)

                // now to send values from the html to @column_name parameter
                cmd.Parameters.AddWithValue("@full_name", TextBox1.Text.Trim());
                // the text is trimmed of blank spaces from "TextBox1" text box and passed to @full_name parameter                
                cmd.Parameters.AddWithValue("@dob", TextBox2.Text.Trim());
                cmd.Parameters.AddWithValue("@contact_no", TextBox3.Text.Trim());
                cmd.Parameters.AddWithValue("@email", TextBox4.Text.Trim());
                cmd.Parameters.AddWithValue("@state", DropDownList1.SelectedItem.Value);
                // the selected value from "DropDownList1" passed to @state parameter
                cmd.Parameters.AddWithValue("@city", TextBox6.Text.Trim());
                cmd.Parameters.AddWithValue("@pincode", TextBox7.Text.Trim());
                cmd.Parameters.AddWithValue("@full_address", TextBox5.Text.Trim());
                cmd.Parameters.AddWithValue("@member_id", TextBox8.Text.Trim());
                cmd.Parameters.AddWithValue("@password", TextBox9.Text.Trim());
                cmd.Parameters.AddWithValue("@account_status", "Pending");

                // executes the above SQL query
                cmd.ExecuteNonQuery();
                // closes the connection with the DB
                con.Close();

                Response.Redirect("userlogin.aspx");
            }
            catch (Exeception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }

        }

    }
}