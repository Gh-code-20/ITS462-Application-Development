using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using Newtonsoft.Json;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Data.SQLite;
using HtmlAgilityPack;

namespace FP_Service
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        //Declare  an application path.
        static string appPath = @"C:\Users\sarab\source\repos\FinalProject_Service\FinalProject_Service";
        //Set sqllite connection with database name "Scape Database"
        static SQLiteConnection conn = new SQLiteConnection("Data Source=" + appPath + "\\Database.db; Version=3;New=True;Compress=True;");
        SQLiteCommand sqlCmd = new SQLiteCommand(conn);
       
       

        DataTable dtLaptops = new DataTable();
        DataTable dtTablets = new DataTable();
        DataTable dtPhones = new DataTable();
        DataTable dtLaptops2 = new DataTable();
        DataTable dtTablets2 = new DataTable();
        DataTable dtPhones2 = new DataTable();
        DataTable dtAllProductQuery = new DataTable();
        DataTable dtAllProduct2Query = new DataTable();
        DataTable dtPhonesQuery = new DataTable();
        DataTable dtPhones2Query = new DataTable();
        DataTable dtLaptopsQuery = new DataTable();
        DataTable dtLaptop2Query = new DataTable();
        DataTable dtTabletsQuery = new DataTable();
        DataTable dtTablets2Query = new DataTable();
        DataTable dtDescriptionQuery = new DataTable();
        DataTable dtDescription2Query = new DataTable();



        [WebMethod]
        public string ScrapeIO()
        {
          
            conn.Close();


            //----------HTML PARSERS FROM WEB-----------
            string L_url = "https://webscraper.io/test-sites/e-commerce/allinone/computers/laptops";
            string T_url = "https://webscraper.io/test-sites/e-commerce/allinone/computers/tablets";
            string P_url = "https://webscraper.io/test-sites/e-commerce/allinone/phones/touch";

            HtmlWeb L_web = new HtmlWeb();
            HtmlWeb T_web = new HtmlWeb();
            HtmlWeb P_web = new HtmlWeb();

            var L_doc = L_web.Load(L_url);
            var T_doc = T_web.Load(T_url);
            var P_doc = P_web.Load(P_url);

            //-------------HTML AGILITY PACK----------------
            //Gets count for how many LAPTOPS, TABLETS, & PHONES are on website
            int L_count = L_doc.DocumentNode.SelectNodes("//div[@class='col-sm-4 col-lg-4 col-md-4']").Count();
            int L_correctCount = +L_count;

            int T_count = T_doc.DocumentNode.SelectNodes("//div[@class='col-sm-4 col-lg-4 col-md-4']").Count();
            int T_correctCount = +T_count;

            int P_count = P_doc.DocumentNode.SelectNodes("//div[@class='col-sm-4 col-lg-4 col-md-4']").Count();
            int P_correctCount = +P_count;


            //Creates an array with # of products for LAPTOPS, TABLETS, & PHONES
            Products[] L_product = new Products[L_correctCount];
            for (int y = 0; y < L_correctCount; y++)
            {
                L_product[y] = new Products();
            }

            Products[] T_product = new Products[T_correctCount];
            for (int y = 0; y < T_correctCount; y++)
            {
                T_product[y] = new Products();
            }
            Products[] P_product = new Products[P_correctCount];
            for (int y = 0; y < P_correctCount; y++)
            {
                P_product[y] = new Products();
            }

            //-------------------------HTML SELECTORS-----------------------
            HtmlNode node = L_doc.DocumentNode.SelectNodes("//div[@class='col-sm-4 col-lg-4 col-md-4']").First();
            // scrapes laptop price
            HtmlNode[] lpNodes = L_doc.DocumentNode.SelectNodes(".//h4[@class='pull-right price']").ToArray();
            //scrapes laptop title
            HtmlNode[] lbNodes = L_doc.DocumentNode.SelectNodes(".//a[@class='title']").ToArray();
            //scrapes laptop description
            HtmlNode[] ldNodes = L_doc.DocumentNode.SelectNodes(".//p[@class='description']").ToArray();
            //----------------------
            HtmlNode T_node = T_doc.DocumentNode.SelectNodes("//div[@class='col-sm-4 col-lg-4 col-md-4']").First();
            // scrape tablet price
            HtmlNode[] tpNodes = T_doc.DocumentNode.SelectNodes(".//h4[@class='pull-right price']").ToArray();
            //scrape tablettitle
            HtmlNode[] tbNodes = T_doc.DocumentNode.SelectNodes(".//a[@class='title']").ToArray();
            //scrape tablet description
            HtmlNode[] tdNodes = T_doc.DocumentNode.SelectNodes(".//p[@class='description']").ToArray();
            //-----------------------
            HtmlNode P_node = P_doc.DocumentNode.SelectNodes("//div[@class='col-sm-4 col-lg-4 col-md-4']").First();
            // gets price
            HtmlNode[] pNodes = P_doc.DocumentNode.SelectNodes(".//h4[@class='pull-right price']").ToArray();
            //// gets title
            HtmlNode[] bNodes = P_doc.DocumentNode.SelectNodes(".//a[@class='title']").ToArray();
            // gets description
            HtmlNode[] dNodes = P_doc.DocumentNode.SelectNodes(".//p[@class='description']").ToArray();

            //-------------------------HTML MANIPULATION------------------
            // puts values into the array
            for (int x = 0; x < L_correctCount; x++)
            {
                L_product[x].ProductName = lbNodes[x].Attributes["title"].Value;
                L_product[x].Price = lpNodes[x].InnerHtml;
                L_product[x].Description = ldNodes[x].InnerHtml;
            }
            for (int x = 0; x < P_correctCount; x++)
            {
                P_product[x].ProductName = bNodes[x].Attributes["title"].Value;
                P_product[x].Price = pNodes[x].InnerHtml;
                P_product[x].Description = dNodes[x].InnerHtml;
            }
            for (int x = 0; x < T_correctCount; x++)
            {
                T_product[x].ProductName = tbNodes[x].Attributes["title"].Value;
                T_product[x].Price = tpNodes[x].InnerHtml;
                T_product[x].Description = tdNodes[x].InnerHtml;
            }
            conn.Open();
            //creates table
            string createTablesQuery = @"DROP TABLE Laptops; CREATE TABLE IF NOT EXISTS [Laptops] ([ProductName] VARCHAR(200) NULL,[Description] VARCHAR(200) NULL,
            [Price] VARCHAR(200) NULL); DROP TABLE Tablets; CREATE TABLE IF NOT EXISTS [Tablets] ([ProductName] VARCHAR(200) NULL,[Description] VARCHAR(200) NULL,
            [Price] VARCHAR(200) NULL); DROP TABLE Phones;CREATE TABLE IF NOT EXISTS [Phones] ([ProductName] VARCHAR(200) NULL,[Description] VARCHAR(200) NULL,
            [Price] VARCHAR(200) NULL);";

            sqlCmd.CommandText = createTablesQuery;

            //conn.Open();
            sqlCmd.ExecuteNonQuery();

            sqlCmd = new SQLiteCommand(conn);
            SQLiteDataAdapter dataAdapter;
            int l = 0;
            int t = 0;
            int p = 0;

            //inserts array into database
            while (l < L_correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = "Select * from Laptops where ProductName='" + L_product[l].ProductName + "'";

                dataAdapter.Fill(dtLaptops);
                
                sqlCmd.CommandText = "Insert into Laptops(ProductName, Description, Price) values " +
                        "('" + L_product[l].ProductName + "','" + L_product[l].Description + "','" + L_product[l].Price + "')";
                sqlCmd.ExecuteNonQuery();
                l++;
            }
            while (t < T_correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = "Select * from Tablets where ProductName='" + T_product[t].ProductName + "'";
                dataAdapter.Fill(dtTablets);
                sqlCmd.CommandText = "Insert into Tablets(ProductName, Description, Price) values " +
                        "('" + T_product[t].ProductName + "','" + T_product[t].Description + "','" + T_product[t].Price + "')";
                sqlCmd.ExecuteNonQuery();
                t++;
            }
            while (p < P_correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = "Select * from Phones where ProductName='" + P_product[p].ProductName + "'";
                dataAdapter.Fill(dtPhones);

                sqlCmd.CommandText = "Insert into Phones(ProductName, Description, Price) values " +
                        "('" + P_product[p].ProductName + "','" + P_product[p].Description + "','" + P_product[p].Price + "')";
                sqlCmd.ExecuteNonQuery();
                p++;
            }
            string L_result = JsonConvert.SerializeObject(dtLaptops);
            string T_result = JsonConvert.SerializeObject(dtTablets);
            string P_result = JsonConvert.SerializeObject(dtPhones);
            return L_result + T_result + P_result;



        }
      

        //----------------------------

        [WebMethod]
        public string ScrapeWebsite2()
        {
            conn.Close();
            //Scrape();

            //----------HTML PARSERS FROM WEB-----------
            string L2_url = "https://www.discount-computer.com/desktops/";
            string T2_url = "https://www.discount-computer.com/search.php?search_query=tablets&section=product&_bc_fsnf=1&Operating%20System=Windows%2010%20Pro";
            string P2_url = "https://www.discount-computer.com/search.php?search_query=phones&section=product&_bc_fsnf=1&Apple%20";

            HtmlWeb L2_web = new HtmlWeb();
            HtmlWeb T2_web = new HtmlWeb();
            HtmlWeb P2_web = new HtmlWeb();

            var L2_doc = L2_web.Load(L2_url);
            var T2_doc = T2_web.Load(T2_url);
            var P2_doc = P2_web.Load(P2_url);

          

            // gets number of items on page
            int L2_count = L2_doc.DocumentNode.SelectNodes("//li[@class='product']").Count();
            int T2_count = T2_doc.DocumentNode.SelectNodes("//li[@class='product']").Count();
            int P2_count = P2_doc.DocumentNode.SelectNodes("//li[@class='product']").Count();
            // adjusts count for the 0 start in array
            int L2_correctCount = +L2_count;
            int T2_correctCount = +T2_count;
            int P2_correctCount = +P2_count;

            //Creates an array with # of products for LAPTOPS, TABLETS, & PHONES
            Products[] L2_product = new Products[L2_correctCount];
            Products[] T2_product = new Products[T2_correctCount];
            Products[] P2_product = new Products[P2_correctCount];
            for (int L2 = 0; L2 < L2_correctCount; L2++)
            {
                L2_product[L2] = new Products();
            }
            for (int t2 = 0; t2 < T2_correctCount; t2++)
            {
                T2_product[t2] = new Products();
            }
            for (int p2 = 0; p2 < T2_correctCount; p2++)
            {
                P2_product[p2] = new Products();
            }

            //-------------------------HTML SELECTORS------------------------------
            // gets price, title, descriptions for laptops,tablets and phones
            HtmlNode L2_node = L2_doc.DocumentNode.SelectNodes("//li[@class='product']").First();
            //scrapes price
            HtmlNode[] L2_pNodes = L2_doc.DocumentNode.SelectNodes(".//span[@class='price price--withoutTax price--main']").ToArray();
            //scrapes title
            HtmlNode[] L2_bNodes = L2_doc.DocumentNode.SelectNodes(".//p[@class='card-text card-text--brand']").ToArray();
            //scrapes description
            HtmlNode[] L2_dNodes = L2_doc.DocumentNode.SelectNodes(".//h4[@class='card-title']").ToArray();
            //---------------------------
            HtmlNode T2_node = T2_doc.DocumentNode.SelectNodes("//li[@class='product']").First();
            //scrapes price
            HtmlNode[] T2_pNodes = T2_doc.DocumentNode.SelectNodes(".//span[@class='price price--withoutTax price--main']").ToArray();
            //scrapes title
            HtmlNode[] T2_bNodes = T2_doc.DocumentNode.SelectNodes(".//p[@class='card-text card-text--brand']").ToArray();
            //scrapes description
            HtmlNode[] T2_dNodes = T2_doc.DocumentNode.SelectNodes(".//h4[@class='card-title']").ToArray();
           // --------------------------
            HtmlNode P2_node = T2_doc.DocumentNode.SelectNodes("//li[@class='product']").First();
            //scrapes price
            HtmlNode[] P2_pNodes = T2_doc.DocumentNode.SelectNodes(".//span[@class='price price--withoutTax price--main']").ToArray();
            //scrapes title
            HtmlNode[] P2_bNodes = T2_doc.DocumentNode.SelectNodes(".//p[@class='card-text card-text--brand']").ToArray();
            //scrapes description
            HtmlNode[] P2_dNodes = T2_doc.DocumentNode.SelectNodes(".//h4[@class='card-title']").ToArray();


            //-------------------------HTML MANIPULATION------------------
            // puts values into the array
            for (int b = 0; b < L2_correctCount; b++)
            {
               
                L2_product[b].ProductName = L2_bNodes[b].InnerHtml;
                  L2_product[b].Price = L2_pNodes[b].InnerHtml;
                  L2_product[b].Description = L2_dNodes[b].InnerText;
              
            }
            for (int b = 0; b < T2_correctCount; b++)
            {

                T2_product[b].ProductName = T2_bNodes[b].InnerHtml;
                T2_product[b].Price = T2_pNodes[b].InnerHtml;
                T2_product[b].Description = T2_dNodes[b].InnerText;

            }
            for (int b = 0; b < T2_correctCount; b++)
            {

                P2_product[b].ProductName = P2_bNodes[b].InnerHtml;
                P2_product[b].Price = P2_pNodes[b].InnerHtml;
                P2_product[b].Description = P2_dNodes[b].InnerText;

            }

            string createTablesQuery = @"DROP TABLE Laptops2; CREATE TABLE IF NOT EXISTS [Laptops2] ([ProductName] VARCHAR(200) NULL,
            [Description] VARCHAR(200) NULL, [Price] VARCHAR(200) NULL); DROP TABLE Tablets2; CREATE TABLE IF NOT EXISTS [Tablets2] ([ProductName] VARCHAR(200) NULL,
             [Description] VARCHAR(200) NULL, [Price] VARCHAR(200) NULL);  CREATE TABLE IF NOT EXISTS [Phones2] ([ProductName] VARCHAR(200) NULL,
             [Description] VARCHAR(200) NULL, [Price] VARCHAR(200) NULL);";

            sqlCmd.CommandText = createTablesQuery;

            conn.Open();
            sqlCmd.ExecuteNonQuery();

            sqlCmd = new SQLiteCommand(conn);
            SQLiteDataAdapter dataAdapter;
            int x = 0;
            int t = 0;
            int y = 0;
            //inserts array into database
            while (x < L2_correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = "Select * from Laptops2 where ProductName='" + L2_product[x].ProductName + "'";
                dataAdapter.Fill(dtLaptops2);

                sqlCmd.CommandText = "Insert into Laptops2(ProductName, Description, Price) values " +
                        "('" + L2_product[x].ProductName + "','" + L2_product[x].Description + "','" + L2_product[x].Price + "')";
                sqlCmd.ExecuteNonQuery();
                x++;
            }
            while (t < T2_correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = "Select * from Tablets2 where ProductName='" + T2_product[t].ProductName + "'";
                dataAdapter.Fill(dtTablets2);

                sqlCmd.CommandText = "Insert into Tablets2(ProductName, Description, Price) values " +
                        "('" + T2_product[t].ProductName + "','" + T2_product[t].Description + "','" + T2_product[t].Price + "')";
                sqlCmd.ExecuteNonQuery();
                t++;
            }
            while (y < P2_correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = "Select * from Phones2 where ProductName='" + P2_product[t].ProductName + "'";
                dataAdapter.Fill(dtPhones2);

                sqlCmd.CommandText = "Insert into Phones2(ProductName, Description, Price) values " +
                        "('" + P2_product[t].ProductName + "','" + P2_product[t].Description + "','" + P2_product[t].Price + "')";
                sqlCmd.ExecuteNonQuery();
                t++;
            }
            string L2_result = JsonConvert.SerializeObject(dtLaptops2);
            string T2_result = JsonConvert.SerializeObject(dtTablets2);
            string P2_result = JsonConvert.SerializeObject(dtPhones2);
            return L2_result + T2_result;

        }





        [WebMethod]
        public string QueryAllProduct()
        {
            conn.Close();
            string allProductsQuery = @"select t.* from Laptops t union all select t.* from Phones t union all select t.* from Tablets t order by price;";

            sqlCmd.CommandText = allProductsQuery;
            conn.Open();
            //sqlCmd.ExecuteNonQuery();
            sqlCmd = new SQLiteCommand(conn);
            SQLiteDataAdapter dataAdapter;
            dataAdapter = new SQLiteDataAdapter(sqlCmd);
            int correctCount = 1;
            int x = 0;
            while (x < correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = allProductsQuery;
                dataAdapter.Fill(dtAllProductQuery);
                sqlCmd.ExecuteNonQuery();
                x++;
            }


            string result = JsonConvert.SerializeObject(dtAllProductQuery);
            return result;
        }
        [WebMethod]
        public string QueryDescription()
        {
            conn.Close();
            string allDescriptionQuery = @"select t.Description, t.Price from Laptops t union all select t.Description, t.Price from Phones t union all select t.Description ,t.Price from Tablets t order by price;";

            sqlCmd.CommandText = allDescriptionQuery;
            conn.Open();
            //sqlCmd.ExecuteNonQuery();
            sqlCmd = new SQLiteCommand(conn);
            SQLiteDataAdapter dataAdapter;
            dataAdapter = new SQLiteDataAdapter(sqlCmd);
            int correctCount = 1;
            int x = 0;
            while (x < correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = allDescriptionQuery;
                dataAdapter.Fill(dtDescriptionQuery);
                sqlCmd.ExecuteNonQuery();
                x++;
            }


            string result = JsonConvert.SerializeObject(dtDescriptionQuery);
            return result;
        }


        [WebMethod]
        public string QueryLaptops()
        {
            conn.Close();
            string alllaptopQuery = @"select * from Laptops order by price;";

            sqlCmd.CommandText = alllaptopQuery;
            conn.Open();
            //sqlCmd.ExecuteNonQuery();
            sqlCmd = new SQLiteCommand(conn);
            SQLiteDataAdapter dataAdapter;
            dataAdapter = new SQLiteDataAdapter(sqlCmd);
            int correctCount = 1;
            int x = 0;
            while (x < correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = alllaptopQuery;
                dataAdapter.Fill(dtLaptopsQuery);
                sqlCmd.ExecuteNonQuery();
                x++;
            }


            string result = JsonConvert.SerializeObject(dtLaptopsQuery);
            return result;
        }
        [WebMethod]
        public string QueryTablets()
        {
            conn.Close();
            string allTabetsQuery = @"select * from Tablets
                                        order by price;";

            sqlCmd.CommandText = allTabetsQuery;
            conn.Open();
            //sqlCmd.ExecuteNonQuery();
            sqlCmd = new SQLiteCommand(conn);
            SQLiteDataAdapter dataAdapter;
            dataAdapter = new SQLiteDataAdapter(sqlCmd);
            int correctCount = 1;
            int x = 0;
            while (x < correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = allTabetsQuery;
                dataAdapter.Fill(dtTabletsQuery);
                sqlCmd.ExecuteNonQuery();
                x++;
            }
            string result = JsonConvert.SerializeObject(dtTabletsQuery);
            return result;
        }
        [WebMethod]
        public string QueryPhones()
        {
            conn.Close();
            string allPhonesQuery = @"select * from Phones order by price;";

            sqlCmd.CommandText = allPhonesQuery;
            conn.Open();
            //sqlCmd.ExecuteNonQuery();
            sqlCmd = new SQLiteCommand(conn);
            SQLiteDataAdapter dataAdapter;
            dataAdapter = new SQLiteDataAdapter(sqlCmd);
            int correctCount = 1;
            int x = 0;
            while (x < correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = allPhonesQuery;
                dataAdapter.Fill(dtPhonesQuery);
                sqlCmd.ExecuteNonQuery();
                x++;
            }

            string result = JsonConvert.SerializeObject(dtPhonesQuery);
            return result;
            
        }
       

        [WebMethod]
        public string QueryLaptop2()
        {
            conn.Close();
            string alllaptopQuery = @"select * from Laptops2 order by price;";

            sqlCmd.CommandText = alllaptopQuery;
            conn.Open();
            //sqlCmd.ExecuteNonQuery();
            sqlCmd = new SQLiteCommand(conn);
            SQLiteDataAdapter dataAdapter;
            dataAdapter = new SQLiteDataAdapter(sqlCmd);
            int correctCount = 1;
            int x = 0;
            while (x < correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = alllaptopQuery;
                dataAdapter.Fill(dtLaptop2Query);
                sqlCmd.ExecuteNonQuery();
                x++;
            }


            string result = JsonConvert.SerializeObject(dtLaptop2Query);
            return result;
        }

        [WebMethod]
        public string QueryTablets2()
        {
            conn.Close();
            string allTablets2Query = @"select * from Tablets2 order by price;";

            sqlCmd.CommandText = allTablets2Query;
            conn.Open();
            //sqlCmd.ExecuteNonQuery();
            sqlCmd = new SQLiteCommand(conn);
            SQLiteDataAdapter dataAdapter;
            dataAdapter = new SQLiteDataAdapter(sqlCmd);
            int correctCount = 1;
            int x = 0;
            while (x < correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = allTablets2Query;
                dataAdapter.Fill(dtTablets2Query);
                sqlCmd.ExecuteNonQuery();
                x++;
            }


            string result = JsonConvert.SerializeObject(dtTablets2Query);
            return result;
        }
        [WebMethod]
        public string QueryPhones2()
        {
            conn.Close();
            string allPhones2Query = @"select * from Phones2 order by price;";

            sqlCmd.CommandText = allPhones2Query;
            conn.Open();
            //sqlCmd.ExecuteNonQuery();
            sqlCmd = new SQLiteCommand(conn);
            SQLiteDataAdapter dataAdapter;
            dataAdapter = new SQLiteDataAdapter(sqlCmd);
            int correctCount = 1;
            int x = 0;
            while (x < correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = allPhones2Query;
                dataAdapter.Fill(dtPhones2Query);
                sqlCmd.ExecuteNonQuery();
                x++;
            }


            string result = JsonConvert.SerializeObject(dtPhones2Query);
            return result;
        }

        [WebMethod]
        public string QueryAllProduct2()
        {
            conn.Close();
            string allProducts2Query = @"select t.* from Laptops2 t union all select t.* from Phones2 t union all select t.* from Tablets2 t order by price;";

            sqlCmd.CommandText = allProducts2Query;
            conn.Open();
            //sqlCmd.ExecuteNonQuery();
            sqlCmd = new SQLiteCommand(conn);
            SQLiteDataAdapter dataAdapter;
            dataAdapter = new SQLiteDataAdapter(sqlCmd);
            int correctCount = 1;
            int x = 0;
            while (x < correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = allProducts2Query;
                dataAdapter.Fill(dtAllProduct2Query);
                sqlCmd.ExecuteNonQuery();
                x++;
            }
            string result = JsonConvert.SerializeObject(dtAllProduct2Query);
            return result;
        }

        [WebMethod]
        public string QueryDescription2()
        {
            conn.Close();
            string allDescription2Query = @"select t.Description, t.Price from Laptops2 t union all select t.Description, t.Price from Phones2 t union all select t.Description ,t.Price from Tablets2 t order by price;";

            sqlCmd.CommandText = allDescription2Query;
            conn.Open();
            //sqlCmd.ExecuteNonQuery();
            sqlCmd = new SQLiteCommand(conn);
            SQLiteDataAdapter dataAdapter;
            dataAdapter = new SQLiteDataAdapter(sqlCmd);
            int correctCount = 1;
            int x = 0;
            while (x < correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = allDescription2Query;
                dataAdapter.Fill(dtDescription2Query);
                sqlCmd.ExecuteNonQuery();
                x++;
            }


            string result = JsonConvert.SerializeObject(dtDescription2Query);
            return result;
        }


    }
}
