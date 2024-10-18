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
using System.Data.SqlClient;

namespace EtütProje
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=Metehan\\SQLEXPRESS;Initial Catalog=Etut;Integrated Security=True;Encrypt=False");

        // Value member ile Sqlden çekme
        void derslistesi()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * From TblDersler", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbders.ValueMember = "DersId";
            cmbders.DisplayMember = "DersAd";
            cmbders.DataSource = dt;
            cmbders.Text = "";
        }

        private void cmbders_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlDataAdapter d = new SqlDataAdapter("Select * From TblÖğretmen Where BranşId = " + cmbders.SelectedValue, baglanti);
            DataTable dt = new DataTable();
            d.Fill(dt);
            cmböğretmen.ValueMember = "OğretmenId";
            cmböğretmen.DisplayMember = "AdSoyad";
            cmböğretmen.DataSource = dt;
        }

        void EtutListesi()
        {
            SqlDataAdapter da = new SqlDataAdapter("execute Etut", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            derslistesi();
            EtutListesi();
        }
      // C#'dan Etüt oluşturup Sql veri tabanına kaydetme.
        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand com = new SqlCommand("insert into TblEtüt (DersId,OğretmenId,Tarih,Saat) values (@p1,@p2,@p3,@p4)", baglanti);
            com.Parameters.AddWithValue("@p1", cmbders.SelectedValue);
            com.Parameters.AddWithValue("@p2", cmböğretmen.SelectedValue);
            com.Parameters.AddWithValue("@p3", maskedTextBox1.Text);
            com.Parameters.AddWithValue("@p4", maskedTextBox2.Text);
            com.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Etüt oluşturuldu.","Bilgi",MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand com = new SqlCommand("insert into TblÖğrenci (AdSoyad,Mail,Tel) values (@p1,@p2,@p3)", baglanti);
            com.Parameters.AddWithValue("@p1", textBox1.Text);
            com.Parameters.AddWithValue("@p2", textBox2.Text);
            com.Parameters.AddWithValue("@p3", maskedTextBox3.Text);
            com.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Öğrenci kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Secilen değeri dataGridViewden çekme.
        public void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            txtDers.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            txtÖğretmen.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            mskTarih.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            mskSaat.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
        }
       
        // Etüt alındığı zaman alınmış olarak göstermek için.
        private void button3_Click(object sender, EventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            baglanti.Open();
            SqlCommand com = new SqlCommand("Update TblEtüt set Durum=@a1 Where Id=@a2", baglanti);
            com.Parameters.AddWithValue("@a1", 1);
            com.Parameters.AddWithValue("@a2", dataGridView1.Rows[secilen].Cells[0].Value.ToString());
            com.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Etüt alındı.","Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Information);
            EtutListesi();
        }
    }
}
