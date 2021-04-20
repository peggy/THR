<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_home.master" AutoEventWireup="true" CodeFile="meal_query.aspx.cs" Inherits="meal_query" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .container-fluid {
            padding-left: 10%;
            padding-right: 10%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid">
        <div class="row justify-content-center" style="padding-top: 20px;">
            <div class="col text-center">
                <h2>個人資料</h2>
            </div>
        </div>
        <hr style="width: 50%" />
        <div class="row justify-content-center" style="padding-bottom: 20px">
            <div class="col text-center">
                <h2>訂餐查詢</h2>
            </div>
        </div>

        <div class="row justify-content-center" style="padding-bottom: 20px; padding-left: 2%; padding-right: 2%">
            <div class="col-12 col-lg-1 col-sm-12 text-lg-right">
                <asp:Label ID="lb_make_date" runat="server" Text="日期："></asp:Label>
            </div>
            <div class="col-12 col-lg-2 col-sm-12 text-left">
                <asp:TextBox ID="tb_make_date" runat="server" Class="form-control"></asp:TextBox>
            </div>
            <div class="col-12 col-lg-1 col-sm-12 text-lg-right">
                <asp:Label ID="lb_work_no" runat="server" Text="工號："></asp:Label>
            </div>
            <div class="col-12 col-lg-2 col-sm-12 text-left">
                <asp:TextBox ID="tb_work_no" runat="server" Class="form-control"></asp:TextBox>
            </div>
            <div class="col-12 col-lg-1 col-sm-12 text-lg-right">
                <asp:Label ID="lb_op_state" runat="server" Text="狀態："></asp:Label>
            </div>
            <div class="col-12 col-lg-1 col-sm-12 text-left">
                <asp:DropDownList ID="ddl_op_state" runat="server" Class="form-control">
                    <asp:ListItem Value="%">請選擇</asp:ListItem>
                    <asp:ListItem Value="1">新增</asp:ListItem>
                    <asp:ListItem Value="0">取消</asp:ListItem>
                    <asp:ListItem Value="代訂">代訂</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-12 col-lg-1 col-sm-12 text-lg-right">
                <asp:Label ID="lb_class" runat="server" Text="班別："></asp:Label>
            </div>
            <div class="col-12 col-lg-1 col-sm-12 text-left">
                <asp:DropDownList ID="ddl_class" runat="server" Class="form-control">
                    <asp:ListItem Value="%">請選擇</asp:ListItem>
                    <asp:ListItem>D</asp:ListItem>
                    <asp:ListItem>N</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-12 col-lg-1 col-sm-12 text-lg-right">
                <asp:Label ID="lb_scheduling" runat="server" Text="排班："></asp:Label>
            </div>
            <div class="col-12 col-lg-1 col-sm-12  text-left">
                <asp:DropDownList ID="ddl_scheduling" runat="server" Class="form-control">
                    <asp:ListItem Value="%">請選擇</asp:ListItem>
                    <asp:ListItem>DD</asp:ListItem>
                    <asp:ListItem>DA</asp:ListItem>
                    <asp:ListItem>DB</asp:ListItem>
                    <asp:ListItem>DC</asp:ListItem>
                    <asp:ListItem>NA</asp:ListItem>
                    <asp:ListItem>NB</asp:ListItem>
                    <asp:ListItem>NC</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="row justify-content-center" style="padding-bottom: 50px">
            <div class="col-12 col-lg-12  text-center">
                <asp:Button ID="btn_query" runat="server" Text="查詢" Class="btn btn-secondary" OnClick="btn_query_Click" />
            </div>
        </div>


        <div class="row justify-content-center" style="padding-bottom: 20px">
            <div class="col text-center">
                <asp:GridView ID="gv_meal" runat="server" Class=" table table-sm table-hover table-responsive-sm table-bordered" HeaderStyle-BackColor="Silver" HeaderStyle-ForeColor="White" OnRowDataBound="gv_meal_RowDataBound" OnRowUpdating="gv_meal_RowUpdating" ShowFooter="True">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btn_update" runat="server" CommandName="Update" Text="取消" class="btn btn-sm btn-outline-dark" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>

