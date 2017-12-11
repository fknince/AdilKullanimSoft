using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace GZSProjesi
{
    public partial class Form2 : Form
    {

        ServerController sc;
        public BluetoothServer cs1 { get; set; }
        public BluetoothClientController bcc { get; set; }
        //list view
        ImageList il = new ImageList();
        List<string> names = new List<string>();
        ListViewItem lst = new ListViewItem();
        DBController db;
        public Boolean isKlima { get; set; }
        arduinoController ac;
        public Boolean hasHardwareName = false;
        public  Boolean TempListIsOpen { get; set; }
        public Boolean UserListIsOpen { get; set; }
        public Boolean isAutoMod { get; set; }
        public int azamiDeger { get; set; }
        public String gelenDeger { get; set; }



        public Form2()
        {
          
            InitializeComponent();
            //listView ayarları
            listView1.View = View.Details;
            listView1.Columns.Add("", 223);
            il.ImageSize = new Size(80,80);
            listView1.SmallImageList = il;
            isKlima = false;

            TempListIsOpen = false;
            UserListIsOpen = false;
            isAutoMod = false;

         
            




        }

        private void Form2_Load(object sender, EventArgs e)
        {
            pictureBox1.Image= Image.FromFile("C:/Users/fince/Documents/Visual Studio 2017/Projects/GZSProjesiC#/GZSProjesi/images/klima.png");
            pictureBox2.Image = Image.FromFile("C:/Users/fince/Documents/Visual Studio 2017/Projects/GZSProjesiC#/GZSProjesi/images/speaker.png");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;


            sc = new ServerController(this);
            cs1 = new BluetoothServer(this, sc);
            cs1.ConnectAsServer();

            db = new DBController(this);
            db.InitializeDB();
            bcc = new BluetoothClientController(this);
            ac = new arduinoController(this);
            ac.scan();

            if (db.hasDonanim("Klima"))
            {
                hasHardwareName = true;
                comboBox1.Text = db.getComNameHardware("Klima");
            }
            else
            {
                hasHardwareName = false;
                MessageBox.Show("Veri tabanında 'Klima' donanımına ilişkin adres atanması yapılmamıştır.\n" +
                    "Lütfen bağlanmayı denemeden önce geçerli adresi atayınız.", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button4.Enabled = false;
            }

          













        }

        private void bGo_Click(object sender, EventArgs e)
        {

        }
        public void setText(String message)
        {
            tbOutput.Text += message;
        }

        
       
        public void addList(Bitmap bmp,String name)
        {
            
            if (il.Images.IndexOfKey(name) < 0)
            {
               
                il.Images.Add(bmp);


            }
            if (names.IndexOf(name) < 0)
            {
                names.Add(name);
                
            }
            Console.WriteLine(names.Count);
            Console.WriteLine(il.Images.Count);
            setListItems();


        }
        public void removeList(String name)
        {
            int index = names.IndexOf(name);
            names.Remove(name);
            il.Images.RemoveAt(index);
            setListItems();
        }
        public void setListItems()
        {

            listView1.Items.Clear();
            for (int i = 0; i < names.Count; i++)
            {
                listView1.Items.Add(names[i], i);
            }

        }

        private void pic1Enter(object sender, EventArgs e)
        {
            panel10.BackColor = Color.SteelBlue;
        }

        private void pic1Leave(object sender, EventArgs e)
        {
            panel10.BackColor = SystemColors.ActiveCaption;
        }

        private void pic2Enter(object sender, EventArgs e)
        {
            panel12.BackColor = Color.SteelBlue;
        }

        private void pic2Leave(object sender, EventArgs e)
        {
            panel12.BackColor = SystemColors.ActiveCaption;
        }

    
        private void klimaTiklandi(object sender, MouseEventArgs e)
        {
          
        }

        private void hoparlorTiklandi(object sender, MouseEventArgs e)
        {

        }
        public void klimaAcmaTalebi(String name, InTheHand.Net.Sockets.BluetoothClient bc)
        {
            DialogResult dr = MessageBox.Show(name+" adlı kullanıcı klima açma talebinde bulunuyor.Klima açılsın mı ?",
                      "Klima Açma Talebi ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                {
                        if(!isKlima)
                        {
                            bcc.sendMessageToClient("kli#aç#yes", bc);
                            ac.gonder("acK");
                            this.setText("Klima açıldı");
                            pictureBox3.Image = Image.FromFile("C:/Users/fince/Documents/Visual Studio 2017/Projects/GZSProjesiC#/GZSProjesi/images/fanWorking.gif");
                            isKlima = true;
                        }
                     
                      
                        break;
                }
                case DialogResult.No:
                {
                        if(!isKlima)
                        {
                            bcc.sendMessageToClient("kli#aç#false", bc);
                            this.setText("Klima açılmadı. ");
                            isKlima = false;
                        }
                       
                       
                        break;
                }
            }
            
        }
        public void klimaKapamaTalebi(String name, InTheHand.Net.Sockets.BluetoothClient bc)
        {
            DialogResult dr = MessageBox.Show(name + " adlı kullanıcı klima kapama talebinde bulunuyor.Klima kapansın mı ?",
                 "Klima Kapama Talebi ", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    {
                        if (isKlima)
                        {
                            bcc.sendMessageToClient("kli#kapa#yes", bc);
                            this.setText("Klima kapandı.");
                            pictureBox3.Image = Image.FromFile("C:/Users/fince/Documents/Visual Studio 2017/Projects/GZSProjesiC#/GZSProjesi/images/fanStopping.png");
                            ac.gonder("kapaK");
                            isKlima = false;
                        }
                     

                        break;
                    }
                case DialogResult.No:
                    {
                        if (isKlima)
                        {
                            bcc.sendMessageToClient("kli#kapa#false", bc);
                            this.setText("Klima kapanmadı.");
                            isKlima = true;
                        }
                     

                        break;
                    }
            }
           
        }
        public void updateList(List<String> list)
        {
            comboBox1.Items.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                comboBox1.Items.Add(list[i]);
                
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (hasHardwareName)
                ac.baglan(comboBox1.Text);
        }

        private void startMouseEnter(object sender, EventArgs e)
        {
            pictureBox4.Image = Image.FromFile("C:/Users/fince/Documents/Visual Studio 2017/Projects/GZSProjesiC#/GZSProjesi/images/startOnEnter.png");
        }

        private void startMouseLeave(object sender, EventArgs e)
        {
            pictureBox4.Image = Image.FromFile("C:/Users/fince/Documents/Visual Studio 2017/Projects/GZSProjesiC#/GZSProjesi/images/startFan.png");
        }

        private void stopMouseEnter(object sender, EventArgs e)
        {
            pictureBox5.Image = Image.FromFile("C:/Users/fince/Documents/Visual Studio 2017/Projects/GZSProjesiC#/GZSProjesi/images/stopFanOnEnter.png");
        }

        private void stopMouseLeave(object sender, EventArgs e)
        {
            pictureBox5.Image = Image.FromFile("C:/Users/fince/Documents/Visual Studio 2017/Projects/GZSProjesiC#/GZSProjesi/images/stopFan.png");
        }

        private void otomatikModDegisti(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                label6.Enabled = true;
                trackBar1.Enabled = true;
                textBox1.Enabled = true;
                button2.Enabled = true;

              
            }
            else
            {
                label6.Enabled = false;
                trackBar1.Enabled = false;
                textBox1.Enabled = false;
                button2.Enabled = false;
                isAutoMod = false;
                if (ac.ısActive())
                    ac.gonder("kapa/25");
            }
        }

        private void trackChanged(object sender, EventArgs e)
        {
            textBox1.Text = trackBar1.Value.ToString();
        }

        private void trackTextChanged(object sender, EventArgs e)
        {
            int value;
            var isNumeric = int.TryParse(textBox1.Text, out value);
          
            if(isNumeric)
            {
                if((value >= 25 && value <= 100))
                    trackBar1.Value = Convert.ToInt32(textBox1.Text);
                else if (value < 25)
                {
                    trackBar1.Value = 25;
                    textBox1.Text = "25";
                }
                else if (value > 100)
                {
                    trackBar1.Value = 100;
                    textBox1.Text = "100";
                }
            }
            else
            {
                textBox1.ResetText();

            }
            
          
           
        }

        private void otomatikModKaydetTiklandi(object sender, EventArgs e)
        {
          
            int deger = trackBar1.Value;
            azamiDeger = deger;
            isAutoMod = true;
            String metin = "ac/" + deger.ToString();
            if (ac.ısActive())
                ac.gonder(metin);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Boolean sonuc = false;
            if (db.hasDonanim("Klima"))
                sonuc=db.donanimGuncelle("Klima", comboBox1.Text.ToString());
            else
                sonuc=db.donanimKaydet("Klima", comboBox1.Text.ToString());
            if (sonuc)
                button4.Enabled = true;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
         
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
          
            
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
           
            if (!isKlima)
            {
               if(ac.ısActive())
                {
                    pictureBox3.Image = Image.FromFile("C:/Users/fince/Documents/Visual Studio 2017/Projects/GZSProjesiC#/GZSProjesi/images/fanWorking.gif");
                    isKlima = true; 
                    ac.gonder("acK");  //klima açma kodu = acK
                    isAutoMod = false;
                    if (cs1.connessioniServer.Count > 0)
                        bcc.sendMessageToAllClients("kli#acik", cs1.connessioniServer, new BluetoothClient());
                }
               else
                {
                    MessageBox.Show("İlk önce donanıma bağlanınız.", "HATA", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
               
                
            }
            else
            {
                MessageBox.Show("Klima zaten çalışıyor.", "BİLGİLENDİRME", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if(isKlima)
            {
                if(ac.ısActive())
                {
                    pictureBox3.Image = Image.FromFile("C:/Users/fince/Documents/Visual Studio 2017/Projects/GZSProjesiC#/GZSProjesi/images/fanStopping.png");
                    isKlima = false;
                    ac.gonder("kapaK"); //klima kapama kodu = kapaK
                    isAutoMod = false;
                    if (cs1.connessioniServer.Count > 0)
                        bcc.sendMessageToAllClients("kli#kapali", cs1.connessioniServer, new BluetoothClient());
                }
                else
                {
                    MessageBox.Show("İlk önce donanıma bağlanınız.", "HATA",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
             
            }
            else
            {
                MessageBox.Show("Klima zaten kapalı.", "BİLGİLENDİRME", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

       

        private void listViewTiklandi(object sender, MouseEventArgs e)
        {
            
        }
        private void detayVerTiklandi(Object sender, System.EventArgs e)
        {
            
        }
        private void baglantiyiKesTiklandi(Object sender, System.EventArgs e)
        {

        }
        public void editTemp(String gelen)
        {
            label4.Text = gelen;
            gelenDeger = gelen;
        }
        public void sendTempValue(String gelen)
        {
            if (cs1.connessioniServer.Count > 0)
                bcc.sendMessageToAllClients(gelen, cs1.connessioniServer,new BluetoothClient());
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            
            if (e.Button == MouseButtons.Right)
            {

                if (listView1.SelectedItems.Count > 0)
                {
                    contextMenuStrip1.Show(listView1, e.Location);
                }
                else
                {
                    //do nothing
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(!TempListIsOpen)
            {
                //detay ver basıldı
                TempList tl = new TempList(this);
                tl.StartPosition = FormStartPosition.CenterScreen;
                tl.TopMost = true;
                tl.Text = "Şu ana kadar tutulan sıcaklık bilgilerinin detayı";
                TempListIsOpen = true;
                tl.Show();
            }
           
        }

        private void detayVerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!UserListIsOpen)
            {
                String name = listView1.SelectedItems[0].Text;
                UserList tl = new UserList(this, name);
                tl.StartPosition = FormStartPosition.CenterScreen;
                tl.TopMost = true;
                tl.Text = "Kişinin şu ana kadarki sunucuya bağlanma bilgileri";
                UserListIsOpen = true;
                tl.Show();
            }
           
        }
        public void startingFan(Boolean gelen)
        {
            if(gelen)
                pictureBox3.Image = Image.FromFile("C:/Users/fince/Documents/Visual Studio 2017/Projects/GZSProjesiC#/GZSProjesi/images/fanWorking.gif");
            else
                pictureBox3.Image = Image.FromFile("C:/Users/fince/Documents/Visual Studio 2017/Projects/GZSProjesiC#/GZSProjesi/images/fanStopping.png");

        }

        private void bağlantıyıKesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String name = listView1.SelectedItems[0].Text;
            int index = cs1.nameList.IndexOf(name);
            if(index != -1)
            {
                BluetoothClient client = cs1.connessioniServer[index];
                bcc.sendMessageToClient("left", client );

            }
          


        }

       


























        /*private void listBoxDoubleClick(object sender, EventArgs e)
        {
            
        }*/


    }
}
