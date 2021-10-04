using System;
using Persistance;
using BL;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleAppPL
{
    public class InvoicePL : Menu
    {
        private InputAndOutputData data = new InputAndOutputData();
        private InvoiceBL bl = new InvoiceBL();
        public void MenuInvoice(Cashier cashier){
            string[] keyword = new string[]{"End", "LeftArrow", "RightArrow", "UpArrow", "DownArrow", "Escape"};
            string[] tutorial = new string[]{"A: Search By ID", "B: Search By Name", "C: Search By Category", "ALL: Show All", "ESC: Cancel", "End: Create Invoice"};
            Invoice invoice = new Invoice(){InvoiceCashier = cashier};
            Product product = new Product();
            ProductBL bL = new ProductBL();
            List<Page> pages = new List<Page>();
            List<Page> order = ProductPages(invoice.ListProduct);
            List<Page> current_pages = ProductPages(bL.SearchByName(0, new Product(){ProductName = ""}));
            // 
            int current_page_left = 0;int max_page_left = 0;
            bool isControlRight = false;
            int current_page_right = 1; int max_page_right = order.Count; // box right
            ProductPL pl = new ProductPL();
            Dictionary<int, int> key_index = null;
            string choice = "Update";
            do{
                switch(choice){
                    case "Update":
                        order = ProductPages(invoice.ListProduct);
                        current_page_right = 1;  max_page_right = order.Count; // box right
                        pages = current_pages;
                        current_page_left = 1;
                        max_page_left = pages.Count;
                        key_index = pages[0].KeyIndex;
                        isControlRight = false;
                        choice = "View";
                        break;
                    case "View":
                        ViewBox(pages[current_page_left - 1].View, "List Product", false);
                        ViewBox(order[current_page_right - 1].View, "Order", true);
                        ShowNumberPage(current_page_left, max_page_left, false);
                        ShowNumberPage(current_page_right, max_page_right, true);
                        CurrentBox(isControlRight);
                        BoxTutorial(tutorial);
                        if(isControlRight){
                            key_index = order[current_page_right-1].KeyIndex;
                        }else{
                            key_index = pages[current_page_left-1].KeyIndex;
                        }
                        choice = data.GetChoice("Your Choice", keyword);
                        break;
                    case "A": case "a":// search by id 
                        
                        break;
                    case "B": case "b":// search by name
                        List<Page> p = pl.SearchByName();
                        if(p != null){
                            current_pages = p;
                            choice = "Update";
                        }else{
                            choice = "View";
                        }
                        break;
                    case "C": case "c":// search by category
                    Console.WriteLine("Search By Category"); Console.ReadKey();
                        break;
                    case "End": // -> create invoice
                        CreateInvoice(invoice);
                        choice = "View";
                        break;
                    case "LeftArrow":
                        if(current_page_left > 1){
                            current_page_left--;
                        }
                        choice = "View";
                        break;
                    case "RightArrow":
                        if (current_page_left < max_page_left){
                            current_page_left++;
                        }
                        choice = "View";
                        break;
                    case "UpArrow":
                        if(current_page_right > 1){
                            current_page_right--;
                        }
                        choice = "View";
                        break;
                    case "DownArrow":
                        if(current_page_right < max_page_right){
                            current_page_right++;
                        }
                        choice = "View";
                        break;
                    case "Enter":
                        if(isControlRight){
                            isControlRight = false;
                        }else{
                            isControlRight = true;
                        }
                        choice = "View";
                        break;
                    case "ALL":
                        break;
                    case "Escape":
                        break;
                    default:
                        int key, value;
                        int.TryParse(choice, out key);
                        if(key_index.TryGetValue(key, out value)){
                            if(isControlRight){
                                // next right
                            }else{
                                product = pl.MenuProduct(bL.SearchByID(value));
                                if(product.Quantity > 0){
                                    invoice.ListProduct.Add(product);
                                }
                                choice = "Update";
                            }
                        }else{
                            InvalidSelection("You choice invalid!");
                            choice = "View";
                        }
                        break;
                }
            }while(choice != "Escape");
        }
        public void CreateInvoice(Invoice invoice){
            string[] menu = new string[]{"1. Payment Confirmation", "2. Waiting For Progressing", "3. Add Notes", "4. Back"};
            string choice = string.Empty;
            do{
                choice = ViewBox(menu, menu.Length, new string[]{""}, "Menu", false);
                switch(choice){
                    case "1":
                        if(bl.CreateInvoice(invoice)){
                            Console.WriteLine("Complete!");
                        }else{
                            Console.WriteLine("Not complete");
                        }
                        break;
                    case "2":
                        break;
                    case "3":
                        break;
                    case "4":
                        break;
                    default:
                        InvalidSelection("You choice invalid!");
                        break;
                }
            }while(choice != "4");
        }
    }
}