using System;
using System.Text.RegularExpressions;
using Persistance;
using BL;
using System.Collections.Generic;

namespace ConsoleAppPL
{
    class CashierPL:Menu{
        private InputAndOutputData data = new InputAndOutputData();
        public void MenuCashier(Cashier cashier){
            string[] keyword = new string[]{"ESCAPE", "END", "LEFTARROW", "RIGHTARROW", "UPRROW", "DOWNARROW", "ENTER"};
            string[] tutorial = new string[]{"A: Search By ID", "B: Search By Name", "C: Search By Category", "ALL: Show All", "ESC: Cancel", "End: Create Invoice"};
            string[] menu = new string[]{" 1: Order", " 2: Log Out"};
            Console.Clear();
            Form("Menu Cashier");
            InvoicePL invoicePL = new InvoicePL();
            ProductBL productBL = new ProductBL();
            InvoiceBL invoiceBL = new InvoiceBL();
            List<Invoice> invoices = invoiceBL.GetByStatus(2);
            string choice = string.Empty;
            string current_box = "BoxLeft";
            List<Page> Page_Left = InvoicePages(invoices);
            int box_left = 1;
            int box_right = 1;
            int max_left = Page_Left.Count;
            int max_right = 9;
            ViewLeft(Page_Left[0].View, "List Invoice");
            ViewRight(menu, "Menu");
            ShowNumberPage(Box.PAGE_LEFT.Left-1, Box.PAGE_LEFT.Top-1, box_left, max_left, true);
            choice = current_box;
            do{
                switch(choice){
                    case "Enter":
                        if(current_box == "BoxLeft"){
                            current_box = "BoxRight";
                        }else{
                            current_box = "BoxLeft";
                        }
                        choice = current_box;
                        break;
                    case "Escape":
                        break;
                    case "BoxLeft":
                        data.WriteAt("◄ Current", Box.BOX_LEFT.Right-10, Box.BOX_LEFT.Top);
                        data.WriteAt(new string(' ', 13), Box.BOX_RIGHT.Right-13, Box.BOX_RIGHT.Top);
                        choice = data.GetChoice("Your Choice", keyword);
                        break;
                    case "BoxRight":
                        data.WriteAt("◄ Current", Box.BOX_RIGHT.Right-10, Box.BOX_RIGHT.Top);
                        data.WriteAt(new string(' ', 13), Box.BOX_LEFT.Right-13, Box.BOX_LEFT.Top);
                        choice = data.GetChoice("Your Choice", keyword);
                        break;
                    case "LetfArrow":
                        if(current_box == "BoxLeft"){
                            if(box_left > 1){
                                box_left--;
                            }
                        }else{
                            if(box_right > 1){
                                box_right--;
                            }
                        }
                        ViewLeft(Page_Left[0].View, "List Invoice");
                        data.WriteAt("◄ Current", Box.BOX_LEFT.Right-10, Box.BOX_LEFT.Top);
                        data.WriteAt(new string(' ', 13), Box.BOX_RIGHT.Right-13, Box.BOX_RIGHT.Top);
                        choice = data.GetChoice("Your Choice", keyword);
                        break;
                    case "RightArrow":
                        if(current_box == "BoxLeft"){
                            if(box_left < max_left){
                                box_left++;
                            }
                        }else{
                            if(box_right < max_right){
                                box_right++;
                            }
                        }
                        ViewLeft(Page_Left[0].View, "List Invoice");
                        data.WriteAt("◄ Current", Box.BOX_LEFT.Right-10, Box.BOX_LEFT.Top);
                        data.WriteAt(new string(' ', 13), Box.BOX_RIGHT.Right-13, Box.BOX_RIGHT.Top);
                        choice = data.GetChoice("Your Choice", keyword);
                        break;
                    default:
                        if(current_box == "BoxLeft"){
                            //
                        }else{
                            
                        }
                        break;
                }

            }while(choice != "Escape");
            
        }
    }
}