<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Codebehind="Slave.aspx.cs"
 Inherits="DirtyPanel._Slave" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div>
  Dirty Panel
 </div>
 <p>
  Write something, then <a href="Default.aspx">click here</a> to go back.
 </p>
 <p>
  <asp:TextBox ID="demoTextBoxSimple" runat="server" />
 </p>
 <p>
  <asp:DropDownList ID="demoDropDown" runat="server">
   <asp:ListItem Selected="true" Text="first" />
   <asp:ListItem Text="second" />
   <asp:ListItem Text="third" />
  </asp:DropDownList>
 </p>
 <!--
     <p>
      <asp:RadioButtonList ID="demoRadio" runat="server">
       <asp:ListItem Selected="true" Text="first" />
       <asp:ListItem Text="second" />
       <asp:ListItem Text="third" />
      </asp:RadioButtonList>   
     </p>
     -->
 <p>
  Enter Text:
  <br />
  <asp:TextBox ID="demoTextBox" runat="server" TextMode="MultiLine" Rows="5" />
 </p>
 <p>
  <asp:CheckBox ID="demoCheckBox" runat="server" Checked="true" Text="Checkbox" />
 </p>
 <p>
  <asp:Button ID="demoButton" runat="server" Text="Save" OnClick="save_Click" />
  <asp:Label ID="lastsaved" runat="server" Text="not saved" />
 </p>
</asp:Content>
