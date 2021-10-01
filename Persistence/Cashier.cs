using System;

namespace Persistance
{
    public class Cashier
    {
        public int? CashierId{set; get;}
        public string UserName{set; get;}
        public string Password{set; get;}
        public int Role{set; get;}
        public string FirstName{set; get;}
        public string MiddleName{set; get;}
        public string LastName{set; get;}
        public string Phone{set; get;}
        public string Email{set; get;}
        public string Address{set; get;}
        public string FullName{
            get{
                return string.Format("{0} {1} {2}", FirstName, MiddleName, LastName);
            }
        }
        
        private int SALE_ROLE = 2;
        private int MANAGER_ROLE = 1;
    }
}
