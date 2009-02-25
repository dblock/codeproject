<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
 CodeFile="Default.aspx.cs" Inherits="_Default" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <p>
  Logged in as:
  <asp:LoginName ID="loginName" runat="server" />
  &#187;
  <asp:LoginStatus ID="loginStatus" runat="server" />
 </p>
 <hr />
 <p>
  <h3>Create a Blog</h3>
  <div class="form">
   <p>
    Name:
    <asp:TextBox ID="inputBlogName" runat="server" />
    <asp:Button CssClass="button" ID="createBlog" runat="server" OnClick="createBlog_Click" Text="Create" />
   </p>
  </div>
  <h3><asp:Label ID="labelBlogs" Text="Your Blogs" runat="server" /></h3>
  <asp:GridView ID="gridBlogs" runat="server" AutoGenerateColumns="False" 
  CellPadding="4" ShowHeader="false">
   <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
   <RowStyle BackColor="#EFF3FB" />
   <Columns>
    <asp:BoundField DataField="Id" ItemStyle-HorizontalAlign="Center" />
    <asp:TemplateField>
     <ItemTemplate>
      <a href="Blog.aspx?id=<%# Eval("Id") %>">
       <%# Eval("Name") %>
      </a>
     </ItemTemplate>       
    </asp:TemplateField>
    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
     <ItemTemplate>
      <asp:LinkButton ID="linkDelete" runat="server" CommandArgument='<%# Eval("Id") %>' Text="delete" OnCommand="linkDelete_Command" />
     </ItemTemplate>
    </asp:TemplateField>
   </Columns>
   <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
   <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
   <EditRowStyle BackColor="#2461BF" />
   <AlternatingRowStyle BackColor="White" />
  </asp:GridView>
 </p>
</asp:Content>
