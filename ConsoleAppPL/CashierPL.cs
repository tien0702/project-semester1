using System;
using System.Text.RegularExpressions;
using Persistance;
using BL;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleAppPL
{
    class CashierPL:Menu{
        private InputAndOutputData data = new InputAndOutputData();
        public void MenuCashier(Cashier cashier){
            string[] keyword = new string[]{"Escape", "LeftArrow", "RightArrow"};
            string[] menu = new string[]{" A: Order", " ESC: Log Out"};
            InvoiceBL invoiceBL = new InvoiceBL();
            List<Invoice> invoices = invoiceBL.GetByStatus(2);
            List<Page> pages = InvoicePages(invoices);
            int current_page = 1;
            int max_page = pages.Count;
            string choice = string.Empty;
            Form("Menu Cashier");
            ViewBox(pages[current_page-1].View, "List Invoice", false);
            ViewBox(menu, "Menu", true);
            ShowNumberPage(current_page, max_page, false);
            do{
                choice = data.GetChoice("Your Choice", keyword);
                switch(choice){
                    case "A": case "a":
                        InvoicePL pl = new InvoicePL();
                        pl.MenuInvoice(cashier);
                        break;
                    case "LeftArrow":
                        if(current_page > 1){
                            current_page--;
                            ViewBox(pages[current_page - 1].View, "List Invoice", false);
                            ShowNumberPage(current_page, max_page, false);
                        }
                        break;
                    case "RightArrow":
                        if (current_page < max_page){
                            current_page++;
                            ViewBox(pages[current_page -1].View, "List Invoice", false);
                            ShowNumberPage(current_page, max_page, false);
                        }
                        break;
                    case "Escape":
                        break;
                    default:
                        int key, value;
                        int.TryParse(choice, out key);
                        if(pages[current_page-1].KeyIndex.TryGetValue(key, out value)){
                            Console.WriteLine("Inted");
                        }else{
                            Console.SetCursorPosition(Box.BOX_CHOICE.Left, Box.BOX_CHOICE.Bott);
                            data.TextColor(" Invalid!", ConsoleColor.Red);
                            Console.ReadKey();
                        }
                        break;
                }
            }while(choice != "Escape");
        }
    }
}