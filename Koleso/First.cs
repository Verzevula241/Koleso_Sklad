using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using Ext.Net;
using Ext.Net.Utilities;
using Oracle.DataAccess.Client;

namespace Koleso
{
    public partial class First : System.Web.UI.Page
    {


        string oradb = "Password=rjktcysq;User ID=kpc;Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = Home)(PORT = 1521))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = XE))); Persist Security Info=True";
        List<Dots> users = new List<Dots>();
        string Kran = "1";
        static int prolet = 0;
        public static int id;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack && !X.IsAjaxRequest)
            {
                MAC.Text = GetMAC();
                prolet = get_prolet(Kran);
                hProlet.Text = prolet.ToString();
                hKran.Text = Kran.ToString();
                //UpdateTabs();
                UpdateWorkTabs();
                ImageSprite img = new ImageSprite()
                {
                    SpriteID = "img",
                    Src = "imgs/Sklad A.png",
                    ZIndex = 0
                };
                Draw1.Add(img);
                Draw1.RenderFrame();
            }
            users = XmlDots();
            OnRunCreate();


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


        protected List<Dots> XmlDots()
        {
            List<Dots> users = new List<Dots>();
            XmlDocument xDoc = new XmlDocument();
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/doc/dotsA.xml");
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
                }
            }
            return tmp;
        }

        protected void CreateSprite(string name, int x, int y)
        {

            RectSprite sprite = new RectSprite
            {
                SpriteID = name,
                Width = 38,
                Height = 38,
                X = x,
                Y = y,
                FillOpacity = 0.5,
                FillStyle = "rgb(255,165,0)",
                Hidden = true
            };
            Draw1.Add(sprite);
            Draw1.RenderFrame();
        }
        [DirectMethod]
        public void lable(string spriteA, string spriteB)
        {
            bool a = users.Exists(x => x.DotName == spriteA);
            bool b = users.Exists(x => x.DotName == spriteB);
            if (a)
            {
                Draw1.GetSprite(spriteA).SetAttributes(new Sprite { Hidden = false });
            }
            if (b)
            {
                Draw1.GetSprite(spriteB).SetAttributes(new Sprite { Hidden = false });
            }
            Draw1.RenderFrame();

        }


        void UpdateWorkTabs()
        {
            OracleConnection conn = new OracleConnection(oradb);

            //Open the connection to the database
            conn.Open();

            OracleCommand cmd = new OracleCommand("select * from OPP where status=1 or status = 5 and kran='" + prolet + "' and kran_mac='" + Kran + "'");

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

                        sql = @"UPDATE OPP SET STATUS = " + stat + ",KRAN_MAC = " + mac + " WHERE ID = " + id + "";

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
                
                        
                            update(Convert.ToInt32(row["ID"]), 1, Kran.ToString());
                       
                        break;
                    case "Перемещение":
                            update(Convert.ToInt32(row["ID"]), 1, Kran.ToString());
                        break;

                }
                //else
                //{
                //    update(Convert.ToInt32(row["ID"]), 1, Kran.ToString());
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
            update(id, 2, Kran.ToString());
        }

    }


}
