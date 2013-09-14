<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Sample.WebForms_NET35._Default" %>
<%@ Import Namespace="StackExchange.Profiling" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

<% using (MiniProfilerExtensions.Step(MiniProfiler.Current, "Default's <head>", ProfileLevel.Info))
   {
       System.Threading.Thread.Sleep(20); %>

    <script type="text/javascript"></script>

<% } %>

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to ASP.NET!
    </h2>
    <p>
        To learn more about ASP.NET visit <a href="http://www.asp.net" title="ASP.NET Website">www.asp.net</a>.
    </p>
    <p>
        You can also find <a href="http://go.microsoft.com/fwlink/?LinkID=152368&amp;clcid=0x409"
            title="MSDN ASP.NET Docs">documentation on ASP.NET at MSDN</a>.
    </p>
</asp:Content>
