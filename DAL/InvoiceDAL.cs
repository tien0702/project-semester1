using System;
using Persistance;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace DAL
{
    public class InvoiceDAL{
        private MySqlConnection connection = DbHelper.GetConnection();
        
        private Invoice GetInvoice(MySqlDataReader reader){
            Invoice invoice = null;
            try{
                invoice = new Invoice(){
                    InvoiceNo = reader.GetInt32("invoice_no"),
                    InvoiceCashier = new Cashier(){
                        CashierId = reader.GetInt32("cashierId"),
                        Role = reader.GetInt32("role"),
                        FirstName = reader.GetString("firstName"),
                        MiddleName = reader.GetString("midlleName"),
                        LastName = reader.GetString("lastName")
                    },
                    Date = reader.GetDateTime("date"),
                    Total = reader.GetDouble("total_due"),
                    PaymentMethod = reader.GetInt32("payment_method"),
                    Status = reader.GetInt32("status"),
                    Note = reader.GetString("note")
                };
                reader.Close();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"select *from InvoiceDetail, Product, Category
                                    where invoice_no = @invoiceNo
                                    and InvoiceDetail.product_id = Product.product_id
                                    and Product.product_category_id = Category.category_id;";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("invoiceNo", invoice.InvoiceNo);
                reader = cmd.ExecuteReader();
                if(reader.Read()){
                    do{
                        invoice.ListProduct.Add(new Product(){
                            ProductId = reader.GetInt32("product_id"),
                            ProductName = reader.GetString("product_name"),
                            Size = reader.GetString("size"),
                            ProductType = reader.GetString("type"),
                            ProductCategory = new Category()
                            {
                                CategoryId = reader.GetInt32("category_id"),
                                CategoryName = reader.GetString("category_name")
                            },
                            Price = reader.GetDouble("unit_price"),
                            Sugar = reader.GetString("sugar"),
                            Ice = reader.GetString("ice"),
                            Quantity = reader.GetInt32("amount")
                        });
                        Console.WriteLine(invoice.ListProduct[0].ProductInfo); Console.ReadKey();
                    }while(reader.Read());
                    reader.Close();
                    foreach(var p in invoice.ListProduct){
                        cmd.CommandText = @"select *from InvoiceDetailTopping, Topping
                                        where invoice_no = @invoiceNo
                                        and product_id = @productId
                                        and InvoiceDetailTopping.topping_id = Topping.topping_id;";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@invoiceNo", invoice.InvoiceNo);
                        cmd.Parameters.AddWithValue("@productId", p.ProductId);
                        reader = cmd.ExecuteReader();
                        if(reader.Read()){
                            do{
                                p.ListTopping.Add( new Topping(){
                                    ToppingId = reader.GetInt32("topping_id"),
                                    ToppingName = reader.GetString("topping_name"),
                                    UnitPrice = reader.GetDouble("unit_price")
                                    }
                                );
                            }while(reader.Read());
                        }
                        reader.Close();
                    }
                }
                }catch(Exception ex){
                    invoice = null;
                    Console.WriteLine(ex);
                }
            return invoice;
        }
        public double[] RevenueStatistics(DateTime start, DateTime end){
            double[] result = new double[2];
            string query = @"select sum(amount) as sum_amount from invoice, invoicedetail
                                where date between '"+start.ToString("yyyy-MM-dd HH:mm:ss.fff")+"' and '"+end.ToString("yyyy-MM-dd HH:mm:ss.fff")+"' and invoice.invoice_no = invoicedetail.invoice_no;";
            try{
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = query;
                MySqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read()){
                    try{
                       double.TryParse(reader.GetDouble("sum_amount").ToString(), out result[1]); 
                    }
                    catch{}
                }
                reader.Close();
                cmd.CommandText = @"select sum(invoice.total_due) as total_due from invoice
                                    where date between '"+start.ToString("yyyy-MM-dd HH:mm:ss.fff")+"' and '"+end.ToString("yyyy-MM-dd HH:mm:ss.fff")+"';";
                reader = cmd.ExecuteReader();
                if(reader.Read()){
                    try{
                        result[0] = reader.GetDouble("total_due");
                    }catch{}
                }
                reader.Close();
            }catch(Exception ex){
                throw new Exception(ex.Message);
            }finally{
                connection.Close();
            }
            return result;
        }
        public bool CreateInvoice(Invoice invoice){
            bool result = false;
            try{
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "lock tables Invoice write, InvoiceDetail write, InvoiceDetailTopping write;";
                cmd.ExecuteNonQuery();
                MySqlTransaction trans = connection.BeginTransaction();
                cmd.Transaction = trans;
                MySqlDataReader reader = null;
                try{
                    // check exitsts cashier
                    if(invoice.InvoiceCashier == null || invoice.InvoiceCashier.CashierId == null){
                        throw new Exception("Không tìm thấy người bán!");
                    }
                    bool invoice_no_already = false;
                    int status = invoice.Status;
#region             Check invoice_no and create invoice_no

                    // check is first number
                    cmd.CommandText = "select invoice_no from Invoice limit 1;";
                    reader = cmd.ExecuteReader();
                    if(reader.Read()){
                        reader.Close();
                        cmd.CommandText = "select invoice_no, status from Invoice where invoice_no = '"+invoice.InvoiceNo+"';";
                        reader = cmd.ExecuteReader();
                        if(reader.Read()){
                            invoice_no_already = true;
                            status = reader.GetInt32("status");
                            reader.Close();
                        }
                        else
                        {
                            reader.Close();
                            cmd.CommandText = @"select invoice_no from Invoice order by invoice_no desc limit 1;";
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                invoice.InvoiceNo = reader.GetInt32("invoice_no") + 1;
                            }
                            reader.Close();
                        }
                    }else
                    {
                        invoice.InvoiceNo = 1001;
                    }
#endregion  

#region             Check status invoice
                    switch(status){
                        case 1:
                            if(invoice_no_already){
                                throw new Exception("Hoá đơn đã được thanh toán!");
                            }
                            else {
                                InsertInvoice(invoice);
                                UpdateQuantity(invoice);
                            }
                            break;
                        case 2:
                            if(invoice_no_already){
                                UpdateQuantity(invoice);
                            } 
                            else{
                                InsertInvoice(invoice);
                            } 
                            break;
                        default:
                            throw new Exception("Trạng thái đang cập nhật");
                    }
#endregion  
                    trans.Commit();
                    result = true;
                }catch(Exception ex){
                    try{
                        trans.Rollback();
                    }catch{}
                    throw new Exception(ex.Message);
                }
                finally
                {
                    cmd.CommandText = "unlock tables;";
                    cmd.ExecuteNonQuery();
                }
            }catch(Exception ex){
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return result;
        }
        private void InsertInvoice(Invoice invoice){
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"insert into Invoice(invoice_no, invoice_cashierId, status, payment_method, note)
                                        value (@invoiceNo, @cashierId, @status, @paymentMethod, ifnull(@note, ''));";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@invoiceNo", invoice.InvoiceNo);
            cmd.Parameters.AddWithValue("@cashierId", invoice.InvoiceCashier.CashierId);
            cmd.Parameters.AddWithValue("@status", invoice.Status);
            cmd.Parameters.AddWithValue("@paymentMethod", invoice.PaymentMethod);
            cmd.Parameters.AddWithValue("@note", invoice.Note);
            cmd.ExecuteNonQuery();
            InsertInvoiceDetail(invoice);
            TotalDue(invoice);
        }
        public bool UpdateStatus(Invoice invoice){
            bool result = false;
            MySqlCommand cmd = connection.CreateCommand();

            try{
                connection.Open();
                cmd.CommandText = "update Invoice set status = '"+invoice.Status+"' where invoice_no = '"+invoice.InvoiceNo+"';";
                cmd.ExecuteNonQuery();
                result = true;
            }catch(Exception ex){
                throw new Exception(ex.Message);
            }finally{
                connection.Close();
            }
            return result;
        }
        private void TotalDue(Invoice invoice)
        {
            MySqlCommand cmd = connection.CreateCommand();
            MySqlDataReader reader = null;
            double total_due = 0;
            foreach(var p in invoice.ListProduct){
                cmd.CommandText = @"select unit_price from Product where product_id = '" + p.ProductId + "';";
                reader = cmd.ExecuteReader();
                double size = (p.Size == "2") ? (6000) : (0);
                if (reader.Read()) total_due += (reader.GetDouble("unit_price") + size) * p.Quantity;
                reader.Close();
                foreach(var tp in p.ListTopping){
                    cmd.CommandText = @"select unit_price from Topping where topping_id = '" + p.ProductId + "';";
                    reader = cmd.ExecuteReader();
                    if (reader.Read()) total_due += reader.GetDouble("unit_price");
                    reader.Close();
                }
            }
            cmd.CommandText = "update Invoice set total_due = '"+invoice.Total+"' where invoice_no = '"+invoice.InvoiceNo+"';";
            cmd.ExecuteNonQuery();
        }
        private void UpdateQuantity(Invoice invoice){
            MySqlCommand cmd = connection.CreateCommand();
            MySqlDataReader reader = null;
            cmd.CommandText = "update Invoice set status = 1 where invoice_no = '"+invoice.InvoiceNo+"';";
            cmd.ExecuteNonQuery();
            foreach(var p in invoice.ListProduct){
                cmd.CommandText = "select quantity from Product where product_id = '" + p.ProductId + "';";
                reader = cmd.ExecuteReader();
                int check = 0;
                if (reader.Read())
                {
                    check = reader.GetInt32("quantity");
                }
                reader.Close();
                if (check < p.Quantity)
                {
                    throw new Exception("\"" + p.ProductName + "\" không đủ, còn lại: " + check + "!");
                }
                cmd.CommandText = "update Product set quantity = quantity - '" + p.Quantity + "' where product_id = '" + p.ProductId + "';";
                cmd.ExecuteNonQuery();
            }
        }
        private void InsertInvoiceDetail(Invoice invoice)
        {
            MySqlCommand cmd = connection.CreateCommand();
            foreach (Product p in invoice.ListProduct)
            {
                if (p == null || p.ProductId == null)
                {
                    throw new Exception("Không tìm thấy sản phẩm!");
                }
                cmd.CommandText = @"insert into InvoiceDetail(invoice_no, product_id, amount,
                                             size, type, sugar, ice) value 
                                            (@invoiceNo, @productId, @amount, @size, @type, @sugar, @ice);";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@invoiceNo", invoice.InvoiceNo);
                cmd.Parameters.AddWithValue("@productId", p.ProductId);
                cmd.Parameters.AddWithValue("@amount", p.Quantity);
                cmd.Parameters.AddWithValue("@size", p.Size);
                cmd.Parameters.AddWithValue("@type", p.ProductType);
                cmd.Parameters.AddWithValue("@sugar", p.Sugar);
                cmd.Parameters.AddWithValue("@ice", p.Ice);
                cmd.ExecuteNonQuery();
                InsertInvoiceDetailTopping(p, (int)invoice.InvoiceNo);
            }
        }
        private void InsertInvoiceDetailTopping(Product product, int invoice_no)
        {
            foreach (Topping tp in product.ListTopping)
            {
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"insert into InvoiceDetailTopping(invoice_no, product_id, topping_id) value
                                                        ('" + invoice_no + "', '" + product.ProductId + "', '" + tp.ToppingId + "');";
                cmd.ExecuteNonQuery();
            }
        }
        public Invoice GetByNo(int? invoice_no){
            Invoice invoice = null;
            try{
                connection.Open();
                if(invoice_no == null){
                    throw new Exception("Invoice No Invalid!");
                }
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"select *from Invoice, Cashier
                                    where invoice_no = '"+invoice_no+"' and invoice_cashierId = cashierId;";
                MySqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read()){
                    invoice = GetInvoice(reader);
                }
                reader.Close();
            }catch(Exception ex){
                Console.WriteLine(ex);
            }finally{
                connection.Close();
            }
            return invoice;
        }
        
        public List<Invoice> GetByStatus(int status){
            List<Invoice> invoices = null;
            try{
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"select *from Invoice, Cashier
                                    where status = "+status+" and invoice_cashierId = cashierId;";
                MySqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read()){
                    invoices = new List<Invoice>();
                    do{
                        invoices.Add(new Invoice(){
                            InvoiceNo = reader.GetInt32("invoice_no"),
                            InvoiceCashier = new Cashier()
                            {
                                CashierId = reader.GetInt32("cashierId"),
                                Role = reader.GetInt32("role"),
                                FirstName = reader.GetString("firstName"),
                                MiddleName = reader.GetString("midlleName"),
                                LastName = reader.GetString("lastName")
                            },
                            Date = reader.GetDateTime("date"),
                            Total = reader.GetDouble("total_due"),
                            PaymentMethod = reader.GetInt32("payment_method"),
                            Status = reader.GetInt32("status"),
                            Note = reader.GetString("note")
                        });
                    }while(reader.Read());
                    reader.Close();
                    foreach(Invoice invoice in invoices)
                    {
                        cmd = connection.CreateCommand();
                        cmd.CommandText = @"select *from InvoiceDetail, Product, Category
                                    where invoice_no = @invoiceNo
                                    and InvoiceDetail.product_id = Product.product_id
                                    and Product.product_category_id = Category.category_id;";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("invoiceNo", invoice.InvoiceNo);
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            do
                            {
                                invoice.ListProduct.Add(new Product()
                                {
                                    ProductId = reader.GetInt32("product_id"),
                                    ProductName = reader.GetString("product_name"),
                                    Size = reader.GetString("size"),
                                    ProductType = reader.GetString("type"),
                                    ProductCategory = new Category()
                                    {
                                        CategoryId = reader.GetInt32("category_id"),
                                        CategoryName = reader.GetString("category_name")
                                    },
                                    Price = reader.GetDouble("unit_price"),
                                    Sugar = reader.GetString("sugar"),
                                    Ice = reader.GetString("ice"),
                                    Quantity = reader.GetInt32("amount")
                                });
                            } while (reader.Read());
                            reader.Close();
                            foreach (var p in invoice.ListProduct)
                            {
                                cmd.CommandText = @"select *from InvoiceDetailTopping, Topping
                                        where invoice_no = @invoiceNo
                                        and product_id = @productId
                                        and InvoiceDetailTopping.topping_id = Topping.topping_id;";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@invoiceNo", invoice.InvoiceNo);
                                cmd.Parameters.AddWithValue("@productId", p.ProductId);
                                reader = cmd.ExecuteReader();
                                if (reader.Read())
                                {
                                    do
                                    {
                                        p.ListTopping.Add(new Topping()
                                        {
                                            ToppingId = reader.GetInt32("topping_id"),
                                            ToppingName = reader.GetString("topping_name"),
                                            UnitPrice = reader.GetDouble("unit_price")
                                        }
                                        );
                                    } while (reader.Read());
                                }
                                reader.Close();
                            }
                        }
                        reader.Close();
                    }
                }
            }catch(Exception ex){
                throw new Exception(ex.Message);
            }finally{
                connection.Close();
            }
            return invoices;
        }
    }
}