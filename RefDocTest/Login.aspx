<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="RefDocTest.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card">
        <h2>Login</h2>
        <div class="row">
            <asp:Label ID="lblUserName" runat="server" Text="Username:" AssociatedControlID="txtUsername" /><br />
            <asp:TextBox ID="txtUsername" runat="server" />
        </div>
        <div class="row">
            <asp:Label ID="lblPassword" runat="server" Text="Password:" AssociatedControlID="txtPassword" /><br />
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" />
        </div>
        <div class="row">
            <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
        </div>
        <div class="row">
            <asp:Label ID="lblError" runat="server" CssClass="error" />
        </div>
    </div>
</asp:Content>