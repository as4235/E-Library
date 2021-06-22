using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace API
{
    public partial class adminbookinventory : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            fillAuthorPublisherValues();
            GridView1.DataBind(); // to update the grid view when new book is added immediately
        }
         
        // go button
        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            if (checkBookExists())
            {
                showBook();
            }
            else
            {
                Response.Write("<script>alert('Book ID does not exists');</script>");
                GridView1.DataBind(); // to update the grid view when new book is added immediately
            }
        }

        // add button
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (checkBookExists())
            {
                Response.Write("<script>alert('Book ID exists');</script>");
                GridView1.DataBind(); // to update the grid view when new author is added immediately                
            }
            else
            {
                addBook();
            }
        }

        // update button
        protected void Button3_Click(object sender, EventArgs e)
        {
            if (checkBookExists())
            {
                updatBook();
            }
            else
            {
                Response.Write("<script>alert('Book ID does not exists');</script>");
                GridView1.DataBind(); // to update the grid view when new author is added immediately
            }
        }

        // delete button
        protected void Button2_Click(object sender, EventArgs e)
        {
            if (checkBookExists())
            {
                deleteBook();
            }
            else
            {
                Response.Write("<script>alert('Book ID does not exists');</script>");
                GridView1.DataBind(); // to update the grid view when new author is added immediately
            }
        }

        // function ot get author and publisher data
        void fillAuthorPublisherValues()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                    con.Open();
                    }
                    // to fill author drop down list with all the authors on load
                    SqlCommand cmd = new SqlCommand("SELECT * FROM author_master_tbl;", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    DropDownList3.DataSource = dt; // to populate a drop down list with every data in the table
                    DropDownList3.DataValueField = "author_name"; // specify which column the data needs to come from even if * not used
                    DropDownList3.DataBind(); // to bind the data with the drop down list 

                    // to fill publisher drop down list with all the authors on load
                    SqlCommand cmd2 = new SqlCommand("SELECT * FROM publisher_master_tbl;", con);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);
                    DropDownList2.DataSource = dt2; // to populate a drop down list with every data in the table
                    DropDownList2.DataValueField = "publisher_name"; // specify which column the data needs to come from even if * not used
                    DropDownList2.DataBind(); // to bind the data with the drop down list 
            }
            catch(Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "')</script>");
            }
        }

        // add books
        void addbook()
        {
            try
            {
                string genres = "";
                foreach(int i in ListBox1.GetSelectedIndices())
                {
                    genres = genres + ListBox1.Items[i] + ","; // this will add - thriller, love,
                }
                genres = genres.Remove(genres.Length - 1); // this wil remove the last value which is ","

                string filepath = "~/book_inventory/books1.png";
                string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                FileUpload1.SaveAs(Server.MapPath("book_inventory/" + filename));
                filepath = "~/book_inventory/" + filename;

                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("INSERT INTO book_master_tbl(book_id,book_name,genre,author_name,publisher_name,publish_date,language,edition,book_cost,no_of_pages,book_description,actual_stock,current_stock,book_img_link) values(@book_id,@book_name,@genre,@author_name,@publisher_name,@publish_date,@language,@edition,@book_cost,@no_of_pages,@book_description,@actual_stock,@current_stock,@book_img_link)", con);

                cmd.Parameters.AddWithValue("@book_id", TextBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@book_name", TextBox2.Text.Trim());
                cmd.Parameters.AddWithValue("@genre", genres); // gets multiple value using for each loop written above 
                cmd.Parameters.AddWithValue("@author_name", DropDownList3.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@publisher_name", DropDownList2.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@publish_date", TextBox3.Text.Trim());
                cmd.Parameters.AddWithValue("@language", DropDownList1.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@edition", TextBox9.Text.Trim());
                cmd.Parameters.AddWithValue("@book_cost", TextBox10.Text.Trim());
                cmd.Parameters.AddWithValue("@no_of_pages", TextBox11.Text.Trim());
                cmd.Parameters.AddWithValue("@book_description", TextBox6.Text.Trim());
                cmd.Parameters.AddWithValue("@actual_stock", TextBox4.Text.Trim());
                cmd.Parameters.AddWithValue("@current_stock", TextBox4.Text.Trim());
                cmd.Parameters.AddWithValue("@book_img_link", filepath);
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "')</script>");
            }
        }
















        // show books
        void showBook()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT * from book_master_tbl WHERE book_id='" + TextBox1.Text.Trim() + "';", con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Session["book_name"] = dr.GetValue(0).ToString();
                        Session["genre"] = dr.GetValue(1).ToString();
                        Session["author_name"] = dr.GetValue(2).ToString();
                        Session["publisher_name"] = dr.GetValue(3).ToString();
                        Session["publish_date"] = dr.GetValue(4).ToString();
                        Session["language"] = dr.GetValue(5).ToString();
                        Session["edition"] = dr.GetValue(6).ToString();
                        Session["book_cost"] = dr.GetValue(7).ToString();
                        Session["no_of_pages"] = dr.GetValue(8).ToString();
                        Session["book_description"] = dr.GetValue(9).ToString();
                        Session["actual_stock"] = dr.GetValue(10).ToString();
                        Session["current_stock"] = dr.GetValue(11).ToString();
                        Session["book_img_link"] = dr.GetValue(12).ToString();
                    }
                    TextBox2.Text = Session["book_name"].ToString();
                    ListBox1.Text = Session["genre"].ToString();
                    DropDownList3.Text = Session["author_name"].ToString();
                    DropDownList2.Text = Session["publisher_name"].ToString();
                    TextBox3.Text = Session["publish_date"].ToString();
                    DropDownList1.Text = Session["language"].ToString();
                    TextBox9.Text = Session["edition"].ToString();
                    TextBox10.Text = Session["book_cost"].ToString();
                    TextBox11.Text = Session["no_of_pages"].ToString();
                    TextBox2.Text = Session["book_description"].ToString();
                    TextBox4.Text = Session["actual_stock"].ToString();
                    TextBox5.Text = Session["current_stock"].ToString();
                    TextBox4.Text = Session["book_img_link"].ToString();
                    GridView1.DataBind(); // to update the grid view when new author is added immediately
                    GridView1.DataBind(); // to update the grid view when new author is added immediately
                }

            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "')</script>");
            }
        }

        // check if Book Exists
        bool checkBookExists()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT book_id FROM book_master_tbl WHERE book_id ='" + TextBox1.Text.Trim() + "';", con);
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

                con.Close();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
                return false;
            }
        }
    }   
}