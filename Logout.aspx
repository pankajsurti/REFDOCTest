<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="REFDOCTest.Logout" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Logout</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 50px; text-align: center; }
        .message { max-width: 400px; margin: 0 auto; padding: 20px; }
        a { color: #007bff; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="message">
            <h2>You have been logged out</h2>
            <p><a href="Login.aspx">Click here to login again</a></p>
        </div>
    </form>
</body>
</html>
