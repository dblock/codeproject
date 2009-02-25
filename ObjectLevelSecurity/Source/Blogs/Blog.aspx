<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
 CodeFile="Blog.aspx.cs" Inherits="BlogPage" Title="Blog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <p>
  <a href="Default.aspx">&lt; back</a>
 </p>
 <h1>
  <asp:Label ID="blogName" runat="server" Text="Untitled" />
 </h1>
 <p>
  <h3>Post New</h3>
  <p>
   Title:
   <asp:TextBox ID="inputBlogPostTitle" runat="server" />
  </p>
  <p>
   Body:
   <asp:TextBox ID="inputBlogPostBody" runat="server" TextMode="MultiLine" />
  </p>
  <asp:LinkButton ID="createBlogPost" runat="server" OnClick="createBlogPost_Click" Text="post" />  
 </p>
 <asp:GridView ID="gridBlogPosts" runat="server" AutoGenerateColumns="False" CellPadding="4"
  ForeColor="#333333" GridLines="None">
  <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
  <RowStyle BackColor="#EFF3FB" />
  <Columns>
   <asp:BoundField DataField="Id" />
   <asp:TemplateField>
    <ItemTemplate>
      <h3><%# Eval("Title") %></h3>
    </ItemTemplate>
   </asp:TemplateField>
   <asp:TemplateField>
    <ItemTemplate>
     <div style="border: solid 1px silver;">
      <%# Eval("Body") %>
     </div>
     <div style="font-size: smaller;">
      <%# Eval("Created") %>
     </div>
    </ItemTemplate>
   </asp:TemplateField>
  </Columns>
  <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
  <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
  <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
  <EditRowStyle BackColor="#2461BF" />
  <AlternatingRowStyle BackColor="White" />
 </asp:GridView>
</asp:Content>
