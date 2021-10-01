using System;
using DAL;
using Persistance;

namespace BL
{
    public class CashierBL
    {
        private CashierDAL dal = new CashierDAL();
        public Cashier Login(Cashier cashier){
            return dal.Login(cashier);
        }
    }
}
