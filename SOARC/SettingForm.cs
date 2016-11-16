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
    public partial class Settings : Form
    {
        
        public string[] credintials;

        public Settings()
        {
            InitializeComponent();
            FileInfo fi = new FileInfo("settings.ini");
            if (fi.Exists)
            {
                try
                {
                    string[] readText = File.ReadAllLines(fi.Name);
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
            

            File.WriteAllLines("settings.ini",credintials);
            this.Hide();

        }
    }
}
