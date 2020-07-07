using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Ext.Net;
using Ext.Net.Utilities;
using Oracle.DataAccess.Client;

namespace Koleso
{
    public partial class Kran : System.Web.UI.Page
    {


        string oradb = "Password=rjktcysq;User ID=kpc;Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = Home)(PORT = 1521))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = XE))); Persist Security Info=True";
        List<Dots> users = new List<Dots>();
        string Kran_now = "test";
        string Mac_kran = "1";
        static int prolet ;
        public static int id;
        private static readonly Random rand = new Random();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack && !X.IsAjaxRequest)
            {
                Mac_kran = Request.QueryString["mac"].ToString();
                prolet = get_prolet(Mac_kran);
                Kran_now = get_kran(Mac_kran);
                hProlet.Text = prolet.ToString();
                hKran.Text = Kran_now.ToString();
                Map(prolet);
                //UpdateTabs();
               // UpdateWorkTabs();
                
            }
            users = XmlDots(prolet);
            OnRunCreate();


        }
        private string GetRandomColor()
        {
            return "rgb("+rand.Next(256)+ "," + rand.Next(256) + "," + rand.Next(256) + ")";
        }
        public void Map(int prolet) {

            string map = "";
            
            switch (prolet) {

                case 1:
                    map = "imgs/Sklad A.png";
                    break;
                case 2:
                    map = "imgs/Sklad B.png";
                    break;
                case 3:
                    map = "imgs/Sklad G.png";
                    break;

            }
                ImageSprite img = new ImageSprite()
                {
                    SpriteID = "img",
                    Src = map,
                    ZIndex = 0
                };
                Draw1.Add(img);
                Draw1.RenderFrame();
        
        }


        protected void Create(object sender, DirectEventArgs e)
        {
            string json = e.ExtraParams["Values"];

            Dictionary<string, string>[] tasks = JSON.Deserialize<Dictionary<string, string>[]>(json);


            foreach (Dictionary<string, string> row in tasks)
            {

                task(row["PartB"]);

            }
            //CreateSprite("B/1/1", 82, 60);
        }


        protected List<Dots> XmlDots(int prolet)
        {
            List<Dots> users = new List<Dots>();
            XmlDocument xDoc = new XmlDocument();
            string path = "";
            switch (prolet) {

                case 1:
                    path = System.Web.Hosting.HostingEnvironment.MapPath("~/doc/dotsA.xml");
                    break;
                case 2:
                    path = System.Web.Hosting.HostingEnvironment.MapPath("~/doc/dotsB.xml");
                    break;
                case 3:
                    path = System.Web.Hosting.HostingEnvironment.MapPath("~/doc/dotsG.xml");
                    break;

            }
            
           
            xDoc.Load(path);
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {
                Dots block = new Dots();
                XmlNode name = xnode.Attributes.GetNamedItem("name");
                block.DotName = name.Value;
                foreach (XmlNode child in xnode.ChildNodes)
                {
                    if (child.Name == "x")
                        block.DotX = Int32.Parse(child.InnerText);

                    if (child.Name == "y")
                        block.DotY = Int32.Parse(child.InnerText);
                }
                users.Add(block);
            }
            return users;

        }

        [DirectMethod]
        public bool SAW(string box)
        {

            OracleConnection conn = new OracleConnection(oradb);

            //Open the connection to the database
            conn.Open();

            OracleCommand cmd = new OracleCommand("Select count(OPP.ID) from OPP where OPP.B = '" + box + "' and OPP.Status =1");

            cmd.CommandType = CommandType.Text;

            cmd.Connection = conn;

            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());

            if (count >= 2) return false;
            else return true;

        }

        [DirectMethod]
        public int get_prolet(string mac)
        {
            int tmp = 0;
            string msg = "";
            using (OracleConnection conn = new OracleConnection(oradb))
            {
                using (OracleCommand oc = conn.CreateCommand())
                {
                    string sql = "";

                    try
                    {
                        conn.Open();

                        sql = @"SELECT PROLET FROM KRANS  WHERE MAC='" + mac + "'";

                        oc.CommandText = sql;

                        OracleDataReader dr = oc.ExecuteReader();

                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                tmp = Convert.ToInt32(dr["PROLET"]);

                            }
                            dr.Close();
                        }
                       

                        conn.Dispose();

                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                        throw new Exception(msg);
                    }
                    return tmp;
                }
            }
        }

        [DirectMethod]
        public string get_kran(string mac)
        {
            string kran = "";
            string msg = "";
            using (OracleConnection conn = new OracleConnection(oradb))
            {
                using (OracleCommand oc = conn.CreateCommand())
                {
                    string sql = "";

                    try
                    {
                        conn.Open();

                        sql = @"SELECT KRAN_NAME FROM KRANS  WHERE MAC='" + mac + "'";

                        oc.CommandText = sql;

                        OracleDataReader dr = oc.ExecuteReader();

                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                               kran = dr["KRAN_NAME"].ToString();

                            }
                            dr.Close();
                        }


                        conn.Dispose();

                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                        throw new Exception(msg);
                    }
                    return kran;
                }
            }
        }

        protected void CreateSprite(string name, int x, int y)
        {

            
            CircleSprite sprite = new CircleSprite
            {
                X = x,
                Y = y,
                SpriteID = name,
                Radius = 10,
                FillOpacity = 0.5,
            };
            Draw1.Add(sprite);
            Draw1.RenderFrame();
        }
        [DirectMethod]
        public void lable(string spriteA, string spriteB, int id)
        {
            bool a = users.Exists(x => x.DotName == spriteA);
            bool b = users.Exists(x => x.DotName == spriteB);
            string col = GetRandomColor();
            if (a)
            {
                Draw1.GetSprite(spriteA).SetAttributes(new Sprite { Hidden = false});
                Draw1.GetSprite(spriteA).SetAttributes(new Sprite { FillStyle = col });
            }
            if (b)
            {
                Draw1.GetSprite(spriteB).SetAttributes(new Sprite { Hidden = false, FillStyle = col });
            }
            Draw1.RenderFrame();

        }
        [DirectMethod]
        public void all()
        {
            string col = GetRandomColor();
            foreach (var name in users)
            {
                Draw1.GetSprite(name.DotName).SetAttributes(new Sprite { Hidden = false , FillStyle = col });
                Draw1.RenderFrame();
            }
            Draw1.RenderFrame();

        }

        void UpdateWorkTabs()
        {
            OracleConnection conn = new OracleConnection(oradb);

            //Open the connection to the database
            conn.Open();

            OracleCommand cmd = new OracleCommand("select  from OPP where status=1 or status = 5 and kran='" + prolet + "' and kran_mac='" + Kran_now + "'");

            cmd.CommandType = CommandType.Text;

            cmd.Connection = conn;

            OracleDataReader dr = cmd.ExecuteReader();
            List<Part> parts = new List<Part>();
            if (dr.HasRows)
            {
                while (dr.Read())
                {

                    int i = Convert.ToInt32(dr["ID"]);
                    string name = dr["NAME"].ToString();
                    string a = dr["A"].ToString();
                    string b = dr["B"].ToString();
                    string stat = dr["STATUS"].ToString();


                    parts.Add(new Part() { PartName= name, id = i, PartA = a, PartB = b , Status = stat});

                }
                dr.Close();
            }


            Store2.DataSource = parts;
            Store2.DataBind();

        }

        public String GetMAC()

        {

            String result = String.Empty;

            try

            {

                result = NetBIOS.GetMacAddressByHostname(System.Web.HttpContext.Current.Request.UserHostName);

            }

            catch (Exception ex)

            {

            }

            return result.ToUpper();

        }

       
        [DirectMethod]
        public void update(int id, int stat, string mac)
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
        public void SubmitSelection(string j)
        {
            //string json = e.ExtraParams["Values"];

            Dictionary<string, string>[] tasks = JSON.Deserialize<Dictionary<string, string>[]>(j);

            StringBuilder sb = new StringBuilder();




            foreach (Dictionary<string, string> row in tasks)
            {
                switch (row["NAME"]) {
                    case "перемещение на ПК":
                            update(Convert.ToInt32(row["ID"]), 1, Kran_now.ToString());
                        break;
                    case "Перемещение":
                            update(Convert.ToInt32(row["ID"]), 1, Kran_now.ToString());
                        break;
                    case "ПО складу":
                        update(Convert.ToInt32(row["ID"]), 1, Kran_now.ToString());
                        break;
                    case "С ПК на Склад":
                        update(Convert.ToInt32(row["ID"]), 1, Kran_now.ToString());
                        break;
                    case "РМ-8 в нагревательную печь №1":
                        update(Convert.ToInt32(row["ID"]), 1, Kran_now.ToString());
                        break;

                }
                //else
                //{
                //    update(Convert.ToInt32(row["ID"]), 1, Kran_now.ToString());
                //}
            }
            
        }



        [DirectMethod]
        public void task(string PartB)
        {


            Dots find = users.Find(x => x.DotName.Contains(PartB));
            CreateSprite(find.DotName, find.DotX, find.DotY);
           // Draw1.GetSprite("one").Destroy();


        }
        public void OnRunCreate() {

            foreach (Dots t in users) {

                CreateSprite(t.DotName, t.DotX, t.DotY);

            }
        
        }

        [DirectMethod]
        public void HideALL()
        {

            foreach (Dots t in users)
            {

               Draw1.GetSprite(t.DotName).SetAttributes(new Sprite { Hidden = true });
               Draw1.RenderFrame();

            }

        }

        protected void Sprite(object sender, DirectEventArgs e)
        {
            ImageSprite sprite = new ImageSprite
            {
                SpriteID = "Sprite1",
                Src = "imgs/Sklad.png",
                Scaling = 0.5

            };
        }

        [DirectMethod]
        protected void Update_OPP(object sender, DirectEventArgs e)
        {
            string json = e.ExtraParams["Values"];

            Dictionary<string, string>[] tasks = JSON.Deserialize<Dictionary<string, string>[]>(json);
            id = Convert.ToInt32(tasks[0]["id"]);
            int stat = 3;
            // Configure individualock Buttons using a ButtonsConfig...
            X.Msg.Confirm("Message", "Confirm?", new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig
                {
                    Handler = "App.direct.DoNo();",
                    Text = "Yes Please"
                },
                No = new MessageBoxButtonConfig
                {
                    Handler = "",
                    Text = "No Thanks"
                }
            }).Show();
        }
        [DirectMethod]
        public void DoNo(int id)
        {
            update(id, 2, Kran_now.ToString());
        }

    }
    public class Part
    {
        public int id { get; set; }

        public string PartName { get; set; }


        public string PartA { get; set; }

        public string PartB { get; set; }

        public string Status { get; set; }

    }
    public class Dots
    {
        public string DotName { get; set; }

        public int DotX { get; set; }

        public int DotY { get; set; }

    }


}
