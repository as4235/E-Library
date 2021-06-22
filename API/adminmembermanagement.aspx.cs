using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace API
{
    public partial class adminmembermanagement : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataBind(); // to update the grid view when new author is added immediately
        }

        // delete member permanently
        protected void Button2_Click(object sender, EventArgs e)
        {
            if (checkMemberExists())
            {
                deleteMember();
            }
            else if(TextBox1.Text.Trim().Equals(""))
            {
                Response.Write("<script>alert('Member ID cannot be empty');</script>");
            }
            else
            {
                Response.Write("<script>alert('Author ID does not exists');</script>");
                GridView1.DataBind(); // to update the grid view when new author is added immediately
            }
        }

        // go button
        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            if (checkMemberExists())
            {
                showMember();
            }
            else
            {
                Response.Write("<script>alert('Member ID does not exists');</script>");
                GridView1.DataBind(); // to update the grid view when new author is added immediately
            }
        }

        //activate button
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            if (checkMemberExists())
            {
                try
                {
                    SqlConnection con = new SqlConnection(strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand("UPDATE member_master_tbl set account_status = @account_status WHERE member_id = '" + TextBox1.Text.Trim() + "';", con);
                    // above is the query to insert data. INSERT DATA table_name (column_names) values(@column_names)

                    // now to send values from the html to @column_name parameter                    
                    cmd.Parameters.AddWithValue("@account_status", "Activated");

                    // executes the above SQL query
                    cmd.ExecuteNonQuery();
                    // closes the connection with the DB
                    con.Close();

                    Response.Write("<script>alert('Member status changed');</script>");
                    GridView1.DataBind(); // to update the grid view when new author is added immediately
                }
                catch (Exeception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Member ID does not exists');</script>");
            }
        }

        // pending button
        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            if (checkMemberExists())
            {
                try
                {
                    SqlConnection con = new SqlConnection(strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand("UPDATE member_master_tbl set account_status = @account_status WHERE member_id = '" + TextBox1.Text.Trim() + "';", con);
                    // above is the query to insert data. INSERT DATA table_name (column_names) values(@column_names)

                    // now to send values from the html to @column_name parameter                    
                    cmd.Parameters.AddWithValue("@account_status", "Pending");

                    // executes the above SQL query
                    cmd.ExecuteNonQuery();
                    // closes the connection with the DB
                    con.Close();

                    Response.Write("<script>alert('Member status changed');</script>");
                    GridView1.DataBind(); // to update the grid view when new author is added immediately
                }
                catch (Exeception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Author ID does not exists');</script>");
            }
        }

        // deactivate button
        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            if (checkMemberExists())
            {
                try
                {
                    SqlConnection con = new SqlConnection(strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand("UPDATE member_master_tbl set account_status = @account_status WHERE member_id = '" + TextBox1.Text.Trim() + "';", con);
                    // above is the query to insert data. INSERT DATA table_name (column_names) values(@column_names)

                    // now to send values from the html to @column_name parameter                    
                    cmd.Parameters.AddWithValue("@account_status", "Deactivated");
                    GridView1.DataBind(); // to update the grid view when new author is added immediately

                    // executes the above SQL query
                    cmd.ExecuteNonQuery();
                    // closes the connection with the DB
                    con.Close();

                    Response.Write("<script>alert('Member status changed');</script>");
                }
                catch (Exeception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Author ID does not exists');</script>");
            }
        }

        // show member (go button)
        void showMember()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT * FROM member_master_tbl WHERE member_id = '" + TextBox1.Text.Trim() + "'", con);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Session["full_name"] = dr.GetValue(0).ToString();
                        Session["dob"] = dr.GetValue(1).ToString();
                        Session["contact_no"] = dr.GetValue(2).ToString();
                        Session["email"] = dr.GetValue(3).ToString();
                        Session["state"] = dr.GetValue(4).ToString();
                        Session["city"] = dr.GetValue(5).ToString();
                        Session["pincode"] = dr.GetValue(6).ToString();
                        Session["full_address"] = dr.GetValue(7).ToString();
                        Session["account_status"] = dr.GetValue(8).ToString();
                    }
                    TextBox2.Text = Session["full_name"].ToString();
                    TextBox8.Text = Session["dob"].ToString();
                    TextBox3.Text = Session["contact_no"].ToString();
                    TextBox4.Text = Session["email"].ToString();
                    TextBox9.Text = Session["state"].ToString();
                    TextBox10.Text = Session["city"].ToString();
                    TextBox11.Text = Session["pincode"].ToString();
                    TextBox6.Text = Session["full_address"].ToString();
                    TextBox7.Text = Session["account_status"].ToString();
                    GridView1.DataBind(); // to update the grid view when new author is added immediately
                }
                else
                {
                    Response.Write("<script>alert('ID does not exists');</script>");
                }
            }
            catch (Exeception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        // delete existing member
        void deleteMember()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("DELETE FROM member_master_tbl WHERE member_id = '" + TextBox1.Text.Trim() + "'", con);
                // above is the query to insert data. DELETE DATA table_name (column_names) values(@column_names)

                // executes the above SQL query
                cmd.ExecuteNonQuery();
                // closes the connection with the DB
                con.Close();
                Response.Write("<script>alert('Member deleted successfully');</script>");
                clearForm();
                GridView1.DataBind(); // to update the grid view when new author is added immediately
            }
            catch (Exeception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        // function to check if member exist
        bool checkMemberExists()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT * FROM member_master_tbl WHERE member_id = '" + TextBox1.Text.Trim() + "';", con);
                // above is the query to select all data from author_master_tbl where value is equal to what user enters in "TextBox1"
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count >= 1)
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

        // fnction to clear form after memeber is deleted permanently
        void clearForm()
        {
            TextBox2.Text = "";
            TextBox8.Text = "";
            TextBox3.Text = "";
            TextBox4.Text = "";
            TextBox9.Text = "";
            TextBox10.Text = "";
            TextBox11.Text = "";
            TextBox6.Text = "";
            TextBox7.Text = "";
        }
    }
}