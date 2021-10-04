drop database if exists salesSystem;

create database if not exists salesSystem;

use salesSystem;

create table Cashier(
	cashierId int primary key auto_increment,
    userName varchar(30) not null,
    password varchar(100) not null,
    role int not null,
    firstName varchar(25),
    midlleName varchar(25),
    lastName varchar(25),
    phone varchar(11) unique,
    email varchar(50) unique,
    address varchar(150)
);
insert into Cashier(userName, password, role, firstName, midlleName, lastName, phone, email, address) values
				('Administrator', 'ec876c82eca6bf0024fc9d37569541e1', 1, 'Admin', '', '', '', '', ''),
                -- AdiminPF13
                ('Tientv', '928c29958130e9ee161059b4ee24bfc5', 2, 'Tiến', 'Văn', 'Trần', '01234567899', 'tientv@gmail.com', 'Thanh Hoá'),
                -- TienPF13
                ('Phucvv', '99feb164738b42ba9f03fdc7d1024afe', 2, 'Phúc', 'Văn', 'Vũ', '01478523699', 'phucvv@gmail.com', 'Hà Nội'),
                -- PhucPF13
                ('Phuocmh', '2918b929ee36abcbc26a0b52da2651af', 2, 'Phước', 'Hồng', 'Mạc', '09874563211', 'phuocmh@gmail.com', 'Hà Nội');
                -- PhuocPF13
                
create table Invoice(
	invoice_no int primary key auto_increment,
    invoice_cashierId int not null,
	foreign key(invoice_cashierId) references Cashier(cashierId),
    date datetime default now(),
    total_due numeric,
    status int not null,
    payment_method int not null,
    note varchar(50) default ' '
);

create table Category(																												
	category_id int auto_increment primary key not null,
    category_name varchar(150)
);
insert into Category(category_name) values 
			('Trà Sữa'),
            ('Fresh Fruit Tea'),
            ('Macchiato'),
            ('Special Drink'),
            ('Beauty Drinks');

create table Topping(
	topping_id int auto_increment primary key not null,
    topping_name varchar(250) not null,
    unit_price numeric not null
);

insert into Topping(topping_name, unit_price) values 
			('Trân Châu Sương Mai', 9000),
            ('Hạt Dẻ', 8000),
            ('Trân Châu Baby', 8000),
            ('Cream Cake', 9000),
            ('Khoai Môn', 9000),
            ('Trân Châu Hoàng Kim', 9000),
            ('Thạch Băng Tuyết', 8000),
            ('Machiato', 9000),
            ('Pudding', 8000),
            ('Rau Câu', 8000),
            ('Thạch Cafe', 8000),
            ('Trân Châu Sợi', 8000),
            ('Đậu Đỏ', 8000),
            ('Trân Châu Ruby', 8000);

create table Product(
	product_id int auto_increment primary key not null,
    product_category_id int not null,
    product_name varchar(150) not null,
    unit_price double not null,
    product_type varchar(10),
    product_size varchar(10) default '1,2',
    product_sugar varchar(20),
    product_ice varchar(20),
    quantity int not null,
    foreign key(product_category_id) references Category(category_id)
);
alter table Product
drop column product_size,
add column product_size varchar(20) default '1,2';

-- 1- Không đường, 2- 30%, 3- 50%, 4- 70%, 5- 100%
-- 1- Không đá mát, 2- 30%, 3- 50%, 4- 70%, 5- 100%, 6- Không đá, 7- Làm nóng
insert into Product(product_category_id, product_name, quantity, unit_price, product_type, product_sugar, product_ice) values
				-- Trà Sữa
					(1, 'Matcha Đậu Đỏ', '50', "25000", '2', '1,2,3,4,5', '1,2,3,4,5,6,7'),                          -- 1
                    (1, 'Tiger Sugar', '50', "25000", '2', '4,5', '1,2,3,4,5,6,7'),                            -- 2
                    (1, 'Sữa Tươi Khoai Môn Hoàng Kim', '50', "25000", '1,2', '4,5', '1,2,3,4,5,6,7'),           -- 3
                    (1, 'Sữa Tươi Trân Châu Baby kem Cafe', '50', "25000", '2', '1,2,3,4,5', '5'),       -- 4
                    (1, 'Ô Long Trân Châu Baby Kem Cafe', '50', "25000", '2', '1,2,3,4,5', '5'),         -- 5
                    (1, 'Trà Xanh', '50', "25000", '1, 2', '1,2,3,4,5', '1,2,3,4,5,6,7'),                               -- 6
                    (1, 'Trà Sữa Ba Anh Em', '50', "25000", '1,2', '1,2,3,4,5', '1,2,3,4,5,6,7'),                      -- 7
                    (1, 'Trà Xanh Sữa Vị Nhài', '50', "25000", '1,2,', '1,2,3,4,5', '1,2,3,4,5,6,7'),                   -- 8
                    (1, 'Trà Sữa Hạnh Phúc', '50', "25000", '1,2', '1,2,3,4,5', '1,2,3,4,5,6,7'),                      -- 9
                    (1, 'Trà Sữa Matcha', '50', "25000", '1,2', '1,2,3,4,5', '1,2,3,4,5,6,7'),                         -- 10
                    (1, 'Trà Sữa Ô Long', '50', "25000", '1,2', '1,2,3,4,5', '1,2,3,4,5,6,7'),                         -- 11
                    (1, 'Ô Long Thái Cực', '50', "25000", '1,2', '1,2,3,4,5', '1,2,3,4,5,6,7'),                        -- 12
                    (1, 'Trà Sữa Caramal Grilles 130', '50', "25000", '1,2', '4,5', '1,2,3,4,5,6,7'),            -- 13
                    (1, 'Trà Sữa Khoai Môn Hoàng Kim', '50', "25000", '1,2', '1,2,3,4,5', '1,2,3,4,5,6,7'),            -- 14
                    (1, 'Trà Sữa Dâu Tây', '50', "25000", '1,2', '1,2,3,4,5', '1,2,3,4,5,6,7'),                        -- 15
                    (1, 'Trà Sữa Trân Châu Hoàng Gia', '50', "25000", '1,2', '1,2,3,4,5', '1,2,3,4,5,6,7'),            -- 16
                    (1, 'Hồng Trà', '50', "25000", '1, 2', '1,2,3,4,5', '1,2,3,4,5,6,7'),                               -- 17
                    (1, 'Trà Sữa Panda', '50', "25000", '1,2', '1,2,3,4,5', '1,2,3,4,5,6,7'),                          -- 18
                    (1, 'Trà Sữa Kim Cương Đen Okinawa', '50', "25000", '1,2', '4,5', '1,2,3,4,5,6,7'),          -- 19
                    (1, 'Trà Sữa Socola', '50', "25000", '1,2', '1,2,3,4,5', '1,2,3,4,5,6,7'),                         -- 20
                    (1, 'Trà Sữa Bạc Hà', '50', "25000", '1,2', '4,5', '1,2,3,4,5,6,7'),                         -- 21
                    (1, 'Trà Sữa', '50', "25000", '1.2', '1,2,3,4,5', '1,2,3,4,5,6,7'),                                -- 22
                -- Fresh Fruit Tea
                    (2, 'Trà Dứa Hồng Hạc', '50', "25000", '2', '1,2,3,4,5', '1,2,3,4,5,6,7'),                       -- 23
                    (2, 'Probi Bưởi Trân Châu Sương Mai', '50', "25000", '2', '2,3,4,5', '1,2,3,4,5,6,7'),         -- 24
                    (2, 'Probi Xoài Trân Châu Sương Mai', '50', "25000", '2', '1,2,3,4,5', '1,2,3,4,5,6,7'),         -- 25
                    (2, 'Hồng Long Xoài Trân Châu Baby', '50', "25000", '2', '1,2,3,4,5', '1,2,3,4,5,6,7'),          -- 26
                    (2, 'Trà Xanh Chanh Leo', '50', "25000", '2', '1,2,3,4,5', '1,2,3,4,5,6,7'),                     -- 27
                    (2, 'Trà Xanh Xoài', '50', "25000", '2', '1,2,3,4,5', '1,2,3,4,5,6,7'),                          -- 28
                    (2, 'Trà Dứa Nhiệt Đới', '50', "25000", '2', '1,2,3,4,5', '1,2,3,4,5,6,7'),                      -- 29
                    (2, 'Hồng Long Pha Lê Tuyết', '50', "25000", '2', '5', '1,2,3,4,5,6,7'),                 -- 30
                    (2, 'Hồng Long Bạch Ngọc', '50', "25000", '2', '2', '1,2,3,4,5,6,7'),                    -- 31
                    (2, 'Hồng Trà Bưởi Mật Ong', '50', "25000", '1,2', '1,2,3,4,5', '1,2,3,4,5,6,7'),                  -- 32
                -- Macchiato
                    (3, 'Ô Long Kem Phô Mai', '50', "25000", '2', '1,2,3,4,5', '4,5'),                     -- 33
                    (3, 'Dâu Tằm Kem Phô Mai', '50', "25000", '2', '1,2,3,4,5', '4,5'),                    -- 34
                    (3, 'Hồng Trà Kem Phô Mai', '50', "25000", '2', '1,2,3,4,5', '4,5'),                   -- 35
                    (3, 'Trà Xanh Kem Phô Mai', '50', "25000", '2', '4, 5', '1,2,3,4,5,6,7'),                   -- 36
                    (3, 'Socola Kem Phô Mai', '50', "25000", '2', '1,2,3,4,5', '4,5'),                     -- 37
                    (3, 'Matcha Kem Phô Mai', '50', "25000", '2', '1,2,3,4,5', '4,5,6'),                     -- 38
                -- Special Drink
                    (4, 'Song Long Bạch Ngọc', '50', "48000", '2', '1,2,3,4,5', '5'),                    -- 39
                    (4, 'Choco Creamcake Hạt Dẻ', '50', "25000", '2', '1,2,3,4,5', '5'),                 -- 40
					(4, 'Ruby Creamcake Hạt Dẻ', '50', "25000", '2', '1,2,3,4,5', '5'),                  -- 41
                    (4, 'Chanh Leo Trân Châu Sương Mai', '50', "25000", '2', '5', '5'),          -- 42
                    (4, 'Probi Bưởi Trân Châu Sương Mai', '50', "25000", '2', '1,2,3,4,5', '1,2,3,4,5,6,7'),         -- 43
                    (4, 'Probi Xoài Trân Châu Sương Mai', '50', "25000", '2', '1,2,3,4,5', '1,2,3,4,5,6,7'),         -- 44
                    (4, 'Cream - Catcher Cafe', '50', "25000", '2', '4,5', '1,2,3,4,5,6,7'),                   -- 45
                    (4, 'Energy - Booster Cafe', '50', "25000", '2', '4,5', '1,2,3,4,5,6,7'),                  -- 46
                -- Beauty Drinks
                    (5, 'Sữa Chua Thanh Long Hạt Dẻ', '50', "42000", '2', '4,5', '4,5'),             -- 47
                    (5, 'Sữa Chua Dâu Tằm Hoàng Kim', '50', "39000", '2', '4,5', '4,5'),             -- 48
                    (5, 'Sữa Chua Dâu Tằm Hạt Dẻ', '50', "42000", '2', '4,5', '4,5'),                -- 49
                    (5, 'Sữa Chua Trắng', '50', "32000", '2', '4,5', '4,5');                         -- 50


-- create table invoice detail
create table InvoiceDetail(
	invoice_detail_no int not null primary key auto_increment,
	invoice_no int not null,
    product_id int not null,
    amount int default 1,
    size varchar(2),
    type varchar(2),
    sugar varchar(2),
    ice varchar(2),
    foreign key(invoice_no) references Invoice(invoice_no),
    foreign key(product_id) references Product(product_id)
);

create table InvoiceDetailTopping(
	topping_detail_no int not null primary key auto_increment,
	invoice_no int not null,
    product_id int not null,
    topping_id int not null,
    foreign key(invoice_no) references Invoice(invoice_no),
    foreign key(product_id) references Product(product_id),
    foreign key(topping_id) references Topping(topping_id)
);

select *from Invoice;
select *from InvoiceDetail;
select *from InvoiceDetailTopping;

alter table Invoice auto_increment = 1001;
insert into Invoice(invoice_cashierId, total_due, status, payment_method, note) value
				(2, 100000, 2, 1, 'tien order');
insert into InvoiceDetail(invoice_no, product_id, amount, size, type, sugar, ice) values
				(1001, 3, 2, '1', '1', '2', '3'),
                (1001, 3, 1, '1', '2', '5', '7');
insert into InvoiceDetailTopping(invoice_no, product_id, topping_id) values
				(1001, 12, 12),
                (1001, 12, 1),
                (1001, 3, 4),
                (1001, 2, 9);
