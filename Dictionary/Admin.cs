using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dictionary
{
    public partial class Admin : Form
    {
        public static TextBox textBox1 = new TextBox();
        public static TextBox textBox2 = new TextBox();
        public static TextBox textBox3 = new TextBox();
        public static TextBox textBox4 = new TextBox();

        Form1 f1 = new Form1(1);
        AVL tree = new AVL();
        const string fileName = "Dictionary1.dat";
        const string tempfileName = "temp.dat";
        public void init_avl()
        {
            if (File.Exists(fileName))
            {
                using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    while (stream.Position < stream.Length)
                    {
                        DicIndex load = new DicIndex();
                        // Console.WriteLine("start =" + stream.Position);
                        load.index.Insert(stream.Position);

                        Dictionary read = (Dictionary)formatter.Deserialize(stream);
                        load.word = read.word;
                        tree.Add(load);
                    }
                    stream.Close();
                }
            }
        }
        public Admin()
        {
            init_avl();
            InitializeComponent();
        }
        public Admin(int a)
        {

        }
        private void Admin_Load(object sender, EventArgs e)
        {
          SetupTextBox();
            //view_all();
            getIndex();
        }

        public void getIndex()
        {
            Stack<Node> s = new Stack<Node>();
            Queue<long> q = new Queue<long>();
            Node curr = tree.root;

            while (curr != null || s.Count != 0)
            {
                while (curr != null)
                {
                    s.Push(curr);
                    curr = curr.left;
                }
                curr = s.Pop();

                foreach (long n in curr.data.index.ToDataArray())
                {
                    q.Enqueue(n);
                }
                curr = curr.right;
            }
            while (q.Count != 0)
            {
                view_one(q.Dequeue());
            }
        }
        private void populateItems(string w, string t, string p, string m)
        {
            LoadWord[] listword = new LoadWord[1];
            for (int i = 0; i < 1; i++)
            {
                listword[i] = new LoadWord(1);
                listword[i].word = w;
                listword[i].type = p;
                listword[i].pron = t;
                listword[i].meaning = m;
                if (flowLayoutPanel1.Controls.Count < 0)
                {
                    flowLayoutPanel1.Controls.Clear();
                }
                else
                    flowLayoutPanel1.Controls.Add(listword[i]);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            DicIndex s = new DicIndex();
            string str = textBox5.Text;
            s.word = str.ToCharArray();
            Node search = new Node(s);
            if (str != null)
            {
                if (str.Contains("*"))
                {
                    find_similar(str);
                    return;
                }
                Node result = tree.Find(search);
                if (result.data.word != null)
                {
                   
                    foreach (long n in result.data.index.ToDataArray())
                    {
                        view_one(n);
                    }
                }
            }
            if (str.Contains(" "))
            {
                getIndex();
                return;
            }
        }
        void view_one(long Pos)
        {
            if (File.Exists(fileName))
            {
                using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    stream.Position = Pos;
                    Dictionary read = (Dictionary)formatter.Deserialize(stream);
                    string w, t, p, m;
                    w = new string(read.word);
                    t = new string(read.pron);
                    p = new string(read.type);
                    m = new string(read.meaning);
                    populateItems(w, t, p, m);
                    stream.Close();
                }
            }
        }
        public void find_similar(string str)
        {
            int lastCharIndex = str.Length - 1;
            string regex;
            List<Node> matches = new List<Node>();

            // Case: *abc*
            if (str[0] == '*' && str[lastCharIndex] == '*')
            {
                regex = ".*" + str.Substring(1, lastCharIndex - 1) + ".*";
            }
            // Case *abc
            else if (str[0] == '*')
            {
                regex = ".*" + str.Substring(1);
            }
            // Case abc*
            else if (str[lastCharIndex] == '*')
            {
                regex = str.Substring(0, str.Length - 1);
            }
            // Case a*bC
            else
            {
                Console.WriteLine("Invalid wildcard placement");
                regex = null;
            }

            if (!string.IsNullOrEmpty(regex))
            {
                tree.find_matches(regex, matches);
                if (matches.Count > 0)
                {
                    foreach (Node n in matches)
                    {

                        foreach (long p in n.data.index.ToDataArray())
                        {
                            view_one(p);
                        }

                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void SetupTextBox()
        {
            this.Controls.Add(textBox4);
            this.Controls.Add(textBox3);
            this.Controls.Add(textBox2);
            this.Controls.Add(textBox1);

            // 
            // textBox1
            //
           textBox1.Location = new System.Drawing.Point(110, 50);
           textBox1.Name = "textBox1";
           textBox1.Size = new System.Drawing.Size(193, 20);
           textBox1.TabIndex = 4;
           textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox2
            // 
           textBox2.Location = new System.Drawing.Point(110, 106);
           textBox2.Name = "textBox2";
           textBox2.Size = new System.Drawing.Size(193, 20);
           textBox2.TabIndex = 5;
            // 
            // textBox3
            // 
            textBox3.Location = new System.Drawing.Point(110, 165);
            textBox3.Name = "textBox3";
            textBox3.Size = new System.Drawing.Size(193, 20);
            textBox3.TabIndex = 6;
            // 
            // textBox4
            // 
            textBox4.Location = new System.Drawing.Point(110, 226);
            textBox4.Multiline = true;
            textBox4.Name = "textBox4";
            textBox4.Size = new System.Drawing.Size(193, 110);
            textBox4.TabIndex = 7;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string temp;
            // Create A dictionary object and recive input from user
            Dictionary input = new Dictionary();
            temp = textBox1.Text;
            input.word = temp.ToCharArray();
            temp = textBox2.Text;
            input.type = temp.ToCharArray();
            temp = textBox3.Text;
            input.pron = temp.ToCharArray();
            temp = textBox4.Text;
            input.meaning = temp.ToCharArray();

            DicIndex Dindex = new DicIndex();
            Dindex.word = input.word;

            //find the position from the file and assign it to index
            Dindex.index.Insert(word_add(input));
            tree.Add(Dindex);
            flowLayoutPanel1.Controls.Clear();
            getIndex();
        }

        long word_add(Dictionary input)
        {
            long index;

            if (!File.Exists(fileName))
            {
                FileStream fileStream = new FileStream(fileName, FileMode.Create);
                fileStream.Close();
            }
            using (FileStream stream = new FileStream(fileName, FileMode.Append, FileAccess.Write))
            {

                BinaryFormatter formatter = new BinaryFormatter();
                index = stream.Position;
                formatter.Serialize(stream, input);
                stream.Close();
            }

            return index;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DicIndex s = new DicIndex();
            s.word = textBox1.Text.ToCharArray();
            Node search = new Node(s);
            Node result = tree.Find(search);
            if (result.data.word == null)
                MessageBox.Show("Nothing Found", "Dictionary", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                long edit = result.data.index.ToDataArray()[0];
                Dictionary input = new Dictionary();
                input.word = result.data.word;
                input.type = textBox2.Text.ToCharArray();
                input.pron = textBox3.Text.ToCharArray();
                input.meaning = textBox4.Text.ToCharArray();
                word_edit_file(input, edit);
                MessageBox.Show("Inserted", "Dictionary", MessageBoxButtons.OK, MessageBoxIcon.Information);
                flowLayoutPanel1.Controls.Clear();
                getIndex();
            }
        }
        long word_edit_file(Dictionary input, long pos)
        {
            long index;

            if (!File.Exists(fileName))
            {
                FileStream fileStream = new FileStream(fileName, FileMode.Create);
                fileStream.Close();
            }
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Write))
            {

                BinaryFormatter formatter = new BinaryFormatter();
                stream.Position = pos;
                index = stream.Position;
                formatter.Serialize(stream, input);
                stream.Close();
            }

            return index;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DicIndex s = new DicIndex();
            s.word = textBox1.Text.ToCharArray();
            Node search = new Node(s);
            Node result = tree.Find(search);
            if (result.data.word == null)
                MessageBox.Show("Nothing Found", "Dictionary", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                long delete = result.data.index.ToDataArray()[0];
                if (result.data.index.Remove(delete))
                 
                if (result.data.index.IsEmpty())
                        tree.Delete(result);
                Dictionary input = new Dictionary();
                input.word = "null".ToCharArray();
                word_edit_file(input, delete);
                MessageBox.Show("Deleted", "Dictionary", MessageBoxButtons.OK, MessageBoxIcon.Information);
                flowLayoutPanel1.Controls.Clear();
                getIndex();
            }
        }

        public void init_clean()
        {      // ReCreate Temporary File
            FileStream fileStream1 = new FileStream(tempfileName, FileMode.Create);
            fileStream1.Close();
            if (File.Exists(fileName))
            {
                Dictionary ck = new Dictionary();
                ck.word = "null".ToCharArray();
                using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {

                    FileStream stream2 = new FileStream(tempfileName, FileMode.Append, FileAccess.Write);


                    BinaryFormatter formatter = new BinaryFormatter();
                    BinaryFormatter formatter2 = new BinaryFormatter();
                    while (stream.Position < stream.Length)
                    {
                        Dictionary read = (Dictionary)formatter.Deserialize(stream);

                        if (new string(read.word) == new string(ck.word))
                            Console.WriteLine("Cleaning  ---> ");
                        else
                            formatter2.Serialize(stream2, read);
                    }

                    stream2.Close();
                }
                File.Delete(fileName);
                File.Move(tempfileName, fileName);
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            init_clean();
            this.Hide();
            LogIn l1 = new LogIn();
            l1.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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
