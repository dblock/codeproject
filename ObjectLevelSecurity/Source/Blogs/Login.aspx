<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" Title="Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <asp:Login ID="LoginForm" runat="server" BackColor="#EFF3FB" BorderColor="#B5C7DE"
  BorderPadding="4" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em"
  ForeColor="#333333">
  <TextBoxStyle Font-Size="0.8em" />
  <LoginButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid" BorderWidth="1px"
   Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284E98" />
  <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
  <TitleTextStyle BackColor="#507CD1" Font-Bold="True" Font-Size="0.9em" ForeColor="White" />
 </asp:Login>
 <br />
 <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" BackColor="#EFF3FB" 
  BorderColor="#B5C7DE" BorderStyle="Solid" BorderWidth="1px" 
  Font-Names="Verdana" Font-Size="0.8em">
  <SideBarStyle BackColor="#507CD1" Font-Size="0.9em" VerticalAlign="Top" />
  <SideBarButtonStyle BackColor="#507CD1" Font-Names="Verdana" 
   ForeColor="White" />
  <ContinueButtonStyle BackColor="White" BorderColor="#507CD1" 
   BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" 
   ForeColor="#284E98" />
  <NavigationButtonStyle BackColor="White" BorderColor="#507CD1" 
   BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" 
   ForeColor="#284E98" />
  <HeaderStyle BackColor="#284E98" BorderColor="#EFF3FB" BorderStyle="Solid" 
   BorderWidth="2px" Font-Bold="True" Font-Size="0.9em" ForeColor="White" 
   HorizontalAlign="Center" />
  <CreateUserButtonStyle BackColor="White" BorderColor="#507CD1" 
   BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" 
   ForeColor="#284E98" />
  <TitleTextStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
  <StepStyle Font-Size="0.8em" />
  <WizardSteps>
   <asp:CreateUserWizardStep runat="server" />
   <asp:CompleteWizardStep runat="server" />
  </WizardSteps>
 </asp:CreateUserWizard>
</asp:Content>

