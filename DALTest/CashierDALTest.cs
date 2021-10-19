using System;
using Xunit;
using Persistance;
using DAL;

namespace DALTest
{
    public class CashierTest
    {
        private Cashier cashier = new Cashier();
        private CashierDAL dal = new CashierDAL();
        // ---------------------------------------------
        [Theory]
        [InlineData("Administrator", "AdiminPF13", 1)]
        [InlineData("Tientv", "TienPF13", 2)]
        [InlineData("Phucvv", "abcde", 0)]
        [InlineData("Phuocabc", "PhuocPF13", 0)]
        [InlineData("Tienabc", "Tienabc", 0)]
        public void LoginTest(string username, string password, int expected)
        {
            cashier.UserName = username;
            cashier.Password = password;
            int result = dal.Login(cashier).Role;
            Assert.True(result == expected);
        }
        // ---------------------------------------------
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetByID1(int cashier_id){
            Cashier cashier = dal.GetByID(cashier_id);
            Assert.True(cashier != null);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(999999999)]
        public void GetByID2(int cashier_id){
            Cashier cashier = dal.GetByID(cashier_id);
            Assert.True(cashier == null);
        }
        //------------------------------------------------
        
    }
}