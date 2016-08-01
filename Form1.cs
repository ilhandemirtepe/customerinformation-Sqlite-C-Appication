using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Sqlite_Desktop_APP_Rehber
{
    public partial class Form1 : Form
    {
        public class ClassListelemeYap
        {
            public int Myid { get; set; }
            public string MyName { get; set; }
            public string MySurname { get; set; }
            public string MyTelephoneNumber { get; set; }

        }

        string connectstring;
        public int guncellenecekId;
        public Form1()
        {
            InitializeComponent();
            connectstring = @" Data Source=C:\Users\Bey\Downloads\sil\db.Demo.db;Version=3";
            listelemeYap();
        }

       

        private void btn_ekle_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection con = new SQLiteConnection(connectstring))
            {
                try
                {
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.CommandText = @"INSERT INTO rehber(ad,soyad,telefon) VALUES(@AD,@SOYAD,@TELEFON)";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new SQLiteParameter("@AD", txt_Adi.Text));
                    cmd.Parameters.Add(new SQLiteParameter("@SOYAD", txt_Soyadi.Text));
                    cmd.Parameters.Add(new SQLiteParameter("@TELEFON", txt_Telefon.Text));
                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        //   MessageBox.Show("ekleme işlemi başarılı");
                        listelemeYap();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
        public void listelemeYap()
        {
            SQLiteConnection con = new SQLiteConnection(connectstring);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM rehber", con);
            SQLiteDataReader dr = cmd.ExecuteReader();
            List<ClassListelemeYap> list = new List<ClassListelemeYap>();
            while (dr.Read())
            {
                list.Add(new ClassListelemeYap
                {
                    Myid = Convert.ToInt32(dr["id"]),
                    MyName = dr["ad"].ToString(),
                    MySurname = dr["soyad"].ToString(),
                    MyTelephoneNumber = dr["telefon"].ToString()

                });
            }
            dataGridView1.DataSource = list;
        }
        private void btn_guncelle_Click(object sender, EventArgs e)
        {
            if (guncellenecekId > 0)
            {
                SQLiteConnection con = new SQLiteConnection(connectstring);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SQLiteCommand cmd = new SQLiteCommand("UPDATE rehber SET ad=@AD,soyad=@SOYAD,telefon=@TELEFON where id=@guncellelbeni", con);
                cmd.Parameters.AddWithValue("guncellelbeni",guncellenecekId );
                cmd.Parameters.AddWithValue("AD",txt_Adi.Text);
                cmd.Parameters.AddWithValue("SOYAD", txt_Soyadi.Text);
                cmd.Parameters.AddWithValue("TELEFON",txt_Telefon.Text);
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("günceleme işlemi başarılı");
                    listelemeYap();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void btn_sil_Click(object sender, EventArgs e)
        {
            SQLiteConnection con = new SQLiteConnection(connectstring);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();

            }
            SQLiteCommand cmd = new SQLiteCommand("DELETE FROM rehber where id=@silbeni", con);
            cmd.Parameters.AddWithValue("silbeni", Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value));
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("silme işlemi başarılı");
                listelemeYap();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            guncellenecekId = 0;
            guncellenecekId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            txt_Adi.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txt_Soyadi.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            txt_Telefon.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
        }
    }
}
