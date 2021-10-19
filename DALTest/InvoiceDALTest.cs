using System;
using Xunit;
using Persistance;
using System.Collections.Generic;
using DAL;
using System.Threading.Tasks;
using System.Collections;

namespace DALTest
{
    public class InvoiceDALTest{
        private InvoiceDAL dal = new InvoiceDAL();
        [Theory]
        [InlineData(1001)]
        [InlineData(1002)]
        public void GetByNoTest1(int invoice_no){
            Invoice invoice = dal.GetByNo(invoice_no);
            Assert.True(invoice != null);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(99999)]
        public void GetByNoTest2(int invoice_no){
            Invoice invoice = dal.GetByNo(invoice_no);
            Assert.True(invoice == null);
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetByStatusTest1(int status){
            List<Invoice> invoices = dal.GetByStatus(status);
            Assert.True(invoices != null);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1000)]
        public void GetByStatusTest2(int status){
            List<Invoice> invoices = dal.GetByStatus(status);
            Assert.True(invoices == null);
        }
        // [Fact]
        // public void test(){
        //     var invoice = new Invoice(){InvoiceCashier = new Cashier(){CashierId = 1}};
        //     Assert.ThrowsAny<InvalidOperationException>(() => dal.CreateInvoice(invoice));
        // }
    }
}