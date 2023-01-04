using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace FP_Client
{
    public partial class Form1 : Form
    {
        localhost.WebService1 proxy = new localhost.WebService1();

        HttpClient client = new HttpClient();


        public Form1()
        {
            InitializeComponent();

           
        }

        //ASMX
        private void WebServicesSettings()
        {
            client.BaseAddress = new Uri("http://localhost:62790/WebService1.asmx/");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WebServicesSettings();
        }

        //-----------------Removes <> from DOM----------------
        private DataTable stringSplitAllProducts(string allProductsJson)
        {
            string[] json = allProductsJson.Split('>'); //split and store in json
            string[] finalJson = json[2].Split('<');
            DataTable dtAllProductsQuery = JsonConvert.DeserializeObject<DataTable>(finalJson[0]);
            return dtAllProductsQuery;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Text = "";
            if (comboBox1.Text == "All Products")
            {
                
                HttpResponseMessage message = client.GetAsync("QueryAllProduct?").Result;
                string allProductsJson = message.Content.ReadAsStringAsync().Result;
            

                dataGridView1.DataSource = stringSplitAllProducts(allProductsJson);
            }
            else if (comboBox1.Text == "Laptops")
            {
                HttpResponseMessage message = client.GetAsync("QueryLaptops?").Result;
                string allLaptopsJson = message.Content.ReadAsStringAsync().Result;
               

                dataGridView1.DataSource = stringSplitAllProducts(allLaptopsJson);
            }
            else if (comboBox1.Text == "Description")
            {
                HttpResponseMessage message = client.GetAsync("QueryDescription?").Result;
                string allDescJson = message.Content.ReadAsStringAsync().Result;
            

                dataGridView1.DataSource = stringSplitAllProducts(allDescJson);
            }
            else if (comboBox1.Text == "Tablets")
            {
                HttpResponseMessage message = client.GetAsync("QueryTablets?").Result;
                string allTabletsJson = message.Content.ReadAsStringAsync().Result;
      

                dataGridView1.DataSource = stringSplitAllProducts(allTabletsJson);
            }
            else if (comboBox1.Text == "Phones")
            {
                HttpResponseMessage message = client.GetAsync("QueryPhones?").Result;
                string allPhonesJson = message.Content.ReadAsStringAsync().Result;
      

                dataGridView1.DataSource = stringSplitAllProducts(allPhonesJson);
            }


        }

        private void ReportBtn_Click(object sender, EventArgs e)
        {
         
                TextWriter writer = new StreamWriter(@"C:\Users\sarab\source\repos\FP_Client\report.txt");
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++) //rows
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++) //columns
                    {
                        writer.Write("\t" + dataGridView1.Rows[i].Cells[j].Value.ToString());
                    }
                    writer.WriteLine("");
                }
            MessageBox.Show("Report Printed");
            writer.Close();
        
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Text = "";
             if (comboBox2.Text == "All Products")
             {
                 HttpResponseMessage message = client.GetAsync("QueryAllProduct2?").Result;
                 string allProductsJson = message.Content.ReadAsStringAsync().Result;
            

                 dataGridView1.DataSource = stringSplitAllProducts(allProductsJson);
             }
            else if (comboBox2.Text == "Laptops")
            {
                
                HttpResponseMessage message = client.GetAsync("QueryLaptop2?").Result;
                string allLaptopsJson = message.Content.ReadAsStringAsync().Result;
               

                dataGridView1.DataSource = stringSplitAllProducts(allLaptopsJson);
          
            }
            else if (comboBox2.Text == "Tablets")
            {
                HttpResponseMessage message = client.GetAsync("QueryTablets2?").Result;
                string allDescJson = message.Content.ReadAsStringAsync().Result;
           

                dataGridView1.DataSource = stringSplitAllProducts(allDescJson);
            }
            
            else if (comboBox2.Text == "Description")
            {
                HttpResponseMessage message = client.GetAsync("QueryDescription2?").Result;
                string allTabletsJson = message.Content.ReadAsStringAsync().Result;
             

                dataGridView1.DataSource = stringSplitAllProducts(allTabletsJson);
            }
            else if (comboBox2.Text == "Phones")
            {
                HttpResponseMessage message = client.GetAsync("QueryPhones2?").Result;
                string allPhonesJson = message.Content.ReadAsStringAsync().Result;
              

                dataGridView1.DataSource = stringSplitAllProducts(allPhonesJson);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
