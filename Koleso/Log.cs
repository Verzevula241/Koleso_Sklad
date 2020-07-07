using Ext.Net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net.Utilities;
using Oracle.DataAccess.Client;
namespace Koleso
{
    public class Log
    {
        string oradb = "Password=rjktcysq;User ID=kpc;Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = Home)(PORT = 1521))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = XE))); Persist Security Info=True";

        public void Insert_LOG(int id,string name, string A, string B, string kran , string brig , int stat)
        {

            string msg = "";
            using (OracleConnection conn = new OracleConnection(oradb))
            {
                using (OracleCommand oc = conn.CreateCommand())
                {
                    string sql = "";
                    DateTime theDate = DateTime.Now;
                    theDate.ToString("yyyy-MM-dd H:mm:ss");

                    try
                    {
                        conn.Open();
                        oc.Parameters.Clear();

                        sql = @"INSERT INTO LOG(ID_ROW,NAME,A,B,KRAN,BRIG,STATUS,DATE_COM) VALUES(:ID,:NAME,:A,:B,:KRAN,:BRIG,:STATUS,:DATE_COM)";

                        oc.Parameters.Add(":STATUS", OracleDbType.Decimal, id, ParameterDirection.Input);
                        oc.Parameters.Add(":NAME", OracleDbType.Varchar2, name, ParameterDirection.Input);
                        oc.Parameters.Add(":A", OracleDbType.Varchar2, A, ParameterDirection.Input);
                        oc.Parameters.Add(":B", OracleDbType.Varchar2, B, ParameterDirection.Input);
                        oc.Parameters.Add(":KRAN", OracleDbType.Varchar2, kran, ParameterDirection.Input);
                        oc.Parameters.Add(":BRIG", OracleDbType.Varchar2, brig, ParameterDirection.Input);
                        oc.Parameters.Add(":STATUS", OracleDbType.Decimal, stat, ParameterDirection.Input);
                        oc.Parameters.Add(":DATE_COM", OracleDbType.Date, theDate, ParameterDirection.Input);

                        oc.CommandText = sql;

                        int rowsUpdated = oc.ExecuteNonQuery();
                        conn.Dispose();
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                        throw new Exception(msg);
                    }
                }
            }


        }
        public void update_LOG(int id) {
            string msg = "";
            using (OracleConnection conn = new OracleConnection(oradb))
            {
                using (OracleCommand oc = conn.CreateCommand())
                {
                    string sql = "";

                    try
                    {
                        conn.Open();
                        //oc.Parameters.Clear();
                        DateTime theDate = DateTime.Now;
                        theDate.ToString("yyyy-MM-dd H:mm:ss");
                        sql = @"UPDATE LOG set DATE_OUT = :DATE_OUT WHERE ID_ROW = " + id + "";
                        oc.Parameters.Add(":DATE_OUT", OracleDbType.Date, theDate, ParameterDirection.Input);
                        oc.CommandText = sql;

                        int rowsUpdated = oc.ExecuteNonQuery();
                        conn.Dispose();

                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                        throw new Exception(msg);
                    }
                }
            }

        }
    }
}