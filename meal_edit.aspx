<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_home.master" AutoEventWireup="true" CodeFile="meal_edit.aspx.cs" Inherits="meal_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .container-fluid {
            padding-left: 10%;
            padding-right: 10%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid" style="padding-left: 10%; padding-right: 10%;">
        <div class="row justify-content-center" style="padding-top: 20px;">
            <div class="col text-center">
                <h2>個人資料</h2>
            </div>
        </div>
        <hr style="width: 50%" />
        <div class="row justify-content-center" style="padding-bottom: 20px">
            <div class="col text-center">
                <h2>訂餐</h2>
            </div>
        </div>
        <div class="row justify-content-center" style="padding-top: 20px">
            <div class="col text-center">
                現在時間為：
                <asp:Label ID="lb_date" runat="server" Text="Label"></asp:Label>
            </div>
        </div>
    </div>
    <div class="container-fluid" style="padding-left: 15%; padding-right: 15%;">
        <div class="row justify-content-center" style="padding-top: 30px">
            <div class="col-12 col-lg-2 col-sm-12"></div>
            <div class="col-12 col-lg-2 col-sm-12 text-right" style="padding-top: 3%">
                <asp:TextBox ID="tb_empno" runat="server" placeholder="搜尋工號" Class="form-control" Visible="False" AutoPostBack="True" OnTextChanged="tb_empno_TextChanged"></asp:TextBox>
            </div>
            <div class="col-12 col-lg-8 col-sm-12"></div>
        </div>
        <div class="row justify-content-center" style="padding-top: 30px">
            <div class="col-lg-2 col-sm-12">
                <asp:Label ID="lb_card_no" runat="server" Text="" Visible="False"></asp:Label></div>
            <div class="col-lg-1 col-sm-12" style="padding-top: 2%">
                <asp:Label ID="lb_name" runat="server" Text="姓名 (Name)"></asp:Label>
            </div>
            <div class="col-lg-2 col-sm-12" style="padding-top: 2%">
                <asp:TextBox ID="tb_name" runat="server" Class="form-control"></asp:TextBox>
            </div>
            <div class="col-lg-1 col-sm-12" style="padding-top: 2%">
                <asp:Label ID="lb_class" runat="server" Text="班別  (Class)"></asp:Label>
            </div>
            <div class="col-lg-2 col-sm-12" style="padding-top: 2%">
                 <asp:TextBox ID="tb_class" runat="server" Class="form-control" Visible="False" ReadOnly="True"></asp:TextBox>
                <asp:DropDownList ID="ddl_class" runat="server" Class="form-control" Visible="False">
                    <asp:ListItem>請選擇</asp:ListItem>
                    <asp:ListItem Value="DD">DD</asp:ListItem>
                    <asp:ListItem Value="DA">DA</asp:ListItem>
                    <asp:ListItem Value="DB">DB</asp:ListItem>
                    <asp:ListItem Value="DC">DC</asp:ListItem>
                    <asp:ListItem Value="NA">NA</asp:ListItem>
                    <asp:ListItem Value="NB">NB</asp:ListItem>
                    <asp:ListItem Value="NC">NC</asp:ListItem>
                    <asp:ListItem Value="X">無</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-lg-2 col-sm-12" style="padding-top: 2%">
                <asp:CheckBoxList ID="cbl_vegetarian" runat="server" CellPadding="10">
                    <asp:ListItem Value="素食">&nbsp;&nbsp;素食 (Vegetarian)</asp:ListItem>
                </asp:CheckBoxList>
            </div>
            <div class="col-lg-2 col-sm-12"></div>
        </div>
        <div class="row justify-content-center" style="padding-top: 30px">
            <div class="col-lg-2 col-sm-12"></div>
            <div class="col-lg-1 col-sm-12">
                <asp:Label ID="lb_breakfast" runat="server" Text="早餐  (Breakfast)"></asp:Label>
            </div>
            <div class="col-lg-2 col-sm-12">
                <asp:RadioButtonList ID="rbl_breakfast" runat="server" RepeatDirection="Horizontal" CellPadding="10">
                    <asp:ListItem Value="Y">&nbsp;YES</asp:ListItem>
                    <asp:ListItem Value="">&nbsp;NO</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="col-lg-1 col-sm-12">
                <asp:Label ID="lb_lunch" runat="server" Text="午餐  (Lunch)"></asp:Label>
            </div>
            <div class="col-lg-2 col-sm-12">
                <asp:RadioButtonList ID="rbl_lunch" runat="server" RepeatDirection="Horizontal" CellPadding="10">
                    <asp:ListItem Value="Y">&nbsp;YES</asp:ListItem>
                    <asp:ListItem Value="">&nbsp;NO</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="col-lg-1 col-sm-12">
                <asp:Label ID="lb_dinner" runat="server" Text="晚餐  (Dinner)"></asp:Label>
            </div>
            <div class="col-lg-2 col-sm-12">
                <asp:RadioButtonList ID="rbl_dinner" runat="server" RepeatDirection="Horizontal" CellPadding="10">
                    <asp:ListItem Value="Y">&nbsp;YES</asp:ListItem>
                    <asp:ListItem Value="">&nbsp;NO</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="col-lg-1 col-sm-12"></div>
        </div>
        <div class="row justify-content-center" style="padding-top: 30px">
             <div class="col-lg-2 col-sm-12"></div>
            <div class="col-lg-1 col-sm-12">
                <asp:Label ID="lb_remark" runat="server" Text="備註  (Remark)" Visible="False"></asp:Label>
            </div>
            <div class="col-lg-6 col-sm-12">
                <asp:TextBox ID="tb_remark" runat="server" Class="form-control" Visible="False"></asp:TextBox>
            </div>
             <div class="col-lg-3 col-sm-12"></div>
        </div>
        <div class="row justify-content-center" style="padding-top: 40px; padding-bottom: 10px">
            <div class="col-lg-2 col-sm-12"></div>
            <div class="col-lg-8 col-sm-12 text-center">
                <asp:Button ID="btn_submit" runat="server" Text="提交" Class="btn btn-secondary" OnClick="btn_submit_Click" />
            </div>
            <div class="col-lg-2 col-sm-12"></div>
        </div>
    </div>
</asp:Content>

