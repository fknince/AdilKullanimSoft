using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZSProjesi
{
    class LoginInformations
    {
        public int id { get; set; }
        public String nameSurname { get; set; }
        public String loginDate{get;set;}
        public String logOutDate { get; set; }

        public LoginInformations(int id,String nameSurname,String loginDate,String logOutDate)
        {
            this.id = id;
            this.nameSurname = nameSurname;
            this.loginDate = loginDate;
            this.logOutDate = logOutDate;
        }
    }

}
