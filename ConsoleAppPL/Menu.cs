using System;
using System.Text.RegularExpressions;
using Persistance;
using BL;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleAppPL
{
    public class Menu{
        private InputAndOutputData data = new InputAndOutputData();
        public void ViewBox(string[] content, string name_box, bool isRight){
            Coordinates box;
            if(isRight){
                box = Box.BOX_RIGHT;
            }else{
                box = Box.BOX_LEFT;
            }
            data.ClearAt(box);
            data.WriteAt(string.Format("≡ " + name_box), box.Left, box.Top);
            string choice = string.Empty;
            int max_line = content.Length;
            for(int i = 0; i < max_line; i++){
                data.WriteAt(string.Format(" " + content[i]), box.Left, box.Top+i+1);
            }
        }
        public List<Page> InvoicePages(List<Invoice> invoices){
            List<Page> pages = null;
            if(invoices == null){
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
        public void BoxTutorial(string[] options){
            data.ClearAt(Box.BOX_TUTORIAL);
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
        }
        public List<Page> ProductPages(List<Product> products){
            if(products == null){
                return null;
            }
            List<Page> pages = new List<Page>();
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
        
        public void ShowNumberPage(int current_page, int max_page, bool isRight)
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