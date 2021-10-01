using System;
using System.Text.RegularExpressions;
using Persistance;
using BL;
using System.Collections.Generic;

namespace ConsoleAppPL
{
    public class InvoicePL : Menu
    {
        public void MenuInvoice(Cashier cashier){
            
            string[] keyword = new string[]{"ESCAPE", "END", "LEFTARROW", "RIGHTARROW", "UPRROW", "DOWNARROW", "ENTER"};
            string[] tutorial = new string[]{"A: Search By ID", "B: Search By Name", "C: Search By Category", "ALL: Show All", "ESC: Cancel", "End: Create Invoice"};
        }
    }
}