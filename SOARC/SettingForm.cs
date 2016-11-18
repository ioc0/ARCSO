using System;
using System.IO;
using System.Windows.Forms;

namespace SOARC
{
    public partial class Settings : Form
    {
        public string[] credintials;

        public Settings()
        {
            InitializeComponent();
            var fi = new FileInfo("settings.ini");
            if (fi.Exists)
            {
                try
                {
                    var readText = File.ReadAllLines(fi.Name);
                    tbIP.Text = readText[0];
                    tbLogin.Text = readText[1];
                    tbPass.Text = readText[2];
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            credintials = new[] {tbIP.Text, tbLogin.Text, tbPass.Text};


            File.WriteAllLines("settings.ini", credintials);
            Hide();
        }
    }
}