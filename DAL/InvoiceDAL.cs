using System;
using Persistance;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

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
                            Price = reader.GetDouble("price"),
                            Sugar = reader.GetString("sugar"),
                            Ice = reader.GetString("ice"),
                            Quantity = reader.GetInt32("amount")
                        });
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
        public bool CreateInvoice(Invoice invoice){
            bool result = false;
            try{
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "lock tables Invoice write, InvoiceDetail write, InvoiceDetailTopping write";
                cmd.ExecuteNonQuery();
                MySqlTransaction trans = connection.BeginTransaction();
                cmd.Transaction = trans;
                MySqlDataReader reader = null;
                try{
                    if(invoice.InvoiceCashier == null || invoice.InvoiceCashier.CashierId == null){
                        throw new Exception("Can't Find Cashier!");
                    }
                    // get new invoice no
                    cmd.CommandText = @"select invoice_no from Invoice order by invoice_no desc limit 1;";
                    reader = cmd.ExecuteReader();
                    if(reader.Read()){
                        invoice.InvoiceNo  = reader.GetInt32("invoice_no") + 1;
                    }
                    reader.Close();
                    // insert new invoice
                    cmd.CommandText = @"insert into Invoice(invoice_no, invoice_cashierId, total_due, status, payment_method, note)
                                        value (@invoiceNo, @cashierId, @total, @status, @paymentMethod, ifnull(@note, ''));";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@invoiceNo", invoice.InvoiceNo);
                    cmd.Parameters.AddWithValue("@cashierId", invoice.InvoiceCashier.CashierId);
                    cmd.Parameters.AddWithValue("@total", invoice.Total);
                    cmd.Parameters.AddWithValue("@status", invoice.Status);
                    cmd.Parameters.AddWithValue("@paymentMethod", invoice.PaymentMethod);
                    cmd.Parameters.AddWithValue("@note", invoice.Note);
                    cmd.ExecuteNonQuery();
                    foreach(Product p in invoice.ListProduct){
                        if(p == null || p.ProductId == null){
                                throw new Exception("Not Exists Product!");
                        }
                        cmd.CommandText = @"insert into InvoiceDetail(invoice_no, product_id, amount,
                                            price, size, type, sugar, ice) value 
                                            (@invoiceNo, @productId, @amount, @price, @size, @type, @sugar, @ice);";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@invoiceNo", invoice.InvoiceNo);
                        cmd.Parameters.AddWithValue("@productId", p.ProductId);
                        cmd.Parameters.AddWithValue("@amount", p.Quantity);
                        cmd.Parameters.AddWithValue("@price", p.Price);
                        cmd.Parameters.AddWithValue("@size", p.Size);
                        cmd.Parameters.AddWithValue("@type", p.ProductType);
                        cmd.Parameters.AddWithValue("@sugar", p.Sugar);
                        cmd.Parameters.AddWithValue("@ice", p.Ice);
                        cmd.ExecuteNonQuery();
                        foreach(Topping tp in p.ListTopping){
                            cmd.CommandText = @"insert into InvoiceDetailTopping(invoice_no, product_id, topping_id, unit_price) value
                                                        ('"+invoice.InvoiceNo+"', '"+p.ProductId+"', '"+tp.ToppingId+"', '"+tp.UnitPrice+"');";
                            cmd.ExecuteNonQuery();
                        }
                    }
                    trans.Commit();
                    result = true;
                }catch(Exception ex){
                    Console.WriteLine(ex);
                    try{
                        trans.Rollback();
                    }catch{}
                }
                finally
                {
                    cmd.CommandText = "unlock tables;";
                    cmd.ExecuteNonQuery();
                }
            }catch(Exception ex){
                Console.WriteLine(ex);
            }
            finally
            {
                connection.Close();
            }
            return result;
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
                                    Price = reader.GetDouble("price"),
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
                Console.WriteLine(ex);
            }finally{
                connection.Close();
            }
            return invoices;
        }
    }
}