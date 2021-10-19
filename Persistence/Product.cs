using System;
using System.Collections.Generic;
using System.Globalization;

namespace Persistance
{
    public class Product{
        public int? ProductId{set; get;}
        public string ProductName{set; get;}
        public List<Topping> ListTopping{set; get;}
        public Category ProductCategory{set; get;}
        public string ProductType{set; get;}
        public string Size{set; get;}
        public double Price{set; get;}
        public int Quantity{set; get;}
        public string Sugar{set; get;}
        public string Ice{set; get;}
        public string ProductInfo{
            get{
                return string.Format("{0, -53}{2, 2}", ProductName, Quantity, string.Format(new CultureInfo("vi-VN"), "{0:#,##0}đ", Price));
            }
        }
        public Product()
        {
            this.ListTopping = new List<Topping>();
            this.Quantity = 1;
        }
    }
}