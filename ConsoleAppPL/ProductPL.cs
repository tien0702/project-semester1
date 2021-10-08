using System;
using Persistance;
using BL;
using System.Collections.Generic;

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
                ViewBox(new string[1], "Menu Product", false);
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
            options = new string[]{"1. Choose Type", "2. Choose Size", "3. Choose Sugar", "4. Choose Ice", "5. Choose Quantity", "6. Choose Topping"};
            string[] customize;
            string choice = string.Empty;
            string name_box = "Customize";
            do{
                ClearBox(false, false, true, true, true);
                ShowNameMenu("Menu Product");
                BoxTutorial(new string[]{"Escape: Done"});
                ViewBox(options, name_box, false);
                ProductDetails(productCus);
                choice = ViewBox(options, options.Length, new string[]{"Escape"}, name_box, false);
                switch(choice){
                    case "1":
                        customize = GetCustomize(types, 1);
                        string typeProduct = string.Empty;
                        while(typeProduct != "Escape"){
                            typeProduct = ViewBox(customize, customize.Length, new string[]{"Escape"}, "Customize Type", false);
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
                            sizeProduct = ViewBox(customize, customize.Length, new string[]{"Escape"}, "Customize Size", false);
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
                            sugarProduct = ViewBox(customize, customize.Length, new string[]{"Escape"}, "Customize Sugar", false);
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
                            iceProduct = ViewBox(customize, customize.Length, new string[]{"Escape"}, "Customize Ice", false);
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
                        InvalidSelection("You choice invalid!");
                        break;
                }
            }while(choice != "Escape");
            return productCus;
        }
        public List<Page> SearchByName(){
            List<Page> pages = null;
            string search = data.GetChoice("Enter Product Name", new string[]{"Escape"});
            if(search != "Escape"){
                pages = ProductPages(bl.SearchByName(1, new Product(){ProductName = search}));
            }
            return pages;
        }
        public List<Page> SearchByCategory(){
            List<Page> pages = null;
            string search = data.GetChoice("Enter Category Name", new string[]{"Escape"});
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
                BoxTutorial(new string[] { "ESC: Done" });
                input = data.GetChoice("Enter the quantity", new string[]{"Escape"});
                if(input != "Escape"){
                    int.TryParse(input, out amount);
                    if(amount < 0 || amount > quantity){
                        InvalidSelection(string.Format("Invalid, Remaining quantity is {0}!", quantity));
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
            string[] tutorial = new string[]{"Enter: Yes", "ESC: No"};
            string[] customize = new string[count];
            for(int i = 0; i < count; i++){
                customize[i] = string.Format(" {0, -2}. {1, -40}{2, 10}K", i+1, toppings[i].ToppingName, toppings[i].UnitPrice/1000);
            }
            string choice = string.Empty;
            int convert;
            Topping check;
            while(choice != "Escape"){
                BoxTutorial(new string[]{"ESC: Done"});
                choice = ViewBox(customize, count, new string[]{"Escape"}, "Customize Toppings", false);
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
                            remove = data.GetChoice(string.Format(toppings[convert-1].ToppingName + " has been added, do you want to remove it?"), new string[]{"Escape"});
                            if(remove == "Enter"){
                                product.ListTopping.Remove(check);
                            }
                            else if(remove != "Enter" && remove != "Escape"){
                                InvalidSelection("You choice invalid!");
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
            data.WriteAt("≡ Product Details", x, y++);
            data.WriteAt(string.Format(" ▼ " + product.ProductName), x, y++);
            data.WriteAt(string.Format("▸ Type:   " + CustomizeProduct.Types[typeProduct]), x+3, y++);
            data.WriteAt(string.Format("▸ Size:   " + CustomizeProduct.Sizes[size]), x+3, y++);
            data.WriteAt(string.Format("▸ Sugar:  "+CustomizeProduct.Sugars[sugar]), x+3, y++);
            data.WriteAt(string.Format("▸ Ice:    "+CustomizeProduct.Ices[ice]), x+3, y++);
            data.WriteAt(string.Format("▸ Quantity: "+product.Quantity), x+3, y++);
            data.WriteAt(string.Format("● Toppings:"), x+3, y++);
            int old = y;
            int coord = 4+(Box.BOX_RIGHT.Right+Box.BOX_RIGHT.Left)/2;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            int count = product.ListTopping.Count;
            if(count == 0){
                data.WriteAt("No toppings added!", (Box.BOX_RIGHT.Right+Box.BOX_RIGHT.Left)/2 - 9, (Box.BOX_RIGHT.Bott + y)/2);
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
            data.TextColor(string.Format(" >| Price: {0, 53}K", (price)/1000), ConsoleColor.Blue);
        }
    }
}