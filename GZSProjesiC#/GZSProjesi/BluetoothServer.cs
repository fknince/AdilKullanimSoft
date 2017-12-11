using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTheHand;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Ports;
using InTheHand.Net.Sockets;
using System.IO;
using System.Threading;



namespace GZSProjesi
{
    public class BluetoothServer
    {
        public List<BluetoothClient> connessioniServer { get; set; }
        public List<string> nameList { get; set; }
        Form2 fm2;
        ServerController sc;
        BluetoothClient conn { set; get; }
        BluetoothClient sonMesajGonderen { get; set; }



        Boolean isAnketing { get; set; }
        Boolean isKlimaWorking { get; set; }
       
        int[] klimaAcilma{ get; set; }
        int[] klimaKapanma{ get; set; }
        public BluetoothServer(Form2 fm2,ServerController sc)
        {
            this.fm2 = fm2;
            this.sc = sc;
            nameList = new List<string>();
            isAnketing = false;
            //                 toplam - evet - hayir
            klimaAcilma = new int[] { 0, 0, 0 };
            klimaKapanma = new int[] { 0, 0, 0 };
            isKlimaWorking = false;
            connessioniServer = new List<BluetoothClient>();

          
        }
        public void addList(BluetoothClient bc)
        {
            if(connessioniServer.IndexOf(bc) <0)
            {
                nameList.Add("blabla");
                connessioniServer.Add(bc);
                
            }
           
        }
        public void changeStr(BluetoothClient bc,String s)
        {
            nameList[connessioniServer.IndexOf(bc)] = s;
        }
        public void removeClientAndName(String s)
        {
            int index = nameList.IndexOf(s);
            nameList.Remove(s);
            connessioniServer.RemoveAt(index);
        }

        public void ConnectAsServer()
        {
            

            // thread handshake
            Thread bluetoothConnectionControlThread = new Thread(new ThreadStart(ServerControlThread));
            bluetoothConnectionControlThread.IsBackground = true;
            bluetoothConnectionControlThread.Start();

            // thread connessione
            Thread bluetoothServerThread = new Thread(new ThreadStart(ServerConnectThread));
            bluetoothServerThread.IsBackground = true;
            bluetoothServerThread.Start();
        }

        private void ServerControlThread()
        {
            while (true)
            {
                
                updateConnList();
              
                Thread.Sleep(0);
            }
        }

        Guid mUUID = new Guid("8ce255c0-200a-11e0-ac64-0800200c9a66");
        
        private void ServerConnectThread()
        {
            updateUI("server started",true);
            BluetoothListener blueListener = new BluetoothListener(mUUID);
            blueListener.Start();
            while (true)
            {
                conn = blueListener.AcceptBluetoothClient();
                addList(conn);
                Thread appoggio = new Thread(new ParameterizedThreadStart(ThreadAscoltoClient));
                appoggio.IsBackground = true;
                appoggio.Start(conn);
                updateUI(conn.RemoteMachineName + " has connected",true);
                

            }
        }
      
        private void ThreadAscoltoClient(object obj)
        {
            BluetoothClient clientServer = (BluetoothClient)obj;
            sonMesajGonderen = clientServer;
            Stream streamServer = clientServer.GetStream();
            streamServer.ReadTimeout = 1000;
            while (clientServer.Connected)
            {
                try
                {
                    int bytesDaLeggere = clientServer.Available;
                    if (bytesDaLeggere > 0)
                    {
                        byte[] bytesLetti = new byte[bytesDaLeggere];
                        int byteLetti = 0;
                        while (bytesDaLeggere > 0)
                        {
                            int bytesDavveroLetti = streamServer.Read(bytesLetti, byteLetti, bytesDaLeggere);
                            bytesDaLeggere -= bytesDavveroLetti;
                            byteLetti += bytesDavveroLetti;
                        }
                        updateUI( System.Text.Encoding.UTF8.GetString(bytesLetti),false);
                        
                        
                    }
                }
                catch { }
                Thread.Sleep(0);
            }
            updateUI(clientServer.RemoteMachineName + " has gone",true);
        }



        public void updateUI(string message,Boolean isConsole)
        {
            Func<int> del = delegate ()
            {
                fm2.setText(message + System.Environment.NewLine);
                if(!isConsole)
                {
                    sc.incomingMessageSwitcher(message);
                    if(message.Contains("#"))
                    {
                        if (message.Split('#')[0].Equals("si"))
                        {
                            changeStr(conn, message.Split('#')[1]);


                        }
                        if (message.Split('#')[0].Equals("ex"))
                        {
                            removeClientAndName(message.Split('#')[1]);
                        }
                        if (message.Split('#')[0].Equals("kli"))
                        {
                            if (message.Split('#')[1].Equals("aç"))
                            {
                                fm2.klimaAcmaTalebi(nameList[connessioniServer.IndexOf(sonMesajGonderen)], sonMesajGonderen);

                            }
                            else
                            {
                                fm2.klimaKapamaTalebi(nameList[connessioniServer.IndexOf(sonMesajGonderen)], sonMesajGonderen);
                            }

                        }
                    }
                    else
                    {
                        fm2.setText("Gelen sıcaklık: " + message + "\n");
                    }
                    
                }
                return 0;
            };
            fm2.Invoke(del);
        }
        

        private void updateConnList()
        {
            Func<int> del = delegate ()
            {
                //fm2.setClearList();
                
              
                return 0;
            };
            try
            {
                fm2.Invoke(del);
            }
            catch { }
        }
    }
}
