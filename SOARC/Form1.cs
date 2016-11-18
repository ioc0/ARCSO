using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SOARC
{
    public partial class Form1 : Form
    {
        private string cityotp, citypol,finCityO,finCityPol,finDateOtp,finDatePol,finSurname;
        private MySqlConnection connection;
        private string[] data = new string[50];
        private string myConnectionString;
        private readonly string[] readText;
        private readonly Settings sf = new Settings();

        private string Template()
        {
            return "Накладная №:"+" "+textBox1.Text+". \n"
                +"Город отправления: "+finCityO+". \n" 
                +"Город получения: "+finCityPol+". \n"
                +"Дата отправления: "+finDateOtp+" \n"
                +"Дата получения: " + finDatePol+" \n" 
                +"Фамилия получателя: " + finSurname+" \n" 
                ;
        }

        public Form1()
        {
            InitializeComponent();
            tsmLabel.ForeColor = Color.Crimson;
            var fi = new FileInfo("settings.ini");
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
            sqlConnection();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connection.Close();
            Close();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sf.Show();
        }

        private void tsmiLogin_Click(object sender, EventArgs e)

        {
            sqlConnection();
        }

        private void sqlConnection()
        {
            myConnectionString = string.Format(("server={0};uid={1};pwd={2};database=CO;"), readText[0], readText[1],
                readText[2]);
            try
            {
                connection = new MySqlConnection();
                connection.ConnectionString = myConnectionString;
                connection.OpenAsync();
                tsmLabel.ForeColor = Color.Green;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            getData();
            getCityPol();
            getCityOtp();
            this.Cursor = Cursors.Arrow;
        }

        private async void getCityPol()
        {
            var sql = string.Format(("SELECT cityp FROM CO.citypol WHERE id={0}"), citypol);
            var cmd = new MySqlCommand(sql, connection);
            try
            {
                var reader1 = await cmd.ExecuteReaderAsync() as MySqlDataReader;
                while (reader1.Read())
                {
                    tbCitiPol.Text = reader1["cityp"].ToString();
                }
                finCityPol = tbCitiPol.Text;
                reader1.Close();
            }
            catch (Exception)
            {
            }
        }

        private async void getCityOtp()
        {
            var sql = string.Format(("SELECT cityp FROM CO.citypol WHERE id={0}"), cityotp);
            var cmd = new MySqlCommand(sql, connection);
            try
            {
                var reader2 = await cmd.ExecuteReaderAsync() as MySqlDataReader;
                while (reader2.Read())
                {
                    tbCitiOtp.Text = reader2["cityp"].ToString();
                }
                finCityO = tbCitiOtp.Text;
                reader2.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Нет записи о накладной");
            }
        }

        private void keyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar)) return;
            e.Handled = true;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("DIMEX ARC Viewer. \n Winter Mute" +
                            "\n ioc0kernel@gmail.com");
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = textBox1.Text;
            sfd.DefaultExt = ".txt";
            sfd.AddExtension = true;
            sfd.OverwritePrompt = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string filename = sfd.FileName;
                try
                {
                    File.WriteAllText(filename, Template());
                }
                catch (Exception)
                {

                    MessageBox.Show("Не могу записать файл, проверьте путь");
                }
                
            }
        }

        private async void getData()
        {
            var sql = string.Format(("SELECT * FROM CO.nakl WHERE nomer={0}"), textBox1.Text);
            var cmd = new MySqlCommand(sql, connection);

            var reader = await cmd.ExecuteReaderAsync() as MySqlDataReader;


            while (reader.Read())
            {
                //textBox2.Text = reader["idd"].ToString();
                tbDataOtp.Text = reader["data_otp"].ToString();
                tbDataPol.Text = reader["data_pol"].ToString();
                cityotp = reader["city_otp"].ToString();
                citypol = reader["city_pol"].ToString();
                tbSurname.Text = reader["famile_pol"].ToString();
            }
            finCityO = cityotp;
            finCityPol = citypol;
            finDateOtp = tbDataOtp.Text;
            finDatePol = tbDataPol.Text;
            finSurname = tbSurname.Text;


            

            reader.Close();

            if (connection.State.Equals(false))
            {
                tsmLabel.ForeColor = Color.Crimson;
            }
        }
    }
}