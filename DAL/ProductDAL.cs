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
            int result = 0;
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
        public bool UpdateQuantity(int product_id, int order_quantity){
            bool result = false;
            try{
                if(product_id < 0 || order_quantity < 0){
                    throw new Exception("Can't update");
                }
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"select quantity from Product where product_id = '"+product_id+"';";
                MySqlDataReader reader = cmd.ExecuteReader();
                int quantity = 0;
                if(reader.Read()){
                    quantity = reader.GetInt32("quantity");
                }
                reader.Close();
                if(quantity >= order_quantity){
                    cmd.CommandText = @"update Product set quantity = quantity - "+order_quantity+" where product_id = '"+product_id+"';";
                    cmd.ExecuteReader();
                    result = true;
                }
            }catch(Exception ex){
                Console.WriteLine(ex); Console.ReadLine();
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
    }
}