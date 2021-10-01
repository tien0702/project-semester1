using System;
using System.Collections.Generic;

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
                return string.Format("{0, -55}+{2,-2}K", ProductName, Quantity, (Price*Quantity)/1000);
            }
        }
        public Product()
        {
            this.ListTopping = new List<Topping>();
            this.Quantity = 1;
        }
    }
}