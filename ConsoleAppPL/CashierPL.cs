using System;
using Persistance;
using BL;
using System.Collections.Generic;

namespace ConsoleAppPL
{
    class CashierPL:Menu{
        private InputAndOutputData data = new InputAndOutputData();
        public void MenuCashier(Cashier cashier){
            string[] keyword = new string[]{"Escape", "LeftArrow", "RightArrow"};
            string[] menu = new string[]{" A: Order", " ESC: Log Out"};
            string[] tutorial = new string[]{"Esc: Back", "Enter: Confirm", "End: Remove"};
            InvoiceBL invoiceBL = new InvoiceBL();
                string choice = string.Empty;
            do{
                List<Page> pages = InvoicePages(InvoiceProgressing.Invoices);
                int current_page = 1;
                int max_page = pages.Count;
                data.ClearAt(new Coordinates(){Left = 1, Right = 70, Top = 6, Bott = 6});
                data.ClearAt(Box.BOX_TUTORIAL);
                Form("Menu Cashier");
                data.WriteAt(string.Format(" >| Cashier: {0}", cashier.FullName), 1, 6);
                ViewBox(pages[current_page - 1].View, "List Invoice", false);
                ViewBox(menu, "Menu", true);
                ShowNumberPage(current_page, max_page, false);
                choice = data.GetChoice("Your Choice", keyword);
                switch(choice){
                    case "A": case "a":
                        InvoicePL pl = new InvoicePL();
                        pl.MenuInvoice(cashier);
                        ViewBox(pages[current_page - 1].View, "List Invoice", false);
                        ViewBox(menu, "Menu", true);
                        ShowNumberPage(current_page, max_page, false);
                        data.ClearAt(Box.BOX_TUTORIAL);
                        data.ClearAt(Box.PAGE_RIGHT);
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
                        string payment = string.Empty;
                        int.TryParse(choice, out key);
                        if(pages[current_page-1].KeyIndex.TryGetValue(key, out value)){
                            do{
                                BoxTutorial(tutorial);
                                payment = data.GetChoice("Paymant confirmation", new string[]{"Escape", "End"});
                                switch(payment){
                                    case "End":
                                        InvoiceProgressing.Invoices.Remove(InvoiceProgressing.Invoices[value]);
                                        break;
                                    case "Escape":
                                        break;
                                    case "Enter":
                                    try{
                                        InvoiceProgressing.Invoices[value].InvoiceCashier = cashier;
                                        if (invoiceBL.CreateInvoice(InvoiceProgressing.Invoices[value]))
                                        {
                                            ExportInvoice(InvoiceProgressing.Invoices[value]);
                                            InvoiceProgressing.Invoices.Remove(InvoiceProgressing.Invoices[value]);
                                        }else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            data.WriteAt("Invoice not successful! Press any key to return menu", Box.BOX_CHOICE.Left, Box.BOX_CHOICE.Bott);
                                            Console.ResetColor();
                                        }
                                    }catch (Exception ex){
                                        InvalidSelection(ex.Message);
                                    }
                                        break;
                                    default:
                                        InvalidSelection("You choice invalid!");
                                        break;
                                }
                            }while(payment != "Escape" && payment != "Enter" && payment != "End");
                        }else{
                            InvalidSelection("You choice invalid!");
                        }
                        break;
                }
            }while(choice != "Escape");
        }
    }
}