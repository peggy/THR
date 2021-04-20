using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class meal_query : class_login
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (Session["OK"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)//第一次執行本程式
        {
            tb_make_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DBInit("init");
            record_personal_log("LOAD。訂餐查詢載入。", "meal_query");
        }
    }

    //查詢
    protected void btn_query_Click(object sender, EventArgs e)
    {
        DBInit("query");
        record_personal_log("SELECT。訂餐查詢。日期："+tb_make_date.Text+"，工號："+ tb_work_no.Text +"，狀態："+ ddl_op_state.SelectedValue +"，班別：" + ddl_class + "，排班：" + ddl_scheduling.Text , "meal_query");
    }

     //屬性：showfooter = true
    int total_breakfast = 0, total_lunch = 0, total_dinner = 0, total_vegetarian = 0; //因RowData每一列會Bound，所以需做全域變數，才不會歸零
    protected void gv_meal_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //確認取消
            Button d_button = (Button)e.Row.Cells[0].FindControl("btn_update");
            d_button.OnClientClick = "javascript:return confirm('確定取消?')";

            //狀態顯示(s)-----------------------------------------------
            string state = e.Row.Cells[10].Text; //狀態
            if (state == "0")
            {
                e.Row.Cells[10].Text = "取消";
                e.Row.Attributes.Add("style", "color:#8E9EAB");
            }
            else
            {
                e.Row.Cells[10].Text = "新增";
                //代訂判斷
                string make_us = e.Row.Cells[2].Text; //訂餐人
                string modify_user = e.Row.Cells[11].Text; //修改人
                if (make_us != modify_user)
                {
                    e.Row.Cells[10].Text = "代訂";
                }

                //計算加總(s)-----------------------
                if (e.Row.Cells[6].Text == "Y")
                {
                    total_breakfast++;
                }
                if (e.Row.Cells[7].Text == "Y")
                {
                    total_lunch++;
                }
                if (e.Row.Cells[8].Text == "Y")
                {
                    total_dinner++;
                }
                if (e.Row.Cells[9].Text == "Y")
                {
                    total_vegetarian++;
                }
                //計算加總(e)-----------------------
            }
            //狀態顯示(e)-----------------------------------------------
        }
        //最後一列呈現加總
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[5].Text = "小計<hr/>素食<hr/>合計<hr/>";
            e.Row.Cells[6].Text = total_breakfast.ToString() + "<hr/>" + total_vegetarian.ToString() + "<hr/>" + (total_breakfast-total_vegetarian).ToString();
            e.Row.Cells[7].Text = total_lunch.ToString() + "<hr/>" + total_vegetarian.ToString() + "<hr/>" + (total_lunch - total_vegetarian).ToString();
            e.Row.Cells[8].Text = total_dinner.ToString() + "<hr/>" + total_vegetarian.ToString() + "<hr/>" + (total_dinner - total_vegetarian).ToString();
            
        }
    }

    //更新資料表[PM_Opuser_Work]：取消訂餐狀態改為0
    protected void gv_meal_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string[] str_s = Session["OK"].ToString().Split('-');

        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        string sql_cmd = "update [dbo].[PM_Opuser_Work] set sys_date = getdate(), op_state = 0, modify_user = @my_modify_user " +
            "where work_no = @my_work_no and make_date = @my_make_date";

        SqlCommand cmd = new SqlCommand(sql_cmd, Conn);

        cmd.Parameters.Add("@my_modify_user", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_modify_user"].Value = str_s[1];

        cmd.Parameters.Add("@my_work_no", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_work_no"].Value = gv_meal.Rows[e.RowIndex].Cells[1].Text; //工號

        cmd.Parameters.Add("@my_make_date", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_make_date"].Value = gv_meal.Rows[e.RowIndex].Cells[3].Text; //日期

        cmd.ExecuteNonQuery();

        cmd.Cancel();

        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }

        record_personal_log("UPDATE。取消訂餐。姓名：" + gv_meal.Rows[e.RowIndex].Cells[2].Text + "，日期：" + gv_meal.Rows[e.RowIndex].Cells[3].Text, "meal_query");
        
        if (tb_make_date.Text != "" || tb_work_no.Text != "" || ddl_class.SelectedValue != "%" || ddl_op_state.SelectedValue != "%" || ddl_scheduling.SelectedValue != "%")
        {
            DBInit("query");
        }
        else
        {
            DBInit("init");
        }
    }

    //讀取資料表：綁定GridView
    //事件呼叫：Page_Load、gv_meal_RowUpdating、btn_query_Click
    public void DBInit(string type)
    {
        if (tb_make_date.Text == "") //限制每次查詢僅能查一天
        {
            Response.Write("<script language='javascript'>confirm('請選擇日期!');</script>");
            gv_meal.DataSource = null;
            gv_meal.DataBind();
            return;
        }

        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        SqlDataReader dr = null;
        SqlCommand cmd = null;
        string sql_str = null;
        sql_str = "SELECT work_no as 工號, make_us as 姓名, convert(varchar(20),make_date,23) as 日期, class as 班別, Scheduling as 排班, " +
                  "breakfast as 早餐, lunch as 午餐, dinner as 晚餐, Vegetarian as 素食, OP_State as 狀態, modify_user as 修改人, remarks as 備註 " +
                  "FROM [dbo].[PM_Opuser_Work] " +
                  "where class like '%' + @my_class + '%'  and scheduling like '%' + @my_scheduling + '%' " ;
               
        if (type == "init")
        {
            sql_str += " and make_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
        }
        else if (type == "query")
        {
            if (tb_make_date.Text != "")
            {
                sql_str += "and make_date = @my_make_date ";
            }
            if (tb_work_no.Text != "")
            {
                sql_str += " and work_no = @my_work_no ";
            }
            if (ddl_op_state.SelectedValue == "代訂")
            {
                sql_str += " and OP_State not like '0' and make_us != modify_user";
            }
            else if (ddl_op_state.SelectedValue == "1")
            {
                sql_str += " and OP_State like '%' + @my_op_state + '%' and make_us = modify_user ";
            }
            else
            {
                sql_str += " and OP_State like '%' + @my_op_state + '%'";
            }
        }
                  
        cmd = new SqlCommand(sql_str, Conn);
        cmd.Parameters.Add("@my_class", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_class"].Value = ddl_class.SelectedValue; //班別

        cmd.Parameters.Add("@my_scheduling", SqlDbType.VarChar, 5);
        cmd.Parameters["@my_scheduling"].Value = ddl_scheduling.SelectedValue; //組別

        if (ddl_op_state.SelectedValue != "代訂")
        {
            cmd.Parameters.Add("@my_op_state", SqlDbType.VarChar, 5);
            cmd.Parameters["@my_op_state"].Value = ddl_op_state.SelectedValue; //狀態
        }

        if (tb_make_date.Text != "")
        {
            cmd.Parameters.Add("@my_make_date", SqlDbType.DateTime);
            cmd.Parameters["@my_make_date"].Value = tb_make_date.Text; //日期
        }
        

        if (tb_work_no.Text != "")
        {
            cmd.Parameters.Add("@my_work_no", SqlDbType.VarChar, 10);
            cmd.Parameters["@my_work_no"].Value = tb_work_no.Text; //工號
        }

        dr = cmd.ExecuteReader();

        if (dr.HasRows)
        {
            gv_meal.DataSource = dr;
            gv_meal.DataBind();
        }
        else
        {
            Response.Write("<script language='javascript'>confirm('查無資料!');</script>");
            gv_meal.DataSource = null;
            gv_meal.DataBind();
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





   
}