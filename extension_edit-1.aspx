<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_home.master" AutoEventWireup="true" CodeFile="extension_edit-1.aspx.cs" Inherits="extension_edit_1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=0.5, maximum-scale=2.0, user-scalable=yes" />
    <style type="text/css">
        hr {
            width: 30%;
        }

        .container-fluid {
            padding-left: 20%;
            padding-right: 20%;
        }
        .row {
            font-family: 微軟正黑體;
            padding-left: 20%;
            padding-right: 20%;
            font-weight: bold;
        }

        .col {
            padding-top: 30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row justify-content-center" style="padding-top: 20px;">
        <h2>
            <asp:Label ID="lb_title" runat="server" Text="明徽能源分機表"></asp:Label></h2>
    </div>
    <hr />
    <div class="row justify-content-center " style="padding-top: 1px; padding-bottom: 10px">
        <div class="col-12 text-center">
            <h2>編輯介面</h2>
        </div>
    </div>
    <div class="row  justify-content-center">
        <div class="col"></div>
         <div class="col"></div>
        <div class="col">
            <asp:DropDownList ID="ddl_dept" runat="server" Class="form-control form-control-sm" AutoPostBack="True" OnSelectedIndexChanged="ddl_dept_SelectedIndexChanged">
                <asp:ListItem Value="%">請選擇</asp:ListItem>
            </asp:DropDownList>
            <asp:RadioButtonList ID="rbl_dept" runat="server" RepeatDirection="Horizontal" Visible="False" AutoPostBack="True" Class="form-control border-0">
                <asp:ListItem>&nbsp;常日&nbsp;&nbsp;</asp:ListItem>
                <asp:ListItem>&nbsp;輪班</asp:ListItem>
            </asp:RadioButtonList>
            <asp:Label ID="lb_remark" runat="server" Text="備註：類別 常日=B；輪班=無" Font-Size="Small" ForeColor="Gray" Visible="False"></asp:Label>
        </div>
         <div class="col"></div>
         <div class="col"></div>
    </div>
    <div class="row  justify-content-center">
        <div class="col">
            <asp:GridView ID="gv_extension_edit" runat="server" class="table table-hover table-bordered text-center" DataKeyNames="ext_name" OnRowCancelingEdit="gv_extension_edit_RowCancelingEdit" OnRowDataBound="gv_extension_edit_RowDataBound" OnRowDeleting="gv_extension_edit_RowDeleting" OnRowEditing="gv_extension_edit_RowEditing" OnRowUpdating="gv_extension_edit_RowUpdating">
                <Columns>
                    <asp:TemplateField>
                         <EditItemTemplate>
                            <asp:Button ID="btn_update" runat="server" CommandName="Update" Text="更新"/>
                            &nbsp;<asp:Button ID="btn_cancel" runat="server" CommandName="cancel" Text="取消" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Button ID="btn_edit" runat="server" CommandName="Edit" Text="編輯" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btn_delete" runat="server" CommandName="Delete" Text="刪除" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>


</asp:Content>

