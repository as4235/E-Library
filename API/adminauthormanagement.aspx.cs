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
    public partial class adminauthormanagement : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataBind(); // to update the grid view when new author is added immediately
        }

        // add button
        protected void Button2_Click(object sender, EventArgs e)
        {
            if (checkAuthorExists())
            {
                Response.Write("<script>alert('Author ID already exists');</script>");
            }
            else
            {
                addNewAuthor();
            }
        }

        // update
        protected void Button3_Click(object sender, EventArgs e)
        {
            if (checkAuthorExists())
            {
                updateAuthor();                
            }
            else
            {
                Response.Write("<script>alert('Author ID does not exists');</script>");
            }
        }

        //delete
        protected void Button4_Click(object sender, EventArgs e)
        {
            if (checkAuthorExists())
            {
                deleteAuthor();
            }
            else
            {
                Response.Write("<script>alert('Author ID does not exists');</script>");
            }
        }

        // go button
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (checkAuthorExists())
            {
                showAuthor();
            }
            else
            {
                Response.Write("<script>alert('Author ID does not exists');</script>");
            }
        }

        // add new author
        void addNewAuthor()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("INSERT INTO author_master_tbl (author_id, author_name) values(@author_id, @author_name)", con);
                // above is the query to insert data. INSERT DATA table_name (column_names) values(@column_names)

                // now to send values from the html to @column_name parameter
                cmd.Parameters.AddWithValue("@author_id", TextBox1.Text.Trim());
                // the text is trimmed of blank spaces from "TextBox1" text box and passed to @full_name parameter                
                cmd.Parameters.AddWithValue("@author_name", TextBox2.Text.Trim());

                // executes the above SQL query
                cmd.ExecuteNonQuery();
                // closes the connection with the DB
                con.Close();
                Response.Write("<script>alert('Author added successfully');</script>");
                GridView1.DataBind(); // to update the grid view when new author is added immediately
            }
            catch (Exeception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        // update new author
        void updateAuthor()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("UPDATE author_master_tbl set author_name = @author_name WHERE author_id = '" +TextBox1.Text.Trim()+ "'", con);
                // above is the query to update data. UPDATE DATA table_name (column_names) values(@column_names)

                // now to send values from the html to @column_name parameter
                cmd.Parameters.AddWithValue("@author_name", TextBox2.Text.Trim());

                // executes the above SQL query
                cmd.ExecuteNonQuery();
                // closes the connection with the DB
                con.Close();
                Response.Write("<script>alert('Author updated successfully');</script>");
                GridView1.DataBind(); // to update the grid view when new author is added immediately
            }
            catch (Exeception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        // delete existing author
        void deleteAuthor()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("DELETE FROM author_master_tbl WHERE author_id = '" + TextBox1.Text.Trim() + "'", con);
                // above is the query to insert data. DELETE DATA table_name (column_names) values(@column_names)

                // executes the above SQL query
                cmd.ExecuteNonQuery();
                // closes the connection with the DB
                con.Close();
                Response.Write("<script>alert('Author deleted successfully');</script>");
                GridView1.DataBind(); // to update the grid view when new author is added immediately
            }
            catch (Exeception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        // show author (go button)
        void showAuthor()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT author_name FROM author_master_tbl WHERE author_id = '" + TextBox1.Text.Trim() + "'", con);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Session["author_name"] = dr.GetValue(0).ToString();
                    }
                    TextBox2.Text = Session["author_name"].ToString();

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

        // function to check if author exist
        bool checkAuthorExists()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT * FROM author_master_tbl WHERE author_id = '" + TextBox1.Text.Trim() + "';", con);
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

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}