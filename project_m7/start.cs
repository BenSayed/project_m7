using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project_m7
{
    public partial class start : Form
    {
        public start()
        {

            // Initialize the form and its components
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Report report = new Report();
            report.Show();
            this.Hide();

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

            // Create an instance of the sign_in form and show it
            sign_in Signin = new sign_in();
            Signin.Show();
            this.Hide();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            
            login LoginForm = new login();
            LoginForm.Show();
            this.Hide();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            // Create an instance of the Report form and show it
            Report report = new Report();
            report.ShowDialog();
            
        }

        private void start_Load(object sender, EventArgs e)
        {

        
        }
        
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }
    }
}
