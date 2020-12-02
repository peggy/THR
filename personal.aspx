<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_home.master" AutoEventWireup="true" CodeFile="personal.aspx.cs" Inherits="personal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .container-fluid{
            padding-left: 20%;
            padding-right:20%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid">
        <div class="row justify-content-center" style="padding-top: 20px;">
            <h2>
                <asp:Label ID="lb_title" runat="server" Text="個人資料"></asp:Label></h2>
        </div>
        <div class="row justify-content-center " style="padding-top: 50px;">
            <div class="col-lg-3 col-sm-6 text-center ">
                <asp:Label ID="lb_name" runat="server" Text="姓名"></asp:Label>
            </div>
            <div class="col-lg-3 col-sm-6 col-sm-6 text-center ">
                <asp:TextBox ID="tb_name" runat="server" Class="form-control mb-1"></asp:TextBox>
            </div>
            <div class="col-lg-3 col-sm-6 text-center ">
                <asp:Label ID="lb_dept" runat="server" Text="部門"></asp:Label>
            </div>
            <div class="col-lg-3 col-sm-6 text-center ">
                <asp:TextBox ID="tb_dept" runat="server" Class="form-control mb-1"></asp:TextBox>
            </div>
        </div>
    </div>
</asp:Content>

