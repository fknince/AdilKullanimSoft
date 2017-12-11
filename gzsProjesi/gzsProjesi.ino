


const int sensor_pin=A7;
int sensor_deger=0;
float  voltaj_deger=0;
float sicaklik_deger=0;
int gelen_veri;
int isKlima=0;
int otomatikMod_pinON=24;
int otomatikMod_pinOFF=22;
int klima_pinON=28;
int klima_pinOFF=26;
int otomatikMod=0;
int azamiSicaklik=25;
String gelen;


void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(sensor_pin,INPUT);
  pinMode(otomatikMod_pinON,OUTPUT);
  pinMode(otomatikMod_pinOFF,OUTPUT);
  pinMode(klima_pinON,OUTPUT);
  pinMode(klima_pinOFF,OUTPUT);

}

void loop() {

  sensor_deger=analogRead(sensor_pin);
  voltaj_deger=(sensor_deger/1023.0)*5000;
  sicaklik_deger=voltaj_deger/10.0;
  if((sicaklik_deger >= azamiSicaklik) && (otomatikMod == 1) )
    isKlima=1;
  else if((sicaklik_deger < azamiSicaklik) && (otomatikMod == 1))
    isKlima=0;
    
  if(Serial.available())
  {
    
    gelen=Serial.readString();
    if(gelen.indexOf("/") >0 )
      otomatikModBul(gelen);  
    else if(gelen == "kapaK")
    {
      isKlima=0;
      otomatikMod=0;
    }
      
    else if(gelen == "acK")
    {
       isKlima=1;
       otomatikMod=0;
    }
     
      
  }
  Serial.println(sicaklik_deger);
  digitalWrite(otomatikMod_pinON,otomatikMod);
  digitalWrite(otomatikMod_pinOFF,!otomatikMod);
  digitalWrite(klima_pinON,isKlima);
  digitalWrite(klima_pinOFF,!isKlima);
  

  delay(200);
  
}
void fanAc()
{
  
}
void otomatikModBul(String mesaj)
{
  
  String otoMod = getValue(mesaj, '/', 0); // ac/azamiSicaklik
  if(otoMod == "ac")
    otomatikMod=1;
  else if(otoMod=="kapa")                    // kapa/azamiSicaklik
    otomatikMod=0;
  String azamiS = getValue(mesaj, '/', 1);
  azamiSicaklik = azamiS.toInt();
}
String getValue(String data, char separator, int index)
{
    int found = 0;
    int strIndex[] = { 0, -1 };
    int maxIndex = data.length() - 1;

    for (int i = 0; i <= maxIndex && found <= index; i++) {
        if (data.charAt(i) == separator || i == maxIndex) {
            found++;
            strIndex[0] = strIndex[1] + 1;
            strIndex[1] = (i == maxIndex) ? i+1 : i;
        }
    }
    return found > index ? data.substring(strIndex[0], strIndex[1]) : "";
}

