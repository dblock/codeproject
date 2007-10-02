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
  <p>
   <script type="text/javascript" language="javascript">
    function replaceAll(what, source, target) {
        var temp = what;
        var i = temp.indexOf(source);
        while(i > -1) {
            temp = temp.replace(source, target);
            i = temp.indexOf(source, i + target.length + 1);
        }
        return temp;
    }    
    function showDirty() {
        var result = "";
        for (i in DirtyPanelExtender_dirtypanels) {
            var panel = DirtyPanelExtender_dirtypanels[i];
            var panelresult = replaceAll(panel.toString(), "\n", "<li>");
            panelresult = replaceAll(panelresult, ":", ":</b><ul>");
            result = result + "<p><b>" + panelresult + "</ul></p>";
        }
        document.getElementById("dirtyState").innerHTML = result;
    }
   </script>
   <a href="#" onclick="showDirty();">&#187; check dirty state</a>
  </p>
  <div id="dirtyState"></div>
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
     <asp:Button ID="demoDropDownAdd" runat="server" OnClick="demoDropDownAdd_Click" Text="Add" />
    </p>
    <p>
     <asp:RadioButtonList ID="demoRadio" runat="server">
      <asp:ListItem Selected="true" Text="first" />
      <asp:ListItem Text="second" />
      <asp:ListItem Text="third" />
     </asp:RadioButtonList>
     <asp:Button Enabled="false" ID="demoRadioAdd" runat="server" OnClick="demoRadioAdd_Click" Text="Add" />
    </p>
    <p>
     <asp:CheckBoxList ID="demoCheckBoxList" runat="server">
      <asp:ListItem Selected="true" Text="first" />
      <asp:ListItem Text="second" />
      <asp:ListItem Text="third" />
     </asp:CheckBoxList>
     <asp:Button Enabled="false" ID="demoCheckBoxListAdd" runat="server" OnClick="demoCheckBoxListAdd_Click" Text="Add" />
    </p>
    <p>
     <asp:ListBox ID="demoListBox" runat="server">
      <asp:ListItem Selected="true" Text="first" />
      <asp:ListItem Text="second" />
      <asp:ListItem Text="third" />
     </asp:ListBox>
     <asp:Button ID="demoListBoxAdd" runat="server" OnClick="demoListBoxAdd_Click" Text="Add" />
    </p>
    <p>
     <asp:ListBox ID="demoListBoxMultiple" SelectionMode="Multiple" runat="server">
      <asp:ListItem Selected="true" Text="first" />
      <asp:ListItem Text="second" />
      <asp:ListItem Selected="true" Text="third" />
     </asp:ListBox>
     <asp:Button ID="demoListBoxMultipleAdd" runat="server" OnClick="demoListBoxMultipleAdd_Click"
      Text="Add" />
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
