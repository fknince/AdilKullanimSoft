package com.example.fince.gzsproje;

import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.content.ComponentCallbacks2;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.IntentFilter;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.media.Image;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Debug;
import android.provider.Settings;
import android.speech.RecognizerIntent;
import android.speech.tts.TextToSpeech;
import android.support.annotation.RequiresApi;
import android.support.v4.content.LocalBroadcastManager;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.ListAdapter;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.facebook.FacebookSdk;
import com.facebook.login.LoginManager;
import com.facebook.login.widget.LoginButton;

import java.io.InputStream;
import java.nio.charset.Charset;
import java.util.ArrayList;
import java.util.Date;
import java.util.UUID;

public class MainActivity extends AppCompatActivity implements AdapterView.OnItemClickListener{
    private LoginButton loginButton;
    private TextToSpeech textToSpeech;
    private SpeechingController speechingController;
    private BluetoothController bc;
    private TextView test;
    private ImageView mic,bluetooth,serverImage;
    private Date createdDate;
    private ListView lvNewDevices;
    private  String name,surname,imageUrl;
    private Boolean isKlima=false;

    //bluetooth server
    BluetoothConnectionService mBluetoothConnection;
    BluetoothDevice mBTDevice;

    //pc server
    ServerController sc;
    private static final UUID MY_UUID_INSECURE=
            UUID.fromString("8ce255c0-200a-11e0-ac64-0800200c9a66");

    StringBuilder messages;
    public void setMessages(StringBuilder messages) {
        this.messages = messages;
        Log.i("TAG: sendMessages","Gelen Mesaj : "+messages);
    }

    private String sorgu="",cevap="";





    public MainActivity() {

        createdDate=new Date("11/23/2017 21:32:00");
        speechingController=new SpeechingController(this);
        bc=new BluetoothController(this,this);





    }
    public String getNameSurname()
    {
        return name+" "+surname;
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        FacebookSdk.sdkInitialize(this);
        setContentView(R.layout.activity_main2);

        Bundle inBundle=getIntent().getExtras();
        name=inBundle.get("name").toString();
        surname=inBundle.get("surname").toString();
        welcome(name,surname);
        imageUrl=inBundle.get("image").toString();
        TextView nameView=(TextView)findViewById(R.id.text_name);
        nameView.setText(""+name+" "+surname);
        Button logOut=(Button)findViewById(R.id.logout);
        logOut.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
             cikis();
            }
        });
      new MainActivity.DownloadImage((ImageView)findViewById(R.id.profileImage)).execute(imageUrl);


      mic=(ImageView)findViewById(R.id.imageView);
      bluetooth=(ImageView)findViewById(R.id.imgBluetooth);
      serverImage=(ImageView)findViewById(R.id.serverImage);
      lvNewDevices=(ListView)findViewById(R.id.lvNewDevices);
      lvNewDevices.setOnItemClickListener(MainActivity.this);
      bc.prepare4pairingDevices();
      bc.mReceiver(this);

      //başlangıçta bluetooth açık mı kapalımı
        BluetoothAdapter mBluetoothAdapter = BluetoothAdapter.getDefaultAdapter();
        if(mBluetoothAdapter.isEnabled())
            setImageAc();
        else
            setImageAc();






    }

    @Override
    public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
        mBluetoothConnection=bc.cancelDiscovery(i,MainActivity.this,this);

        sc = new ServerController(bc,mBluetoothConnection,this);
    }

    public class DownloadImage extends AsyncTask<String,Void,Bitmap>
    {
        ImageView bmImage;
        public DownloadImage(ImageView bmImage){
            this.bmImage=bmImage;
        }
        protected Bitmap doInBackground(String... urls)
        {
            String urlDisplay=urls[0];
            Bitmap mIcon11=null;
            try
            {
                InputStream in=new java.net.URL(urlDisplay).openStream();
                mIcon11= BitmapFactory.decodeStream(in);
            }
            catch(Exception e)
            {
                Log.e("Error",e.getMessage());
                e.printStackTrace();
            }
            return mIcon11;
        }
        protected  void onPostExecute(Bitmap result)
        {
            bmImage.setImageBitmap(result);
        }
    }
    public void welcome(String name,String surname)
    {

        String metin="Hoşgeldin "+name+" "+surname+" Nasıl yardımcı olabilirim ?";
        this.textToSpeech=speechingController.text2Speech(metin);
    }

    @Override
    protected void onDestroy() {
        textToSpeech.stop();
        textToSpeech.shutdown();
        //unreg receiver
        bc.unRegReceiver();
        //çıkış
        if(sc != null)
            sc.exit();
        Log.d("ON DEstroy","Destyroy oldu");
        super.onDestroy();


    }
    public void getSpeechInput(View view)
    {
        if(!speechingController.isSpeaking())
            speechingController.Speech2Text();
    }

    @RequiresApi(api = Build.VERSION_CODES.M)
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        switch(requestCode)
        {
            case 10:
            {
                if(resultCode == RESULT_OK && data != null)
                {
                    ArrayList<String> result=data.getStringArrayListExtra(RecognizerIntent.EXTRA_RESULTS);
                    cevapla(result.get(0));
                }
                break;
            }
        }
    }
    @RequiresApi(api = Build.VERSION_CODES.M)
    public void cevapla(String s)
    {

        String cevap;
        switch(s)
        {
            case "Neler yapabilirsin":
            {
                cevap="Şimdilik sadece vantilatörü çalıştırabilir ya da bu işlemi otomatik moda alabilirim.";
                this.textToSpeech=speechingController.text2Speech(cevap);
                break;
            }
            case "Çıkış":case "çıkış":
            {
                cikis();
                break;
            }
            case "ismin ne":
            {
                cevap="Yaratıcım adımı Feriha koydu";
                this.textToSpeech=speechingController.text2Speech(cevap);
                break;
            }
            case "kaç yaşındasın":case "yaşın kaç":
            {
                String dizi[]=this.yasHesapla(this.createdDate);
                cevap="Oluşturulduğumdan beri tam olarak";
                for (String eleman:dizi)
                {
                    if(Integer.parseInt(eleman.split(" ")[0]) != 0)
                    {
                        cevap+=" "+eleman;
                    }
                }
                cevap+=" yaşlanmış bulunmaktayım." ;
                this.textToSpeech=speechingController.text2Speech(cevap);
                textToSpeech.setSpeechRate(0.9f);
                break;
            }
            case "Bluetooth'u aç": case "bluetooth'u aç":
            {

                if(!bc.isBtIsActive())
                {
                    bc.enableOrDisableBluetooth();
                    setImageAc();
                }

                else
                    this.textToSpeech=speechingController.text2Speech("Bluetooth zaten açık.");
                break;


            }
            case "Bluetooth'u kapat": case "bluetooth'u kapat":
            {
                 if(bc.isBtIsActive())
                 {
                     bc.enableOrDisableBluetooth();
                     setImagePa();
                 }

                else
                    this.textToSpeech=speechingController.text2Speech("Bluetooth zaten kapalı");
                 break;


            }
            case "Cihazları bul":case "cihazları bul":
            {
                if(bc.isBtIsActive())
                {
                    bc.beDiscovered4Devices();
                    bc.discoverDevices();
                }

                else
                    this.textToSpeech=speechingController.text2Speech("Önce bluetooth'u açman lazım.Bana 'Bluetooth'u aç demen yeterli.'");
                break;

            }
            default:
            {
                cevap="Henüz bu dersi görmedim.";
                this.textToSpeech=speechingController.text2Speech(cevap);
                break;
            }

        }
    }
    public void cikis()
    {

        LoginManager.getInstance().logOut();
        Intent login=new Intent(MainActivity.this,LoginActivity.class);
        startActivity(login);
        finish();

    }
    public String[] yasHesapla(Date date)
    {
        Date currentDate=new Date();
        long difference= (currentDate.getTime()-date.getTime());
        long x = difference / 1000;
        long seconds = x % 60;
        x /= 60;
        long minutes = x % 60;
        x /= 60;
        long hours = x % 24;
        x /= 24;
        long days = x;
        String dizi[]={days+" gün",hours+" saat",minutes+" dakika",seconds+" saniye"};
        return dizi;
    }
    public void bluetoothTiklandi(View view)
    {
        bc.enableOrDisableBluetooth();
        if(bc.isBtIsActive())
            setImageAc();
        else
        {
            setImagePa();
            lvNewDevices.setAdapter(null);
        }





    }
    @RequiresApi(api = Build.VERSION_CODES.M)
    public void searchTiklandi(View view)
    {
        if(bc.isBtIsActive())
        {
            bc.beDiscovered4Devices();
            bc.discoverDevices();



        }
        else
            Toast.makeText(getBaseContext(),"Lütfen ilk önce Bluetooth'u aktifleştirin.",Toast.LENGTH_SHORT).show();
    }
    public void drawView()
    {
        lvNewDevices.setAdapter(bc.getmDeviceListAdapter());
    }
    public void setImagePa()
    {
        bluetooth.setImageResource(R.drawable.bluetoothpa);
    }
    public void setImageAc()
    {
        bluetooth.setImageResource(R.drawable.bluetoothac);
    }

    public void startConnection()
    {
        mBTDevice=bc.getmBTDevice();
        if(mBTDevice != null)
        {
            if(mBluetoothConnection.isConnect() == false)
            {
                mBluetoothConnection.startBTConnection(mBTDevice,MY_UUID_INSECURE);
                while(!mBluetoothConnection.isConnect()){Log.d("TAG:starConnection","telefonun servera bağlanması bekleniyor");};
                Log.i("TAG starConnection","Telefon servera bağlandı.");
                sc.sendMessageToServer("si#"+name+" "+surname+"#"+imageUrl);
                serverImage.setImageResource(R.drawable.serverpcac);
            }

        }
        else
        {
            Toast.makeText(getBaseContext(),"Lütfen ilk önce server cihazı listeden seçiniz.",Toast.LENGTH_SHORT).show();
        }

    }
    public void pcyeBaglanTiklandi(View view)
    {

        startConnection();

    }
    public void klimaTiklandi(View view)
    {
        boolean pass=true;
        String metin="";
        if(bc.isBtIsActive() == false) {
            metin += "\nBluetooth aktifleştirilmedi!";
            pass = false;
        }
        if(mBluetoothConnection == null)
        {
            metin+="\nListeden server seçimi yapılmadı!";
            pass=false;
        }
        else
        {
            if((mBluetoothConnection.isConnect() == false))
            {
                metin+="\nServer ile bağlantı sağlanmadı!";
                pass=false;
            }

        }
        if(mBTDevice == null)
        {
            metin+="\nServer ile bağlantı sağlanmadı!!";
            pass=false;
        }

        if(pass)
        {
            if(isKlima)
            {
                sc.sendMessageToServer("kli#kapat");
            }
            else
            {
                sc.sendMessageToServer("kli#aç");
            }

        }
        else
        {
            Toast.makeText(getBaseContext(),metin,Toast.LENGTH_SHORT).show();
        }

    }
    public void incomingMessage(String message)
    {
        String parts[]=message.split("#");
        if(parts[0].equals("kli"))
        {
            klimaAnketi(parts[1]);
        }

    }
    public void klimaAnketi(String message)
    {
        sorgu="";
        String cevap1,cevap2;
        if(message.equals("aç"))
        {
            sorgu="Klima açılsın mı?";
            cevap1="Açılsın";
            cevap2="Açılmasın";
        }

        else
        {
            sorgu="Klima kapansın mı?";
            cevap1="Kapansın";
            cevap2="Kapanmasın";
        }

        AlertDialog.Builder dlg=new AlertDialog.Builder(this);
        dlg.setTitle("KLİMA İSTEĞİ");
        dlg.setMessage(sorgu);
        dlg.setIcon(R.drawable.klimapa);
        dlg.setPositiveButton(cevap1, new DialogInterface.OnClickListener() {
            @Override
                public void onClick(DialogInterface dialogInterface, int i) {
                    if(sorgu.equals("Klima açılsın mı?"))
                    {

                        cevap="kli#açC#evet";
                        sc.sendMessageToServer(cevap);
                    }
                    else
                    {

                        cevap="kli#kapaC#evet";
                        sc.sendMessageToServer(cevap);
                    }

            }
        });
        dlg.setNegativeButton(cevap2, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                if(sorgu.equals("Klima açılsın mı?"))
                {

                    cevap="kli#açC#hayır";
                    sc.sendMessageToServer(cevap);
                }
                else
                {

                    cevap="kli#kapaC#hayır";
                    sc.sendMessageToServer(cevap);
                }
            }
        });
        AlertDialog ad=dlg.create();
        ad.show();

    }



}
