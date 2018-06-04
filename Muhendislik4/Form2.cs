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
    public partial class Form2 : Form
    {
        int maxIterSayisi = 100;//Döngünün çalışacağı iter sayısı
        double stepSize = 0.001;

        private double oranHesapla(double[] x, double[] b)
        {
            double oran = 0;
            oran += b[0];
            if (b != null && x != null)
            {
                for (int c = 1; c < b.Length; c++)
                {
                    oran += b[c] * x[c - 1];
                }
            }
            oran = 1 / (1 + Math.Exp( - (b[0] + b[1] * x[1] + b[2] * x[2] + b[3] * x[3] + b[4] * x[4] + b[5] * x[5] +
                        b[6] * x[6] + b[7] * x[7] + b[8] * x[8] + b[9] * x[9] + b[10] * x[10] + b[11] * x[11] + b[12] * x[12] +
                        b[13] * x[13] + b[14] * x[14] + b[15] * x[15])));

            return oran;
        }


        private double[] betaHesapla(double[] b, double[] x, double[][] table)
        {
            double toplam = 0;
            for (int a = 0; a < b.Length; a++)
            {
                double hbeta = 1.0 / (1.0 + Math.Exp(-(b[0] + b[1] * x[1] + b[2] * x[2] + b[3] * x[3] + b[4] * x[4] + b[5] * x[5] +
                       b[6] * x[6] + b[7] * x[7] + b[8] * x[8] + b[9] * x[9] + b[10] * x[10] + b[11] * x[11] + b[12] * x[12] +
                       b[13] * x[13] + b[14] * x[14] + b[15] * x[15])));


                toplam += hbeta -table[a][table[a].Length - 1];
            }
            toplam = toplam / b.Length;

            x[0] = b[0] - stepSize * (toplam);

            for (int c = 0; c < 15; c++)
            {
                for (int a = 0; a < b.Length; a++)
                {
                    double hbeta = 0;
                    hbeta += b[0];
                    for (int d = 1; d < b.Length; d++)
                    {
                        hbeta += b[d] * table[a][d - 1];
                    }
                    hbeta = 1.0 / (1.0 + Math.Exp(-(b[0] + b[1] * x[1] + b[2] * x[2] + b[3] * x[3] + b[4] * x[4] + b[5] * x[5] +
                       b[6] * x[6] + b[7] * x[7] + b[8] * x[8] + b[9] * x[9] + b[10] * x[10] + b[11] * x[11] + b[12] * x[12] +
                       b[13] * x[13] + b[14] * x[14] + b[15] * x[15])));


                    toplam += hbeta - table[a][table[a].Length - 1] * table[a][c];
                }
                toplam = toplam / b.Length;

                x[c + 1] = b[c + 1] - stepSize * (toplam);
            }

            return x;
        }

        public Form2()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-3TVI1GN\\SQLEXPRESS;Initial Catalog=ogrenci;Integrated Security=True");
        int kisi = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            String text = textBox1.Text;
            String[] arkadaslar = null;
            List<String> others = new List<String>();

            con.Open();  //sql baglantisini ac
            try
            {

                SqlCommand cmd = new SqlCommand(   //kullanilacak sql komudunu tanimla
                   "SELECT * FROM ogrnetwork " +
                   "WHERE ogrNo=@ogrNo;", con);  //girilen ogrenciye ait network bilgilerini cek
                cmd.Parameters.Add("@ogrNo", SqlDbType.VarChar).Value = text;  //sql parametre tanimi

                SqlDataReader sdr = cmd.ExecuteReader();   //sql okuyucuyu baslat

                if (sdr.HasRows)    //okunacak satir varsa
                {
                    while (sdr.Read())  //satir okunurken
                    {
                        arkadaslar = sdr.GetValue(2).ToString().Split(',');    //arkadaslari databaseten cek
                    }
                }
                sdr.Close();    //sql okuyucuyu kapat

            }
            catch { }

            try
            {

                SqlCommand cmd = new SqlCommand(   //kullanilacak sql komudunu tanimla
                   "SELECT * FROM ogrnetwork " +
                   "WHERE ogrNo <> @ogrNo;", con);   //girilen numaradan farklı numaraları getir
                cmd.Parameters.Add("@ogrNo", SqlDbType.VarChar).Value = text;  //sql parametre tanimi

                SqlDataReader sdr = cmd.ExecuteReader();   //sql okuyucuyu baslat
                kisi = 1;   //kisi sayisini 1 yap
                if (sdr.HasRows)    //okunacak satir varsa
                {
                    while (sdr.Read())  //satir okunurken
                    {
                       
                        others.Add(sdr.GetValue(1).ToString()); //diger kisiler listesine kisi numarasini ekle
                        kisi++; //kisi sayisini artir
                    }
                }
                sdr.Close();    //sql okuyucunu kapat
                for (int a = others.Count() - 1; a >= 0; a--) //diger kisiler sayisi boyunca
                {
                    foreach (String b in arkadaslar)
                    {
                        if (others[a].Equals(b))
                        {
                            others.RemoveAt(a); //diger kisiler listesinden arkdaslari cikar
                        }
                    }
                }

                listBox1.Items.Clear(); //listbox 3 u temizle
                listBox2.Items.Clear(); //listbox 4 u temizle

                int count = 0;  //sayiyi sifirla
                foreach (String a in arkadaslar)
                {
                    count++;
                    listBox1.Items.Add(count + "- " + a);  //arkadaslari listbox 3 e yazdir
                }
                count = 0;
                foreach (String a in others)
                {
                    count++;
                    listBox2.Items.Add(count + "- " + a); //diger kisileri listbox 4 e yazdir
                }

            }
            catch { }

            List<String[]> Table1 = new List<string[]>();
            List<String[]> Table2 = new List<string[]>();
            try
            {
                foreach (String friend in arkadaslar)
                {

                    SqlCommand cmd = new SqlCommand(   //kullanilacak sql komudunu tanimla
                       "SELECT * FROM ogrprf " +
                       "WHERE ogrNo = @ogrNo;", con);    //her arkadasin profilini databaseten cek
                    cmd.Parameters.Add("@ogrNo", SqlDbType.VarChar).Value = friend;    //parametre tanimi

                    SqlDataReader sdr = cmd.ExecuteReader();   //sql okuyucuyu baslat

                    if (sdr.HasRows)    //okuncak satir varsa
                    {
                        while (sdr.Read())  //satir okunurken
                        {
                            String arkadasogrno = sdr.GetValue(1).ToString();    //databaseteki arkadas no degerini cek
                            String[] nitelikler = sdr.GetValue(2).ToString().Split(',');    //databaseteki arkadas ozelliklerini cek

                            String[] satir = new String[nitelikler.Length + 2];

                            satir[0] = arkadasogrno;  //ogrenme satiri ilk degeri ogrno

                            for (int a = 0; a <nitelikler.Length; a++)
                            {
                                satir[a + 1] = nitelikler[a];    //ogrenme satiri ozellikler
                            }

                            satir[nitelikler.Length + 1] = "1";  //ogrenme satiri arkadaslik durumu
                            Table1.Add(satir);    //ogrenme tablosuna ekle
                        }
                    }
                    sdr.Close();
                }
            }
            catch { }

            try
            {
                for (int a = 0; a < others.Count; a++)
                {

                    SqlCommand cmd = new SqlCommand(   //kullanilacak sql komudunu tanimla
                       "SELECT * FROM ogrprofil " +
                       "WHERE ogrNo = @ogrNo;", con);    //her diger kisinin profilini databaseten cek
                    cmd.Parameters.Add("@ogrNo", SqlDbType.VarChar).Value = others[a]; //parametre tanimi

                    SqlDataReader sdr = cmd.ExecuteReader();   //sql okuyucuyu baslat

                    if (sdr.HasRows)    //okuncak satir varsa
                    {
                        while (sdr.Read())  //satir okunurken
                        {
                            String digerno = sdr.GetValue(1).ToString();    //databaseteki diger no degerini cek
                            String[] nitelikler = sdr.GetValue(2).ToString().Split(',');    //databaseteki diger kisi ozelliklerini cek

                            String[] satir = new String[nitelikler.Length + 2];

                            satir[0] = digerno;    //ogrenme satiri ilk degeri ogrno

                            for (int b = 0; b < nitelikler.Length; b++)
                            {
                                satir[b + 1] = nitelikler[b];    //ogrenme satiri ozellikler
                            }

                            satir[nitelikler.Length + 1] = "0";  //ogrenme satiri arkadaslik durumu
                            if (a < others.Count / 2)
                            {
                                Table1.Add(satir);    //ogrenme tablosuna ekle
                            }
                            else if (a >= others.Count / 2)
                            {
                                Table2.Add(satir);
                            }

                        }
                    }
                    sdr.Close();    //sql bağlantisini kapat
                }
            }
            catch { }
            con.Close();

            double[][] dizi = new double[Table1.Count][];   //ogrenme tablosu buyuklugunde iki boyutlu integer dizi

            for (int a = 0; a <dizi.Length; a++)   //ogrenme tablosu sayısınca
            {
                double[] satir = new double[Table1[a].Length];  //tek boyutlu ogrenme satiri degiskeni
                for (int b = 1; b < Table1[a].Length; b++) //ogrenme tablosundaki her satir buyuklugu sayisinca
                {
                    //line[b] = Int32.Parse(LearningTable[a][b]); 
                    double.TryParse(Table1[a][b], out satir[b]);   //string ozellikleri int'e cevir
                }
                dizi[a] = satir;  //satiri integer tabloya ekle

                String print = "";
                foreach (String s in Table1[a])
                {
                    print += s + " ";
                }

                listBox3.Items.Add(print);
            }

        
            double[] betas = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            double[] newbetas = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            newbetas = betaHesapla(betas, newbetas, dizi);
   

            double[][] sonuctable = new double[Table2.Count][];
            double[] sonuc = new double[Table2.Count];
            for (int a = 0; a < Table2.Count; a++)    //others'ta tabloya eklenmeyen ikinci yari sayisinca
            {
                double[] satir = new double[Table2.Count];  //tek boyutlu ogrenme satiri degiskeni
              
                for (int b = 1; b < Table2[a].Length; b++) //ogrenme tablosundaki her satir buyuklugu sayisinca
                {
                    //line[b] = Int32.Parse(LearningTable[a][b]); 
                    double.TryParse(Table2[a][b], out satir[b - 1]);   //string ozellikleri int'e cevir
                }
                sonuc[a] = oranHesapla(satir, newbetas);
            }

            for (int b = 0; b < maxIterSayisi - 1; b++)
            {
                for (int a = 0; a < Table2.Count - 1; a++)
                {
                    if (sonuc[a] < sonuc[a + 1])
                    {
                        double yedekd = sonuc[a];
                        sonuc[a] = sonuc[a + 1];
                        sonuc[a + 1] = yedekd;

                        string[] yedeks = Table2[a];
                        Table2[a] = Table2[a + 1];
                        Table2[a + 1] = yedeks;
                    }
                }
            }

            con.Open();
            listBox4.Items.Clear();
            for (int a = 0; a < 10; a++)
            {
                SqlCommand cmd = new SqlCommand(   //kullanilacak sql komudunu tanimla
                       "SELECT * FROM ogrbilgi " +
                       "WHERE ogrNo = @ogrNo;", con);    //en yüksek ihtimalli on kisinin profilini databaseten cek
               cmd.Parameters.Add("@ogrNo", SqlDbType.VarChar).Value = Table2[a][0]; ; //parametre tanimi

                SqlDataReader sdr = cmd.ExecuteReader();   //sql okuyucuyu baslat

                if (sdr.HasRows)    //okuncak satir varsa
                {
                    while (sdr.Read())  //satir okunurken
                    {
                        String adsoyad = sdr.GetValue(2).ToString();    //databaseteki diger no degerini cek
                        listBox4.Items.Add(adsoyad);
                    }
                }
                sdr.Close();    //sql bağlantisini kapat
            }

            con.Close(); //sql baglantisini kapat
        }


    }
}
