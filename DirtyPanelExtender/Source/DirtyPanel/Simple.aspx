<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Simple.aspx.cs" Inherits="DirtyPanel._Simple" %>

<%@ Register Assembly="DirtyPanelExtender" Namespace="DirtyPanelExtender" TagPrefix="dp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <title>Dirty Panel Demo</title>
</head>
<body>
 <form id="form1" runat="server">
  <asp:ScriptManager ID="ScriptManager1" runat="server" />
  <div>
   Dirty Panel
  </div>
  <p>
   Write something, then <a href="Default.aspx">click here</a> to go back.
  </p>
  <dp:DirtyPanelExtender ID="demoPanelExtender" runat="server" TargetControlID="demoPanel"
   OnLeaveMessage="There's still unsaved data on the page!" />
  <asp:UpdatePanel ID="demoPanel" runat="server">
   <ContentTemplate>
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
    <p>
     <asp:RadioButtonList ID="demoRadio" runat="server">
      <asp:ListItem Selected="true" Text="first" />
      <asp:ListItem Text="second" />
      <asp:ListItem Text="third" />
     </asp:RadioButtonList>
    </p>
    <p>
     <asp:ListBox ID="demoListBox" runat="server">
      <asp:ListItem Selected="true" Text="first" />
      <asp:ListItem Text="second" />
      <asp:ListItem Text="third" />
     </asp:ListBox>
    </p>
    <p>
     <asp:ListBox ID="demoListBoxMultiple" SelectionMode="Multiple" runat="server">
      <asp:ListItem Selected="true" Text="first" />
      <asp:ListItem Text="second" />
      <asp:ListItem Selected="true" Text="third" />
     </asp:ListBox>
    </p>
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
   </ContentTemplate>
  </asp:UpdatePanel>
 </form>
</body>
</html>
