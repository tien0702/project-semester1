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
        public Cashier SearchByID(int cashier_id){
            return dal.GetByID(cashier_id);
        }
        public bool AddCashier(Cashier cashier){
            return dal.AddCashier(cashier);
        }
    }
}
