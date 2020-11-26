using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;

public partial class extension_edit : class_login
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btn_delete.Attributes.Add("onclick ", "return confirm( '確定要刪除嗎?');"); //確定按下「刪除」按鈕 跳出提示訊息：確定即刪除；取消即返回
        if (!IsPostBack)//第一次執行本程式
        {
            DB_init_dept();
        }
    }

    //新增
    protected void btn_insert_Click(object sender, EventArgs e)
    {
        string sql_cmd = "INSERT INTO [dbo].[IT_extension]" +
            "([id_name] ,[id_dept] ,[ext] ,[ext_phone] ,[class] ,[ext_name] ,[dept]) " +
            "Values ( @my_id_name, @my_id_dept ,@my_ext ,@My_ext_phone, @my_class, @my_ext_name, @my_dept ) ";

        DB_store(sql_cmd);

        record_extension_log("extension_log", "新增資料。姓名：" + tb_ext_name.Text + "。部門：" + ddl_dept.SelectedValue + "。手機簡碼：" + tb_ext_phone.Text);
        Response.Write("<script language='javascript'>alert('新增完成! 姓名：" + tb_ext_name.Text + "'); </script>");
    }

    //修改
    protected void btn_update_Click(object sender, EventArgs e)
    {
        string sql_cmd = "UPDATE [dbo].[IT_extension] " +
             "SET [id_name] = @my_id_name , [id_dept] = @my_id_dept ,[ext] = @my_ext ,[ext_phone] = @My_ext_phone , [class] = @my_class ,[ext_name] = @my_ext_name ,[dept] = @my_dept " +
             "WHERE [id_name] = @my_id_name";

        DB_store(sql_cmd);

        record_extension_log("extension_log", "修改資料。姓名：" + tb_ext_name.Text + "。部門：" + ddl_dept.SelectedValue + "。手機簡碼：" + tb_ext_phone.Text);
        Response.Write("<script language='javascript'>alert('修改完成! 姓名：" + tb_ext_name.Text + "'); </script>");
    }

    //刪除：點擊「刪除」刪掉一筆資料
    //[在PageLoad事件 會先判斷是否有點擊按鈕 並 跳出訊息框 確認是否要刪除]
    protected void btn_delete_Click(object sender, EventArgs e)
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();


        SqlCommand Deletecmd = new SqlCommand("delete from [IT_extension] where [ext_name] = @my_ext_name", Conn);
        Deletecmd.Parameters.Add("@my_ext_name", SqlDbType.VarChar, 10);
        Deletecmd.Parameters["@my_ext_name"].Value = tb_ext_name.Text;

        Deletecmd.ExecuteNonQuery();

        Deletecmd.Cancel();
        //----關閉DataReader之前，一定要先「取消」SqlCommand

        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }
        record_extension_log("extension_log", "刪除資料。姓名：" + tb_ext_name.Text + "。部門：" + ddl_dept.SelectedValue + "。手機簡碼：" + tb_ext_phone.Text);
        Response.Write("<script language='javascript'>alert('刪除一筆資料完成! 姓名：" + tb_ext_name.Text + "'); location.href='extension.aspx'; </script>");
    }

    //讀取資料表，綁定ddl_dept的部門
    private void DB_init_dept()
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        string sql_str = "select distinct(dept),id_dept from IT_extension order by id_dept ";
        SqlCommand cmd = new SqlCommand(sql_str, Conn);
        SqlDataReader dr = cmd.ExecuteReader();

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
        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
        }
    }

    //讀取資料表：判斷姓名，帶入部門的序號及人員序號
    protected void tb_ext_name_TextChanged(object sender, EventArgs e)
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        string sql_str = "select id_name,id_dept,ext,class,ext_phone,dept from IT_extension where ext_name = @my_ext_name";
        SqlCommand cmd = new SqlCommand(sql_str, Conn);
        cmd.Parameters.Add("@my_ext_name", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_ext_name"].Value = tb_ext_name.Text; //姓名
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                tb_id_name.Text = dr[0].ToString();
                tb_id_dept.Text = dr[1].ToString();
                tb_ext.Text = dr[2].ToString();
                tb_class.Text = dr[3].ToString();
                tb_ext_phone.Text = dr[4].ToString();
                ddl_dept.SelectedValue = dr[5].ToString();
            }
        }
        else
        {
            tb_id_name.Text = "";
            tb_id_dept.Text = "";
            ddl_dept.SelectedValue = "%";
        }
        if (dr != null)
        {
            cmd.Cancel();
            dr.Close();
        }
        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
        }
    }

    //讀取資料表：判斷部門，帶入部門的序號及人員序號
    protected void ddl_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (tb_ext_name.Text != "")
        {
            SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
            Conn.Open();

            string sql_str = "select max(id_name),id_dept from IT_extension where dept = @my_dept group by id_dept";
            SqlCommand cmd = new SqlCommand(sql_str, Conn);
            cmd.Parameters.Add("@my_dept", SqlDbType.VarChar, 10);
            cmd.Parameters["@my_dept"].Value = ddl_dept.SelectedValue; //部門
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    int id_name = Convert.ToInt32(dr[0].ToString());
                    tb_id_name.Text = (id_name + 1).ToString();
                    tb_id_dept.Text = dr[1].ToString();
                }
            }
            if (dr != null)
            {
                cmd.Cancel();
                dr.Close();
            }

            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close();
            }

            if (ddl_dept.SelectedValue == "工程課-設備" || ddl_dept.SelectedValue == "製程課")
            {
                rbl_dept.Visible = true;
                lb_remark.Visible = true;
            }
            else
            {
                rbl_dept.Visible = false;
                lb_remark.Visible = false;
            }

            if (ddl_dept.SelectedValue == "業務課")
            {
                tb_class.Text = "A";
            }
            else if (ddl_dept.SelectedValue == "警衛室")
            {
                tb_class.Text = "C";
            }
            else if (ddl_dept.SelectedValue == "FAB分機")
            {
                tb_class.Text = "D";
            }
            else if (ddl_dept.SelectedValue == "手機簡碼" || ddl_dept.SelectedValue == "電話簡碼" || ddl_dept.SelectedValue == "工程課-廠務" || rbl_dept.SelectedIndex == 1)
            {
                tb_class.Text = "";
            }
            else
            {
                tb_class.Text = "B";
            }
        }
        else
        {
            return;
        }
    }

    //讀取資料表：判斷製程課、工程課-設備是否常日或輪班，帶入部門的序號及人員序號
    protected void rbl_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        string str_sql_dept = null;

        if (rbl_dept.SelectedIndex == 0)
        {
            str_sql_dept = "select max(id_name) from IT_extension where dept = @my_dept and ext <> ''";
        }
        else
        {
            str_sql_dept = "select max(id_name) from IT_extension where dept = @my_dept";
        }

        SqlCommand cmd_dept = new SqlCommand(str_sql_dept, Conn);
        cmd_dept.Parameters.Add("@my_dept", SqlDbType.VarChar, 10);
        cmd_dept.Parameters["@my_dept"].Value = ddl_dept.SelectedValue; //部門
        int id_name = Convert.ToInt32(cmd_dept.ExecuteScalar()); //回傳第一筆
        tb_id_name.Text = (id_name + 1).ToString(); 
        cmd_dept.Cancel();
        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
        }
    }


    //讀取資料表，判斷手機簡碼是否重複
    protected void tb_ext_phone_TextChanged(object sender, EventArgs e)
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        string str_sql = "SELECT count(ext_phone)  FROM[peggy].[dbo].[IT_extension] where ext_phone = @my_ext_phone";
        SqlCommand cmd = new SqlCommand(str_sql, Conn);
        cmd.Parameters.Add("@my_ext_phone", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_ext_phone"].Value = tb_ext_phone.Text; //手機簡碼
        int ext_phone = Convert.ToInt32(cmd.ExecuteScalar());//回傳第一筆
        if (ext_phone > 0)
        {
            Response.Write("<script language='javascript'>alert('重複! 手機簡碼已存在!');</script>");
        }
        cmd.Cancel();
        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
        }
        tb_ext.Text = tb_ext_phone.Text.Substring(1, 4);
    }


    private void DB_store(string sql_str)
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        string sql_cmd = sql_str;

        SqlCommand cmd = new SqlCommand(sql_cmd, Conn);

        cmd.Parameters.Add("@my_id_name", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_id_name"].Value = tb_id_name.Text; //人員序號

        cmd.Parameters.Add("@my_id_dept", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_id_dept"].Value = tb_id_dept.Text; //部門序號

        cmd.Parameters.Add("@my_ext", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_ext"].Value = tb_ext.Text; //分機

        cmd.Parameters.Add("@my_ext_phone", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_ext_phone"].Value = tb_ext_phone.Text; //手機簡碼

        cmd.Parameters.Add("@my_class", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_class"].Value = tb_class.Text; //手機簡碼

        cmd.Parameters.Add("@my_ext_name", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_ext_name"].Value = tb_ext_name.Text; //姓名

        cmd.Parameters.Add("@my_dept", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_dept"].Value = ddl_dept.SelectedValue; //部門

        cmd.ExecuteNonQuery();

        cmd.Cancel();

        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }
    }



    
}
