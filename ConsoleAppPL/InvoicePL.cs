using System;
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
            List<Page> pages = new List<Page>();
            List<Page> order = ProductPages(bL.SearchByName(1, new Product(){ProductName = "tra"}));
            List<Page> current_pages = ProductPages(bL.SearchByName(0, new Product(){ProductName = ""}));
            // ProductPages(invoice.ListProduct);
            int current_page_left = 0;
            bool isControlRight = false;
            int max_page_left = 0;
            int current_page_right = 1; int max_page_right = order.Count; // box right
            ProductPL pl = new ProductPL();
            Dictionary<int, int> key_index = null;
            string choice = "Update";
            do{
                switch(choice){
                    case "Update":
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
                        List<Page> p = SearchByName();
                        if(p != null){
                            current_pages = p;
                            choice = "Update";
                        }
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
                                Product product1 = bL.SearchByID(value);
                                pl.MenuProduct(product1);
                            }
                        }else{
                            InvalidSelection("You choice invalid!");
                        }
                        choice = "View";
                        break;
                }
            }while(choice != "Escape");
        }
        public List<Page> SearchByName(){
            List<Page> pages = null;
            ProductBL bL = new ProductBL();
            string search = data.GetChoice("Enter Product Name", new string[]{"Escape"});
            if(search != "Escape"){
                pages = ProductPages(bL.SearchByName(1, new Product(){ProductName = search}));
            }
            return pages;
        }
    }
}