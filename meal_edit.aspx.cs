using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class meal_edit : class_login
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lb_date.Text = DateTime.Now.ToString(); //現在時間

        if (Session["OK"] == null)
        {
            Response.Redirect("login.aspx");
        }
        //查詢、班別編輯限制(s)------------------------------------------
        if (tb_empno.Text == "")
        {
            tb_class.Visible = true;
            ddl_class.Visible = false;
            lb_remark.Visible = false;
            tb_remark.Visible = false;
        }
        //else DBInit有查詢結果才顯示
        //查詢、班別編輯限制(e)------------------------------------------

        if (!IsPostBack)//第一次執行本程式
        {
            DBInit();
            //權限(s)-----------------------------------------------
            string[] str_s = Session["OK"].ToString().Split('-');
            string[] arr_auth = DB_authority(Session["ac"].ToString(), "personal");
            if (arr_auth[0] == "99" || arr_auth[0] == "10" || arr_auth[0] == "12" || arr_auth[0] == "21" || arr_auth[0] == "22") //10-人事；12-警衛；21-製造助理；22-助理
            {
                tb_empno.Visible = true;
            }
            //權限(e)-----------------------------------------------
            //班別編輯限制(e)------------------------------------------
            if (ddl_class.SelectedValue.StartsWith("D"))
            {
                rbl_breakfast.Enabled = false;
            }
            else if (ddl_class.SelectedValue.StartsWith("N"))
            {
                rbl_lunch.Enabled = false;
                rbl_dinner.Enabled = false;
            }
            //班別編輯限制(e)------------------------------------------
            record_personal_log("LOAD。訂餐載入。", "meal");
        }
    }

    //工號
    protected void tb_empno_TextChanged(object sender, EventArgs e)
    {
        cbl_vegetarian.SelectedIndex = -1;
        rbl_breakfast.Enabled = true;
        rbl_breakfast.SelectedIndex = -1;
        rbl_lunch.Enabled = true;
        rbl_lunch.SelectedIndex = -1;
        rbl_dinner.Enabled = true;
        rbl_dinner.SelectedIndex = -1;
        tb_remark.Text = "";
        if (DBInit())
        {
            tb_empno.Text = "";
            tb_name.Text = "";
            tb_class.Text = "";
            ddl_class.SelectedValue = "請選擇";
        }
    }

    //提交
    protected void btn_submit_Click(object sender, EventArgs e)
    {
        string[] str_s = Session["OK"].ToString().Split('-');
        //條件(s)---------------------------------------------------------------------------------
        #region
        //判斷工號有無重複
        if (DB_empno()  == true)
        {
            record_personal_log("ALERT。工號已存在。工號：" + tb_empno.Text + "，姓名：" + tb_name.Text, "meal");
            Response.Write("<script language='javascript'>confirm('提交錯誤! 工號重複');</script>");
            return;
        }
        if (rbl_breakfast.SelectedIndex < 0) //早餐
        {
            if (ddl_class.SelectedValue.StartsWith("N") || ddl_class.SelectedValue.StartsWith("X") || tb_empno.Text != "")
            {
                record_personal_log("ALERT。未選擇早餐。姓名："+ tb_name.Text +"，班別：" + ddl_class.SelectedValue, "meal");
                Response.Write("<script language='javascript'>confirm('請選擇早餐');</script>");
                return;
            }            
        }
        if (rbl_lunch.SelectedIndex < 0) //午餐
        {
            if (ddl_class.SelectedValue.StartsWith("D") || ddl_class.SelectedValue.StartsWith("X") || tb_empno.Text != "")
            {
                record_personal_log("ALERT。未選擇午餐。姓名：" + tb_name.Text + "，班別：" + ddl_class.SelectedValue, "meal");
                Response.Write("<script language='javascript'>confirm('請選擇午餐');</script>");
                return;
            }
        }
        if (rbl_dinner.SelectedIndex < 0) //晚餐
        {
            if (ddl_class.SelectedValue.StartsWith("D") || ddl_class.SelectedValue.StartsWith("X") || tb_empno.Text != "")
            {
                record_personal_log("ALERT。未選擇晚餐。姓名：" + tb_name.Text + "，班別：" + ddl_class.SelectedValue, "meal");
                Response.Write("<script language='javascript'>confirm('請選擇晚餐');</script>");
                return;
            }
        }
        #endregion
        //條件(e)---------------------------------------------------------------------------------
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        SqlCommand cmd = null;
        SqlDataReader dr = null;
        string sql_str = null;
        sql_str = "INSERT INTO [dbo].[PM_Opuser_Work] ([work_no],[make_us],[card_no],[class],[Scheduling],[breakfast],[lunch],[dinner],[Vegetarian],[OP_State],[modify_user],[remarks]) " +
                  "VALUES (@my_work_no, @my_name, @my_card_no, @my_class, @my_scheduling, @my_breakfast, @my_lunch, @my_dinner, @my_vegetarian,'1', @my_modify_user, @my_remarks )"; //找常日班

        cmd = new SqlCommand(sql_str, Conn);
        cmd.Parameters.Add("@my_work_no", SqlDbType.Int, 10);
        cmd.Parameters.Add("@my_name", SqlDbType.VarChar, 10);
        cmd.Parameters.Add("@my_card_no", SqlDbType.VarChar, 50);
        cmd.Parameters.Add("@my_class", SqlDbType.VarChar, 10);
        cmd.Parameters.Add("@my_scheduling", SqlDbType.VarChar, 5);
        cmd.Parameters.Add("@my_breakfast", SqlDbType.VarChar, 1);
        cmd.Parameters.Add("@my_lunch", SqlDbType.VarChar, 1);
        cmd.Parameters.Add("@my_dinner", SqlDbType.VarChar, 1);
        cmd.Parameters.Add("@my_vegetarian", SqlDbType.VarChar, 1);
        cmd.Parameters.Add("@my_modify_user", SqlDbType.VarChar, 10);
        cmd.Parameters.Add("@my_remarks", SqlDbType.VarChar, 50);

        if (tb_empno.Text == "") //工號
        {
            cmd.Parameters["@my_work_no"].Value = Session["id"].ToString();
        }
        else
        {
            cmd.Parameters["@my_work_no"].Value = tb_empno.Text;
        }
        cmd.Parameters["@my_name"].Value = tb_name.Text; //姓名
        cmd.Parameters["@my_card_no"].Value = lb_card_no.Text; //卡號
        cmd.Parameters["@my_class"].Value = ddl_class.SelectedValue.Substring(0, 1); //日夜班
        cmd.Parameters["@my_scheduling"].Value = ddl_class.SelectedValue; //班別
        cmd.Parameters["@my_breakfast"].Value = rbl_breakfast.SelectedValue; //早餐
        cmd.Parameters["@my_lunch"].Value = rbl_lunch.SelectedValue; //午餐
        cmd.Parameters["@my_dinner"].Value = rbl_dinner.SelectedValue; //晚餐
        if (cbl_vegetarian.Items[0].Selected)
        {
            cmd.Parameters["@my_vegetarian"].Value = "Y"; //素食
        }
        else
        {
            cmd.Parameters["@my_vegetarian"].Value = "";
        }
        cmd.Parameters["@my_modify_user"].Value = str_s[1]; //修改人
        cmd.Parameters["@my_remarks"].Value = tb_remark.Text; //備註

        cmd.ExecuteNonQuery();

        cmd.Cancel();

        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }
        record_personal_log("SUBMIT。提交。", "meal");
        Response.Write("<script language='javascript'>confirm('提交完成');</script>");
    }

    //讀取資料表[WPA51、MHE_user]：找HRS是否常日班，是則帶資料；否則撈[MHE_user]，帶班別資料
    //事件呼叫：PageLoad
    public Boolean DBInit()
    {
        Boolean shift = false; //輪班
        Boolean err = false; //查無資料
        string dept = null;
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        SqlCommand cmd = null;
        SqlDataReader dr = null;
        string sql_str = null;
        //常日(s)------------------------------------------------------------------------------------------------------------------------
        sql_str = "SELECT a.pa51004,a.pa51023,b.pa11003 FROM EHRS.hrs_mis.dbo.WPA51 AS a INNER JOIN EHRS.hrs_mis.dbo.WPA11 AS b ON a.PA51014 = b.PA11002 " +
            "WHERE a.PA51011 = 1 and pa51002 = @my_empno"; //找常日班

        cmd = new SqlCommand(sql_str, Conn);
        cmd.Parameters.Add("@my_empno", SqlDbType.VarChar, 10);
        if (tb_empno.Text == "")
        {
            cmd.Parameters["@my_empno"].Value = Session["id"].ToString(); //工號
        }
        else
        {
            cmd.Parameters["@my_empno"].Value = tb_empno.Text;
        }
        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                tb_name.Text = dr[0].ToString();
                lb_card_no.Text = dr[1].ToString();
                dept = dr[2].ToString();
            }
            tb_class.Text = "DD";
            ddl_class.SelectedValue = "DD"; //常日班
            if (dept.Contains("輪班"))
            {
                shift = true;
            }
        }
        else
        {
            err = true;
        }
        
        if (dr != null)
        {
            cmd.Cancel();
            dr.Close();
        }
        //常日(e)------------------------------------------------------------------------------------------------------------------------
        //輪班(s)------------------------------------------------------------------------------------------------------------------------
        if (shift == true)
        {
            sql_str = "SELECT make_us,Scheduling FROM [dbo].[MHE_user] where work_no = @my_work_no"; //輪班班別

            cmd = new SqlCommand(sql_str, Conn);
            cmd.Parameters.Add("@my_work_no", SqlDbType.VarChar, 10);
            if (tb_empno.Text == "")
            {
                cmd.Parameters["@my_work_no"].Value = Session["id"].ToString(); //工號
            }
            else
            {
                cmd.Parameters["@my_work_no"].Value = tb_empno.Text;
            }

            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    tb_name.Text = dr[0].ToString();
                    tb_class.Text = dr[1].ToString();
                    ddl_class.SelectedValue = dr[1].ToString();
                }
                err = false;
            }
            else
            {
                err = true;
            }
            if (dr != null)
            {
                cmd.Cancel();
                dr.Close();
            }
        }
        //輪班(e)------------------------------------------------------------------------------------------------------------------------
        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }

        if (err)
        {
            record_personal_log("ALERT。查無此人，工號：" + tb_empno.Text, "meal");
            Response.Write("<script language='javascript'>confirm('查無此人');</script>");
        }
        else
        {
            if (tb_empno.Text != "")
            {
                tb_class.Visible = false;
                ddl_class.Visible = true;
                lb_remark.Visible = true;
                tb_remark.Visible = true;
            }
        }
        return err;
    }

    //讀取資料表[PM_Opuser_Work]：判斷工號有無重複
    //事件呼叫：btn_submit_Click
    public Boolean DB_empno()
    {
        Boolean result = false;
        int count;
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        SqlCommand cmd = null;

        string sql_str = null;
        sql_str = "SELECT count(work_no) FROM [dbo].[PM_Opuser_Work] where work_no = @my_work_no and make_date =  @my_make_date";

        cmd = new SqlCommand(sql_str, Conn);
        cmd.Parameters.Add("@my_work_no", SqlDbType.VarChar, 10);
        cmd.Parameters.Add("@my_make_date", SqlDbType.Date);
        if (tb_empno.Text == "")
        {
            cmd.Parameters["@my_work_no"].Value = Session["id"].ToString(); //工號
        }
        else
        {
            cmd.Parameters["@my_work_no"].Value = tb_empno.Text;
        }
        cmd.Parameters["@my_make_date"].Value = DateTime.Now.ToString();
        count = (int)cmd.ExecuteScalar();

        if (count > 0)
        {
            result = true;
        }
        
        cmd.Cancel();
        
        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }
        return result;
    }



    
}