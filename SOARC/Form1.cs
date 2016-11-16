using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOARC
{
    public partial class Form1 : Form
    {
        private MySql.Data.MySqlClient.MySqlConnection connection;
        private string myConnectionString;
        Settings sf = new Settings();
        private string[] readText;
        public Form1()
        {
            InitializeComponent();
            tsmLabel.ForeColor = Color.Crimson;
            FileInfo fi = new FileInfo("settings.ini");
            if (fi.Exists)
            {
                try
                {
                    readText = File.ReadAllLines(fi.Name);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);

                }

            }
            else
            {
                sf.Show();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            connection.Close();
            this.Close();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            sf.Show();
        }

        private void tsmiLogin_Click(object sender, EventArgs e)

        {
            
            myConnectionString = System.String.Format(("server={0};uid={1};pwd={2};database=CO;"),readText[0],readText[1],readText[2]);
            try
            {
                connection = new MySql.Data.MySqlClient.MySqlConnection();
                connection.ConnectionString = myConnectionString;
                connection.OpenAsync();
                tsmLabel.ForeColor = Color.Green;


            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
