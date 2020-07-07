using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using Ext.Net;
using Oracle.DataAccess.Client;

namespace Koleso.Handlers
{
    
    public class Handler_Kran : IHttpHandler
    {

        string oradb = "Password=rjktcysq;User ID=kpc;Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = Home)(PORT = 1521))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = XE))); Persist Security Info=True";

  
        private void GetWorkInfo(HttpContext context, OracleConnection conn)
        {
            StoreResponseData response = new StoreResponseData();
            System.Collections.Specialized.NameValueCollection rp = context.Request.Params;
            try
            {
                string a = rp.Get("PROLET");
                string b = rp.Get("KRAN_MAC");

                string sql = @"select * from OPP where status=0 and kran='" + a + "' and kran_mac='"+b+"'";
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
        private void GetJobInfo(HttpContext context, OracleConnection conn)
        {
            StoreResponseData response = new StoreResponseData();
            System.Collections.Specialized.NameValueCollection rp = context.Request.Params;
            try
            {
                string a = rp.Get("PROLET");
                string b = rp.Get("KRAN_MAC");

                string sql = @"select * from OPP where status=1 and kran='" + a + "' and kran_mac='" + b + "'";
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

                    case "GetWorkInfo": GetWorkInfo(context, conn); break;
                    case "GetJobInfo": GetJobInfo(context, conn); break;

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