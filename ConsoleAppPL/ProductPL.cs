using System;
using System.Text.RegularExpressions;
using Persistance;
using BL;
using System.Collections.Generic;

namespace ConsoleAppPL
{
    class ProductPL{
        private InputAndOutputData data = new InputAndOutputData();
        public Page ProductDetails(Product product){
            Page page = new Page();
            page.View[0] = string.Format(" ▼ " + product.ProductName);
            page.View[1] = string.Format("   ▸ Type:   " + product.ProductType);
            page.View[2] = string.Format("   ▸ Size:   " + product.Size);
            page.View[3] = string.Format("   ▸ Sugar:  "+product.Sugar);
            page.View[4] = string.Format("   ▸ Ice:    "+product.Ice);
            page.View[5] = string.Format("   ▸ Amount: "+product.Quantity);
            page.View[6] = string.Format("   ▸ Toppings");
            int line = product.ListTopping.Count/2;
            for(int i = 0; i < line; i++){
                
            }
            return page;
        }
    }
}