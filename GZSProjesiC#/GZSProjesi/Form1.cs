using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GZSProjesi
{
    public partial class Form1 : Form
    {
        private DBController dbc;
        private Form2 f2;
        public Form1()
        {
            InitializeComponent();
            f2 = new Form2();
            dbc = new DBController(f2);
            dbc.InitializeDB();
           
            
        }
        private bool gecis = true;
        private String hataMesaji = "";


        private void btn_giris_Click(object sender, EventArgs e)
        {
            hataMesaji = "";
            if (txt_kullaniciAdi.Text == null)
            {
                gecis = false;
                hataMesaji += "Lütfen bir kullanıcı adı giriniz.\n";
            }
            else
            {
                if(dbc.hasUser(txt_kullaniciAdi.Text))
                    gecis = true;
                else
                {
                    gecis = false;
                    hataMesaji += "Kullanıcı adı sistemde mevcut değil.\n";
                }
            }
           if(txt_sifre.Text == null)
            {
                gecis = false;
                hataMesaji+="Lütfen bir şifre giriniz.\n";
            }
            else
            {
                if(txt_sifre.Text.Equals(dbc.getPassword(txt_kullaniciAdi.Text)))
                {
                    gecis = true;
                }
                else
                {
                    gecis = false;
                    hataMesaji += "Hatalı şifre girdiniz.\n";
                }
            }
            if(gecis)
            {
                //ana pencereye geçiş

                f2.StartPosition = FormStartPosition.CenterScreen;
                f2.WindowState = FormWindowState.Maximized;
                f2.MaximizeBox = false;
                f2.Text = "Gerçek Zamanlı Sistemler Dersi Projesi";
                this.Hide();
                f2.Show();
            }
            else
            {
                MessageBox.Show(hataMesaji, "HATA",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
            
            
        }
    }
}
