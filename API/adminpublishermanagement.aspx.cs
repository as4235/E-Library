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
    public partial class adminpublishermanagement : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataBind(); // to update the grid view when new author is added immediately
        }

        // go button
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (checkPublisherExists())
            {
                showPublisher();
            }
            else
            {
                Response.Write("<script>alert('Publisher ID does not exists');</script>");
            }
        }

        // add button
        protected void Button2_Click(object sender, EventArgs e)
        {
            if (checkPublisherExists())
            {
                Response.Write("<script>alert('Publisher ID already exists');</script>");
            }
            else
            {
                addNewPublisher();
            }
        }

        // update button
        protected void Button3_Click(object sender, EventArgs e)
        {
            if (checkPublisherExists())
            {
                updatePublisher();
            }
            else
            {
                Response.Write("<script>alert('Publisher ID does not exists');</script>");
            }
        }

        // delete button
        protected void Button4_Click(object sender, EventArgs e)
        {
            if (checkPublisherExists())
            {
                deletePublisher();
            }
            else
            {
                Response.Write("<script>alert('Publisher ID does not exists');</script>");
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // add new publisher
        void addNewPublisher()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("INSERT INTO publisher_master_tbl (publisher_id, publisher_name) values(@publisher_id, @publisher_name)", con);
                // above is the query to insert data. INSERT DATA table_name (column_names) values(@column_names)

                // now to send values from the html to @column_name parameter
                cmd.Parameters.AddWithValue("@publisher_id", TextBox1.Text.Trim());
                // the text is trimmed of blank spaces from "TextBox1" text box and passed to @full_name parameter                
                cmd.Parameters.AddWithValue("@publisher_name", TextBox2.Text.Trim());

                // executes the above SQL query
                cmd.ExecuteNonQuery();
                // closes the connection with the DB
                con.Close();
                Response.Write("<script>alert('Publisher added successfully');</script>");
                GridView1.DataBind(); // to update the grid view when new author is added immediately
            }
            catch (Exeception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        // update new author
        void updatePublisher()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("UPDATE publisher_master_tbl set publisher_name = @publisher_name WHERE publisher_id = '" + TextBox1.Text.Trim() + "'", con);
                // above is the query to update data. UPDATE DATA table_name (column_names) values(@column_names)

                // now to send values from the html to @column_name parameter
                cmd.Parameters.AddWithValue("@publisher_name", TextBox2.Text.Trim());

                // executes the above SQL query
                cmd.ExecuteNonQuery();
                // closes the connection with the DB
                con.Close();
                Response.Write("<script>alert('publisher updated successfully');</script>");
                GridView1.DataBind(); // to update the grid view when new author is added immediately
            }
            catch (Exeception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        // delete existing author
        void deletePublisher()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("DELETE FROM publisher_master_tbl WHERE publisher_id = '" + TextBox1.Text.Trim() + "'", con);
                // above is the query to insert data. DELETE DATA table_name (column_names) values(@column_names)

                // executes the above SQL query
                cmd.ExecuteNonQuery();
                // closes the connection with the DB
                con.Close();
                Response.Write("<script>alert('publisher deleted successfully');</script>");
                GridView1.DataBind(); // to update the grid view when new author is added immediately
            }
            catch (Exeception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        // show publisher (go button)
        void showPublisher()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT publisher_name FROM publisher_master_tbl WHERE publisher_id = '" + TextBox1.Text.Trim() + "'", con);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Session["publisher_name"] = dr.GetValue(0).ToString();
                    // here the value is 0 because we are only calling one column. We used 8 and stuff because we used *
                    }
                    TextBox2.Text = Session["publisher_name"].ToString();

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
        bool checkPublisherExists()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT * FROM publisher_master_tbl WHERE publisher_id = '" + TextBox1.Text.Trim() + "';", con);
                // above is the query to select all data from publisher_master_tbl where value is equal to what user enters in "TextBox1"
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
    }
}