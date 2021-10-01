using System;
using System.Text.RegularExpressions;
using Persistance;
using BL;
using System.Collections.Generic;

namespace ConsoleAppPL
{
    public class InvoicePL : Menu
    {
        private InputAndOutputData data = new InputAndOutputData();
        public void MenuInvoice(Cashier cashier){
            string[] keyword = new string[]{"End", "LeftArrow", "RightArrow", "UpArrow", "DownArrow", "Escape"};
            string[] tutorial = new string[]{"A: Search By ID", "B: Search By Name", "C: Search By Category", "ALL: Show All", "ESC: Cancel", "End: Create Invoice"};
            Invoice invoice = new Invoice();
            ProductBL bL = new ProductBL();
            List<Page> pages = ProductPages(bL.SearchByName(0, new Product(){ProductName = ""}));
            List<Page> order = ProductPages(invoice.ListProduct);
            string choice = string.Empty;
            int current_page_left = 1; int max_page_left = pages.Count; // box left
            int current_page_right = 1; int max_page_right = order.Count; // box right
            bool isControlRight = true;
            Dictionary<int, int> key_index = pages[0].KeyIndex;
            ViewBox(pages[0].View, "List Product", false);
            ViewBox(order[0].View, "Order", true);
            ShowNumberPage(current_page_left, max_page_left, false);
            ShowNumberPage(current_page_right, max_page_right, true);
            BoxTutorial(tutorial);
            do{
                CurrentBox(isControlRight);
                choice = data.GetChoice("Your Choice", keyword);
                switch(choice){
                    case "A": case "a":// search by id 
                    Console.WriteLine("Search By ID"); Console.ReadKey();
                        break;
                    case "B": case "b":// search by name
                    Console.WriteLine("Search By Name"); Console.ReadKey();
                        break;
                    case "C": case "c":// search by category
                    Console.WriteLine("Search By Category"); Console.ReadKey();
                        break;
                    case "End": // -> create invoice
                    Console.WriteLine("Create Invoice"); Console.ReadKey();
                        break;
                    case "LeftArrow":
                        if(current_page_left > 1){
                            current_page_left--;
                            ViewBox(pages[current_page_left - 1].View, "List Invoice", false);
                            ShowNumberPage(current_page_left, max_page_left, false);
                        }
                        break;
                    case "RightArrow":
                        if (current_page_left < max_page_left){
                            current_page_left++;
                            ViewBox(pages[current_page_left -1].View, "List Invoice", false);
                            ShowNumberPage(current_page_left, max_page_left, false);
                        }
                        break;
                    case "UpArrow":
                        if(current_page_right > 1){
                            current_page_right--;
                            ViewBox(order[current_page_right - 1].View, "List Invoice", true);
                            ShowNumberPage(current_page_right, max_page_right, true);
                        }
                        break;
                    case "DownArrow":
                        if(current_page_right < max_page_right){
                            current_page_right++;
                            ViewBox(order[current_page_right - 1].View, "List Invoice", true);
                            ShowNumberPage(current_page_right, max_page_right, true);
                        }
                        break;
                    case "Enter":
                        if(isControlRight){
                            isControlRight = false;
                            key_index = pages[current_page_left-1].KeyIndex;
                        }else{
                            isControlRight = true;
                            key_index = order[current_page_right-1].KeyIndex;
                        }
                        break;
                    case "Escape":
                        break;
                    default:
                        if(choice.ToLower() == "all"){
                            //code
                            break;
                        }
                        int key, value;
                        int.TryParse(choice, out key);
                        if(key_index.TryGetValue(key, out value)){
                            Console.WriteLine("Inted");
                            if(isControlRight){
                                // next right
                            }else{
                                // next left
                            }
                        }else{
                            Console.SetCursorPosition(Box.BOX_CHOICE.Left, Box.BOX_CHOICE.Bott);
                            data.TextColor(" Invalid!", ConsoleColor.Red);
                            Console.ReadKey();
                        }
                        break;
                }
            }while(choice != "Escape");
        }
        public void CurrentBox(bool isRight){
            if(isRight){
                Console.SetCursorPosition(Box.BOX_RIGHT.Right-10, Box.BOX_RIGHT.Top);
                data.TextColor("<< Current", ConsoleColor.DarkYellow);
                data.WriteAt(new string(' ', 10), Box.BOX_LEFT.Right-10, Box.BOX_LEFT.Top);
            }else{
                Console.SetCursorPosition(Box.BOX_LEFT.Right-10, Box.BOX_LEFT.Top);
                data.TextColor("<< Current", ConsoleColor.DarkYellow);
                data.WriteAt(new string(' ', 10), Box.BOX_RIGHT.Right-10, Box.BOX_RIGHT.Top);
            }
        }
    }
}