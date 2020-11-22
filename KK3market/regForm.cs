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
    public partial class regForm : Form
    {
        private OleDbConnection connection = new OleDbConnection();

        bool mouseDown;
        private Point offset;
        public regForm()
        {
            InitializeComponent();
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\Skill\OOP\KK3market\KK3market\bin\Debug\Database1.accdb;Persist Security Info=False;";
        }

        private void Form2_Load(object sender, EventArgs e)
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
        private void mouseDown_Event(object sender, MouseEventArgs e)
        {
            offset.X = e.X;
            offset.Y = e.Y;
            mouseDown = true;
        }

        private void mouseMove_Event(object sender, MouseEventArgs e)
        {
            if (mouseDown == true)
            {
                Point currrentScreenPos = PointToScreen(e.Location);
                Location = new Point(currrentScreenPos.X - offset.X, currrentScreenPos.Y - offset.Y);
            }
        }
        
        private void mouseUp_Event(object sender, MouseEventArgs e)
        {
            mouseDown = false;
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

        private void exitBtn_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            loginForm form1 = new loginForm();

            this.Hide();
            form1.Show();
        }

        private void Register_Click(object sender, EventArgs e)
        {
            OleDbCommand insCommand = new OleDbCommand();
            insCommand.Connection = connection;
            insCommand.CommandText = "INSERT INTO userTab'" ;

            OleDbDataReader reader = insCommand.ExecuteReader();
        }

        private void passFieldReg_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void Register_Click_1(object sender, EventArgs e)
        {
            string hashData = ComputeSha256(passFieldReg.Text + "SnDev");
            string hashData2 = ComputeMD5(passFieldReg.Text + "SnDev");
            string endPass = (hashData + hashData2);


            connection.Open();
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            command.CommandText = "INSERT INTO userTab (realName, userName, passWd) VALUES (@realName, @userName, @passWd)";
            command.Parameters.AddRange(new[]
            {
                new OleDbParameter("@realName", namaLengkap.Text),
                new OleDbParameter("@userName", uernameFieldReg.Text),
                new OleDbParameter("@passWd", endPass),
            });
            command.ExecuteNonQuery();
            MessageBox.Show("Created");
            connection.Close();
        }
    }
}