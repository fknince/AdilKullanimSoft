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
    public partial class TempList : Form
    {
        private DBController db;
        private List<Temp> list;
        Thread veriCekme;
        Form2 fm2;
        private String gelen;
        private int count;
        public TempList(Form2 fm2)
        {
            InitializeComponent();
            this.fm2 = fm2;
            db = new DBController(fm2);
            db.InitializeDB();
        }

        private void TempList_Load(object sender, EventArgs e)
        {
            //list view ayarları
            listView1.Columns.Add("Tarih", 300);
            listView1.Columns.Add("Zaman", 100);
            listView1.Columns.Add("Sıcaklık", 100);

            
            listView1.View = View.Details;


            veriCekme = new Thread(veriCek);
            veriCekme.IsBackground = true;
            veriCekme.Start();
            count = 0;
        }
        public void veriCek()
        {
            
            gelen = "ilk giris";
            while (gelen != null)
            {
                gelen = fm2.gelenDeger;
                list = db.getAllTempValues();
                if(count != list.Count)
                {
                    updateList();
                    count = list.Count;
                }
                
                Thread.Sleep(1000);
                
            }
        }
        private void updateList()
        {
            Func<int> del = delegate ()
            {
                if(list.Count > 0)
                {
                    listView1.Items.Clear();
                    foreach (Temp t in list)
                    {
                        string[] gecici = { t.date, t.time, t.celcius };
                        ListViewItem gList = new ListViewItem(gecici);
                        listView1.Items.Add(gList);

                    }
                    listView1.Focus();
                    listView1.Items[list.Count - 1].Selected = false;
                    listView1.EnsureVisible(list.Count - 1);

                }
                



                return 0;
            };
            try
            {
                this.Invoke(del);
            }
            catch { }
        }

        private void TempList_FormClosed(object sender, FormClosedEventArgs e)
        {
            veriCekme.Abort();
            fm2.TempListIsOpen = false;
        }
    }
}
