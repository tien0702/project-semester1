using System;
using Persistance;
using BL;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleAppPL
{
    public class Menu{
        private InputAndOutputData data = new InputAndOutputData();
        /// <summary> <c></c> Hiển thị nội dung truyền vào cùng tên của Box, tham số isRight xác định vị trí hiện.</summary>
        public void ViewBox(string[] content, string name_box, bool isRight){
            Coordinates box;
            Coordinates page;
            if(isRight){
                box = Box.BOX_RIGHT;
                page = Box.PAGE_RIGHT;
            }else{
                box = Box.BOX_LEFT;
                page = Box.PAGE_LEFT;
            }
            data.ClearAt(box);
            data.ClearAt(page);
            data.WriteAt(string.Format("≡ " + name_box), box.Left, box.Top);
            if(content[0] == null){
                Console.SetCursorPosition((box.Left+box.Right)/2-20, (box.Bott+box.Top)/2);
                data.TextColor("There are no products in this listing!", ConsoleColor.DarkGray);
                return;
            }
            int max_line = content.Length;
            for(int i = 0; i < max_line; i++){
                data.WriteAt(string.Format(" " + content[i]), box.Left, box.Top+i+1);
            }
        }

        /// <summary> <c></c> Hiển thị nội dung truyền vào cùng tên của Box, tham số isRight xác định vị trí hiện. Return lựa chọn.</summary>
        public string ViewBox(string[] content, int number_choice, string[] keywords, string name_box, bool isRight){
            string choice = string.Empty;
            Coordinates box;
            Coordinates page;
            if(isRight){
                box = Box.BOX_RIGHT;
                page = Box.PAGE_RIGHT;
            }else{
                box = Box.BOX_LEFT;
                page = Box.PAGE_LEFT;
            }
            data.ClearAt(box);
            data.ClearAt(page);
            data.WriteAt(string.Format("≡ " + name_box), box.Left, box.Top);
            if(content == null || content[0] == null){
                Console.SetCursorPosition((box.Left+box.Right)/2-20, (box.Bott+box.Top)/2);
                data.TextColor("There are no products in this listing!", ConsoleColor.DarkGray);
                return "0";
            }
            int max_line = content.Length;
            for(int i = 0; i < max_line; i++){
                data.WriteAt(string.Format(" " + content[i]), box.Left, box.Top+i+1);
            }
            int temp;
            do{
                choice = data.GetChoice("Your Choice", new string[]{"Escape"});
                if(keywords.Contains(choice)){
                    break;
                }else{
                    int.TryParse(choice, out temp);
                    if(temp < 1 || temp > number_choice){
                        InvalidSelection("You choice invalid!");
                    }else{
                        return choice;
                    }
                }
            }while(choice != "Escape");
            return choice;
        }
        public List<Page> ShowOrder(List<Product> products){
            List<Page> pages = new List<Page>();
            string[] Sizes =new string[]{"", "M", "L"};
            if(products == null || products.Count == 0){
                pages.Add(new Page());
                return pages;
            }
            pages = new List<Page>();
            Page page = new Page();
            int count = 0;
            int count_products = products.Count;
            int line = 0;
            int page_number = 1;
            int convert;
            double total;
            while(count < count_products){
                if(line == 0){
                    page = new Page();
                    page.PageNumber = page_number++;
                }
                page.KeyIndex.Add(line+1, (int)products[count].ProductId);
                int.TryParse(products[count].Size.ToCharArray()[0].ToString(), out convert);
                total = products[count].Price;
                foreach(var tp in products[count].ListTopping){
                    total += tp.UnitPrice;
                }
                total = total * products[count].Quantity;
                page.View[line++] = string.Format("{0, -2}. {1, -50}{2, 10}K", line, string.Format("{0} ({1}) x{2}", products[count].ProductName, Sizes[convert], products[count++].Quantity), (total)/1000);
                if(count == count_products){
                    pages.Add(page);
                }
                if(line == 16){
                    line = 0;
                    pages.Add(page);
                }
            }
            return pages;
        }
        public void ClearBox(bool boxLeft, bool boxRight, bool pageLeft, bool pageRight, bool boxTutorial){
            if(boxLeft){
                data.ClearAt(Box.BOX_LEFT);
            }
            if(boxRight){
                data.ClearAt(Box.BOX_RIGHT);
            }
            if(pageLeft){
                data.ClearAt(Box.PAGE_LEFT);
            }
            if(pageRight){
                data.ClearAt(Box.PAGE_RIGHT);
            }
            if(boxTutorial){
                data.ClearAt(Box.BOX_TUTORIAL);
            }
        }
        /// <summary> <c></c>cắt "List Invoice thành List Page.</summary>
        public List<Page> InvoicePages(List<Invoice> invoices){
            List<Page> pages = new List<Page>();
            if(invoices == null){
                pages.Add(new Page());
                return pages;
            }
            pages = new List<Page>();
            Page page = new Page();
            int count = 0;
            int count_invoice = invoices.Count;
            int line = 0;
            int page_number = 1;
            while(count < count_invoice){
                if(line == 0){
                    page = new Page();
                    page.PageNumber = page_number++;
                }
                page.KeyIndex.Add(line+1, (int)invoices[count].InvoiceNo);
                page.View[line++] = string.Format("{0, -2}. {1}", line, invoices[count++].InvoiceInfo);
                if(count == count_invoice){
                    pages.Add(page);
                }
                if(line == 16){
                    line = 0;
                    pages.Add(page);
                }
            }
            return pages;
        }
        public List<Page> ProductPages(List<Product> products){ //cắt list Product thành list Page
            List<Page> pages = new List<Page>();
            if(products == null || products.Count == 0){
                pages.Add(new Page());
                return pages;
            }
            Page page = new Page();
            int count_product = products.Count;
            int page_number = 1;
            int count = 0;
            int product_no = 1;
            int line = 0;
            string category_name = products[0].ProductCategory.CategoryName;

            while(count < count_product){
                if(line == 0){
                    page = new Page();
                    product_no = 1;
                    page.PageNumber = page_number++;
                    page.View[line++] = string.Format(" ▼ "+category_name);
                }
                if(category_name != products[count].ProductCategory.CategoryName){
                    category_name = products[count].ProductCategory.CategoryName;
                    page.View[line++] = string.Format(" ▼ "+category_name);
                    page.View[line++] = string.Format("   {0, -2}. {1}", product_no, products[count].ProductInfo);
                    page.KeyIndex.Add(product_no++, (int)products[count++].ProductId);
                }else{
                    page.View[line++] = string.Format("   {0, -2}. {1}", product_no, products[count].ProductInfo);
                    page.KeyIndex.Add(product_no++, (int)products[count++].ProductId);
                }
                if(count == count_product){
                    pages.Add(page);
                    break;
                }
                if(line == 16){
                    line = 0;
                    pages.Add(page);
                }
            }
            return pages;
        }
        /// <summary> <c></c> Hiển thị thông báo lỗi, tại Box Choice.</summary>
        public void InvalidSelection(string msg){
            Console.ForegroundColor = ConsoleColor.Red;
            data.WriteAt(string.Format(msg + " Press any key to continue..."), Box.BOX_CHOICE.Left, Box.BOX_CHOICE.Bott);
            Console.ResetColor();
            Console.ReadKey();
        }

        public void BoxTutorial(string[] options){// Hiển thị hướng dẫn các nút điều hướng
            data.ClearAt(Box.BOX_TUTORIAL);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            int length = options.Length;
            int pos_x = Box.BOX_TUTORIAL.Left;
            int pos_y = Box.BOX_TUTORIAL.Top;
            for(int i = 0; i <length; i++){
                if(pos_y == Box.BOX_TUTORIAL.Bott+1){
                    pos_x = Box.BOX_TUTORIAL.Left+30;
                    pos_y = Box.BOX_TUTORIAL.Top;
                }
                data.WriteAt(string.Format(" ▸ " + options[i]), pos_x, pos_y++);
            }
            Console.ResetColor();
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
        public void ShowNumberPage(int current_page, int max_page, bool isRight) // hiển thị (số trang hiện tại)/(số trang tối đa) của box phải nếu isRight = true
        {
            Coordinates box;
            if(isRight){
                box = Box.PAGE_RIGHT;
            }else{
                box = Box.PAGE_LEFT;
            }
            data.ClearAt(box);
            int x = box.Left-1;
            int y = box.Top-1;
            string back = (isRight == false)?("←"):("↑");
            string next = (isRight == false)?("→"):("↓");
            if(isRight == true)
            {
                data.WriteAt("╬═════════════════════════════════════════════════════════════════════█", x, y++);
                data.WriteAt(string.Format("║                                  {0, -4}                               █", string.Format(current_page + "/" + max_page)), x, y++);
                data.WriteAt(string.Format("╬═════════════════════════════════════════════════════════════════════█"), x, y);
            }
            else
            {
                data.WriteAt("█═══════════════════════════════════════════════════════════════════════╬", x, y++);
                data.WriteAt(string.Format("█                                  {0, -4}                                 ║", string.Format(current_page + "/" + max_page)), x, y++);
                data.WriteAt(string.Format("█═══════════════════════════════════════════════════════════════════════╬"), x, y);
            }
            if(max_page == 1){
                return;
            }
            else if(current_page == 1)
            {
                data.WriteAt(next, x+40, y-1);
            }
            else if(current_page == max_page)
            {
                data.WriteAt(back, x+32, y-1);
            }
            else
            {
                data.WriteAt(back, x+32, y-1);
                data.WriteAt(next, x+40, y-1);
            }
        }
        public void Form(string name_form)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("█▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀█");
            Console.WriteLine("█                                                                 ♠  RELIFE  ♠                                                                █");
            Console.WriteLine("█                                                              ─── ─────────── ───                                                            █");
            Console.ResetColor();
            Console.WriteLine("█▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀█");
            Console.WriteLine("█                           ~~~               -------                                      -------                 ~~~                        █");
            Console.WriteLine("█═════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════█");
            Console.WriteLine("█                                                                                                                                             █");
            Console.WriteLine("█═══════════════════════════════════════════════════════════════════════╦═════════════════════════════════════════════════════════════════════█");//(0 - 72, 5) - (72 -142, 5)
            Console.WriteLine("█                                                                       ║                                                                     █");
            Console.WriteLine("█                                                                       ║                                                                     █");
            Console.WriteLine("█                                                                       ║                                                                     █");
            Console.WriteLine("█                                                                       ║                                                                     █");
            Console.WriteLine("█                                                                       ║                                                                     █");
            Console.WriteLine("█                                                                       ║                                                                     █");
            Console.WriteLine("█                                                                       ║                                                                     █");
            Console.WriteLine("█                                                                       ║                                                                     █");
            Console.WriteLine("█                                                                       ║                                                                     █");
            Console.WriteLine("█                                                                       ║                                                                     █");
            Console.WriteLine("█                                                                       ║                                                                     █");
            Console.WriteLine("█                                                                       ║                                                                     █");
            Console.WriteLine("█                                                                       ║                                                                     █");
            Console.WriteLine("█                                                                       ║                                                                     █");
            Console.WriteLine("█                                                                       ║                                                                     █");
            Console.WriteLine("█                                                                       ║                                                                     █");
            Console.WriteLine("█                                                                       ║                                                                     █");
            Console.WriteLine("█                                                                       ║                                                                     █");
            Console.WriteLine("█═══════════════════════════════════════════════════════════════════════╬═════════════════════════════════════════════════════════════════════█");//(0 - 72, 24) - (72 -142, 24)
            Console.WriteLine("█                                                                       ║                                                                     █");
            Console.WriteLine("█═══════════════════════════════════════════════════════════════════════╬═════════════════════════════════════════════════════════════════════█");
            Console.WriteLine("█                                                                       ║                                                                     █");
            Console.WriteLine("█                                                                       ║                                                                     █");
            Console.WriteLine("█                                                                       ║                                                                     █");
            Console.WriteLine("▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀");
            data.WriteAt(name_form, 72-(name_form.Length/2), 4);
        }
    }
}