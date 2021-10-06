using System;
using System.Collections.Generic;

namespace Persistance
{
    public class Invoice{
        public int? InvoiceNo{set; get;}
        public Cashier InvoiceCashier{set; get;}
        public List<Product> ListProduct {set; get;}
        public DateTime Date{set; get;}
        public double Total{set; get;}
        public int PaymentMethod{set; get;}
        public int Status{set; get;}
        public string Note{set; get;}
        public string InvoiceInfo{
            get{
                return string.Format("{0} {1}", InvoiceNo, Date);
            }
        }
        public Invoice(){
            ListProduct = new List<Product>();
            Date = DateTime.Now;
            Status = 1;
        }
    }
}