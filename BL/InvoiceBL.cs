using System;
using DAL;
using Persistance;
using System.Collections.Generic;

namespace BL
{
    public class InvoiceBL{
        private InvoiceDAL dal = new InvoiceDAL();
        public Invoice GetByNo(int? invoice_no){
            return dal.GetByNo(invoice_no);
        }
        public bool CreateInvoice(Invoice invoice){
            return dal.CreateInvoice(invoice);
        }
        public List<Invoice> GetByStatus(int status){
            return dal.GetByStatus(status);
        }
    }
}