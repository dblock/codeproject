<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
 CodeFile="Blog.aspx.cs" Inherits="BlogPage" Title="Blog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <p>
  <a href="Default.aspx">&lt; back</a>
 </p>
 <h1>
  <asp:Label ID="blogName" runat="server" Text="Untitled" />
 </h1>
 <h3>Post New</h3>
 <div class="form">
  <p>
   Title:
   <asp:TextBox ID="inputBlogPostTitle" runat="server" Width="400" />
  </p>
  <p>
   Body:
   <asp:TextBox ID="inputBlogPostBody" runat="server" TextMode="MultiLine" Width="400" Height="50" />
  </p>
  <asp:Button CssClass="button" ID="createBlogPost" runat="server" OnClick="createBlogPost_Click"
   Text="Create Post" />
 </div>
 <h3>Posts</h3>
 <asp:GridView ID="gridBlogPosts" runat="server" AutoGenerateColumns="False" CellPadding="4"
  GridLines="None" ShowHeader="false">
  <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
  <RowStyle BackColor="#EFF3FB" />
  <Columns>
   <asp:BoundField DataField="Id" ItemStyle-HorizontalAlign="Center" />
   <asp:TemplateField>
    <ItemTemplate>
     <div style="font-size: 18px; font-weight: bold;"><%# Eval("Title") %></div>
     <div style="font-size: smaller;">
      posted by <b><%# Eval("Account.Name") %></b> on <%# Eval("Created") %>
     </div>
     <div style="padding: 5px 0px 5px 10px;">
      <%# Eval("Body") %>
     </div>
    </ItemTemplate>
   </asp:TemplateField>
   <asp:TemplateField ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
     <asp:LinkButton ID="linkDelete" runat="server" CommandArgument='<%# Eval("Id") %>'
      Text="Delete" OnCommand="linkDelete_Command" />
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
