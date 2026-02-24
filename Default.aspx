<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="REFDOCTest.Default" Async="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 50px; }
        .container { max-width: 900px; margin: 0 auto; }
        .header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 20px; }
        .welcome { color: #333; }
        .btn-logout { padding: 8px 16px; background-color: #dc3545; color: white; text-decoration: none; border-radius: 4px; }
        .btn-logout:hover { background-color: #c82333; }
        .content { padding: 20px; border: 1px solid #ddd; border-radius: 5px; margin-bottom: 20px; }
        .token-section { padding: 20px; border: 1px solid #007bff; border-radius: 5px; background-color: #f8f9fa; }
        .form-group { margin-bottom: 15px; }
        .form-group label { display: block; margin-bottom: 5px; font-weight: bold; }
        .form-group input[type="text"] { width: 100%; padding: 8px; border: 1px solid #ccc; border-radius: 4px; box-sizing: border-box; }
        .btn-get-token { padding: 10px 20px; background-color: #007bff; color: white; border: none; border-radius: 4px; cursor: pointer; font-size: 14px; }
        .btn-get-token:hover { background-color: #0056b3; }
        .token-display { margin-top: 20px; }
        .token-box { background-color: #fff; padding: 15px; border: 1px solid #ddd; border-radius: 4px; word-wrap: break-word; font-family: 'Courier New', monospace; font-size: 12px; max-height: 150px; overflow-y: auto; }
        .claims-table { width: 100%; border-collapse: collapse; margin-top: 15px; background-color: #fff; }
        .claims-table th { background-color: #007bff; color: white; padding: 10px; text-align: left; border: 1px solid #ddd; }
        .claims-table td { padding: 8px; border: 1px solid #ddd; word-break: break-all; }
        .claims-table tr:nth-child(even) { background-color: #f2f2f2; }
        .section-title { color: #007bff; margin-top: 20px; margin-bottom: 10px; }
        .error-message { color: #dc3545; padding: 10px; background-color: #f8d7da; border: 1px solid #f5c6cb; border-radius: 4px; margin-top: 10px; }
        .info-text { color: #666; font-size: 14px; margin-top: 5px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="header">
                <h1 class="welcome">Welcome, <asp:Label ID="lblUsername" runat="server"></asp:Label>!</h1>
                <a href="Logout.aspx" class="btn-logout">Logout</a>
            </div>
            <div class="content">
                <h2>Home Page</h2>
                <p>You are successfully authenticated and viewing the protected content.</p>
                <p>This page requires authentication to access.</p>
            </div>

            <div class="token-section">
                <h2>Access Token Request</h2>
                <div class="form-group">
                    <label for="txtScope">Scope(s):</label>
                    <asp:TextBox ID="txtScope" runat="server" placeholder="e.g., https://graph.microsoft.com/.default or User.Read"></asp:TextBox>
                    <p class="info-text">Enter one or more scopes separated by spaces</p>
                </div>
                <div class="form-group">
                    <asp:Button ID="btnGetToken" runat="server" Text="Get Access Token" CssClass="btn-get-token" OnClick="btnGetToken_Click" />
                </div>

                <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="error-message">
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                </asp:Panel>

                <asp:Panel ID="pnlTokenDisplay" runat="server" Visible="false" CssClass="token-display">
                    <h3 class="section-title">JWT Access Token</h3>
                    <div class="token-box">
                        <asp:Label ID="lblAccessToken" runat="server"></asp:Label>
                    </div>

                    <h3 class="section-title">Decoded Token Claims</h3>
                    <asp:GridView ID="gvClaims" runat="server" CssClass="claims-table" AutoGenerateColumns="False" GridLines="Both">
                        <Columns>
                            <asp:BoundField DataField="Type" HeaderText="Claim Type" />
                            <asp:BoundField DataField="Value" HeaderText="Value" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </div>
        </div>
    </form>
</body>
</html>
