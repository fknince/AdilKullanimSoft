using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;



namespace GZSProjesi
{

    class DBController
    {
        private const String SERVER = "127.0.0.1";
        private const String DATABASE = "gzsproje";
        private const String UID = "root";
        private const String PASSWORD = "174520";
        public MySqlConnection dbConn;
        private Form2 fm2;
        public DBController(Form2 fm2)
        {
            this.fm2 = fm2;
        }

        public  void InitializeDB()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = SERVER;
            builder.UserID = UID;
            builder.Password = PASSWORD;
            builder.Database = DATABASE;
            String connSTring = builder.ToString();
            builder = null;
            fm2.setText(connSTring+"\n");
            dbConn = new MySqlConnection(connSTring);
        }

        public List<LoginInformations> GetAllInformations(String name)
        {
            List<LoginInformations> li = new List<LoginInformations>();
            String query = "Select t1.idloginTable, t1.nameSurname,t2.loginDate,t2.logoutDate from logintable t1, logindates t2 where t1.idloginTable = t2.id " +
                "and t1.nameSurname=@name";
            MySqlCommand cmd = dbConn.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@name", name);
            dbConn.Open();
            MySqlDataReader reader= cmd.ExecuteReader();
         
            while(reader.Read())
            {
                
                int id = (int)reader["idloginTable"];
                String nameSurname = reader["nameSurname"].ToString();
                String loginDate = ((DateTime)reader["loginDate"]).ToString();
                String logoutDate = ((DateTime)reader["logoutDate"]).ToString();
                li.Add(new LoginInformations(id, nameSurname, loginDate, logoutDate));
            }
            dbConn.Close();
            return li;
        }
        public void insertLogInformations(String name)
        {
           
            dbConn.Open();
            MySqlCommand cmd = dbConn.CreateCommand();
            String query = "insert into logintable(nameSurname) values(@name)";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@name", name);
            cmd.ExecuteNonQuery();


            int id = (int)cmd.LastInsertedId;  //son eklenenin idsi
            query = "insert into logindates(id,loginDate) values(@id,@loginD)";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", id);
            DateTime date = DateTime.Now;
            String dt = date.ToString("yyyy-MM-dd H:mm:ss");
            cmd.Parameters.AddWithValue("@loginD",date );
            cmd.ExecuteNonQuery();


            dbConn.Close();

        }
        public int getId(String name)
        {
            int donecek;
            try
            {
                dbConn.Open();
                String query = "Select idloginTable from  logintable where nameSurname=@name order by idloginTable desc";
                MySqlCommand cmd = dbConn.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@name", name);
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                donecek = (Convert.ToInt32(reader["idloginTable"]));
                dbConn.Close();
            }
            catch(Exception e)
            {
                donecek = -1;
                dbConn.Close();
            }
            return donecek;
        }
        public void setLogOutDate(string name)
        {
            int id = getId(name);
            if (id != -1)
            {
                DateTime date = DateTime.Now;
                String dt = date.ToString("yyyy-MM-dd H:mm:ss");
                dbConn.Open();
                String query = "update logindates set logoutDate=@date where id=@id ";
                MySqlCommand cmd = dbConn.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@date", dt);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                dbConn.Close();
            }

        }
        public String  getPassword(String kadi)
        {
            String donecek = "";
            try
            {
                dbConn.Open();
                String query = "Select sifre from  mainLogin where kadi=@kAdi";
                MySqlCommand cmd = dbConn.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@kAdi", kadi);
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                donecek= (reader["sifre"].ToString());
                

            }
            catch(Exception e)
            {
                dbConn.Close();
                donecek= null;
               
            }
            dbConn.Close();
            return donecek;
            



        }
        public bool hasUser(String kadi)
        {
            Boolean returner = false;
            dbConn.Open();
            String query = "Select count(kadi) as adet from  mainLogin where kadi=@kAdi";
            MySqlCommand cmd = dbConn.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@kAdi", kadi);
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            if(Convert.ToInt32(reader["adet"].ToString()) > 0)
            {
                returner = true;
            }
            else
            {
                returner = false;
            }
            dbConn.Close();
            return returner;
        }
        public bool hasDonanim(String name)
        {
            Boolean returner = false;
            dbConn.Open();
            String query = "Select count(Name) as adet from  hardwares where Name=@name";
            MySqlCommand cmd = dbConn.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@name", name);
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            if (Convert.ToInt32(reader["adet"].ToString()) > 0)
            {
                returner = true;
            }
            else
            {
                returner = false;
            }
            dbConn.Close();
            return returner;
        }
        public bool donanimKaydet(String name,String com)
        {
            Boolean returner = false;
            if (!hasDonanim(name))
            {
                dbConn.Open();
                MySqlCommand cmd = dbConn.CreateCommand();
                String query = "insert into hardwares(BTName,Name) values(@btname,@name)";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@btname", com);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.ExecuteNonQuery();
                returner = true;


            }
             dbConn.Close();
             return returner;

        }
        public bool donanimGuncelle(String name,String com)
        {
            Boolean returner = false;
            if(hasDonanim(name))
            {
                dbConn.Open();
                MySqlCommand cmd = dbConn.CreateCommand();
                String query = "update hardwares set BTName=@btname where Name=@name";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@btname", com);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.ExecuteNonQuery();
                returner = true;
            }
            dbConn.Close();
            return returner;
        }
        public String getComNameHardware(String name)
        {
            String BTName = "";
            if(hasDonanim(name))
            {
               
                dbConn.Open();
                String query = "Select BTName from hardwares where Name=@name";
                MySqlCommand cmd = dbConn.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@name", name);
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                BTName= reader["BTName"].ToString();
                dbConn.Close();



            }
            else
            {
                dbConn.Close();
                
            }
            return BTName;
           
        }
        private int second = 55;
        public void setCelcius(String sicaklik)
        {
           
            DateTime currentTime = DateTime.Now;
            
            if(currentTime.Second == 0 && second != currentTime.Second) // dakikalık sıckalık bilgisi aktarımı
            {
                //saat ve dakika görünümü
                String saat, dakika;
                if (currentTime.Hour < 10)
                    saat = "0" + currentTime.Hour.ToString();
                else
                    saat = currentTime.Hour.ToString();

                if (currentTime.Minute < 10)
                    dakika = "0" + currentTime.Minute.ToString();
                else
                    dakika = currentTime.Minute.ToString();
               
                dbConn.Open();
                MySqlCommand cmd = dbConn.CreateCommand();
                String query = "insert into celciustable(date,hours,celcius) values(@date,@hours,@celcius)";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@date", currentTime.Date.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@hours", saat+":"+dakika);
                cmd.Parameters.AddWithValue("@celcius",sicaklik );
                cmd.ExecuteNonQuery();
                dbConn.Close();
                
            }
            second = currentTime.Second;
        }
        public List<Temp> getAllTempValues()
        {
            List<Temp> li = new List<Temp>();
            String query = "Select * from celciustable";
            MySqlCommand cmd = new MySqlCommand(query, dbConn);
            dbConn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {

                int id = (int)reader["idcelciusTable"];
                String date = reader["date"].ToString().Substring(0,10);
                String time = reader["hours"].ToString();
                String celcius = reader["celcius"].ToString();
                li.Add(new Temp(id,date,time,celcius));
            }
            dbConn.Close();
            return li;
        }

    }
   
}
