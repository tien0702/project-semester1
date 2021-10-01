using Xunit;
using Persistance;
using System.Collections.Generic;
using DAL;

namespace DALTest{
    public class ProductDalTest{
        private ProductDAL dal = new ProductDAL();
        [Theory]
        [InlineData(1)]
        [InlineData(50)]
        [InlineData(15)]
        public void GetByIDTest1(int product_id){
            Product product = dal.GetByID(product_id);
            Assert.True(product != null);
        }
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(100000)]
        public void GetByIDTest2(int product_id){
            Product product = dal.GetByID(product_id);
            Assert.True(product == null);
        }
        [Theory]
        [InlineData("Okinawa")]
        [InlineData("nawa")]
        [InlineData("kim")]
        [InlineData("Trà Sữa")]
        [InlineData("Ô Long Trân Châu Baby Kem Cafe")]
        public void GetProductsTest1(string product_name){
            List<Product> products = dal.GetProducts(1, new Product(){ProductName = product_name});
            foreach (Product p in products)
            {
                Assert.Contains(product_name.ToLower(), p.ProductName.ToLower());
            }
        }
        // name
        [Theory]
        [InlineData("abcd")]
        [InlineData("1234")]
        public void GetProductsTest2(string product_name)
        {
            List<Product> products = dal.GetProducts(1, new Product(){ProductName = product_name});
            Assert.True(products == null);
        }

        [Theory]
        [InlineData("Fresh")]
        [InlineData("fruit")]
        [InlineData("Trà Sữa")]
        [InlineData("Fresh Fruit Tea")]
        [InlineData("uit Tea")]
        // Test Get By Category
        public void GetByCategoryTest1(string category_name)
        {
            List<Product> products = dal.GetByCategory(new Category(){CategoryName = category_name});
            Assert.True(products != null);
            foreach(Product p in products){
                Assert.Contains(category_name.ToLower(), p.ProductCategory.CategoryName.ToLower());
            }
        }
        [Theory]
        [InlineData("abc")]
        [InlineData("1234")]
        [InlineData("Freshs")]
        public void GetByCategoryTest2(string category_name)
        {
            List<Product> products = dal.GetByCategory(new Category(){CategoryName = category_name});
            Assert.True(products == null);
        }

    }
}