using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.IO;
using System.Data.SqlClient;
//EMEL AKKUYU 2015123054
namespace Muhendislik4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-3TVI1GN\\SQLEXPRESS;Initial Catalog=ogrenci;Integrated Security=True");
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Comma-Seperated Values|*.csv";
            openFileDialog1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "Comma-Seperated Values|*.csv";
            openFileDialog2.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog3.Filter = "Comma-Seperated Values|*.csv";
            openFileDialog3.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            con.Open();  

                using (SqlCommand cmd = new SqlCommand(   
                "DROP TABLE ogrnetwork; " +
                "DROP TABLE ogrprofil; " +
                "DROP TABLE ogrbilgi;", con))  
                {
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = new SqlCommand(    
                "CREATE TABLE ogrnetwork( " +
                "no int," +
                "ogrNo varchar(10) NOT NULL, " +
                "arkadaslar varchar(255) NOT NULL, " +
                ");", con)) 
                {
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = new SqlCommand(   
                "CREATE TABLE ogrbilgi( " +
                "no int," +
                "ogrNo varchar(10) NOT NULL, " +
                "adsoyad varchar(255) NOT NULL, "  +
                "); ", con)) 
                {
                    cmd.ExecuteNonQuery();
                }



                using (SqlCommand cmd = new SqlCommand( 
                "CREATE TABLE ogrprofil( " +
                "no int," +
                "ogrNo varchar(10) NOT NULL, " +
                "nitelikler varchar(255) NOT NULL, " +
                "); ", con)) 
                {
                    cmd.ExecuteNonQuery();
                }

            try
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName);
                listBox1.Items.Clear();
                int count = 0;
                while (!sr.EndOfStream)
                {
                    count++;
                    String satir = sr.ReadLine();
                    satir = satir.ToUpper();
                    String[] splitet = satir.Split(',');
                    String x = "";
                    String arkadaslar = "";
                    bool basla = false;
                    foreach (String a in splitet)
                    {
                        if (a == splitet[0] && !basla)
                        {
                            x = a;
                           basla = true;
                        }
                        else if (a != "" && a != splitet[0])
                        {
                            arkadaslar += a + ",";
                        }
                    }

                    arkadaslar = arkadaslar.Remove(arkadaslar.Length - 1); 
                    listBox1.Items.Add(count + "----->" + x + "----->" + arkadaslar);

                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(
                        "INSERT INTO ogrnetwork (no, ogrNo, arkadaslar)" +
                        "VALUES (@no, @ogrNo, @arkadaslar); ", con))
                        {
                            cmd.Parameters.Add("@no", SqlDbType.Int).Value = count;
                            cmd.Parameters.Add("@ogrNo", SqlDbType.VarChar).Value = x;
                            cmd.Parameters.Add("@arkadaslar", SqlDbType.VarChar).Value = arkadaslar;
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                        }   //eklenecek parametreleri belirle

                    }
                    catch { }
                }

                sr = new StreamReader(openFileDialog2.FileName);
                listBox2.Items.Clear();
                count = 0;
                while (!sr.EndOfStream)
                {
                    count++;
                    String satir = sr.ReadLine();
                    satir = satir.ToUpper();
                    String[] splitet = satir.Split(',');
                    String x = "";
                    String nitelikler = "";
                    bool basla = false;
                    foreach (String a in splitet)
                    {
                        if (a == splitet[0] && !basla)
                        {
                            x = a;
                            basla = true;
                        }
                        else if (a != "" && a != splitet[0])
                        {
                            nitelikler += a + ",";
                        }
                    }
                    nitelikler =nitelikler.Remove(nitelikler.Length - 1); 
                    listBox2.Items.Add(count + "----->" + x + "----->" + nitelikler);   

                    try
                    {

                        using (SqlCommand cmd = new SqlCommand(   
                        "INSERT INTO ogrprofil (no, ogrNo, nitelikler)" +
                        "VALUES (@no, @ogrNo, @nitelikler); ", con)) 
                        {
                            cmd.Parameters.Add("@no", SqlDbType.Int).Value = count;
                            cmd.Parameters.Add("@ogrNo", SqlDbType.VarChar).Value = x;
                            cmd.Parameters.Add("@nitelikler", SqlDbType.VarChar).Value = nitelikler;
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                        }

                    }
                    catch { }
                }

                sr = new StreamReader(openFileDialog3.FileName);
                listBox3.Items.Clear();
                count = 0;
                while (!sr.EndOfStream)
                {
                    count++;
                    String satir = sr.ReadLine();//Satırı okuyup stringe at
                    satir = satir.ToUpper();
                    String[] splitet = satir.Split(',');//Stringi virgüllerden parçala
                    String x = "";
                    String adsoyad = "";
                    bool basla = false;
                    foreach (String a in splitet)
                    {
                        if (a == splitet[0] && !basla)
                        {
                            x = a;
                            basla = true;
                        }
                        else if (a != "" && a != splitet[0])
                        {
                            
                            adsoyad += a + ",";
                        }
                    }
                    adsoyad = adsoyad.Remove(adsoyad.Length - 1); 
                    listBox3.Items.Add(count + "----->" + x + "----->" + adsoyad);

                    try
                    {

                        using (SqlCommand cmd= new SqlCommand(
                        "INSERT INTO ogrbilgi (no, ogrNo, adsoyad)" +
                        "VALUES (@no, @ogrNo, @adsoyad); ", con)) 
                        {
                            cmd.Parameters.Add("@no", SqlDbType.Int).Value = count;
                            cmd.Parameters.Add("@ogrNo", SqlDbType.VarChar).Value = x;
                            cmd.Parameters.Add("@adsoyad", SqlDbType.VarChar).Value = adsoyad;
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                        }

                    }
                    catch { }
                }

                con.Close(); 
            }
            catch { }

        }
        private void button5_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
            this.Hide();
        }
    }
}

