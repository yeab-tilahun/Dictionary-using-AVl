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
    public partial class Form1 : Form
    {
        AVL tree = new AVL();

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
        public Form1()
        {  
            init_avl();
            InitializeComponent();
        }
        public Form1(int a)
        {

        }

        const string fileName = "Dictiona1.dat";
        const string tempfileName = "temp.dat";

        private void Form1_Load(object sender, EventArgs e)
        {
            //populateItems();
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
                listword[i] = new LoadWord(0);
                listword[i].word = w;
                listword[i].type = t;
                listword[i].pron = p;
                listword[i].meaning = m;
                if (flowLayoutPanel1.Controls.Count < 0)
                {
                    flowLayoutPanel1.Controls.Clear();
                }
                else
                    flowLayoutPanel1.Controls.Add(listword[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            DicIndex s = new DicIndex();
            string str = textBox1.Text;
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
                        flowLayoutPanel1.Controls.Clear();
                        view_one(n);
                    }
                }
            }
        }

     public  void find_similar(string str)
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

     public   void view_one(long Pos)
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

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            init_clean();
            Application.Exit();
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
                System.IO.File.Delete(fileName);
                System.IO.File.Move(tempfileName, fileName);
            }
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

