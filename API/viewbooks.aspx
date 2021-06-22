<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="viewbooks.aspx.cs" Inherits="API.viewbooks" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content1" runat="server">

    <div class="container">
        <div class="col">
              <center>
                     <h4>Book Inventory List</h4>                         
               </center>
         </div>
         <div class="row">
                     <div class="col">
                        <asp:GridView class="table table-striped table-bordered" ID="GridView1" runat="server"></asp:GridView>
                     </div>
         </div>
         <div class="row">
                     <div class="col">
                        <a href="homepage.aspx"><< Back to Home</a><br><br>
                     </div>
                  </div>
    </div>

</asp:Content>
