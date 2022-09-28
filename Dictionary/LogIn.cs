using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dictionary
{
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.Equals("Admin"))
            {
                this.Hide();
                Admin a1 = new Admin();
                a1.Show();
            }
            if (comboBox1.SelectedItem.Equals("User"))
            {
                this.Hide();
                Form1 f1 = new Form1();
                f1.Show();
            }
        }

        private void LogIn_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedItem = "User";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public bool MouseDown;
        public Point LastLocation;

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDown = true;
            LastLocation = e.Location;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            MouseDown = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseDown)
            {
                this.Location = new Point((this.Location.X - LastLocation.X) + e.X, (this.Location.Y - LastLocation.Y) + e.Y);
                this.Update();
            }
        }
    }
}
