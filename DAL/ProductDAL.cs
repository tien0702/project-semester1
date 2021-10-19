using System;
using Persistance;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DAL
{
    public static class ProductFilter{
        public const int GET_ALL = 0;
        public const int FILTER_BY_PROUCT_NAME = 1;
    }
    public class ProductDAL{
        private MySqlConnection connection = DbHelper.GetConnection();
        private Product GetProduct(MySqlDataReader reader){
            Product product = new Product()
            {
                ProductId = reader.GetInt32("product_id"),
                ProductName = reader.GetString("product_name"),
                ProductCategory = new Category()
                {
                    CategoryId = reader.GetInt32("category_id"),
                    CategoryName = reader.GetString("category_name")
                },
                Price = reader.GetDouble("unit_price"),
                Size = reader.GetString("product_size"),
                ProductType = reader.GetString("product_type"),
                Sugar = reader.GetString("product_sugar"),
                Quantity = reader.GetInt32("quantity"),
                Ice = reader.GetString("product_ice"),
            };
            return product;
        }
        public int GetQuantity(int product_id){
            int result = -1;
            try{
                if(product_id <= 0){
                    throw new Exception("Can't Update");
                }
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"select quantity from Product where product_id = '"+product_id+"';";
                MySqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read()){
                    result = reader.GetInt32("quantity");
                }
                reader.Close();
            }catch(Exception ex){
                Console.WriteLine(ex);
            }finally{
                connection.Close();
            }
            return result;
        }
        private Topping GetTopping(MySqlDataReader reader){
            Topping topping = new Topping(){
                ToppingId = reader.GetInt32("topping_id"),
                ToppingName = reader.GetString("topping_name"),
                UnitPrice = reader.GetDouble("unit_price")
            };
            return topping;
        }
        public Product GetByID(int product_id){
            Product product = null;
            try{
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"select *from Product, Category where product_id = '"+product_id+"' and product_category_id = category_id;";
                MySqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read()){
                    product = GetProduct(reader);
                }
                reader.Close();
            }catch(Exception ex){
                Console.WriteLine(ex);
            }finally{
                connection.Close();
            }
            return product;
        }
        public List<Product> GetProducts(int filterProduct, Product product){
            List<Product> products = null;
            try{
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                switch(filterProduct){
                    case ProductFilter.GET_ALL:
                        cmd.CommandText = @"select *from Product, Category where product_category_id = category_id;";
                        break;
                    case ProductFilter.FILTER_BY_PROUCT_NAME:
                        cmd.CommandText = @"select *from Product, Category where product_name like '%"+product.ProductName+"%' and product_category_id = category_id;";
                        break;
                    default:
                        break;
                }
                MySqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read()){
                    products = new List<Product>();
                    do{
                        products.Add(GetProduct(reader));
                    }while(reader.Read());
                }
                reader.Close();
            }catch(Exception ex){
                Console.WriteLine(ex);
            }finally{
                connection.Close();
            }
            return products;
        }
        public List<Product> GetByCategory(Category category){
            List<Product> products = null;
            try{
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"select *from Product, Category
                                    where product_category_id = category_id and category_name like '%"+category.CategoryName+"%';";
                MySqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read()){
                    products = new List<Product>();
                    do{
                        products.Add(GetProduct(reader));
                    }while(reader.Read());
                }
                reader.Close();
            }catch(Exception ex){
                Console.WriteLine(ex);
            }finally{
                connection.Close();
            }
            return products;
        }
        public List<Topping> GetToppings(){
            List<Topping> toppings = null;
            try
            {
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"select *from Topping;";
                MySqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read()){
                    toppings = new List<Topping>();
                    do{
                        toppings.Add(new Topping(){
                            ToppingId = reader.GetInt32("topping_id"),
                            ToppingName = reader.GetString("topping_name"),
                            UnitPrice = reader.GetDouble("unit_price")
                        });
                    }while(reader.Read());
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally{
                connection.Close();
            }
            return toppings;
        }

        public bool UpdateQuantity(Product product){
            if(product == null || product.Quantity < 0){
                throw new Exception("Không thể cập nhật!");
            }
            bool result = false;
            try{
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "lock tables Product write;";
                cmd.ExecuteNonQuery();
                MySqlTransaction trans = connection.BeginTransaction();
                cmd.Transaction = trans;
                try{
                    cmd.CommandText = "update Product set quantity = '"+product.Quantity+"' where product_id = '"+product.ProductId+"';";
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                    result = true;
                }catch(Exception e){
                    try{
                        trans.Rollback();
                    }catch{}
                    throw new Exception(e.Message);
                }finally{
                    cmd.CommandText = "unlock tables;";
                    cmd.ExecuteNonQuery();
                }
            }catch(Exception ex){
                throw new Exception(ex.Message);
            }finally{
                connection.Close();
            }
            return result;
        }
        public bool UpdatePrice(Product product){
            if(product == null || product.Price <= 0){
                throw new Exception("Không thể cập nhật!");
            }
            bool result = false;
            try{
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "lock tables Cashier write;";
                cmd.ExecuteNonQuery();
                MySqlTransaction trans = connection.BeginTransaction();
                cmd.Transaction = trans;
                try{
                    cmd.CommandText = "update Product set unit_price = '"+product.Price+"' where product_id = '"+product.ProductId+"';";
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                    result = true;
                }catch(Exception e){
                    try{
                        trans.Rollback();
                    }catch{}
                    throw new Exception(e.Message);
                }finally{
                    cmd.CommandText = "unlock tables;";
                    cmd.ExecuteNonQuery();
                }
            }catch(Exception ex){
                throw new Exception(ex.Message);
            }finally{
                connection.Close();
            }
            return result;
        }
    }
}