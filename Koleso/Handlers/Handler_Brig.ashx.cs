using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using Ext.Net;
using Oracle.DataAccess.Client;

namespace Koleso.Handlers
{
    /// <summary>
    /// Сводное описание для Handler
    /// </summary>
    public class Handler_Brig : IHttpHandler
    {
        string oradb = "Password=rjktcysq;User ID=kpc;Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = Home)(PORT = 1521))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = XE))); Persist Security Info=True";

        private void GetWheelInfo(HttpContext context, OracleConnection conn)
        {
            string sql = "";
            StoreResponseData response = new StoreResponseData();
            System.Collections.Specialized.NameValueCollection rp = context.Request.Params;
            try
            {
                Boolean a = Convert.ToBoolean(rp.Get("PROLET"));
                if (a == false)
                {
                    sql = @"SELECT p.id, p.name ,p.A,p.B,p.kran,p.kran_mac,p.status, ps.name as STAT FROM OPP p INNER JOIN STATUS_TAB ps ON ps.id = p.status WHERE p.status != '3'";
                }
                else {
                    sql = @"SELECT p.id, p.name ,p.A,p.B,p.kran,p.kran_mac,p.status, ps.name as STAT FROM OPP p INNER JOIN STATUS_TAB ps ON ps.id = p.status";
                }
                DataTable dt = new DataTable();
                using (OracleDataAdapter oda = new OracleDataAdapter(sql, conn)) oda.Fill(dt);
                List<Dictionary<String, Object>> rows = new List<Dictionary<String, Object>>();
                foreach (DataRow dr in dt.Rows)
                {
                    Dictionary<String, Object> r = new Dictionary<String, Object>();
                    foreach (DataColumn dc in dt.Columns) r.Add(dc.ColumnName, dr[dc.ColumnName]);
                    rows.Add(r);
                }
                response.Success = true;
                //response.Total = count;
                response.Data = JSON.Serialize(rows);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            response.Return();
        }
        private void GetKranInfo(HttpContext context, OracleConnection conn)
        {
            StoreResponseData response = new StoreResponseData();
            System.Collections.Specialized.NameValueCollection rp = context.Request.Params;
            try
            {
                string a = rp.Get("PROLET");
                string sql = @"SELECT KRAN_NAME FROM KRANS WHERE PROLET='"+a+"'";
                DataTable dt = new DataTable();
                using (OracleDataAdapter oda = new OracleDataAdapter(sql, conn)) oda.Fill(dt);
                List<Dictionary<String, Object>> rows = new List<Dictionary<String, Object>>();
                foreach (DataRow dr in dt.Rows)
                {
                    Dictionary<String, Object> r = new Dictionary<String, Object>();
                    foreach (DataColumn dc in dt.Columns) r.Add(dc.ColumnName, dr[dc.ColumnName]);
                    rows.Add(r);
                }
                response.Success = true;
                //response.Total = count;
                response.Data = JSON.Serialize(rows);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            response.Return();
        }
        private void GetPlavInfo(HttpContext context, OracleConnection conn)
        {
            StoreResponseData response = new StoreResponseData();
            System.Collections.Specialized.NameValueCollection rp = context.Request.Params;
            try
            {
                string a = rp.Get("PLACE");
                if (a != "")
                {

                    string sql = @"SELECT PLAV_NAME , COUNT FROM PLAV WHERE PLACE='" + a + "' AND COUNT > '0'";
                    DataTable dt = new DataTable();
                    using (OracleDataAdapter oda = new OracleDataAdapter(sql, conn)) oda.Fill(dt);
                    List<Dictionary<String, Object>> rows = new List<Dictionary<String, Object>>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        Dictionary<String, Object> r = new Dictionary<String, Object>();
                        foreach (DataColumn dc in dt.Columns) r.Add(dc.ColumnName, dr[dc.ColumnName]);
                        rows.Add(r);
                    }
                    response.Success = true;
                    //response.Total = count;
                    response.Data = JSON.Serialize(rows);
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            response.Return();
        }
        private void GetPK(HttpContext context, OracleConnection conn)
        {
            StoreResponseData response = new StoreResponseData();
            System.Collections.Specialized.NameValueCollection rp = context.Request.Params;
            try
            {
                string a = rp.Get("PROLET");
                string sql = @"SELECT NAME FROM SKLAD WHERE PLACE='" + a + "'";
                DataTable dt = new DataTable();
                using (OracleDataAdapter oda = new OracleDataAdapter(sql, conn)) oda.Fill(dt);
                List<Dictionary<String, Object>> rows = new List<Dictionary<String, Object>>();
                foreach (DataRow dr in dt.Rows)
                {
                    Dictionary<String, Object> r = new Dictionary<String, Object>();
                    foreach (DataColumn dc in dt.Columns) r.Add(dc.ColumnName, dr[dc.ColumnName]);
                    rows.Add(r);
                }
                response.Success = true;
                //response.Total = count;
                response.Data = JSON.Serialize(rows);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            response.Return();
        }
        private void GetKrans(HttpContext context, OracleConnection conn)
        {
            StoreResponseData response = new StoreResponseData();
            try
            {
                string sql = @"SELECT KRAN_NAME FROM KRANS";
                DataTable dt = new DataTable();
                using (OracleDataAdapter oda = new OracleDataAdapter(sql, conn)) oda.Fill(dt);
                List<Dictionary<String, Object>> rows = new List<Dictionary<String, Object>>();
                foreach (DataRow dr in dt.Rows)
                {
                    Dictionary<String, Object> r = new Dictionary<String, Object>();
                    foreach (DataColumn dc in dt.Columns) r.Add(dc.ColumnName, dr[dc.ColumnName]);
                    rows.Add(r);
                }
                response.Success = true;
                //response.Total = count;
                response.Data = JSON.Serialize(rows);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            response.Return();
        }
        private void GetLog(HttpContext context, OracleConnection conn)
        {
            StoreResponseData response = new StoreResponseData();
            try
            {
                string sql = @"SELECT * FROM LOG";
                DataTable dt = new DataTable();
                using (OracleDataAdapter oda = new OracleDataAdapter(sql, conn)) oda.Fill(dt);
                List<Dictionary<String, Object>> rows = new List<Dictionary<String, Object>>();
                foreach (DataRow dr in dt.Rows)
                {
                    Dictionary<String, Object> r = new Dictionary<String, Object>();
                    foreach (DataColumn dc in dt.Columns) r.Add(dc.ColumnName, dr[dc.ColumnName]);
                    rows.Add(r);
                }
                response.Success = true;
                //response.Total = count;
                response.Data = JSON.Serialize(rows);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            response.Return();
        }
        private void AroundSaw(HttpContext context, OracleConnection conn)
        {
            StoreResponseData response = new StoreResponseData();
            System.Collections.Specialized.NameValueCollection rp = context.Request.Params;
            try
            {
                string sql = "";
                string a = rp.Get("PROLET");
                if (a != "")
                {
                    switch (a)
                    {
                        case "1":
                            sql = @"SELECT NAME FROM SKLAD WHERE PLACE='D'";
                            break;
                        case "2":
                            sql = @"SELECT NAME FROM SKLAD WHERE PLACE='D'";
                            break;
                        case "3":
                            sql = @"SELECT NAME FROM SKLAD WHERE PLACE='G'";
                            break;
                    }
                    DataTable dt = new DataTable();
                    using (OracleDataAdapter oda = new OracleDataAdapter(sql, conn)) oda.Fill(dt);
                    List<Dictionary<String, Object>> rows = new List<Dictionary<String, Object>>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        Dictionary<String, Object> r = new Dictionary<String, Object>();
                        foreach (DataColumn dc in dt.Columns) r.Add(dc.ColumnName, dr[dc.ColumnName]);
                        rows.Add(r);
                    }
                    response.Success = true;
                    //response.Total = count;
                    response.Data = JSON.Serialize(rows);
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            response.Return();
        }
        private void GetStats(HttpContext context, OracleConnection conn)
        {
            StoreResponseData response = new StoreResponseData();
            try
            {
                string sql = @"SELECT NAME FROM STATUS_TAB";
                DataTable dt = new DataTable();
                using (OracleDataAdapter oda = new OracleDataAdapter(sql, conn)) oda.Fill(dt);
                List<Dictionary<String, Object>> rows = new List<Dictionary<String, Object>>();
                foreach (DataRow dr in dt.Rows)
                {
                    Dictionary<String, Object> r = new Dictionary<String, Object>();
                    foreach (DataColumn dc in dt.Columns) r.Add(dc.ColumnName, dr[dc.ColumnName]);
                    rows.Add(r);
                }
                response.Success = true;
                //response.Total = count;
                response.Data = JSON.Serialize(rows);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            response.Return();
        }
        private void GetPlaceInfo(HttpContext context, OracleConnection conn)
        {
            StoreResponseData response = new StoreResponseData();
            System.Collections.Specialized.NameValueCollection rp = context.Request.Params;
            string sql="";
            try
            {
                if (rp.Get("PROLET") != "")
                {
                    string a = rp.Get("PROLET");
                    switch (a) {
                        case "1":
                            sql = @"SELECT NAME FROM SKLAD WHERE PLACE= 'А'";
                            break;
                        case "2":
                            sql = @"SELECT NAME FROM SKLAD WHERE PLACE= 'B' OR PLACE='D'";
                            break;
                        case "3":
                            sql = @"SELECT NAME FROM SKLAD WHERE PLACE= 'V' OR PLACE='G'";
                            break;

                    }
                    DataTable dt = new DataTable();
                    using (OracleDataAdapter oda = new OracleDataAdapter(sql, conn)) oda.Fill(dt);
                    List<Dictionary<String, Object>> rows = new List<Dictionary<String, Object>>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        Dictionary<String, Object> r = new Dictionary<String, Object>();
                        foreach (DataColumn dc in dt.Columns) r.Add(dc.ColumnName, dr[dc.ColumnName]);
                        rows.Add(r);
                    }
                    response.Success = true;
                    //response.Total = count;
                    response.Data = JSON.Serialize(rows);
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            response.Return();
        }
        private void GetPKInfo(HttpContext context, OracleConnection conn)
        {
            StoreResponseData response = new StoreResponseData();
            System.Collections.Specialized.NameValueCollection rp = context.Request.Params;
            string sql = "";
            try
            {
                if (rp.Get("PROLET") != "")
                {
                    string a = rp.Get("PROLET");
                    switch (a)
                    {
                        case "1":
                            sql = @"SELECT NAME FROM SKLAD WHERE PLACE= '1'";
                            break;
                        case "2":
                            sql = @"SELECT NAME FROM SKLAD WHERE PLACE= '2' or PLACE = '1'";
                            break;
                        case "3":
                            sql = @"SELECT NAME FROM SKLAD WHERE PLACE= '3'";
                            break;
                    }
                    DataTable dt = new DataTable();
                    using (OracleDataAdapter oda = new OracleDataAdapter(sql, conn)) oda.Fill(dt);
                    List<Dictionary<String, Object>> rows = new List<Dictionary<String, Object>>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        Dictionary<String, Object> r = new Dictionary<String, Object>();
                        foreach (DataColumn dc in dt.Columns) r.Add(dc.ColumnName, dr[dc.ColumnName]);
                        rows.Add(r);
                    }
                    response.Success = true;
                    //response.Total = count;
                    response.Data = JSON.Serialize(rows);
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            response.Return();
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            String cmd = context.Request.Params.Get("cmd");
            OracleConnection conn = new OracleConnection(oradb);
            try
            {
                switch (cmd)
                {
                    // Заполнение табличной части склада с возможностью фильтрации
                 
                    case "GetWheelInfo": GetWheelInfo(context, conn); break;
                    case "GetKranInfo": GetKranInfo(context, conn); break;
                    case "GetPlaceInfo": GetPlaceInfo(context, conn); break;
                    case "GetPlavInfo": GetPlavInfo(context, conn); break;
                    case "GetPKInfo": GetPKInfo(context, conn); break;
                    case "GetPK": GetPK(context, conn); break;
                    case "GetKrans": GetKrans(context, conn); break;
                    case "GetStats": GetStats(context, conn); break;
                    case "GetLog": GetLog(context, conn); break;
                    case "AroundSaw": AroundSaw(context, conn); break;

                    // Команда не найдена
                    default: SendDirectResponse(false, "Команда не найдена"); break;
                }
            }
            catch (Exception ex)
            {
                SendDirectResponse(false, ex.Message);
            }
            conn.Close();
            GC.Collect();
        }
        private void SendDirectResponse(Boolean inSuccess, String inErrorMessage)
        {
            DirectResponse response = new DirectResponse() { Success = inSuccess, ErrorMessage = inErrorMessage };
            response.Return();
        }

        public bool IsReusable { get { return false; } }
    }
}