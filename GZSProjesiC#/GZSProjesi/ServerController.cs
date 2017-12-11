using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GZSProjesi
{
    public class ServerController
    {

       
        private Form2 fm2;
        private DBController dbc;
       
        public ServerController(Form2 fm2)
        {
            this.fm2 = fm2;
            dbc = new DBController(fm2);
            dbc.InitializeDB();
           
        }
      
        public void incomingMessageSwitcher(String message)
        {
           
            String[] tmp;
            tmp = message.Split('#');
            String processes = tmp[0];
            
            switch(processes)
            {
                case "si":
                {
              
                        Bitmap bmp = downloadImage(tmp[2]);
                        fm2.addList(bmp, tmp[1]);
                        dbc.insertLogInformations(tmp[1]);
                        break;
                }
                case "ex":
                 {
                      
                        fm2.removeList(tmp[1]);
                        dbc.setLogOutDate(tmp[1]);
                        break;
                 }
            }
        }
        public Bitmap downloadImage(String url)
        {
            
            System.Net.WebRequest request = System.Net.WebRequest.Create(url);
            System.Net.WebResponse resp = request.GetResponse();
            System.IO.Stream respStream = resp.GetResponseStream();
            Bitmap bmp = new Bitmap(respStream);
            respStream.Dispose();
            return bmp;
        }
    }
}
