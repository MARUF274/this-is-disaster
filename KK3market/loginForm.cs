using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Security.Cryptography;

namespace KK3market
{
    public partial class loginForm : Form
    {
        private OleDbConnection connection = new OleDbConnection();



        bool mouseDown;
        private Point offset;
        public loginForm()
        {
            InitializeComponent();
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\Skill\OOP\KK3market\KK3market\bin\Debug\Database1.accdb;Persist Security Info=False;";
        }
        static string ComputeSha256(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        static string ComputeMD5(string rawData)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(rawData));
            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
     
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void mouseDown_Event(object sender, MouseEventArgs e)
        {
            offset.X = e.X;
            offset.Y = e.Y;
            mouseDown = true;
        }

        private void mouseMove_Event(object sender, MouseEventArgs e)
        {
            if(mouseDown == true)
            {
                Point currrentScreenPos = PointToScreen(e.Location);
                Location = new Point(currrentScreenPos.X - offset.X, currrentScreenPos.Y - offset.Y);
            }
        }

        private void mouseUp_Event(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                connection.Close();
            }
            catch (Exception a)
            {
                MessageBox.Show("Error " + a);
            }
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (uernameField.Text == "" || passField.Text == "")
            {
                MessageBox.Show("All Field Must Filled");
            }
            else
            {
                
                string hashData = ComputeSha256(passField.Text + "SnDev");
                string hashData2 = ComputeMD5(passField.Text + "SnDev");
                string endPass = (hashData + hashData2);
                
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM userTab WHERE userName='"+uernameField.Text+"'AND passWd='"+ endPass +"'";

                OleDbDataReader reader = command.ExecuteReader();
                int count = 0;
                while (reader.Read())
                {
                    count = count+1;
                }if(count == 1)
                {
                   
                    mainForm mForm = new mainForm();
                    this.Hide();
                    mForm.Show();
                }
                else
                {
                    if(count > 1)
                    {
                        MessageBox.Show("Duplicate Username");
                    }
                    else
                    {
                        MessageBox.Show("Incorrect");
                    }
                }


                connection.Close();
            }
        }
        private void linkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            regForm form2 = new regForm();

            this.Hide();
            form2.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Hide();
            admLogin alog = new admLogin();
            alog.Show();
        }
    }
}
