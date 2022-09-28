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
    public partial class LoadWord : UserControl
    {
        public LoadWord(int a)
        {
            InitializeComponent();
            if (a == 0)
                pictureBox2.Visible = false;
        }

        private string _word;
        [Category("Custom Props")]
        public string word
        {
            get { return _word; }
            set { _word = value; label1.Text = value; }
        }

        private string _type;
        [Category("Custom Props")]
        public string type
        {
            get { return _type; }
            set { _type = value; label3.Text = value; }
        }


        private string _pron;
        [Category("Custom Props")]
        public string pron
        {
            get { return _pron; }
            set { _pron = value; label2.Text = value; }
        }

        private string _meaning;
        [Category("Custom Props")]
        public string meaning
        {
            get { return _meaning; }
            set { _meaning = value; label4.Text = value; }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
           // Admin a1 = new Admin(1);
            Admin.textBox1.Text = word;
            Admin.textBox2.Text = pron;
            Admin.textBox3.Text = type;
            Admin.textBox4.Text = meaning;
        }
    }
}
