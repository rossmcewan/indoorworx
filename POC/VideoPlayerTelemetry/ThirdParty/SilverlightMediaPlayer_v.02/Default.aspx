<%@ Page Language="C#" 
AutoEventWireup="true"  
CodeFile="Default.aspx.cs" 
Inherits="ASPX_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="System.Web.Silverlight" 
    Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>

<%@ Register assembly="AjaxControlToolkit" 
namespace="AjaxControlToolkit" 
tagprefix="cc1" %>

<html xmlns="http://www.w3.org/1999/xhtml">

    <head runat="server">
        <title>SILVERLIGHT MEDIA PLAYER</title>
    </head>
    <body>
        
        <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div style ="width:600px; margin-left:auto; margin-right:auto;">
        <hr />
           
            <!--AJAX UPDATE PANEL-->
           <asp:UpdatePanel ID="UpdatePanel1" runat="server" updatemode="Conditional" >
           <ContentTemplate> 
                
                <!--STYLE SELECTOR-->
                <div>
                    <h2>SILVERLIGHT MEDIA PLAYER</h2>
                    <div style="float:left">
                        <asp:DropDownList ID="cmbSkins" runat="server" 
                        onselectedindexchanged="cmbSkins_SelectedIndexChanged" />
                    </div>
                    <h5>&nbsp;&nbsp;MEDIA PLAYER STYLE SELECTOR</h5>
               </div>

                <!-- SILVERLIGHT MEDIA PLAYER -->
                <asp:MediaPlayer ID="MediaPlayer1" runat="server" /> 
      
            </ContentTemplate>
            </asp:UpdatePanel>
  
            <h4>
                Note: this video response to "What's your name" by Boston is shown for demo purpose only:
                please do not copy/distribute!
            </h4>
            <hr />
        </div>
        </form>
    </body>
</html>