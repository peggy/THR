using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;

public partial class extension_edit_1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DB_init_dept();
        }
    }

    //讀取資料表，綁定部門及GridView
    public void DB_init_dept()
    {
        string sql_str = null;
        SqlCommand cmd = null;
        SqlDataReader dr = null;

        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        //綁定部門
        sql_str = "select distinct(dept),id_dept from IT_extension order by id_dept ";
        cmd = new SqlCommand(sql_str, Conn);
        dr = cmd.ExecuteReader();

        if (dr.HasRows)
        {
            while (dr.Read())
            {
                ddl_dept.Items.Add(dr[0].ToString());
            }
        }

        if (dr != null)
        {
            cmd.Cancel();
            dr.Close();
        }

        //綁定Gridview
        if(ddl_dept.SelectedValue != "%")
        {
            sql_str = "SELECT *  FROM [dbo].[IT_extension] where dept = @my_dept order by id_name";
            cmd = new SqlCommand(sql_str, Conn);
            cmd.Parameters.Add("@my_dept", SqlDbType.VarChar, 10);
            cmd.Parameters["@my_dept"].Value = ddl_dept.SelectedValue; //部門

            dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                gv_extension_edit.DataSource = dr;
                gv_extension_edit.DataBind();
            }
            if (dr != null)
            {
                cmd.Cancel();
                dr.Close();
            }
        }
        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }
    }

    //ddl選擇部門，綁定GridView
    protected void ddl_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        DB_init();
    }

    public void DB_init()
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        //綁定部門
        string sql_str = "SELECT *  FROM [dbo].[IT_extension] where dept = @my_dept order by id_name";
        SqlCommand cmd = new SqlCommand(sql_str, Conn);
        cmd.Parameters.Add("@my_dept", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_dept"].Value = ddl_dept.SelectedValue; //部門
        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.HasRows)
        {
            gv_extension_edit.DataSource = dr;
            gv_extension_edit.DataBind();
        }
        if (dr != null)
        {
            cmd.Cancel();
            dr.Close();
        }
        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }
    }


    //GridView 編輯模式
    protected void gv_extension_edit_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gv_extension_edit.EditIndex = e.NewEditIndex;
        DB_init();
    }

    //GridView 離開編輯模式
    protected void gv_extension_edit_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gv_extension_edit.EditIndex = -1;
        DB_init();
    }
    //GridView 刪除
    protected void gv_extension_edit_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        SqlDataReader dr = null;
        SqlCommand Deletecmd = new SqlCommand("delete from [IT_extension] where [ext_name] = @my_ext_name", Conn);
        Deletecmd.Parameters.Add("@my_ext_name", SqlDbType.VarChar, 10);
        Deletecmd.Parameters["@my_ext_name"].Value = gv_extension_edit.DataKeys[e.RowIndex].Value; //姓名

        Deletecmd.ExecuteNonQuery();

        if (dr != null)
        {
            Deletecmd.Cancel();
            //----關閉DataReader之前，一定要先「取消」SqlCommand
            dr.Close();
        }
        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }
        //Response.Write("<script language='javascript'>alert('刪除完成! 姓名：" + gv_extension_edit.DataKeys[e.RowIndex].Value + "'); </script>");
        DB_init();
    }
    //GridView 更新
    protected void gv_extension_edit_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        TextBox myt_id_name, myt_id_dept, myt_ext_name;
        myt_id_name = (TextBox)gv_extension_edit.Rows[e.RowIndex].Cells[2].Controls[0]; //人名序號
        myt_id_dept = (TextBox)gv_extension_edit.Rows[e.RowIndex].Cells[3].Controls[0]; //部門序號
        

        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        string sql_cmd = "UPDATE [dbo].[IT_extension] " +
             "SET [id_name] = @my_id_name , [id_dept] = @my_id_dept " +
             "WHERE [ext_name] = @my_ext_name";

        SqlCommand cmd = new SqlCommand(sql_cmd, Conn);

        cmd.Parameters.Add("@my_id_name", SqlDbType.Decimal, 10);
        cmd.Parameters["@my_id_name"].Value = myt_id_name.Text; //人員序號

        cmd.Parameters.Add("@my_id_dept", SqlDbType.Decimal, 10);
        cmd.Parameters["@my_id_dept"].Value = myt_id_dept.Text; //部門序號

        cmd.Parameters.Add("@my_ext_name", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_ext_name"].Value = gv_extension_edit.DataKeys[e.RowIndex].Value; //姓名

        cmd.ExecuteNonQuery();

        cmd.Cancel();

        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }
        DB_init();
        gv_extension_edit.EditIndex = -1;
    }

    protected void gv_extension_edit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //刪除確認
            Button d_button = (Button)e.Row.Cells[2].FindControl("btn_delete");
            d_button.OnClientClick = "javascript:return confirm('確定刪除?')";
        }
    }

    
}