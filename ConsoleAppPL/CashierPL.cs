using System;
using Persistance;
using BL;
using System.Collections.Generic;

namespace ConsoleAppPL
{
    class CashierPL:Menu{
        private InputAndOutputData data = new InputAndOutputData();
        private CashierBL bl = new CashierBL();
        public void MenuCashier(Cashier cashier){
            string[] keyword = new string[]{"Escape", "LeftArrow", "RightArrow"};
            string[] menu = new string[]{"A    : Chọn món", string.Format("ESC  : {0}", (cashier.CashierId == 1)?("Thoát"):("Đăng xuất"))};
            string[] tutorial = new string[]{"Esc: Trở Lại", "Enter: Xác Nhận", "End: Xoá"};
            InvoiceBL invoiceBL = new InvoiceBL();
            List<Invoice> invoices;
            string choice = string.Empty;
            do{
                invoices = invoiceBL.GetByStatus(2);
                List<Page> pages = InvoicePages(invoices);
                int current_page = 1;
                int max_page = pages.Count;
                data.ClearAt(new Coordinates(){Left = 1, Right = 70, Top = 6, Bott = 6});
                data.ClearAt(Box.BOX_TUTORIAL);
                Form((cashier.CashierId == 1)?("Admin"):("Nhân Viên"));
                data.WriteAt(string.Format(" >| Xin chào, {0}!", (cashier.CashierId == 1)?("Admin"):(cashier.FullName)), 1, 6);
                ViewBox(pages[current_page - 1].View, "Danh sách hoá đơn chờ xử lý", false);
                BoxTutorial(menu);
                ShowNumberPage(current_page, max_page, false);
                choice = data.GetChoice("Lựa Chọn", keyword);
                switch(choice){
                    case "A": case "a":
                        InvoicePL pl = new InvoicePL();
                        pl.MenuInvoice(cashier);
                        ViewBox(pages[current_page - 1].View, "Danh sách hoá đơn", false);
                        BoxTutorial(menu);
                        ShowNumberPage(current_page, max_page, false);
                        data.ClearAt(Box.BOX_TUTORIAL);
                        data.ClearAt(Box.PAGE_RIGHT);
                        break;
                    case "LeftArrow":
                        if(current_page > 1){
                            current_page--;
                            ViewBox(pages[current_page - 1].View, "Danh sách hoá đơn", false);
                            ShowNumberPage(current_page, max_page, false);
                        }
                        break;
                    case "RightArrow":
                        if (current_page < max_page){
                            current_page++;
                            ViewBox(pages[current_page -1].View, "Danh sách hoá đơn", false);
                            ShowNumberPage(current_page, max_page, false);
                        }
                        break;
                    case "Escape":
                        if(cashier.CashierId == 2){
                            choice = (ConfirmSelection("Bạn chắc chắn muốn đăng xuất?"))?("Escape"):(string.Empty);
                        }
                        break;
                    default:
                        int key, value;
                        string payment = string.Empty;
                        int.TryParse(choice, out key);
                        if(pages[current_page-1].KeyIndex.TryGetValue(key, out value)){
                            do{
                                BoxTutorial(tutorial);
                                payment = data.GetChoice("Xác nhận thanh toán", new string[]{"Escape", "End"});
                                switch(payment){
                                    case "End":
                                        bool confirm;
                                        if(confirm = ConfirmSelection("Bạn chắc chắn muốn xoá?")){
                                            invoices[value].Status = 3;
                                            if(invoiceBL.UpdateStatus(invoices[value])){
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                data.WriteAt("Đã xoá! Nhấn phím bất kỳ để cập nhật...", Box.BOX_CHOICE.Left, Box.BOX_CHOICE.Bott);
                                                Console.ResetColor();
                                                Console.ReadKey();
                                            }else
                                            {
                                                InvalidSelection("Có lỗi, không thể xoá!");
                                            }
                                        }
                                        break;
                                    case "Escape":
                                        break;
                                    case "Enter":
                                    try{
                                        invoices[value].InvoiceCashier = cashier;
                                        invoices[value].Status = 1;
                                        if (invoiceBL.CreateInvoice(invoices[value]))
                                        {
                                            ExportInvoice(invoices[value]);
                                            invoices.Remove(invoices[value]);
                                        }
                                    }catch (Exception ex){
                                        InvalidSelection(ex.Message);
                                    }
                                        break;
                                    default:
                                        InvalidSelection("Lựa chọn sai!");
                                        break;
                                }
                            }while(payment != "Escape" && payment != "Enter" && payment != "End");
                        }else{
                            InvalidSelection("Lựa chọn sai!");
                        }
                        break;
                }
            }while(choice != "Escape");
        }
        public void MenuAdmin(Cashier cashier){
            ProductPL pl = new ProductPL();
            string[] menu = new string[]{"1. Cập nhật sản phẩm", "2. Quản lý nhân viên", "3. Thống kê doanh thu", "4. Bán hàng"};
            string name_box = "Menu";
            string[] tutorial = new string[]{"ESC: Đăng xuất"};
            string[] keyword = new string[]{"Escape"};
            Form((cashier.CashierId == 1)?("Admin"):("Nhân Viên"));
            string choice = string.Empty;
            do{
                data.ClearAt(Box.BOX_RIGHT);
                BoxTutorial(tutorial);
                choice = ViewBox(menu, 4, keyword, name_box, false);
                switch(choice){
                    case "1":
                        string search = data.GetChoice("Nhập ID sản phẩm", new string[]{"Escape"});
                        int id;
                        int.TryParse(search, out id);
                        ProductBL productBL = new ProductBL();
                        Product prod = productBL.SearchByID(id);
                        if(prod != null){
                            pl.UpdateProduct(prod);
                        }else{
                            InvalidSelection("Không tìm thấy!");
                            choice = "view";
                        }
                        break;
                    case "2":
                        ManageStaff(1);
                        break;
                    case "3":
                        InvoicePL invoicePL = new InvoicePL();
                        invoicePL.RevenueStatistics();
                        break;
                    case "4":
                        MenuCashier(cashier);
                        break;
                    case "Escape":
                        choice = (ConfirmSelection("Bạn chắc chắn muốn đăng xuất?"))?("Escape"):(string.Empty);
                        break;
                    default:
                        InvalidSelection("Lựa chọn sai!");
                        break;
                }
            }while(choice != "Escape");
        }
        public void ManageStaff(int staff_id){
            if(staff_id != 1) return;
            string[] menu = new string[]{"1. Tìm kiếm nhân viên"};
            string choice = string.Empty;
            string[] keyword = new string[]{"Escape"};
            string name_box = "Quản lý nhân viên";
            Cashier cashier = null;

            do{
                choice = ViewBox(menu, 1, keyword, name_box, false);
                switch(choice){
                    case "1":
                        string id = data.GetChoice("Nhập ID nhân viên", keyword);
                        int idd;
                        int.TryParse(id, out idd);
                        cashier = bl.SearchByID(idd);
                        if(cashier == null){
                            InvalidSelection("Không tìm thấy!");
                        }else
                        {
                            StaffDetail(cashier);
                        }
                        break;
                    case "Escape":
                        break;
                    default:
                        InvalidSelection("Lựa chọn sai!");
                        break;
                }
            }while(choice != "Escape");
        }
        public void StaffDetail(Cashier cashier){
            string[] info = new string[5];
            info[0] = string.Format("ID            : {0}", cashier.CashierId);
            info[1] = string.Format("Họ Tên        : {0}", cashier.FullName);
            info[2] = string.Format("Số điện thoại : {0}", (cashier.Phone == null) ? ("...") : (cashier.Phone));
            info[3] = string.Format("Email         : {0}", (cashier.Email == null) ? ("...") : (cashier.Email));
            info[4] = string.Format("Địa chỉ       : {0}", (cashier.Address == null) ? ("....") : (cashier.Address));
            ViewBox(info, "Thông tin nhân viên", false);
            Console.ReadKey();
        }
        public Cashier NewStaff(){
            Cashier cashier = new Cashier();
            string choice = string.Empty;
                while(!ValidData.IsUserName(cashier.UserName = data.GetChoice("User Name", new string[]{""}))) InvalidSelection("Tên đăng nhập phải dài hơn 4 ký tự");
                while(!ValidData.IsPassword(cashier.Password = data.GetChoice("Password", new string[]{""}))) InvalidSelection("Mật khẩu phải dài hơn 6 ký tự");
                while(ValidData.IsNamePerson(cashier.FirstName = data.GetChoice("Tên", new string[]{""}))) InvalidSelection("Tên không hợp lệ");
                while(ValidData.IsNamePerson(cashier.MiddleName = data.GetChoice("Tên đệm", new string[]{""}))) InvalidSelection("Tên không hợp lệ");
                while(ValidData.IsNamePerson(cashier.MiddleName = data.GetChoice("Họ", new string[]{""}))) InvalidSelection("Tên không hợp lệ");
                return cashier;
        }
    }
}