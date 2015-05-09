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
using System.Security.Cryptography;

namespace VirtualieUzdevumi2v
{
    public partial class Login : MetroForm
    {

        public static string connectionString = "Server=95.68.69.45;Database=Prakse;UID=root;Password=123123;";
        public static string name, pasts, attels, klase;
        public static int ID;


        public Login()
        {
            InitializeComponent();
        }

        public string CreateHD5(string s)
        {
            MD5 md5 = MD5.Create();
            byte[] input = System.Text.Encoding.ASCII.GetBytes(s);
            byte[] bytes = md5.ComputeHash(input);

            StringBuilder str = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                str.Append(bytes[i].ToString("X2"));
            }

            return str.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Lietotaji", connection);
                MySqlDataReader read;
                read = cmd.ExecuteReader();
                if (TB_Pasts.Text != "")
                    while (read.Read())
                    {
                        if (read.GetString("epasts") == TB_Pasts.Text)
                        {
                            if (read.GetString("parole") == CreateHD5(TB_Parole.Text))
                            {
                                name = read.GetString("vards");
                                pasts = read.GetString("epasts");
                                // Skatas kads ID un pec ta aizmet uz formam
                                if (read.GetInt32("status") == 0) LB_Alert.Text = "Admin Sveiks!";
                                else if (read.GetInt32("status") == 1 || read.GetInt32("status") == 2)
                                {
                                    if (read.GetInt32("status") == 1) ID = 1;
                                    else
                                    {
                                        klase = read.GetString("kurss");
                                        ID = 2;
                                    }
                                    attels = read.GetString("attels");
                                    this.Visible = false;
                                    TB_Parole.Text = "";
                                    Galvena form2 = new Galvena();
                                    form2.ShowDialog();
                                    LB_Alert.Text = "Lietotāj Sveiks!";
                                }
                                else LB_Alert.Text = "Jūsu ID ir radusies kļūda. \nSazinaties ar Administrāciju!";
                            }
                            else LB_Alert.Text = "E-Pasts vai Parole nepareiza!";
                            goto beigas;
                        }
                        else LB_Alert.Text = "E-Pasts vai Parole nepareiza!";
                    }
                else LB_Alert.Text = "Nekas netika ievadīts!";
            beigas:
                connection.Close();
            }
            catch { LB_Alert.Text = "Radās kļūda ar datubāzi!"; }
            TB_Parole.Text = "";
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            if (metroTile1.Style == MetroFramework.MetroColorStyle.White)
            {
                metroTile1.Style = MetroFramework.MetroColorStyle.Blue;
                metroTile2.Style = MetroFramework.MetroColorStyle.White;
                metroComboBox1.Enabled = false;
                this.Refresh();
            }
        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            if (metroTile2.Style == MetroFramework.MetroColorStyle.White)
            {
                metroTile2.Style = MetroFramework.MetroColorStyle.Blue;
                metroTile1.Style = MetroFramework.MetroColorStyle.White;
                metroComboBox1.Enabled = true;
                this.Refresh();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!TB_Email.Text.Contains("@"))
                {
                    LB_Alert2.Text = "Epasta formāts ir nepareiz!";
                    goto beigas;
                }
                string email = TB_Email.Text;
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT epasts FROM Lietotaji", connection);
                MySqlDataReader read;
                read = cmd.ExecuteReader();
                while (read.Read())
                {
                    if (email == read.GetString("epasts"))
                    {
                        LB_Alert2.Text = "Ievadītais E-Pasts jau eksistē!";
                        goto beigas;
                    }
                }
                connection.Close();

                if (TB_Pass.Text == TB_APass.Text && TB_Email.Text != "" && TB_Vards.Text != "" && TB_Pass.Text != "")
                {
                    
                    string status = "1";
                    if (metroTile2.Style == MetroFramework.MetroColorStyle.Blue)
                    {
                        status = "2";
                        if (metroComboBox1.Text == "") goto beigas;
                    }
                    else if (metroTile1.Style == MetroFramework.MetroColorStyle.Blue)
                    {
                        metroComboBox1.Text = "";
                        status = "1";
                    }
                    MySqlConnection con = new MySqlConnection(connectionString);
                    con.Open();
                    MySqlCommand cmd2 = new MySqlCommand("INSERT INTO Lietotaji (kurss,status,vards,parole,epasts,attels) VALUES('" + metroComboBox1.Text + "','" + status + "','" + TB_Vards.Text + " " + TB_Uzvards.Text + "','" + CreateHD5(TB_Pass.Text) + "','" + TB_Email.Text + "','" + TB_Attels.Text + "')", con);
                    MySqlDataReader read2 = cmd2.ExecuteReader();
                    con.Close();

                    TB_APass.Text = "";
                    TB_Pass.Text = "";
                    TB_Attels.Text = "";
                    TB_Email.Text = "";
                    TB_Vards.Text = "";
                    TB_Uzvards.Text = "";
                    LB_Alert2.Text = "";
                }
                else LB_Alert2.Text = "Kaut kas netika ievadīts pareizi!";
            }
            catch { LB_Alert2.Text = "Radās kļūda ar datubāzi!"; }
        beigas:
            ;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            metroComboBox1.Items.Clear();
            string[] klases = Properties.Resources.Klases.Split(';');
            foreach (string s in klases)
                metroComboBox1.Items.Add(s);
        }
    }
}
