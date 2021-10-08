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
            string choice = "update";
            do{
                switch(choice.ToLower()){
                    case "update":
                        order = ShowOrder(invoice.ListProduct);
                        current_page_right = 1;  max_page_right = order.Count; // box right
                        pages = current_pages;
                        current_page_left = 1;
                        max_page_left = pages.Count;
                        key_index = pages[0].KeyIndex;
                        isControlRight = false;
                        choice = "view";
                        break;
                    case "view":
                        ShowNameMenu("Menu Invoice");
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
                    case "a":// search by id
                        string search = data.GetChoice("Enter Product ID", new string[]{"Escape"});
                        int id;
                        int.TryParse(search, out id);
                        ProductBL productBL = new ProductBL();
                        Product prod = productBL.SearchByID(id);
                        if(prod != null){
                            prod = pl.MenuProduct(prod);
                            if(prod.Quantity > 0){
                                invoice.ListProduct.Add(prod);
                                choice = "update";
                            }else
                            {
                                choice = "view";
                            }
                        }else
                        {
                            InvalidSelection("Not Found!");
                            choice = "view";
                        }
                        break;
                    case "b":// search by name
                        List<Page> p = pl.SearchByName();
                        if(p != null){
                            current_pages = p;
                            choice = "update";
                        }else{
                            choice = "view";
                        }
                        break;
                    case "c":// search by category 
                        List<Page> ct = pl.SearchByCategory();
                        if(ct != null){
                            current_pages = ct;
                            choice = "update";
                        }else{
                            choice = "view";
                        }
                        break;
                    case "end": // -> create invoice
                        if(invoice.ListProduct.Count == 0){
                            InvalidSelection("Order is empty!");
                            choice = "view";  
                        }else
                        {
                            if(CreateInvoice(invoice)){
                            invoice = new Invoice();
                                choice = "Escape";
                            }else
                            {
                              choice = "view";
                            }
                        }
                        break;
                    case "leftarrow":
                        if(current_page_left > 1){
                            current_page_left--;
                        }
                        choice = "view";
                        break;
                    case "rightarrow":
                        if (current_page_left < max_page_left){
                            current_page_left++;
                        }
                        choice = "view";
                        break;
                    case "uparrow":
                        if(current_page_right > 1){
                            current_page_right--;
                        }
                        choice = "view";
                        break;
                    case "downarrow":
                        if(current_page_right < max_page_right){
                            current_page_right++;
                        }
                        choice = "view";
                        break;
                    case "enter":
                        if(isControlRight){
                            isControlRight = false;
                        }else{
                            isControlRight = true;
                        }
                        choice = "view";
                        break;
                    case "all":
                        current_pages = ProductPages(bL.SearchByName(0, new Product(){ProductName = ""}));
                        choice = "update";
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
                                choice = "update";
                            }else{
                                product = pl.MenuProduct(bL.SearchByID(value));
                                if(product.Quantity > 0){
                                    invoice.ListProduct.Add(product);
                                }
                                choice = "update";
                            }
                        }else{
                            InvalidSelection("You choice invalid!");
                            choice = "view";
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
                            choice = "view";
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
            data.ClearAt(Box.BOX_TUTORIAL);
            string choice = string.Empty;
            double total = invoice.ListProduct.Select(
                (p) => { 
                    return (p.Price + p.ListTopping.Select((tp) => {return tp.UnitPrice;}).Sum()) * p.Quantity;
                }
            ).Sum();
            string[] order_information = new string[]{
                string.Format("{0, -30} {1}", "Number of Cups: ", invoice.ListProduct.Select(p => {return p.Quantity;}).Sum()),
                string.Format("{0, -30} {1}", "Total: ", string.Format(new CultureInfo("vi-VN"), "{0:#,##0}đ", total))
            };
            invoice.Total = total;
            ViewBox(order_information, "Order Information", true);
            do{
                choice = ViewBox(menu, menu.Length, new string[]{""}, "Menu", false);
                switch(choice){
                    case "1":
                        invoice.Status = 1;
                        try{
                            if(bl.CreateInvoice(invoice)){
                                ExportInvoice(invoice);
                                invoice = new Invoice();
                                return true;
                            }else
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                data.WriteAt("Invoice creation failed! Press any key to return menu", Box.BOX_CHOICE.Left, Box.BOX_CHOICE.Bott);
                                Console.ResetColor();
                            }
                        }catch(Exception ex){
                            InvalidSelection(ex.Message);
                        }
                        break;
                    case "2":
                        invoice.Status = 2;
                        InvoiceProgressing.Invoices.Add(invoice);
                        Console.ForegroundColor = ConsoleColor.Green;
                        data.WriteAt("Successful! Press any key to return menu...", Box.BOX_CHOICE.Left, Box.BOX_CHOICE.Bott);
                        Console.ResetColor();
                        Console.ReadKey();
                        invoice = new Invoice();
                        return true;
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
            double total = 0;
            foreach(var p in invoice.ListProduct){
                price = 0;
                price = p.Price;
                if(p.Size == "2") price += 6000;
                foreach(var tp in p.ListTopping){
                    price += tp.UnitPrice;
                }
                total += price*p.Quantity;
            }
            Console.SetCursorPosition(Box.BOX_RIGHT.Left, Box.BOX_RIGHT.Bott);
            data.TextColor(string.Format(" >| Price: {0, 53}K", (total)/1000), ConsoleColor.Blue);
        }
    }
}