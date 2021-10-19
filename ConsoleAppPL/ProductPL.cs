using System;
using Persistance;
using BL;
using System.Collections.Generic;
using System.Globalization;

namespace ConsoleAppPL
{
    public static class CustomizeProduct {
        public static string[] Sugars = new string[]{"", "70%", "30%", "50%", "100%", "Không Đường"};
        public static string[] Ices = new string[]{"","Không Đá Mát", "30%", "50%", "70%", "100%", "Không Đá", "Làm Nóng"};
        public static string[] Types = new string[]{"","Nóng", "Lạnh"};
        public static string[] Sizes = new string[]{"", "M", "L"};
    }
    class ProductPL: Menu{
        private InputAndOutputData data = new InputAndOutputData();
        private ProductBL bl = new ProductBL();
        public void DefaultProduct(Product product){
            product.Size = product.Size.ToCharArray()[0].ToString();
            product.ProductType = product.ProductType.ToCharArray()[0].ToString();
            product.Sugar = product.Sugar.ToCharArray()[0].ToString();
            product.Ice = product.Ice.ToCharArray()[0].ToString();
        }
        public Product MenuProduct(Product productCus){
            Product product = bl.SearchByID((int)productCus.ProductId);
            if(product == null || product.ProductId == null){
                ViewBox(new string[1], "Sản Phẩm", false);
                return null;
            }
            DefaultProduct(productCus);
            int convert;
            int quantity = product.Quantity; productCus.Quantity = 1;
            string types = product.ProductType;
            string sizes = product.Size;
            string sugars = product.Sugar;
            string ices = product.Ice;
            string[] options;
            options = new string[]{"1. Chọn loại", "2. Chọn Size", "3. Chọn đường", "4. Chọn đá", "5. Chọn số lượng", "6. Chọn Topping"};
            string[] customize;
            string choice = string.Empty;
            string name_box = "Tuỳ chỉnh sản phẩm";
            do{
                ClearBox(false, false, true, true, true);
                ShowNameMenu("Sản Phẩm");
                BoxTutorial(new string[]{"ESC: Hoàn thành"});
                ViewBox(options, name_box, false);
                ProductDetails(productCus);
                choice = ViewBox(options, options.Length, new string[]{"Escape"}, name_box, false);
                switch(choice){
                    case "1":
                        customize = GetCustomize(types, 1);
                        string typeProduct = string.Empty;
                        while(typeProduct != "Escape"){
                            typeProduct = ViewBox(customize, customize.Length, new string[]{"Escape"}, "Tuỳ chỉnh loại", false);
                            if(typeProduct != "Escape"){
                                int.TryParse(typeProduct, out convert);
                                productCus.ProductType = types.Split(",", StringSplitOptions.RemoveEmptyEntries)[convert-1];
                                ProductDetails(productCus);
                            }
                        }
                        break;
                    case "2":
                        customize = GetCustomize(sizes, 2);
                        string sizeProduct = string.Empty;
                        while(sizeProduct != "Escape"){
                            sizeProduct = ViewBox(customize, customize.Length, new string[]{"Escape"}, "Tuỳ chỉnh Size", false);
                            if(sizeProduct != "Escape"){
                                int.TryParse(sizeProduct, out convert);
                                productCus.Size = sizes.Split(",", StringSplitOptions.RemoveEmptyEntries)[convert-1];
                                ProductDetails(productCus);
                            }
                        }
                        break;
                    case "3":
                        customize = GetCustomize(sugars, 3);
                        string sugarProduct = string.Empty;
                        while(sugarProduct != "Escape"){
                            sugarProduct = ViewBox(customize, customize.Length, new string[]{"Escape"}, "Tuỳ chỉnh lượng đường", false);
                            if(sugarProduct != "Escape"){
                                int.TryParse(sugarProduct, out convert);
                                productCus.Sugar = sugars.Split(",", StringSplitOptions.RemoveEmptyEntries)[convert-1];
                                ProductDetails(productCus);
                            }
                        }
                        break;
                    case "4":
                        customize = GetCustomize(ices, 4);
                        string iceProduct = string.Empty;
                        while(iceProduct != "Escape"){
                            iceProduct = ViewBox(customize, customize.Length, new string[]{"Escape"}, "Tuỳ chỉnh lượng đá", false);
                            if(iceProduct != "Escape"){
                                int.TryParse(iceProduct, out convert);
                                productCus.Ice = ices.Split(",", StringSplitOptions.RemoveEmptyEntries)[convert-1];
                                ProductDetails(productCus);
                            }
                        }
                        break;
                    case "5":
                        CustomizeQuantity(productCus);
                        break;
                    case "6":
                        CustomizeTopping(productCus);
                        break;
                    case "Escape":
                        break;
                    default :
                        InvalidSelection("Bạn chọn sai!");
                        break;
                }
            }while(choice != "Escape");
            return productCus;
        }
        public List<Page> SearchByName(){
            List<Page> pages = null;
            string search = data.GetChoice("Nhập tên sản phẩm", new string[]{"Escape"});
            if(search != "Escape"){
                pages = ProductPages(bl.SearchByName(1, new Product(){ProductName = search}));
            }
            return pages;
        }
        public List<Page> SearchByCategory(){
            List<Page> pages = null;
            string search = data.GetChoice("Nhập tên loại", new string[]{"Escape"});
            if(search != "Escape"){
                pages = ProductPages(bl.SearchByCategory(new Category(){CategoryName = search}));
            }
            return pages;
        }
        public void CustomizeQuantity(Product product){
            string input = string.Empty;
            int quantity = bl.GetQuantity((int)product.ProductId);
            int amount = 0;
            while(true){
                ProductDetails(product);
                BoxTutorial(new string[] { "ESC: Xong" });
                input = data.GetChoice("Nhập số lượng", new string[]{"Escape"});
                if(input != "Escape"){
                    int.TryParse(input, out amount);
                    if(amount < 0 || amount > quantity){
                        InvalidSelection(string.Format("Lỗi số lượng, số lượng còn lại: {0}!", quantity));
                    }else{
                        product.Quantity = amount;
                    }
                }else{
                    break;
                }
            }
        }
        public void CustomizeTopping(Product product){
            List<Topping> toppings = bl.GetToppings();
            List<Topping> toppingsProduct = product.ListTopping;
            int count = toppings.Count;
            string[] tutorial = new string[]{"Enter: Đồng ý", "ESC: Không"};
            string[] customize = new string[count];
            for(int i = 0; i < count; i++){
                customize[i] = string.Format(" {0, -2}. {1, -40}{2, 10}", i+1, toppings[i].ToppingName
                , string.Format(new CultureInfo("vi-VN"), "{0:#,##0}đ", toppings[i].UnitPrice));
            }
            string choice = string.Empty;
            int convert;
            Topping check;
            while(choice != "Escape"){
                BoxTutorial(new string[]{"ESC: Hoàn thành"});
                choice = ViewBox(customize, count, new string[]{"Escape"}, "Tuỳ chỉnh Topping", false);
                ProductDetails(product);
                if(choice != "Escape"){
                    int.TryParse(choice, out convert);
                    check = product.ListTopping.Find(x => x.ToppingId == toppings[convert-1].ToppingId);
                    if(check == null){
                        product.ListTopping.Add(toppings[convert-1]);
                    }else{
                        string remove;
                        do{
                            BoxTutorial(tutorial);
                            remove = data.GetChoice(string.Format("\""+toppings[convert-1].ToppingName+"\" đã được thêm, bạn muốn bỏ?"), new string[]{"Escape"});
                            if(remove == "Enter"){
                                product.ListTopping.Remove(check);
                            }
                            else if(remove != "Enter" && remove != "Escape"){
                                InvalidSelection("Bạn chọn sai!");
                            }
                        }while(remove != "Enter" && remove != "Escape");
                    }
                    ProductDetails(product);
                }
            }
        }
        public string[] GetCustomize(string customize, int option)
        {
            string[] result;
            int count_option;
            string[] split = customize.Split(",", StringSplitOptions.RemoveEmptyEntries);
            int[] temp = new int[split.Length];
            int count = split.Length;
            for(int i = 0; i < count; i++){
                int.TryParse(split[i], out temp[i]);
            }
            count_option = temp.Length;
            result = new string[count_option];
            switch(option){
                case 1://type
                    for(int i = 0; i < count_option; i++){
                        result[i] = string.Format(" {0, -2}. {1}", i+1, CustomizeProduct.Types[temp[i]]);
                    }
                    break;
                case 2://size
                    for(int i = 0; i < count_option; i++){
                        result[i] = string.Format(" {0, -2}. {1}", i+1, CustomizeProduct.Sizes[temp[i]]);
                    }
                    break;
                case 3://sugar
                    for(int i = 0; i < count_option; i++){
                        result[i] = string.Format(" {0, -2}. {1}", i+1, CustomizeProduct.Sugars[temp[i]]);
                    }
                    break;
                case 4://ice
                    for(int i = 0; i < count_option; i++){
                        result[i] = string.Format(" {0, -2}. {1}", i+1, CustomizeProduct.Ices[temp[i]]);
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
        public void ProductDetails(Product product){
            data.ClearAt(Box.BOX_RIGHT);
            Page page = new Page();
            double price = product.Price;
            if(product.Size == "2") price += 6000;
            int x = Box.BOX_RIGHT.Left;
            int y = Box.BOX_RIGHT.Top;
            int sugar; int.TryParse(product.Sugar.ToCharArray()[0].ToString(), out sugar);
            int typeProduct; int.TryParse(product.ProductType.ToCharArray()[0].ToString(), out typeProduct);
            int ice; int.TryParse(product.Ice.ToCharArray()[0].ToString(), out ice);
            int size; int.TryParse(product.Size.ToCharArray()[0].ToString(), out size);
            data.WriteAt("≡ Chi Tiết Sản Phẩm", x, y++);
            data.WriteAt(string.Format(" ▼ " + product.ProductName), x, y++);
            data.WriteAt(string.Format("▸ Loại       : " + CustomizeProduct.Types[typeProduct]), x+3, y++);
            data.WriteAt(string.Format("▸ Size       : " + CustomizeProduct.Sizes[size]), x+3, y++);
            data.WriteAt(string.Format("▸ Lượng Đường: "+CustomizeProduct.Sugars[sugar]), x+3, y++);
            data.WriteAt(string.Format("▸ Lượng Đá   : "+CustomizeProduct.Ices[ice]), x+3, y++);
            data.WriteAt(string.Format("▸ Số lượng   : "+product.Quantity), x+3, y++);
            data.WriteAt(string.Format("● Danh sách các topping"), x+3, y++);
            int old = y;
            int coord = 4+(Box.BOX_RIGHT.Right+Box.BOX_RIGHT.Left)/2;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            int count = product.ListTopping.Count;
            if(count == 0){
                data.WriteAt("Không có topping được thêm!", (Box.BOX_RIGHT.Right+Box.BOX_RIGHT.Left)/2 - 13, (Box.BOX_RIGHT.Bott + y)/2);
            }else{
                for(int i = 0; i < count; i++){
                    if(y == Box.BOX_RIGHT.Bott-1){
                        data.WriteAt(string.Format("+ " + product.ListTopping[i].ToppingName), coord, old++);
                    }else{
                        data.WriteAt(string.Format("+ " + product.ListTopping[i].ToppingName), x+4, y++);
                    }
                    price += product.ListTopping[i].UnitPrice;
                }
            }
            price = price * product.Quantity;
            Console.SetCursorPosition(Box.BOX_RIGHT.Left, Box.BOX_RIGHT.Bott);
            data.TextColor(string.Format(" >| Giá: {0, 53}", string.Format(new CultureInfo("vi-VN"), "{0:#,##0}đ", price)), ConsoleColor.Blue);
        }

        public void UpdateProduct(Product product){
            string[] options = new string[]{"1. Thay đổi số lượng", "2. Thay đổi giá"};
            string[] tutorial = new string[]{"ESC: Hoàn thành"};
            string[] details;
            string[] keyword = new string[]{"Escape"};
            string choice = string.Empty;
            int quantity;
            double price;
            do{
                details = new string[]{string.Format("Tên sản phẩm: {0}", product.ProductName)
                                        , string.Format("Số lượng    : {0}", bl.GetQuantity((int)product.ProductId))
                                        , string.Format("Giá         : {0}", product.Price)};
                ViewBox(details, "Thông tin sản phẩm", true);
                BoxTutorial(tutorial);
                choice = ViewBox(options, 2, keyword, "Menu", false);
                switch(choice){
                    case "1":
                        quantity = UpdateQuantity();
                        if(bl.UpdateQuantity(new Product(){ProductId = product.ProductId, Quantity = quantity})){
                            product.Quantity = quantity;
                            data.WriteAt("Hoàn thành! Nhấn phím bất kỳ để tiếp tục", Box.BOX_CHOICE.Left, Box.BOX_CHOICE.Bott);
                            Console.ReadKey();
                        }
                        else
                        {
                            InvalidSelection("Cập nhật thất bại!");
                        }
                        break;
                    case "2":
                        price = UpdatePrice();
                        if(bl.UpdatePrice(new Product(){ProductId = product.ProductId, Price = price})){
                            product.Price = price;
                            data.WriteAt("Hoàn thành! Nhấn phím bất kỳ để tiếp tục", Box.BOX_CHOICE.Left, Box.BOX_CHOICE.Bott);
                            Console.ReadKey();
                        }else
                        {
                            InvalidSelection("Cập nhật thất bại!");
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
        public double UpdatePrice(){
            string[] keyword = new string[]{"Escape"};
            double price;
            while(true){
                try{
                    price = double.Parse(data.GetChoice("Nhập giá", keyword));
                    break;
                }catch{
                    InvalidSelection("Giá phải lớn hơn hoặc bằng 0!");
                }
            }
            return price;
        }
        public int UpdateQuantity(){
            string[] keyword = new string[]{"Escape"};
            int quantity;
            while(true){
                try{
                    quantity = int.Parse(data.GetChoice("Nhập số lượng", keyword));
                    break;
                }catch{
                    InvalidSelection("Số lượng phải lơn hơn hoặc bằng 0!");
                }
            }
            return quantity;
        }
    }
}