using System;
using DAL;
using Persistance;
using System.Collections.Generic;

namespace BL
{
    public class ProductBL{
        private ProductDAL dal = new ProductDAL();
        public Product SearchByID(int product_id){
            return dal.GetByID(product_id);
        }
        public List<Product> SearchByName(int filterProduct, Product product){
            return dal.GetProducts(filterProduct, product);
        }
        public List<Product> SearchByCategory(Category category){
            return dal.GetByCategory(category);
        }
        public List<Topping> GetToppings(){
            return dal.GetToppings();
        }
    }
}