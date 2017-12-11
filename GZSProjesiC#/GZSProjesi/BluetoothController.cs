using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using InTheHand;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Ports;
using InTheHand.Net.Sockets;
using System.IO;
namespace GZSProjesi
{
    class BluetoothController
    {
        private Form2 fm2;
        public ServerController sc { get; set; }
        public Thread bluetoothScanThread;
        public Thread bluetoothServerThread;
        public Thread bluetoothClientThread;
       


        public BluetoothController(Form2 fm2)
        {
            this.fm2 = fm2;
          
        }
        public void connectAsClient()
        {

        }
        public void connectAsServer()
        {
        
            bluetoothServerThread = new Thread(new ThreadStart(ServerConnectThread));
            bluetoothServerThread.IsBackground = true;
            bluetoothServerThread.Start();
        }
        Guid mUUID = new Guid("8ce255c0-200a-11e0-ac64-0800200c9a66");
        private bool serverStarted = false;
        public void ServerConnectThread()
        {
            serverStarted = true;
            updateUI("TAG: ServerConnectThread Message: server started,waiting for clients");
            BluetoothListener blueListener = new BluetoothListener(mUUID);
            blueListener.Start();
            

            updateUI("TAG: ServerConnectThread Message:Client has connected");
           
            
            while(true)
            {
                BluetoothClient conn = blueListener.AcceptBluetoothClient();
                Stream mStream = conn.GetStream();
                try
                {
                    //handle server connection
                    byte[] received = new byte[1024];
                    mStream.Read(received, 0, received.Length);
                    updateUI(Encoding.UTF8.GetString(received));
                   
                }
                catch(IOException e)
                {
                    updateUI("TAG: ServerConnectedThread Message : Program catch a some error: " + e.Message);
                }

            }
        }
        private void updateUI(String message)
        {
            Func<int> del = delegate ()
            {
                String infMes=">>>>" + message +"\n";
                fm2.setText(infMes);
       
                return 0;
            };
          
            fm2.Invoke(del);
        }
        public void startScan()
        {
            bluetoothScanThread= new Thread(new ThreadStart(scan));
            bluetoothScanThread.Start();
        }
        BluetoothDeviceInfo[] devices;
        private void scan()
        {
            List<String> items = new List<string>();
            updateUI("TAG:scan Message: Starting Scan..");
            BluetoothClient client = new BluetoothClient();
            devices = client.DiscoverDevicesInRange();
            updateUI("TAG:scan Message: Scan Complete.");
            updateUI("TAG:scan Message: " + devices.Length.ToString() + " devices discovered.");
            foreach(BluetoothDeviceInfo d in devices)
            {
                items.Add(d.DeviceName);
            }
            updateDeviceList(items);
        }
        private void updateDeviceList(List<String> items) 
        {
            Func<int> del = delegate ()
            {
                fm2.updateList(items);
                
                //bluetotohu açık cihazların listesinin güncellenmeis
                return 0;
            };
            fm2.Invoke(del);
        }
        BluetoothDeviceInfo deviceInfo;
        public void selectDeviceToArray(int selectedItemIndex)
        {
            deviceInfo = devices.ElementAt(selectedItemIndex);
            updateUI("TAG: selectDeviceToArray Message :" + deviceInfo.DeviceName + " was selected, attempting connect");
            if(pairDevice())
            {
                
                updateUI("TAG:selectDeviceToArray MESSAGE: device paired");
                updateUI("TAG:selectDeviceToArray MESSAGE: starting connect thread");
                bluetoothClientThread = new Thread(new ThreadStart(ClientConnectThread));
                bluetoothClientThread.Start();
            }
            else
            {
                updateUI("TAG:selectDeviceToArray MESSAGE: Pair failed.");
            }
        }
        private void ClientConnectThread()
        {
            BluetoothClient client = new BluetoothClient();
            updateUI("TAG: ClientConnectThread MESSAGE: Attempting Connect...");
            client.BeginConnect(deviceInfo.DeviceAddress, mUUID,this.BluetoothClientConnectCallback,client);
       
            
        }
        void BluetoothClientConnectCallback(IAsyncResult result)
        {
            BluetoothClient client = (BluetoothClient)result.AsyncState;
            client.EndConnect(result);
            Stream stream = client.GetStream();
            stream.ReadTimeout = 1000;
            while(true)
            {
                while (!ready) ;
                stream.Write(byteMessage, 0, byteMessage.Length);
                ready = false;
            }
        }
        string myPin = "1234";
        private bool pairDevice()
        {
            if(!deviceInfo.Authenticated)
            {
                if(!BluetoothSecurity.PairRequest(deviceInfo.DeviceAddress,myPin))
                {
                    return false;
                }
            }
            return true;
        }
        bool ready = false;
        byte[] byteMessage= Encoding.UTF8.GetBytes("Default Message");
        public  void sendMessage(String message)
        {
            byteMessage = Encoding.UTF8.GetBytes(message);
            ready = true;
        }

        
    }
}
