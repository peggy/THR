﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;

using System.DirectoryServices; //需加入參考。AD使用
using System.Runtime.InteropServices;

using System.IO; //檔案讀寫
using System.Text.RegularExpressions;

public partial class login : class_login
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session.Clear();
            Session.Abandon();

            HttpBrowserCapabilities hbc = Request.Browser;
            if (hbc.Browser.ToString() != "Chrome")
            {
                Response.Write("<script language='javascript'>alert('錯誤!請使用Chrome瀏覽器開啟');location.href='https://www.google.com/'</script>");
            }
        }
    }

    protected void btn_login_login_Click(object sender, EventArgs e)
    {
        //在blank.aspx.cs裡，判斷Session是否同一人登入，如在不同分頁重複開啟blank.aspx，javascript則原頁面會被導向login.aspx。
        Session["OK"] = "";

        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
       
        string ad_id = tb_login_id.Text;
        string ad_ps = tb_login_pass.Text;
        string strExpression = "^[0-9]*[1-9][0-9]*$"; //正規表達式：數字
        Regex reg = new Regex(strExpression);
        DirectoryEntry ent = new DirectoryEntry("LDAP://mhe.com.tw/dc=mhe,dc=com,dc=tw", ad_id, ad_ps); //網域名稱 , 以 "."為分隔 ,接續帳號,密碼
        DirectorySearcher ds = new DirectorySearcher(ent); //建立 搜尋 AD的物件。利用 DirectorySearcher 類別來對 Active Directory 進行查詢

        ds.Filter = "(sAMAccountName=" + ad_id + ")"; //設立條件 , 這裡是找帳號與輸入ID一樣
        ds.PropertiesToLoad.Add("sn"); //搜尋期間要擷取的屬性清單
        ds.SearchScope = SearchScope.Subtree;  //伺服器觀察的搜尋範圍

        //非AD帳號
        if (reg.IsMatch(tb_login_id.Text))
        {
            if (DB_login_authority(ad_id, ad_ps, "NAD") != null)
            {
                Session["OK"] = DB_login_authority(ad_id, ad_ps, "NAD"); //儲存顯示名稱
                Session["id"] = ad_id; //工號
                Session["ac"] = ad_id; //帳號
                Record("NAD"); //紀錄登入使用者
                Response.Redirect("blank.aspx"); //登入成功跳轉空白頁面
                return;
            }
        }

        //AD帳號
        try
        { 
            SearchResult sr = ds.FindOne(); //搜尋到的第一個物件
            if (sr == null)
            {
                //找不到帳號
                Response.Write("找不到帳號");
            }
            else
            {
                string[] atestarr = { "displayName", "department", "userPrincipalName" };//顯示名稱,部門,帳號

                if (sr.GetDirectoryEntry().Properties[atestarr[1]].Value != null) //部門不為空值
                {
                    //判斷部門是否符合權限
                    string att = sr.GetDirectoryEntry().Properties[atestarr[0]].Value.ToString();
                    string[] list_auth = DB_login_authority(ad_id, ad_ps, "AD").Split('-');
                    if (list_auth [0]== "1")
                    {
                        Session["OK"] = att; //顯示名稱
                        Session["id"] = list_auth[1]; //工號
                        Session["ac"] = ad_id; //帳號
                        Record(); //紀錄登入使用者
                        Response.Redirect("blank.aspx"); //登入成功跳轉空白頁面
                    }
                    else
                    {
                        Response.Write("<script language='javascript'>alert('無此權限登入失敗!!');</script>");
                    }
                }
                else //空值
                {
                    Response.Write("<script language='javascript'>alert('無此權限登入失敗');</script>");
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex);
        }

    }

    //以文字檔寫入，紀錄登入使用者
    public void Record([Optional]string type)
    {
        string login_name = null;
        
        string[] str_s = Session["OK"].ToString().Split('-');
        login_name = str_s[1];

        //(務必修改這個檔案的權限，需要「寫入」的權限)
        //寫入檔案
        StreamWriter sw = new StreamWriter("C:\\Users\\peggy.liou\\Documents\\VS\\測試區\\HR\\file\\login_log.txt",true);
        sw.Write(login_name);
        sw.Write("---");
        sw.Write(DateTime.Now.ToString());
        sw.WriteLine();

        sw.Close();
        sw.Dispose();
    }

}