using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using System.Xml.Xsl;
using Ext.Net.Utilities;
using Oracle.DataAccess.Client;
using System.Xml;

namespace Koleso
{
    public partial class Brig : System.Web.UI.Page
    {
        string oradb = "Password=rjktcysq;User ID=kpc;Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = Home)(PORT = 1521))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = XE))); Persist Security Info=True";
        static long REC_ID;
        static int id;
        static string mac;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !X.IsAjaxRequest)
            {
                List<object> list = new List<object>
            {
                new {Text = "Пролет 1", Value = 1},
                new {Text = "Пролет 2", Value = 2},
                new {Text = "Пролет 3", Value = 3}
            };

                this.Store1.DataSource = list;
                this.Store1.DataBind();

            }
        }


        [DirectMethod]
        public void Prolet_Task(string task) {

            if (task == "4" || task == "5")
            {
                List<object> list = new List<object>
            {
                new {Text = "Пролет 2", Value = 2},
                new {Text = "Пролет 3", Value = 3}
            };

                this.Store1.DataSource = list;
                this.Store1.DataBind();
            }
            if (task == "1") {
                List<object> list = new List<object>
            {
                new {Text = "Пролет 1", Value = 1},
            };

                this.Store1.DataSource = list;
                this.Store1.DataBind();
            }
            else
            {

                List<object> list = new List<object>
            {
                new {Text = "Пролет 1", Value = 1},
                new {Text = "Пролет 2", Value = 2},
                new {Text = "Пролет 3", Value = 3}
            };

                this.Store1.DataSource = list;
                this.Store1.DataBind();
            }


        }


        protected void ToExcel(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Today;
            string date = dt.ToString("dd MMMM yyyy");
            string json = GridData.Value.ToString();
            StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
            XmlNode xml = eSubmit.Xml;
            this.Response.Clear();
            this.Response.ContentType = "application/vnd.ms-excel";
            this.Response.AddHeader("Content-Disposition", "attachment; filename=" + date + ".xls");
            XslCompiledTransform xtExcel = new XslCompiledTransform();
            xtExcel.Load(Server.MapPath("Excel.xsl"));
            xtExcel.Transform(xml, null, this.Response.OutputStream);
            this.Response.End();
        }


        [DirectMethod]
        public List<Sklad_place> Store(string spot)
        {

            OracleConnection conn = new OracleConnection(oradb);

            //Open the connection to the database
            conn.Open();


            OracleCommand cmd = new OracleCommand("select * from SKLAD where PLACE = '" + spot + "'");

            cmd.CommandType = CommandType.Text;

            cmd.Connection = conn;

            OracleDataReader dr = cmd.ExecuteReader();

            List<Sklad_place> place = new List<Sklad_place>();

            if (dr.HasRows)
            {
                while (dr.Read())
                {

                    string p = dr["NAME"].ToString();
                    string s = dr["PLACE"].ToString();

                    place.Add(new Sklad_place() { SkladPlase = p, SkladPart = s });


                }
                dr.Close();
            }

            return place;
        }
        [DirectMethod]
        public bool SAW(string box)
        {

            OracleConnection conn = new OracleConnection(oradb);

            //Open the connection to the database
            conn.Open();

            OracleCommand cmd = new OracleCommand("Select count(OPP.ID) from OPP where OPP.B = '" + box + "' and OPP.Status =0");

            cmd.CommandType = CommandType.Text;

            cmd.Connection = conn;

            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());

            if (count >= 1) return false;
            else return true;

        }
        [DirectMethod]
        public int MAXSAW(string box)
        {

            OracleConnection conn = new OracleConnection(oradb);

            //Open the connection to the database
            conn.Open();

            OracleCommand cmd = new OracleCommand("Select MAX(OPP.ID) from OPP where OPP.B = '" + box + "' and OPP.Status !='1'");

            cmd.CommandType = CommandType.Text;

            cmd.Connection = conn;

            string result = cmd.ExecuteScalar().ToString();
            if (result != "")
            {
                Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
                return count;
            }
            else
            {
                return 0;
            }
        }
        [DirectMethod]
        public void LOG(int id,string name, string A, string B, string kran, string brig, int stat) {

            Log log = new Log();
            log.Insert_LOG(id,name, A, B, kran, brig, stat);
        
        }

        [DirectMethod]
        public long Insert(string name, string A, string B, string prolet, string kran,int stat,int parent)
        {

            string msg = "";
            using (OracleConnection conn = new OracleConnection(oradb))
            {
                using (OracleCommand oc = conn.CreateCommand())
                {
                    string sql = "";

                    try
                    {
                        conn.Open();
                        oc.Parameters.Clear();

                        sql = @"INSERT INTO OPP(NAME,A,B,KRAN,KRAN_MAC,STATUS,PARENT) VALUES(:NAME,:A,:B,:KRAN,:KRAN_MAC,:STATUS,:PARENT) RETURNING ID INTO :ID";

                        oc.Parameters.Add(":NAME", OracleDbType.Varchar2, name, ParameterDirection.Input);
                        oc.Parameters.Add(":A", OracleDbType.Varchar2, A, ParameterDirection.Input);
                        oc.Parameters.Add(":B", OracleDbType.Varchar2, B, ParameterDirection.Input);
                        oc.Parameters.Add(":KRAN", OracleDbType.Varchar2, prolet, ParameterDirection.Input);
                        oc.Parameters.Add(":KRAN_MAC", OracleDbType.Varchar2, kran, ParameterDirection.Input);
                        oc.Parameters.Add(":STATUS", OracleDbType.Decimal, stat, ParameterDirection.Input);
                        oc.Parameters.Add(":PARENT", OracleDbType.Decimal, parent, ParameterDirection.Input);
                        oc.Parameters.Add(":ID", OracleDbType.Int64, ParameterDirection.Output);

                        oc.CommandText = sql;

                        int rowsUpdated = oc.ExecuteNonQuery();
                        conn.Dispose();
                        StoreBrig1.Reload();
                        Grid1.DataBind();
                        return Int64.Parse(oc.Parameters[":ID"].Value.ToString());
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                        throw new Exception(msg);
                    }
                }
            }
            

        }

        [DirectMethod]
        public long Insert_Plav(string name, string place, string col)
        {

            string msg = "";
            using (OracleConnection conn = new OracleConnection(oradb))
            {
                using (OracleCommand oc = conn.CreateCommand())
                {
                    string sql = "";

                    try
                    {
                        conn.Open();
                        oc.Parameters.Clear();

                        sql = @"INSERT INTO PLAV(PLAV_NAME,PLACE,COUNT) VALUES(:NAME,:PLACE,:COUNT) RETURNING COUNT INTO :FIN";

                        oc.Parameters.Add(":NAME", OracleDbType.Varchar2, name, ParameterDirection.Input);
                        oc.Parameters.Add(":PLACE", OracleDbType.Varchar2, place, ParameterDirection.Input);
                        oc.Parameters.Add(":COUNT", OracleDbType.Decimal, Convert.ToInt32(col), ParameterDirection.Input);
                        oc.Parameters.Add(":FIN", OracleDbType.Int64, ParameterDirection.Output);
                        oc.CommandText = sql;

                        int rowsUpdated = oc.ExecuteNonQuery();
                        conn.Dispose();
                        return Int64.Parse(oc.Parameters[":FIN"].Value.ToString());
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                        throw new Exception(msg);
                    }
                }
            }


        }
        [DirectMethod]
        public void update_plav(string place,string plav, int count,int acount)
        {
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
                        count = acount - count;
                        sql = @"UPDATE PLAV SET COUNT = " + count + " WHERE PLACE = '" + place + "' and PLAV_NAME='"+plav+"'";

                        // oc.Parameters.Add(":ID", OracleDbType.Decimal, id, ParameterDirection.Input);
                        // oc.Parameters.Add(":STAT", OracleDbType.Decimal, stat, ParameterDirection.Input);

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

        private Object StringOrNull(String input)
        {
            Object value = DBNull.Value;
            if (input == null || String.IsNullOrEmpty(input)) return value;
            else value = input;
            return value;
        }

        protected void Reload(object sender, DirectEventArgs e)
        {
            StoreBrig1.Reload();
        }

        protected void Confirm(object sender, DirectEventArgs e)
        {
            string json = e.ExtraParams["Values"];
            if (json == "[]") {

                //X.Toast("Строка не выбрана");
                X.Js.AddScript("WarningMessageBox('Строка не выбрана');");

            }
            else
            {
                Dictionary<string, string>[] tasks = JSON.Deserialize<Dictionary<string, string>[]>(json);
                id = Convert.ToInt32(tasks[0]["id"]);
                mac = tasks[0]["Kran_Mac"].ToString();
                if (tasks[0]["Status"] == "Требует проверки")
                {
                    X.Msg.Confirm("Message", "Подтвердить выполнение?", new MessageBoxButtonsConfig
                    {
                        Yes = new MessageBoxButtonConfig
                        {
                            Handler = "App.direct.DoYes();",
                            Text = "ДА"
                        },
                        No = new MessageBoxButtonConfig
                        {
                            Handler = "",
                            Text = "НЕТ"
                        }
                    }).Show();
                }
                else
                {
                    X.Js.AddScript("WarningMessageBox('Задача еще не выполнена');");
                }
            }
        }
        [DirectMethod]
        public void DoYes(string id, string mac)
        {
            update(id, 3);
        }
        [DirectMethod]
        public void show()
        {

            foreach (var item in CompanyInfoTab.Items)
            {
                item.Hidden = true;
            }
            OPP1.Hide();



        }
        [DirectMethod]
        public void AfterYes(string parent_id, string status,int stat)
        {
            update_second(parent_id, stat, Convert.ToInt32(status));
        }
        [DirectMethod]
        public void Log_up(int id)
        {
            Log log = new Log();
            log.update_LOG(id);
        }
        [DirectMethod]
        void update(string id, int stat)
        {
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

                        sql = @"UPDATE OPP SET STATUS = " + stat + " WHERE ID = " + id + "";

                        // oc.Parameters.Add(":ID", OracleDbType.Decimal, id, ParameterDirection.Input);
                        // oc.Parameters.Add(":STAT", OracleDbType.Decimal, stat, ParameterDirection.Input);

                        oc.CommandText = sql;

                        int rowsUpdated = oc.ExecuteNonQuery();
                        if (rowsUpdated == 0)
                            X.Js.AddScript("WarningMessageBox('Данные не добавлены')");
                        else
                            X.Js.AddScript("SuccessMessageBox('Данные добавлены')");
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
        [DirectMethod]
        void update_second(string id, int stat,int stat_cur)
        {
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

                        sql = @"UPDATE OPP SET STATUS = " + stat + " WHERE PARENT = " + id + " and STATUS = "+stat_cur+"";

                        oc.CommandText = sql;

                        int rowsUpdated = oc.ExecuteNonQuery();
                        
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                        throw new Exception(msg);
                    }
                }
            }

        }
        protected void Button2_Click(object sender, DirectEventArgs e)
        {
            show();
            int opp = Convert.ToInt32(OPP.Value);
            switch (opp)
            {
                case 1:
                    Window1.Title = OPP.RawText;
                    Prolet_Task(opp.ToString());
                    Window1.Show();
                    OPP1.Show();
                    VagonNum.Show();
                    Prolet.Show();
                    KranMan.Show();
                    PlaceSklad.Show();
                    NamePlav.Show();
                    ColPlav.Show();
                    break;
                case 2:
                    Window1.Title = OPP.RawText;
                    Prolet_Task(opp.ToString());
                    Window1.Show();
                    OPP1.Show();
                    OT.Show();
                    Prolet.Show();
                    KranMan.Show();
                    PlaceSklad.Show();
                    DO.Show();
                    Prolet1.Show();
                    KranMan1.Show();
                    PlaceSklad1.Show();
                    Telega.Show();
                    PlavCombo.Show();
                    PlavCol1.Show();
                    break;
                case 3:
                    Window1.Title = OPP.RawText;
                    Prolet_Task(opp.ToString());
                    Window1.Show();
                    KranMan.Show();
                    PlaceSklad.Show();
                    OPP1.Show();
                    Prolet.Show();
                    PilaBox.Show();
                    PKBox.Show();
                    PlavCombo.Show();
                    break;
                case 4:
                    Window1.Title = OPP.RawText;
                    Prolet_Task(opp.ToString());
                    Window1.Show();
                    OPP1.Show();
                    KranMan.Show();
                    Prolet.Show();
                    PilaBox.Show();
                    PKBox.Show();
                    DSAW.Show();
                    PlavCombo1.Show();
                    break;
                case 5:
                    Window1.Title = OPP.RawText;
                    Prolet_Task(opp.ToString());
                    Window1.Show();
                    OPP1.Show();
                    Prolet.Show();
                    KranMan.Show();
                    PilaBox.Show();
                    PKBox.Show();
                    DSAW.Show();
                    //Box.Show();
                    Telega_Forge.Show();
                    break;
            }

        }
        
    }
   
    public class Sklad_place { 
    
        public string SkladPart { get; set; }

        public string SkladPlase { get; set; }

    }


}
