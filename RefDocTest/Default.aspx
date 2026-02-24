<%@ Page Title="Default" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RefDocTest._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card">
        <h2>Default Page</h2>
        <div class="row">
            <asp:Label ID="lblStatus" runat="server" CssClass="muted" />
        </div>
        <div class="row">
            <asp:Label ID="lblScope" runat="server" Text="Scope (editable):" AssociatedControlID="txtScope" /><br />
            <asp:TextBox ID="txtScope" runat="server" Text="https://graph.microsoft.com/.default" />
        </div>
        <div class="row">
            <asp:Button ID="btnGetToken" runat="server" Text="Get Access Token" OnClick="btnGetToken_Click" />
            <asp:Button ID="btnCallGraph" runat="server" Text="Call Graph API" OnClick="btnCallGraph_Click" />
        </div>
        <div class="row">
            <asp:Label ID="lblMessage" runat="server" CssClass="error" />
        </div>
        <div class="row">
            <asp:TextBox ID="txtOutput" runat="server" TextMode="MultiLine" />
        </div>
    </div>
</asp:Content>