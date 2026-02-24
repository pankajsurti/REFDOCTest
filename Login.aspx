<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="REFDOCTest.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 50px; background-color: #f5f5f5; }
        .login-container { max-width: 400px; margin: 100px auto; padding: 40px; border: 1px solid #ddd; border-radius: 8px; background-color: white; box-shadow: 0 2px 10px rgba(0,0,0,0.1); text-align: center; }
        .btn-microsoft { display: inline-flex; align-items: center; justify-content: center; padding: 12px 24px; background-color: #2f2f2f; color: white; border: none; cursor: pointer; border-radius: 4px; font-size: 16px; width: 100%; }
        .btn-microsoft:hover { background-color: #1a1a1a; }
        .btn-microsoft img { margin-right: 12px; }
        .error { color: red; margin-bottom: 15px; display: block; }
        h2 { margin-bottom: 30px; color: #333; }
        .divider { margin: 20px 0; color: #999; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <h2>Sign In</h2>
            <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false"></asp:Label>
            <asp:Button ID="btnMicrosoftLogin" runat="server" Text="Sign in with Microsoft" CssClass="btn-microsoft" OnClick="btnMicrosoftLogin_Click" />
        </div>
    </form>
</body>
</html>
