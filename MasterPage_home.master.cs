using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

using System.IO; //檔案讀寫

public partial class MasterPage_home : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["OK"] != null)
            {
                lb_login_name.Text = Session["OK"].ToString();
                lbtn_login.Text = "Logout";

                string[] str_s = Session["OK"].ToString().Split('-');
                class_login cs_l = new class_login();

                //非AD帳號
                string[] arr_auth_mhe_user = cs_l.DB_authority(str_s[1], "make"); 
                if (arr_auth_mhe_user[0] == "1") //evaluation_make
                {
                    li_personal.Visible = true; //個人資料
                    li_e.Visible = true; //考核
                    lbtn_evaluation_make_all.Visible = true; //考核_製造課
                }

                //AD帳號
                string[] arr_auth = cs_l.DB_authority(str_s[1], "masterpage");

                if (arr_auth[2] == "99" || arr_auth[2] == "10") //store
                {
                    li_s.Visible = true; //特約商店_編輯
                }

                if (arr_auth[2] != "10") //store
                {
                    lbtn_store_all.Visible = true;
                    li_public.Visible = true;
                }

                if (arr_auth[0] == "99" || arr_auth[0] == "10") //interview
                {
                    li_ii.Visible = true; //邀約
                    lbtn_ii_edit.Visible= true; //編輯
                }

                if (arr_auth[1] == "99" || arr_auth[1] == "10" || arr_auth[1] == "11") //evaluation
                {
                    li_e.Visible = true; //考核
                    li_ev.Visible = true; //考核_試用期_編輯
                }

                if (arr_auth[1] == "20" || arr_auth[1] == "21") //evaluation
                {
                    li_e.Visible = true; //考核
                    lbtn_evaluation_all.Visible = true; //考核_試用期
                }

                if (arr_auth[4] == "99" || arr_auth[4] == "20") //personal,evaluation_make
                {
                    li_personal.Visible = true; //個人資料
                    li_e.Visible = true; //考核
                    li_ev_m.Visible = true; //考核_製造課_編輯
                }

                if (arr_auth[4] == "10") //evaluation_make
                {
                    lbtn_evaluation_make_all.Visible = true; //考核_製造課
                }

                if (arr_auth[2] == "99" || arr_auth[3] == "99" || arr_auth[4] == "99") 
                {
                    lbtn_extension_edit.Visible = true; //3 extension
                    lbtn_evaluation_all.Visible = true; //2 evaluation
                    lbtn_evaluation_make_all.Visible = true; // 4 evaluation_make
                }
                
            }
            else
            {
                lb_login_name.Text = "";
            }
        }
    }

    //邀約
    //[預設值：導覽列點擊編輯頁面帶入]
    protected void lbtn_interview_edit_Click(object sender, EventArgs e)
    {

        string str_cmd = "select MAX(c_id) from dbo.HR_interview";
        string str_id = DB_insert(str_cmd,"year");

        Response.Redirect("/interview_edit.aspx?ID=" + str_id + "&s=i");
        //帶入新增的預設值(end)-----------------------------------------------------------------------
    }

    //考核_試用期
    //[預設值：導覽列點擊編輯頁面帶入]
    protected void lbtn_evaluation_edit_Click(object sender, EventArgs e)
    {
        string str_cmd = "select MAX(e_id) from dbo.HR_evaluation";
        string str_id = DB_insert(str_cmd,"year");
        Response.Redirect("/evaluation_edit.aspx?ID=" + str_id + "&s=i");

    }

    //特約
    //[預設值：導覽列點擊編輯頁面帶入]
    protected void lbtn_store_edit_Click(object sender, EventArgs e)
    {
        string str_cmd = "select MAX(id) from dbo.HR_store";
        string str_id = DB_insert(str_cmd, "number");
        Response.Redirect("/store_edit.aspx?ID=" + str_id + "&s=i");
    }

    //特約
    //計數訪客人數
    protected void lbtn_store_query_Click(object sender, EventArgs e)
    {
        class_login cs_l = new class_login();
        cs_l.record_store_count(); //訪客人數加一寫入
        
        Response.Redirect("/store_query.aspx");
    }

    //登入、登出
    protected void lbtn_login_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        lb_login_name.Text = "";
        Response.Redirect("~/login.aspx");
    }

    //帶入新增的預設值(start)
    //[事件呼叫：lbtn_interview_edit_Click、lbtn_e_edit_Click]
    //[預設值：導覽列點擊編輯頁面帶入]
    public string DB_insert(string str_db,string type)
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        string str_c_id = null;

        SqlCommand cmd = new SqlCommand(str_db, Conn); //取最大值
        int c_id = (int)cmd.ExecuteScalar(); //回傳第一筆資料：筆數
        cmd.Cancel();
        if (type == "year")
        {
            if (c_id.ToString().Substring(0, 2) == (DateTime.Now.ToString("yyyy")).Substring(2, 2))
            {
                str_c_id = (c_id + 1).ToString(); //當年度
            }
            else
            {
                c_id = int.Parse((DateTime.Now.ToString("yyyy")).Substring(2, 2)); //西元年後兩位數
                str_c_id = ((c_id * 1000000) + 1).ToString();
            }
        }
        else if (type == "number")
        {
            str_c_id = (c_id + 1).ToString();
            str_c_id = string.Format("{0:000}", Convert.ToInt32(str_c_id));
        }
        
        Conn.Close();

        return str_c_id;
    }



    





    
}
