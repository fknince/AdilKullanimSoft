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
    public partial class UserList : Form
    {
        private DBController db;
        private List<LoginInformations> list;
        Thread veriCekme;
        Form2 fm2;
        String name;
        public UserList(Form2 fm2,String name)
        {
            InitializeComponent();
            this.fm2 = fm2;
            db = new DBController(fm2);
            db.InitializeDB();
            this.name = name;

        }

        private void UserList_Load(object sender, EventArgs e)
        {
            //list view ayarları
            listView1.Columns.Add("İd", 100);
            listView1.Columns.Add("Giriş Zamanı", 300);
            listView1.Columns.Add("Çıkış Zamanı", 300);
            


            listView1.View = View.Details;

            list = db.GetAllInformations(name);

            for (int i=0;i<list.Count;i++)
            {

                ListViewItem gLis;
                if (i != list.Count-1)
                {
                    String[] gecici={ list[i].id.ToString(),list[i].loginDate,list[i].logOutDate};
                    gLis = new ListViewItem(gecici); 
                }
                else
                {
                    String[] gecici = { list[i].id.ToString(), list[i].loginDate, "Hala AKTİF" };
                    gLis = new ListViewItem(gecici);
                }
                listView1.Items.Add(gLis);
             
            }
        }

        private void UserList_FormClosed(object sender, FormClosedEventArgs e)
        {
            fm2.UserListIsOpen = false;
        }
    }
}
