<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
 CodeFile="Default.aspx.cs" Inherits="_Default" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <p>
  Logged in as:
  <asp:LoginName ID="loginName" runat="server" />
  &#187;
  <asp:LoginStatus ID="loginStatus" runat="server" />
  <hr />
  <asp:Label ID="labelUserContext" runat="server" />
 </p>
 <hr />
 <p>
  <h3>Blogs</h3>
  <asp:GridView ID="gridBlogs" runat="server" AutoGenerateColumns="False" 
  CellPadding="4" ForeColor="#333333" GridLines="None">
   <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
   <RowStyle BackColor="#EFF3FB" />
   <Columns>
    <asp:BoundField DataField="Id" />
    <asp:TemplateField>
     <ItemTemplate>
      <a href="Blog.aspx?id=<%# Eval("Id") %>">
       <%# Eval("Name") %>
      </a>
     </ItemTemplate>   
    </asp:TemplateField>
   </Columns>
   <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
   <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
   <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
   <EditRowStyle BackColor="#2461BF" />
   <AlternatingRowStyle BackColor="White" />
  </asp:GridView>
  <h3>Create a Blog</h3>
  <p>
   Name:
   <asp:TextBox ID="inputBlogName" runat="server" />
   <asp:LinkButton ID="createBlog" runat="server" OnClick="createBlog_Click" Text="create" />
  </p>
 </p>
</asp:Content>
