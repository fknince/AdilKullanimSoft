using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GZSProjesi
{
    class arduinoController
    {
        private Form2 fm2;
        private DBController dbc;
        
        public arduinoController(Form2 fm2)
        {
            this.fm2 = fm2;
            dbc = new DBController(fm2);
            dbc.InitializeDB();
        }

       public void  scan()
        {
            List<String> gecici = new List<string>();
            foreach (string portlar in System.IO.Ports.SerialPort.GetPortNames())
            {
                gecici.Add(portlar);
               
            }
            fm2.updateList(gecici);
        }
        public void baglan(String name)
        {
            try
            {

                fm2.serialPort1.PortName = name;
                fm2.serialPort1.BaudRate = 9600;
                fm2.serialPort1.Open();
                fm2.setText("klimaya bağlanıldı.\n");
                gonder("1");
                Thread appoggio = new Thread(seriPortDinle);
                appoggio.IsBackground = true;
                appoggio.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("Hata:" + e.Message);
            }

        }
        
        private void seriPortDinle()
        {
            while(fm2.serialPort1.IsOpen)
            {

                
                String message = fm2.serialPort1.ReadLine();
                dbc.setCelcius(message);
                if(fm2.isAutoMod)
                {
                  
                    
                    if (fm2.azamiDeger <= Convert.ToInt32(message.Substring(0, 2)))
                    {
                        fm2.isKlima = true;
                        updateUI("ac");
                        if (fm2.cs1.connessioniServer.Count > 0)
                            fm2.bcc.sendMessageToAllClients("kli#acik", fm2.cs1.connessioniServer, new BluetoothClient());
                    }
                        
                    else
                    {
                        fm2.isKlima = false;
                        updateUI("kapa");
                        if (fm2.cs1.connessioniServer.Count > 0)
                            fm2.bcc.sendMessageToAllClients("kli#kapali", fm2.cs1.connessioniServer, new BluetoothClient());

                    }
                        
                }
                fm2.sendTempValue(message);
                updateUI(message);
                
                    
            }
        }
        public bool ısActive()
        {
            return fm2.serialPort1.IsOpen;
        }
        public void closePort()
        {
            fm2.serialPort1.Close();
        }
        public void gonder(string veri)
        {

           
            fm2.serialPort1.Write(veri);

        }
        private void updateUI(String message)
        {
            Func<int> del = delegate ()
            {
                
                
                if(message.Equals("ac"))
                {
                    fm2.startingFan(true);
                }
                else if(message.Equals("kapa"))
                {
                    fm2.startingFan(false);
                }
                else
                    fm2.editTemp(message);
                return 0;
            };

            fm2.Invoke(del);
        }
    }
}
