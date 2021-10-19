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
            string[] tutorial = new string[]{"A: Tìm Theo ID", "B: Tìm Theo Tên", "C: Tìm Theo Loại", "ALL: Hiển Thị Tất Cả", "ESC: Trở Lại", "End: Thanh Toán"};
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
                        ShowNameMenu("Hoá Đơn");
                        ViewBox(pages[current_page_left - 1].View, "Danh Sách Sản Phẩm", false);
                        ViewBox(order[current_page_right - 1].View, "Danh sách các món đã thêm", true);
                        Console.SetCursorPosition(Box.BOX_RIGHT.Left, Box.BOX_RIGHT.Bott);
                        data.TextColor(string.Format(" >| Tổng cộng: {0, 53}", string.Format(new CultureInfo("vi-VN"), "{0:#,##0}đ", Calculate(invoice))), ConsoleColor.Blue);
                        ShowNumberPage(current_page_left, max_page_left, false);
                        ShowNumberPage(current_page_right, max_page_right, true);
                        CurrentBox(isControlRight);
                        BoxTutorial(tutorial);
                        if(isControlRight){
                            key_index = order[current_page_right-1].KeyIndex;
                        }else{
                            key_index = pages[current_page_left-1].KeyIndex;
                        }
                        choice = data.GetChoice("Lựa Chọn", keyword);
                        break;
                    case "a":// search by id
                        string search = data.GetChoice("Nhập ID sản phẩm", new string[]{"Escape"});
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
                            InvalidSelection("Không tìm thấy!");
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
                            InvalidSelection("Danh sách rỗng!");
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
                            InvalidSelection("Bạn chọn sai!");
                            choice = "view";
                        }
                        break;
                }
                if(choice == "Escape")
                {
                    if(invoice == null || invoice.ListProduct.Count == 0){
                        return;
                    }
                    if(!ConfirmSelection("Rời đi sẽ xoá hết danh sách món đã chọn, vẫn rời?")) choice = "view";
                }
            }while(choice != "Escape");
        }
        public bool CreateInvoice(Invoice invoice){
            bool result = false;
            string[] menu = new string[]{"1. Xác nhận thanh toán", "2. Chờ xử lý", "3. Trở lại"};
            Console.SetCursorPosition(Box.BOX_RIGHT.Left, Box.BOX_RIGHT.Bott);
            data.TextColor(string.Format(" >| Tổng cộng: {0, 53}", string.Format(new CultureInfo("vi-VN"), "{0:#,##0}đ", Calculate(invoice))), ConsoleColor.Blue);
            data.ClearAt(Box.BOX_TUTORIAL);
            string choice = string.Empty;
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
            string[] order_information = new string[]{
                string.Format("{0, -30} {1} cốc", "Số lượng cốc: ", invoice.ListProduct.Select(p => {return p.Quantity;}).Sum()),
                string.Format("{0, -30} {1}", "Tổng cộng   : ", string.Format(new CultureInfo("vi-VN"), "{0:#,##0}đ", total))
            };
            invoice.Total = total;
            ViewBox(order_information, "Thông tin đơn", true);
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
                                InvalidSelection("Tạo hoá đơn thất bại!");
                            }
                        }catch(Exception ex){
                            InvalidSelection(ex.Message);
                        }
                        break;
                    case "2":
                        invoice.Status = 2;
                        try{
                            if(bl.CreateInvoice(invoice)){
                                Console.ForegroundColor = ConsoleColor.Green;
                                data.WriteAt("Hoàn thành! Nhấn phím bất kỳ để tiếp tục...", Box.BOX_CHOICE.Left, Box.BOX_CHOICE.Bott);
                                Console.ResetColor();
                                Console.ReadKey();
                                return true;
                            }else
                            {
                                InvalidSelection("Tạo hoá đơn thất bại!");
                            }
                        }catch(Exception ex){
                            InvalidSelection(ex.Message);
                        }
                        break;
                    case "3":
                        break;
                    default:
                        InvalidSelection("Bạn chọn sai!");
                        break;
                }
            }while(choice != "3");
            return result;
        }
        /// <summary> <c></c> Tính tổng giá của hoá đơn và hiển thị.</summary>
        private double Calculate(Invoice invoice){
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
            return total;
        }
        public void RevenueStatistics(){
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            string[] options = new string[]{"1. Theo ngày", "2. Theo tháng", "3. Tuỳ chọn"};
            double[] info = new double[2];
            string[] str = new string[5];
            string choice = string.Empty;
            do{
                BoxTutorial(new string[]{"ESC: Quay lại"});
                choice = ViewBox(options, 3, new string[]{"Escape"}, "Menu", false);
                switch(choice){
                    case "1":
                        start = GetDate(true);
                        info = bl.RevenueStatistics(start, new DateTime(start.Year, start.Month, start.Day, 23, 59, 00));
                        break;
                    case "2":
                        int month;
                        while(true){
                            try{
                               month = int.Parse(data.GetChoice("Nhập tháng kiểm tra", new string[]{""}));
                                if (month < 0 || month > DateTime.Now.Month){
                                    InvalidSelection("Tháng không hợp lệ");
                                }else{
                                    break;
                                }
                            }catch{
                                InvalidSelection("Ngày không hợp lệ");
                            }
                        }
                        start = new DateTime(2021, month, 01, 00, 00, 00);
                        end = new DateTime(2021, month, int.Parse(new DateTime().AddMonths(month).AddDays(-1).ToString("dd")), 23, 59, 00);
                        info = bl.RevenueStatistics(start, end);
                        break;
                    case "3":
                        start = GetDate(true);
                        end = GetDate(false);
                        info = bl.RevenueStatistics(start, end);
                        break;
                    case "Escape":
                        break;
                    default:
                        InvalidSelection("Lựa chọn sai!");
                        break;
                }
                if(choice == "1" || choice == "2" || choice == "3"){
                    if (info != null)
                    {
                        str[0] = string.Format("Từ                  : {0}", start.ToString("MM/dd/yyyy"));
                        str[1] = string.Format("Đến                 : {0}", (choice == "1") ? start.ToString("MM/dd/yyyy"):end.ToString("MM/dd/yyyy"));
                        str[2] = "──────────────────────────────────────";
                        str[3] = string.Format("Doanh thu           : {0}", string.Format(new CultureInfo("vi-VN"), "{0:#,##0}đ", info[0]));
                        str[4] = string.Format("Tổng số cốc bán được: {0}", info[1]);
                    }
                    else
                    {
                        str[0] = "Không có thông tin!";
                        str[1] = ""; str[2] = ""; str[3] = ""; str[4] = ""; 
                    }
                }
                    
                ViewBox(str, "Thông tin", true);
            }while(choice != "Escape");
        }
        public DateTime GetDate(bool isStartDay){
            DateTime dateTime;
            string getDay;
            int year;
            int month;
            int day;
            while(true){
                getDay = data.GetChoice(isStartDay?("Nhập ngày kiểm tra"):("Nhập ngày kết thúc"), new string[]{""});
                try{
                    day = int.Parse(getDay.Substring(0, 2));
                    month = int.Parse(getDay.Substring(3, 2));
                    year = int.Parse(getDay.Substring(6, 4));
                    if(isStartDay){
                        dateTime = new DateTime(year, month, day, 00, 00, 00);
                        if(dateTime > DateTime.Now){
                            throw new Exception();
                        }
                    }
                    else dateTime = new DateTime(year, month, day, 23, 59, 00);
                    break;
                }catch{
                    InvalidSelection("Ngày không hợp lệ");
                }
            }
            return dateTime;
        }
    }
}