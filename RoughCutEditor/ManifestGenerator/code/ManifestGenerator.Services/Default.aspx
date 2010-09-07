<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ManifestGenerator.Services.Default"
    ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <title>Composite Stream Manifest Generator Test Page</title>
</head>
<body>
    <div id="container">
        <h1>
            Composite Streaming Manifest Generator Test Page</h1>
        <form runat="server">
        <fieldset>
            <legend>Generate CSM from Manifest applying Clip Begin and Clip End </legend>
            <label for="manifestUri">
                Manifest:</label>
            <asp:TextBox ID="ManifestUriTextBox" name="manifestUri" runat="server" CssClass="uri" /><br />
            <label for="clipBegin">
                Clip Begin:</label>
            <asp:TextBox ID="ClipBeginTextBox" name="clipBegin" runat="server" /><br />
            <label for="clipEnd">
                Clip End:</label>
            <asp:TextBox ID="ClipEndTextBox" name="clipEnd" runat="server" /><br />
            <asp:Button runat="server" OnClick="GenerateSubClipManifest" Text="Generate" CssClass="button" />
            <asp:TextBox runat="server" Width="900" Height="500" TextMode="MultiLine" Wrap="true"
                Visible="false" ID="SubClipManifestTextBox" />
        </fieldset>
        <hr />
        <fieldset class="rce">
            <legend>Generate CSM from RCE Project XML</legend>
            <label for="adsStreamName">ADs Data Stream Name:</label>
            <asp:TextBox ID="AdsDataStreamName" runat="server" Text="AD" name="adsStreamName"></asp:TextBox><br />
            <label for="pbpStreamName">PBP Data Stream Name:</label>
            <asp:TextBox ID="PBPDataStreamName" runat="server" Text="PBP" name="pbpStreamName"></asp:TextBox><br/>
            <label for="rceProject">RCE Project:</label><br />
            <asp:TextBox ID="ProjectTextBox" runat="server" Width="900" Height="250" TextMode="MultiLine" name="rceProject"/><br />
            <asp:Button ID="Button1" runat="server" OnClick="GenerateManifest" Text="Generate" CssClass="button" /> 
            <asp:TextBox runat="server" Visible="false" ID="ManifestTextBox" Width="900" Height="500"
                TextMode="MultiLine" Wrap="true" />
        </fieldset>
        </form>
    </div>
</body>
</html>
