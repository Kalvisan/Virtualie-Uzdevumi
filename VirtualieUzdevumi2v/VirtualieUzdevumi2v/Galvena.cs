using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using MySql.Data.MySqlClient;

namespace VirtualieUzdevumi2v
{
    public partial class Galvena : MetroForm
    {
        List<string> id = new List<string>();
        List<string> skolotajs = new List<string>();
        List<string> atbildes = new List<string>();
        List<string> jautajumi = new List<string>();
        List<string> nosaukums = new List<string>();
        List<string> prieksmets = new List<string>();
        List<string> pareiza_atbilde = new List<string>();
        List<string> aktivs = new List<string>();
        List<string> laiks = new List<string>();
        List<string> pilditie = new List<string>();

        public Galvena()
        {
            InitializeComponent();
        }

        public void ClearLists()
        {
            id.Clear();
            skolotajs.Clear();
            atbildes.Clear();
            jautajumi.Clear();
            nosaukums.Clear();
            prieksmets.Clear();
            pareiza_atbilde.Clear();
            aktivs.Clear();
            laiks.Clear();
            pilditie.Clear();
        }

        private void Galvena_Load(object sender, EventArgs e)
        {
            LB_Iziet.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            ClearLists();

            LB_Dati.Text = Login.name;
            pictureBox1.ImageLocation = Login.attels;

            if (Login.ID == 1)
            {
                LB_Punkti.Visible = false;
                metroTabControl1.Left = 10;
                metroTabControl2.Left = 780;
                metroTabControl2.Visible = false;
                metroTabControl2.Enabled = false;
            }
            if (Login.ID == 2)
            {
                metroTabControl2.Left = 10;
                metroTabControl1.Left = 780;
                metroTabControl1.Visible = false;
                metroTabControl1.Enabled = false;
            }

            if (Login.ID == 2)
            {
                int uzdevumi = 0;
                MySqlConnection connection = new MySqlConnection(Login.connectionString);
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Uzdevumi", connection);
                MySqlDataReader read;
                read = cmd.ExecuteReader();
                while (read.Read())
                {
                    if (Login.klase == read.GetString("klasei") && read.GetString("aktivs") == "1" && !read.GetString("izpildija").Contains(Login.pasts))
                    {
                        id.Add(read.GetString("uzd_id"));
                        skolotajs.Add(read.GetString("skolotajs"));
                        atbildes.Add(read.GetString("atbildes"));
                        jautajumi.Add(read.GetString("jautajumi"));
                        prieksmets.Add(read.GetString("prieksmets"));
                        pareiza_atbilde.Add(read.GetString("p_atbilde"));
                        nosaukums.Add(read.GetString("Nosaukums"));
                        aktivs.Add(read.GetString("aktivs"));
                        laiks.Add(read.GetString("laiks"));
                        pilditie.Add(read.GetString("izpildija"));
                        uzdevumi++;
                    }
                }
                connection.Close();
                if (uzdevumi > 0)
                {
                    int j = uzdevumi;
                    for (int i = 0; i < uzdevumi; i++)
                    {
                        j--;
                        Panel p = new Panel();
                        Label l1 = new Label();
                        Label l2 = new Label();
                        Label l3 = new Label();
                        PictureBox pb = new PictureBox();

                        p.Tag = i.ToString();
                        l1.Tag = i.ToString();
                        l2.Tag = i.ToString();
                        l3.Tag = i.ToString();
                        pb.Tag = i.ToString();

                        l1.Font = new Font("Calibri Light", 10);
                        l2.Font = new Font("Calibri Light", 13);

                        p.Width = metroPanel3.Width - 25;
                        p.Height = 63;
                        pb.Width = 51;
                        pb.Height = 51;

                        p.Location = new Point(metroPanel3.Left, metroPanel3.Top + (j * 65) + 2);
                        l1.Location = new Point(p.Left + 60, p.Top + 10);
                        l2.Location = new Point(p.Left + 60, p.Top + 32);
                        l3.Location = new Point(p.Left + metroPanel3.Width - 25 - 120, p.Top + 40);
                        pb.Location = new Point(p.Left + 5, p.Top + 5);

                        p.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                        pb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                        pb.SizeMode = PictureBoxSizeMode.StretchImage;
                        pb.Image = Properties.Resources.book;

                        l1.AutoSize = true;
                        l2.AutoSize = true;
                        l3.AutoSize = true;

                        l1.Text = skolotajs[i];
                        l2.Text = nosaukums[i];
                        l3.Text = "Izpildīt līdz: " + laiks[i];

                        p.BackColor = Color.FromArgb(158, 204, 255);
                        l1.BackColor = Color.FromArgb(158, 204, 255);
                        l2.BackColor = Color.FromArgb(158, 204, 255);
                        l3.BackColor = Color.FromArgb(158, 204, 255);
                        pb.BackColor = Color.White;
                        p.Cursor = Cursors.Hand;
                        l1.Cursor = Cursors.Hand;
                        l2.Cursor = Cursors.Hand;
                        l3.Cursor = Cursors.Hand;
                        pb.Cursor = Cursors.Hand;
                        p.Click += new EventHandler(p_Click);
                        l1.Click += new EventHandler(p_Click);
                        l2.Click += new EventHandler(p_Click);
                        l3.Click += new EventHandler(p_Click);
                        pb.Click += new EventHandler(p_Click);

                        metroPanel3.Controls.Add(l1);
                        metroPanel3.Controls.Add(l2);
                        metroPanel3.Controls.Add(l3);
                        metroPanel3.Controls.Add(pb);
                        metroPanel3.Controls.Add(p);
                    }
                }
                else if (uzdevumi == 0)
                {
                    Label l1 = new Label();
                    l1.AutoSize = true;
                    l1.Text = "Vel nav pievienots neviens uzdevums!";
                    l1.Location = new Point(metroPanel3.Left + 5, metroPanel3.Top + 10);
                    l1.BackColor = Color.Transparent;
                    metroPanel3.Controls.Add(l1);
                }
            }

            if (Login.ID == 1)
            {
                string[] klases = Properties.Resources.Klases.Split(';');
                foreach (string s in klases)
                    metroComboBox1.Items.Add(s);
                string[] priekšmeti = Properties.Resources.prieksmeti.Split(';');
                foreach (string s in priekšmeti)
                    metroComboBox2.Items.Add(s);

                int uzdevumi = 0;

                string email = Login.pasts;

                MySqlConnection connection = new MySqlConnection(Login.connectionString);
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Uzdevumi", connection);
                MySqlDataReader read;
                read = cmd.ExecuteReader();
                while (read.Read())
                {
                    if (email == read.GetString("epasts"))
                    {
                        id.Add(read.GetString("uzd_id"));
                        skolotajs.Add(read.GetString("skolotajs"));
                        atbildes.Add(read.GetString("atbildes"));
                        jautajumi.Add(read.GetString("jautajumi"));
                        prieksmets.Add(read.GetString("prieksmets"));
                        pareiza_atbilde.Add(read.GetString("p_atbilde"));
                        nosaukums.Add(read.GetString("Nosaukums"));
                        aktivs.Add(read.GetString("aktivs"));
                        laiks.Add(read.GetString("laiks"));
                        uzdevumi++;
                    }
                }
                connection.Close();
                int j = uzdevumi;
                for (int i = 0; i < uzdevumi; i++)
                {
                    j--;
                    Panel p = new Panel();
                    Label l1 = new Label();
                    Label l2 = new Label();
                    Label ln = new Label();

                    PictureBox p1 = new PictureBox();

                    l1.Width = 250;
                    l1.Height = 25;
                    l2.Width = 250;
                    l2.Height = 25;
                    p1.Width = 55;
                    p1.Height = 55;
                    p.Height = 64;
                    p.Width = 664;
                    ln.Height = 25;
                    ln.Width = 145;

                    l1.Font = new Font("Calibri Light", 13);
                    l2.Font = new Font("Calibri Light", 11);

                    l1.Location = new Point(TB1.Left + 65, TB1.Top + (j * 65) + 5);
                    l2.Location = new Point(TB1.Left + 65, TB1.Top + (j * 65) + 35);
                    p1.Location = new Point(TB1.Left + 5, TB1.Top + (j * 65) + 5);
                    p.Location = new Point(TB1.Left + 1, TB1.Top + (j * 65) + 1);
                    ln.Location = new Point(TB1.Width - 170, TB1.Top + (j * 65) + 40);

                    l1.Text = nosaukums[i];
                    l2.Text = jautajumi[i];
                    ln.Text = "Izpildes laiks: " + laiks[i];

                    l1.BringToFront();
                    ln.BringToFront();
                    l2.BringToFront();
                    p1.BringToFront();
                    p.BringToFront();

                    p.Click += new EventHandler(p_Click);
                    l1.Click += new EventHandler(p_Click);
                    l2.Click += new EventHandler(p_Click);
                    ln.Click += new EventHandler(p_Click);
                    p1.Click += new EventHandler(p_Click);

                    p.Tag = i.ToString();
                    l1.Tag = i.ToString();
                    l2.Tag = i.ToString();
                    ln.Tag = i.ToString();
                    p1.Tag = i.ToString();

                    p.Cursor = Cursors.Hand;
                    l1.Cursor = Cursors.Hand;
                    l2.Cursor = Cursors.Hand;
                    ln.Cursor = Cursors.Hand;
                    p1.Cursor = Cursors.Hand;


                    //p.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                    if (aktivs[i] == "1")
                    {
                        p.BackColor = Color.FromArgb(158, 204, 255);
                        l1.BackColor = Color.FromArgb(158, 204, 255);
                        ln.BackColor = Color.FromArgb(158, 204, 255);
                        l2.BackColor = Color.FromArgb(158, 204, 255);
                        p1.BackColor = Color.FromArgb(158, 204, 255);
                    }
                    else
                    {
                        p.BackColor = Color.LightGray;
                        ln.BackColor = Color.LightGray;
                        l1.BackColor = Color.LightGray;
                        l2.BackColor = Color.LightGray;
                        p1.BackColor = Color.LightGray;
                    }

                    p1.SizeMode = PictureBoxSizeMode.StretchImage;
                    p1.Image = Properties.Resources.book;

                    TB1.Controls.Add(l1);
                    TB1.Controls.Add(ln);
                    TB1.Controls.Add(l2);
                    TB1.Controls.Add(p1);
                    TB1.Controls.Add(p);

                }

                if (uzdevumi == 0)
                {
                    Label l1 = new Label();
                    l1.AutoSize = true;
                    l1.Text = "Vel nav pievienots neviens uzdevums!\n\nNospiediet uz 'Uzdot Uzdevumu', lai pievienotu skolenam uzdevumus.";
                    l1.Location = new Point(TB1.Left + 5, TB1.Top + 10);
                    l1.BackColor = Color.Transparent;
                    TB1.Controls.Add(l1);
                }
            }
        }

        private void Galvena_FormClosing(object sender, FormClosingEventArgs e) { Application.Exit(); }
        private void LB_Iziet_Click(object sender, EventArgs e) { Close(); Login l = new Login(); l.Visible = true; }

        private void p_Click(object sender, EventArgs e)
        {
            metroRadioButton5.Visible = false;
            metroRadioButton6.Visible = false;
            metroRadioButton7.Visible = false;
            metroRadioButton8.Visible = false;
            metroLabel1.Visible = false;
            button1.Visible = false;

            Control c = sender as Control;
            uzd_id = Convert.ToInt32(c.Tag.ToString());
            metroPanel4.Enabled = true;
            //metroPanel4.Left = 14;
            uzdev = Convert.ToInt32(id[uzd_id]);
            ctimer = metroPanel4;
            to = 14;
            timer1.Enabled = true;
            LBU_Skolotajs.Text = skolotajs[uzd_id];
            LBU_1.Text = nosaukums[uzd_id];
            string jautajum = jautajumi[uzd_id];
            jautajum.Replace('\n', '\r');
            LBU_3.Text = jautajumi[uzd_id] + "\r\n\r\n";
            string[] atb = atbildes[uzd_id].Split(';');
            int kurs = 0;
            foreach (string s in atb)
            {
                char b = (char)(kurs + 65);
                LBU_3.Text += b.ToString() + ") " + s + "\r\n";
                kurs++;
            }
            kurs--;

            if (Login.ID == 2)
            {
                metroLabel1.Visible = true;
                button1.Visible = true;
                if (kurs >= 0) metroRadioButton5.Visible = true;
                if (kurs >= 1) metroRadioButton6.Visible = true;
                if (kurs >= 2) metroRadioButton7.Visible = true;
                if (kurs >= 3) metroRadioButton8.Visible = true;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            metroPanel4.Enabled = false;
            metroPanel4.Left = 780;
        }


        Control ctimer = new Control();
        int to, yes = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if(yes == 0)
                if (ctimer.Left >= to)
                    ctimer.Left -= 20;
                else timer1.Enabled = false;

            if (yes == 1)
                if (ctimer.Left <= to)
                    ctimer.Left += 20;
                else timer1.Enabled = false;
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            ClearLists();
            if (Login.ID == 1)
            {
                TB1.Controls.Clear();
                string[] klases = Properties.Resources.Klases.Split(';');
                foreach (string s in klases)
                    metroComboBox1.Items.Add(s);
                string[] priekšmeti = Properties.Resources.prieksmeti.Split(';');
                foreach (string s in priekšmeti)
                    metroComboBox2.Items.Add(s);

                int uzdevumi = 0;

                string email = Login.pasts;

                MySqlConnection connection = new MySqlConnection(Login.connectionString);
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Uzdevumi", connection);
                MySqlDataReader read;
                read = cmd.ExecuteReader();
                while (read.Read())
                {
                    if (email == read.GetString("epasts"))
                    {
                        id.Add(read.GetString("uzd_id"));
                        skolotajs.Add(read.GetString("skolotajs"));
                        atbildes.Add(read.GetString("atbildes"));
                        jautajumi.Add(read.GetString("jautajumi"));
                        prieksmets.Add(read.GetString("prieksmets"));
                        pareiza_atbilde.Add(read.GetString("p_atbilde"));
                        nosaukums.Add(read.GetString("Nosaukums"));
                        aktivs.Add(read.GetString("aktivs"));
                        laiks.Add(read.GetString("laiks"));
                        uzdevumi++;
                    }
                }
                connection.Close();
                int j = uzdevumi;
                for (int i = 0; i < uzdevumi; i++)
                {
                    j--;
                    Panel p = new Panel();
                    Label l1 = new Label();
                    Label l2 = new Label();
                    Label ln = new Label();

                    PictureBox p1 = new PictureBox();

                    l1.Width = 250;
                    l1.Height = 25;
                    l2.Width = 250;
                    l2.Height = 25;
                    p1.Width = 55;
                    p1.Height = 55;
                    p.Height = 64;
                    p.Width = 664;
                    ln.Height = 25;
                    ln.Width = 145;

                    l1.Font = new Font("Calibri Light", 13);
                    l2.Font = new Font("Calibri Light", 11);

                    l1.Location = new Point(TB1.Left + 65, TB1.Top + (j * 65) + 5);
                    l2.Location = new Point(TB1.Left + 65, TB1.Top + (j * 65) + 35);
                    p1.Location = new Point(TB1.Left + 5, TB1.Top + (j * 65) + 5);
                    p.Location = new Point(TB1.Left + 1, TB1.Top + (j * 65) + 1);
                    ln.Location = new Point(TB1.Width - 170, TB1.Top + (j * 65) + 40);

                    l1.Text = nosaukums[i];
                    l2.Text = jautajumi[i];
                    ln.Text = "Izpildes laiks: " + laiks[i];

                    l1.BringToFront();
                    ln.BringToFront();
                    l2.BringToFront();
                    p1.BringToFront();
                    p.BringToFront();

                    p.Click += new EventHandler(p_Click);
                    l1.Click += new EventHandler(p_Click);
                    l2.Click += new EventHandler(p_Click);
                    ln.Click += new EventHandler(p_Click);
                    p1.Click += new EventHandler(p_Click);

                    p.Tag = i.ToString();
                    l1.Tag = i.ToString();
                    l2.Tag = i.ToString();
                    ln.Tag = i.ToString();
                    p1.Tag = i.ToString();

                    p.Cursor = Cursors.Hand;
                    l1.Cursor = Cursors.Hand;
                    l2.Cursor = Cursors.Hand;
                    ln.Cursor = Cursors.Hand;
                    p1.Cursor = Cursors.Hand;


                    //p.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                    if (aktivs[i] == "1")
                    {
                        p.BackColor = Color.FromArgb(158, 204, 255);
                        l1.BackColor = Color.FromArgb(158, 204, 255);
                        ln.BackColor = Color.FromArgb(158, 204, 255);
                        l2.BackColor = Color.FromArgb(158, 204, 255);
                        p1.BackColor = Color.FromArgb(158, 204, 255);
                    }
                    else
                    {
                        p.BackColor = Color.LightGray;
                        ln.BackColor = Color.LightGray;
                        l1.BackColor = Color.LightGray;
                        l2.BackColor = Color.LightGray;
                        p1.BackColor = Color.LightGray;
                    }

                    p1.SizeMode = PictureBoxSizeMode.StretchImage;
                    p1.Image = Properties.Resources.book;

                    TB1.Controls.Add(l1);
                    TB1.Controls.Add(ln);
                    TB1.Controls.Add(l2);
                    TB1.Controls.Add(p1);
                    TB1.Controls.Add(p);

                }

                if (uzdevumi == 0)
                {
                    Label l1 = new Label();
                    l1.AutoSize = true;
                    l1.Text = "Vel nav pievienots neviens uzdevums!\n\nNospiediet uz 'Uzdot Uzdevumu', lai pievienotu skolenam uzdevumus.";
                    l1.Location = new Point(TB1.Left + 5, TB1.Top + 10);
                    l1.BackColor = Color.Transparent;
                    TB1.Controls.Add(l1);
                }
            }

            if (Login.ID == 2)
            {
                metroPanel3.Controls.Clear();
                int uzdevumi = 0;
                MySqlConnection connection = new MySqlConnection(Login.connectionString);
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Uzdevumi", connection);
                MySqlDataReader read;
                read = cmd.ExecuteReader();
                while (read.Read())
                {
                    if (Login.klase == read.GetString("klasei") && read.GetString("aktivs") == "1" && !read.GetString("izpildija").Contains(Login.pasts)) 
                    {
                        id.Add(read.GetString("uzd_id"));
                        skolotajs.Add(read.GetString("skolotajs"));
                        atbildes.Add(read.GetString("atbildes"));
                        jautajumi.Add(read.GetString("jautajumi"));
                        prieksmets.Add(read.GetString("prieksmets"));
                        pareiza_atbilde.Add(read.GetString("p_atbilde"));
                        nosaukums.Add(read.GetString("Nosaukums"));
                        aktivs.Add(read.GetString("aktivs"));
                        laiks.Add(read.GetString("laiks"));
                        pilditie.Add(read.GetString("izpildija"));
                        uzdevumi++;
                    }
                }
                connection.Close();
                if (uzdevumi > 0)
                {
                    int j = uzdevumi;
                    for (int i = 0; i < uzdevumi; i++)
                    {
                        j--;
                        Panel p = new Panel();
                        Label l1 = new Label();
                        Label l2 = new Label();
                        Label l3 = new Label();
                        PictureBox pb = new PictureBox();

                        p.Tag = i.ToString();
                        l1.Tag = i.ToString();
                        l2.Tag = i.ToString();
                        l3.Tag = i.ToString();
                        pb.Tag = i.ToString();

                        l1.Font = new Font("Calibri Light", 10);
                        l2.Font = new Font("Calibri Light", 13);

                        p.Width = metroPanel3.Width - 25;
                        p.Height = 63;
                        pb.Width = 51;
                        pb.Height = 51;

                        p.Location = new Point(metroPanel3.Left, metroPanel3.Top + (j * 65) + 2);
                        l1.Location = new Point(p.Left + 60, p.Top + 10);
                        l2.Location = new Point(p.Left + 60, p.Top + 32);
                        l3.Location = new Point(p.Left + metroPanel3.Width - 25 - 120, p.Top + 40);
                        pb.Location = new Point(p.Left + 5, p.Top + 5);

                        p.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                        pb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                        pb.SizeMode = PictureBoxSizeMode.StretchImage;
                        pb.Image = Properties.Resources.book;

                        l1.AutoSize = true;
                        l2.AutoSize = true;
                        l3.AutoSize = true;

                        l1.Text = skolotajs[i];
                        l2.Text = nosaukums[i];
                        l3.Text = "Izpildīt līdz: " + laiks[i];

                        p.BackColor = Color.FromArgb(158, 204, 255);
                        l1.BackColor = Color.FromArgb(158, 204, 255);
                        l2.BackColor = Color.FromArgb(158, 204, 255);
                        l3.BackColor = Color.FromArgb(158, 204, 255);
                        pb.BackColor = Color.White;
                        p.Cursor = Cursors.Hand;
                        l1.Cursor = Cursors.Hand;
                        l2.Cursor = Cursors.Hand;
                        l3.Cursor = Cursors.Hand;
                        pb.Cursor = Cursors.Hand;
                        p.Click += new EventHandler(p_Click);
                        l1.Click += new EventHandler(p_Click);
                        l2.Click += new EventHandler(p_Click);
                        l3.Click += new EventHandler(p_Click);
                        pb.Click += new EventHandler(p_Click);

                        metroPanel3.Controls.Add(l1);
                        metroPanel3.Controls.Add(l2);
                        metroPanel3.Controls.Add(l3);
                        metroPanel3.Controls.Add(pb);
                        metroPanel3.Controls.Add(p);
                    }
                }
                else if (uzdevumi == 0)
                {
                    Label l1 = new Label();
                    l1.AutoSize = true;
                    l1.Text = "Vel nav pievienots neviens uzdevums!";
                    l1.Location = new Point(metroPanel3.Left + 5, metroPanel3.Top + 10);
                    l1.BackColor = Color.Transparent;
                    metroPanel3.Controls.Add(l1);
                }
            }
        }
        int uzdev;
        int uzd_id;
        private void button1_Click(object sender, EventArgs e)
        {
            string atbildetais = "1";
            if (metroRadioButton5.Checked) atbildetais = "1";
            if (metroRadioButton6.Checked) atbildetais = "2";
            if (metroRadioButton7.Checked) atbildetais = "3";
            if (metroRadioButton8.Checked) atbildetais = "4";

            MySqlConnection con = new MySqlConnection(Login.connectionString);
            con.Open(); //klasei,Nosaukums,epasts,skolotajs,prieksmets,jautajumi,atbildes,p_atbilde,aktivs,laiks
            MySqlCommand cmd = new MySqlCommand("UPDATE Lietotaji SET pilditi_uzdevumi='" + pilditie[uzd_id] + "," + uzdev.ToString() + ":" + atbildetais + "' WHERE epasts='" + Login.pasts + "'", con);
            MySqlDataReader read = cmd.ExecuteReader();
            con.Close();

            MySqlConnection con2 = new MySqlConnection(Login.connectionString);
            con2.Open();
            MySqlCommand cmd2 = new MySqlCommand("UPDATE Uzdevumi SET izpildija='" + Login.pasts + "' WHERE uzd_id='" + uzdev.ToString() + "'", con2);
            MySqlDataReader read2 = cmd2.ExecuteReader();
            con2.Close();

            metroPanel4.Enabled = false;
            metroPanel4.Left = 780;
            metroTile1.PerformClick();
        }

        private void metroButton1_Click_1(object sender, EventArgs e)
        {
            if (metroComboBox2.Text != "" && metroComboBox1.Text != "" && TB_1.Text != "" && TB_2.Text != "")
            {
                string atbilde = "";
                string pareiza = "1";
                if (metroRadioButton1.Checked) pareiza = "1";
                if (metroRadioButton2.Checked) pareiza = "2";
                if (metroRadioButton3.Checked) pareiza = "3";
                if (metroRadioButton4.Checked) pareiza = "4";
                string vards = Login.name;
                atbilde = TB_1.Text; //+ TB_2.Text + ";" + TB_3.Text + ";" + TB_4.Text;
                if (TB_2.Text != "") atbilde += ";" + TB_2.Text;
                if (TB_3.Text != "") atbilde += ";" + TB_3.Text;
                if (TB_4.Text != "") atbilde += ";" + TB_4.Text;
                MySqlConnection con = new MySqlConnection(Login.connectionString);
                con.Open(); //klasei,Nosaukums,epasts,skolotajs,prieksmets,jautajumi,atbildes,p_atbilde,aktivs,laiks
                MySqlCommand cmd = new MySqlCommand("INSERT INTO Uzdevumi (klasei,Nosaukums,epasts,skolotajs,prieksmets,jautajumi,atbildes,p_atbilde,aktivs,laiks) VALUES('" + metroComboBox1.Text + "','" + TB_Nosaukums.Text + "','" + Login.pasts + "','" + Login.name + "','" + metroComboBox2.Text + "','" + RTB_Jautajumi.Text + "','" + atbilde + "','" + pareiza + "','1','" + dateTimePicker1.Text + "')", con);
                MySqlDataReader read;
                read = cmd.ExecuteReader();
                con.Close();

                TB_1.Text = "";
                TB_2.Text = "";
                TB_3.Text = "";
                TB_4.Text = "";
                RTB_Jautajumi.Text = "";
                dateTimePicker1.Text = "";
                TB_Nosaukums.Text = "";
                metroTile1.PerformClick();
            }
        }
    }
}
