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
            string[] keyword = new string[]{"End", "LeftArrow", "RightArrow", "UpArrow", "DownArrow"};
            string[] tutorial = new string[]{"A: Search By ID", "B: Search By Name", "C: Search By Category", "ALL: Show All", "ESC: Cancel", "End: Create Invoice"};
            string choice = string.Empty;
            ProductBL bL = new ProductBL();
            List<Page> pages = ProductPages(bL.SearchByName(0, new Product(){ProductName = ""}));
            ViewBox(pages[0].View, "List Product", false);
            Dictionary<int, int> key_index = pages[0].KeyIndex;
            BoxTutorial(tutorial);
            do{
                switch(choice){
                    case "A": case "a":
                        break;
                    case "B": case "b":
                        break;
                    case "C": case "c":
                        break;
                    case "End":
                        // -> create invoice
                        break;
                    case "LeftArrow":
                        break;
                    case "RightArrow":
                        break;
                    case "UpArrow":
                        break;
                    case "DownArrow":
                        break;
                    default:
                        break;
                }
            }while(choice != "Escape");
        }
    }
}