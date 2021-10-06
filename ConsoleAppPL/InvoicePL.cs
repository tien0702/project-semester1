using System;
using Persistance;
using BL;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace ConsoleAppPL
{
    public class InvoicePL : Menu
    {
        private InputAndOutputData data = new InputAndOutputData();
        private InvoiceBL bl = new InvoiceBL();
        public void MenuInvoice(Cashier cashier){
            string[] keyword = new string[]{"End", "LeftArrow", "RightArrow", "UpArrow", "DownArrow", "Escape"};
            string[] tutorial = new string[]{"A: Search By ID", "B: Search By Name", "C: Search By Category", "ALL: Show All", "ESC: Cancel", "End: Payment"};
            Invoice invoice = new Invoice(){InvoiceCashier = cashier};
            Product product = new Product();
            ProductBL bL = new ProductBL();
            List<Page> pages = new List<Page>();
            List<Page> order = ShowOrder(invoice.ListProduct);
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
                        order = ShowOrder(invoice.ListProduct);
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
                        Pricing(invoice);
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
                        if(invoice.ListProduct.Count == 0){
                            InvalidSelection("Order is empty!");
                            choice = "View";  
                        }else
                        {
                            if(CreateInvoice(invoice)){
                            invoice = new Invoice();
                            Console.WriteLine(invoice.ListProduct.Count);
                                choice = "Escape";
                            }else
                            {
                              choice = "View";
                            }
                        }
                        
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
                    case "all":
                        current_pages = ProductPages(bL.SearchByName(0, new Product(){ProductName = ""}));
                        choice = "Update";
                        break;
                    default:
                        int key, value;
                        int.TryParse(choice, out key);
                        if(key_index.TryGetValue(key, out value)){
                            if(isControlRight){
                                product = invoice.ListProduct[key-1];
                                pl.CustomizeQuantity(product);
                                if(product.Quantity == 0){
                                    invoice.ListProduct.Remove(product);
                                }
                                choice = "Update";
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
                if(choice == "Escape")
                {
                    if(invoice == null || invoice.ListProduct.Count == 0){
                        return;
                    }
                    string esc = string.Empty;
                    do
                    {
                        BoxTutorial(new string[] { "Enter: Yes", "ESC: No" });
                        esc = data.GetChoice("Leaving will delete the current listing!", new string[] { "Escape" });
                        if (esc == "Escape")
                        {
                            choice = "View";
                        }
                        else if (esc != "Enter" && esc != "Escape")
                        {
                            InvalidSelection("You choice invalid!");
                        }
                    } while(esc != "Enter" && esc != "Escape");
                }
            }while(choice != "Escape");
        }
        public bool CreateInvoice(Invoice invoice){
            bool result = false;
            string[] menu = new string[]{"1. Payment confirmation", "2. Waiting for progressing", "3. Back"};
            Pricing(invoice);
            string choice = string.Empty;
            double total = invoice.ListProduct.Select(
                (p) => { 
                    return (p.Price + p.ListTopping.Select((tp) => {return tp.UnitPrice;}).Sum()) * p.Quantity;
                }
            ).Sum();
            string[] order_information = new string[]{
                string.Format("{0, -30} {1}", "Number of Cups: ", invoice.ListProduct.Select(p => {return p.Quantity;}).Sum()),
                string.Format("{0, -30} {1}", "Total: ", string.Format(new CultureInfo("vi-VN"), "{0:#,##0.00}đ", total))
            };
            ViewBox(order_information, "Order Information", true);
            do{
                choice = ViewBox(menu, menu.Length, new string[]{""}, "Menu", false);
                switch(choice){
                    case "1":
                        invoice.Status = 1;
                        if(bl.CreateInvoice(invoice)){
                            Console.ForegroundColor = ConsoleColor.Green;
                            data.WriteAt("Invoice creation successful! Press any key to return menu", Box.BOX_CHOICE.Left, Box.BOX_CHOICE.Bott);
                            Console.ResetColor();
                            Console.ReadKey();
                            invoice = new Invoice();
                            return true;
                        }
                        break;
                    case "2":
                        invoice.Status = 2;
                        if(bl.CreateInvoice(invoice)){
                            Console.ForegroundColor = ConsoleColor.Green;
                            data.WriteAt("Invoice creation successful! Press any key to return menu", Box.BOX_CHOICE.Left, Box.BOX_CHOICE.Bott);
                            Console.ResetColor();
                            invoice = new Invoice();
                            Console.ReadKey();
                            return true;
                        }
                        break;
                    case "3":
                        break;
                    default:
                        InvalidSelection("You choice invalid!");
                        break;
                }
            }while(choice != "3");
            return result;
        }
        /// <summary> <c></c> Tính tổng giá của hoá đơn và hiển thị.</summary>
        public void Pricing(Invoice invoice){
            double price = 0;
            price += invoice.ListProduct.Select(
                (p) => { 
                    return (p.Price + p.ListTopping.Select((tp) => {return tp.UnitPrice;}).Sum()) * p.Quantity;
                }
            ).Sum();
            Console.SetCursorPosition(Box.BOX_RIGHT.Left, Box.BOX_RIGHT.Bott);
            data.TextColor(string.Format(" >| Price: {0, 53}K", (price)/1000), ConsoleColor.Blue);
        }
    }
}