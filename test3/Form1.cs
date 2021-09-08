using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace test3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadGrid();
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


        MySqlConnection con = new MySqlConnection("datasource=127.0.0.1;port=3306;database=test;username=root;password=;SslMode=none");
        MySqlCommand cmd;
        MySqlDataReader read;



        string sql;
        bool Mode = true;

        public void LoadGrid()

        {
            try 
            {
                sql = "SELECT * FROM siti_web";
                cmd = new MySqlCommand(sql, con);
                con.Open();

                dataGridView1.Rows.Clear();
                read = cmd.ExecuteReader();
                while (read.Read()) 
                {
                    dataGridView1.Rows.Add(read[0], read[1], read[2], read[3]);
                }
                con.Close();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            con.Close();
        }

        public void getId(String id)
        {
            sql = "SELECT * FROM siti_web where id = '" + id +  "'   ";

            cmd = new MySqlCommand(sql,con);
            con.Open();
            read = cmd.ExecuteReader();
            while(read.Read())
            {
                txtName.Text = read[1].ToString();
                txtTipologia.Text = read[2].ToString();
                txtUsername.Text = read[3].ToString();
                txtNote.Text = read[5].ToString();
                txtPassword.Text = Base64Decode(read[4].ToString());

            }
            con.Close();
        
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            txtNote.Clear();
            txtRicerca.Clear();
            LoadGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string tipologia = txtTipologia.Text;
            string username = txtUsername.Text;
            string password = Base64Encode(txtPassword.Text);
          
            string id;
            string note = txtNote.Text;

            if (Mode == true)
            {
                sql = "INSERT INTO siti_web(NomeSito,Tipologia,Username,Password,Note) values(@name,@tipologia,@username,@password,@note)";
                con.Open();
                cmd = new MySqlCommand(sql,con);
                cmd.Parameters.AddWithValue("@name",name);
                cmd.Parameters.AddWithValue("@tipologia",tipologia);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@note", note);
                MessageBox.Show("inserito");
                cmd.ExecuteNonQuery();

                txtName.Clear();
                txtUsername.Clear();
                txtPassword.Clear();
                txtNote.Clear();
                con.Close();
                LoadGrid();

            }
            else 
            {
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                sql = "UPDATE siti_web SET NomeSito=@name, Tipologia=@tipologia, Username=@username, Password=@password, Note=@note WHERE id=@id";
                con.Open();
                cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@tipologia", tipologia);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@note", note);
                cmd.Parameters.AddWithValue("@id", id);
                MessageBox.Show("aggiornato");
                cmd.ExecuteNonQuery();

                txtName.Clear();
                txtUsername.Clear();
                txtPassword.Clear();
                txtNote.Clear();
                Mode = true;
                con.Close();
                LoadGrid();
            }
           
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string id;
            if (e.ColumnIndex == dataGridView1.Columns["Edita"].Index && e.RowIndex >= 0)
            {
                Mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString() ;
                getId(id);
            }
            else if (e.ColumnIndex == dataGridView1.Columns["Cancella"].Index && e.RowIndex >= 0)
            {
                Mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                sql = "DELETE FROM siti_web WHERE id=@id";
                con.Open();
                cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
                MessageBox.Show("cancellato");
                cmd.ExecuteNonQuery();
                con.Close();
                LoadGrid();
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void txtRicerca_TextChanged(object sender, EventArgs e)
        {
            SearchData(txtRicerca.Text);
        }

        public void SearchData(string search)
        {
            string sql = "SELECT * FROM siti_web WHERE NomeSito like '%" + search + "%' ";
            con.Open();
            cmd = new MySqlCommand(sql, con);

            dataGridView1.Rows.Clear();
            read = cmd.ExecuteReader();

            while (read.Read())
            {
                dataGridView1.Rows.Add(read[0], read[1], read[2], read[3]);
            }

            
            con.Close();
        }
    }
}
