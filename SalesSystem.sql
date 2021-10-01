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
                
			
select *from Cashier where userName = 'Administrator' and password='ec876c82eca6bf0024fc9d37569541e1';
create table Invoice(
	invoice_no int primary key,
    invoice_cashierId int not null,
	foreign key(invoice_cashierId) references Cashier(cashierId),
    date datetime not null,
    total_due numeric,
    status int not null,
    payment_method int not null,
    note varchar(50)
);
create table Category(
	category_id int auto_increment primary key not null,
    category_name varchar(150),
    is_active boolean
);
insert into Category(category_name, is_active) values 
			('Trà Sữa', true),
            ('Fresh Fruit Tea', true),
            ('Macchiato', true),
            ('Special Drink', true),
            ('Beauty Drinks', true);

select *from Category;

create table Topping(
	topping_id int auto_increment primary key not null,
    topping_name varchar(250) not null,
    unit_price numeric not null,
    is_active boolean
);
insert into Topping(topping_name, unit_price, is_active) values 
			('Trân Châu Sương Mai', 9000, true),
            ('Hạt Dẻ', 8000, true),
            ('Trân Châu Baby', 8000, true),
            ('Cream Cake', 9000, true),
            ('Khoai Môn', 9000, true),
            ('Trân Châu Hoàng Kim', 9000, true),
            ('Thạch Băng Tuyết', 8000, true),
            ('Machiato', 9000, true),
            ('Pudding', 8000, true),
            ('Rau Câu', 8000, true),
            ('Thạch Cafe', 8000, true),
            ('Trân Châu Sợi', 8000, true),
            ('Đậu Đỏ', 8000, true),
            ('Trân Châu Ruby', 8000, true);

select *from Topping;

create table Type(
	type_id int primary key,
    type_value varchar(40) not null
);

insert into Type(type_id, type_value) values 
				(1, 'Làm Nóng'), -- Làm nóng
                (2, 'Làm Lạnh'); -- Làm Lạnh
create table Sugar(
	sugar_id int not null primary key,
    percent varchar(50) not null
);

insert into Sugar(sugar_id, percent) values
				(1, 'Không Đường'), -- Không đường
                (2, '30% Đường'), -- 30%
                (3, '50% Đường'), -- 50%
                (4, '70% Đường'), -- 70%
                (5, '100% Đường'); -- 100% Đường
                
                
create table Ice(
	ice_id int not null primary key,
    percent varchar(50) not null
);

insert into Ice(ice_id, percent) values 
				(1, 'Không Đá Mát'), -- Không đá - mát
                (2, '30% Đá'), -- 30%
                (3, '50% Đá'), -- 50%
                (4, '70% Đá'), -- 70%
                (5, '100% Đá'), -- 100%
                (6, 'Không Đá'), -- Không đá
                (7, 'Làm Nóng'); -- Làm nóng

create table Product(
	product_id int auto_increment primary key not null,
    product_category_id int not null,
    foreign key(product_category_id) references Category(category_id),
    product_name varchar(150) not null,
    unit_price double not null,
    quantity int not null,
    is_active boolean not null
);

insert into Product(product_category_id, product_name, quantity, is_active, unit_price) values
					-- Trà Sữa
					(1, 'Matcha Đậu Đỏ', '50', true, "25000"),
                    (1, 'Tiger Sugar', '50', true, "25000"),
                    (1, 'Sữa Tươi Khoai Môn Hoàng Kim', '50', true, "25000"),
                    (1, 'Sữa Tươi Trân Châu Baby kem Cafe', '50', true, "25000"),
                    (1, 'Ô Long Trân Châu Baby Kem Cafe', '50', true, "25000"),
                    (1, 'Trà Xanh', '50', true, "25000"),
                    (1, 'Trà Sữa Hạnh Phúc', '50', true, "25000"),
                    (1, 'Trà Sữa Matcha', '50', true, "25000"),
                    (1, 'Trà Sữa Ô Long', '50', true, "25000"),
                    (1, 'Ô Long Thái Cực', '50', true, "25000"),
                    (1, 'Trà Sữa Caramal Grilles 130', '50', true, "25000"),
                    (1, 'Trà Sữa Khoai Môn Hoàng Kim', '50', true, "25000"),
                    (1, 'Trà Sữa Ba Anh Em', '50', true, "25000"),
                    (1, 'Trà Sữa Vị Nhài', '50', true, "25000"),
                    (1, 'Hồng Trà', '50', true, "25000"),
                    (1, 'Trà Sữa Panda', '50', true, "25000"),
                    (1, 'Trà Sữa Kim Cương Đen Okinawa', '50', true, "25000"),
                    (1, 'Trà Sữa Socola', '50', true, "25000"),
                    (1, 'Trà Sữa Bạc Hà', '50', true, "25000"),
                    (1, 'Trà Sữa Dâu Tây', '50', true, "25000"),
                    (1, 'Trà Sữa Trân Châu Hoàng Gia', '50', true, "25000"),
                    (1, 'Trà Sữa', '50', true, "25000"),
                    -- Fresh Fruit Tea
                    (2, 'Trà Dứa Hồng Hạc', '50', true, "25000"),
                    (2, 'Hồng Long Xoài Trân Châu Baby', '50', true, "25000"),
                    (2, 'Probi Bưởi Chân Châu Sương Mai', '50', true, "25000"),
                    (2, 'Probi Xoài Trân Châu Sương Mai', '50', true, "25000"),
                    (2, 'Trà Xanh Chanh Leo', '50', true, "25000"),
                    (2, 'Trà Xanh Xoài', '50', true, "25000"),
                    (2, 'Trà Dứa Nhiệt Đới', '50', true, "25000"),
                    (2, 'Hồng Long Pha Lê Tuyết', '50', true, "25000"),
                    (2, 'Hồng Long Bạch Ngọc', '50', true, "25000"),
                    (2, 'Hồng Trà Bưởi Mật Ong', '50', true, "25000"),
                    -- Macchiato
                    (3, 'Ô Long Kem Phô Mai', '50', true, "25000"),
                    (3, 'Dâu Tằm Kem Phô Mai', '50', true, "25000"),
                    (3, 'Hồng Trà Kem Phô Mai', '50', true, "25000"),
                    (3, 'Trà Xanh Kem Phô Mai', '50', true, "25000"),
                    (3, 'Socola Kem Phô Mai', '50', true, "25000"),
                    (3, 'Matcha Kem Phô Mai', '50', true, "25000"),
                    -- Special Drink
                    (4, 'Song Long Bạch Ngọc', '50', true, "48000"),
                    (4, 'Choco Creamcake Hạt Dẻ', '50', true, "25000"),
					(4, 'Ruby Creamcake Hạt Dẻ', '50', true, "25000"),
                    (4, 'Chanh Leo Trân Châu Sương Mai', '50', true, "25000"),
                    (4, 'Probi Bưởi Trân Châu Sương Mai', '50', true, "25000"),
                    (4, 'Probi Xoài Trân Châu Sương Mai', '50', true, "25000"),
                    (4, 'Cream - Catcher Cafe', '50', true, "25000"),
                    (4, 'Energy - Booster Cafe', '50', true, "25000"),
                    -- Beauty Drinks
                    (5, 'Sữa Chua Thanh Long Hạt Dẻ', '50', true, "42000"),
                    (5, 'Sữa Chua Dâu Tằm Hoàng Kim', '50', true, "39000"),
                    (5, 'Sữa Chua Dâu Tằm Hạt Dẻ', '50', true, "42000"),
                    (5, 'Sữa Chua Trắng', '50', true, "32000");

create table ProductTypes(
	product_id int not null,
    type_id int not null,
    primary key(product_id, type_id),
    foreign key(product_id) references Product(product_id),
    foreign key(type_id) references Type(type_id)
);


insert into ProductTypes(product_id, type_id) value
			(1, 2), -- Matcha Đậu Đỏ
            (2, 2), -- Tiger Sugar
            (3, 1), (3, 2), -- Khoai Môn Hoàng Kim
            (4, 1), -- Kem Cafe
            (5, 1), -- Baby Kem Cafe
            (6, 1), (6, 2), -- Trà Xanh
            (7, 1), (7, 2), -- Caramel 130
            (8, 1), (8, 2), -- Khoai Môn Hoàng Kim
            (9, 1), (9, 2), -- Ba Anh Em
            (10, 1), (10, 2), -- Vị Nhài
            (11, 1), (11, 2), -- Hạnh Phúc
            (12, 1), (12, 2), -- Matcha
            (13, 1), (13, 2), -- Ô Long
            (14, 1), (14, 2), -- Ô Long Thái Cực
            (15, 1), (15, 2), -- Hồng Trà
            (16, 1), (16, 2), -- Panda
            (17, 1), (17, 2), -- Kim Cương Đen Okinawa
            (18, 1), (18, 2), -- Socola
            (19, 1), (19, 2), -- Bạc hà
            (20, 1), (20, 2), -- Dâu Tây
            (21, 1), (21, 2), -- Trân Châu Hoàng Gia
            (22, 1), (22, 2), -- Trà Sữa
            -- Fresh Fruit Tea
            (23, 2), -- Hồng hạc
            (24, 2), -- Xoài Trân Châu Baby
            (25, 2), -- Bưởi Trân Châu Sương Mai
            (26, 2), -- Xoài Trân Châu Sương Mai
            (27, 2), -- Chanh Leo
            (28, 2), -- Xoài
            (29, 2), -- Dứa Nhiệt Đới
            (30, 2), -- Pha Lê Tuyết
            (31, 2), -- Bạch Ngọc
            (32, 2), (32, 1), -- Bưởi Mật Ong
            -- Macchiato
            (33, 2), -- Ô Long Kem Phô Mai
            (34, 2), -- Dâu Tằm kem Phô Mai
            (35, 2), -- Hồng Trà Kem Phô Mai
            (36, 2), -- Trà Xanh Kem Phô Mai
            (37, 2), -- Matcha Kem Phô Mai
            -- 
            (39, 2), -- Song Long Bạch Ngọc
            (40, 2), -- Choco Creamcake hạt dẻ
            (41, 2), -- Ruby Creamcake Hạt Dẻ
            (42, 2), -- Chanh Leo Trân Châu Sương Mai 
            (43, 2), -- 
            (44, 2), -- 
            (45, 2), -- 
            (46, 2), -- 
            --
            (47, 2), --
            (48, 2), --
            (49, 2), --
            (50, 2);
            
            
create table ProductSugar(
	product_id int,
    sugar_id int,
    primary key (product_id, sugar_id),
    foreign key(product_id) references Product(product_id),
    foreign key(sugar_id) references Sugar(sugar_id)
); 
create table ProductIce(
	product_id int,
    ice_id int,
    primary key(product_id, ice_id),
    foreign key(product_id) references Product(product_id),
    foreign key(ice_id) references Ice(ice_id)
);
insert into ProductSugar(product_id, sugar_id) values 
-- 1- Không đường, 2- 30%, 3- 50%, 4- 70%, 5- 100%
		-- Trà Sữa
			(1, 1), (1, 2), (1, 3), (1, 4), (1, 5),  -- Matcha Đậu Đỏ
            (2, 4), (2, 5), -- Tiger Sugar
            (3, 4), (3, 5), -- STKM Hoàng Kim
			(4, 1), (4, 2), (4, 3), (4, 4), (4, 5),  -- 
			(5, 1), (5, 2), (5, 3), (5, 4), (5, 5),  --
			(6, 1), (6, 2), (6, 3), (6, 4), (6, 5),  --
			(7, 4), (7, 5), --
			(8, 1), (8, 2), (8, 3), (8, 4), (8, 5),  --
			(9, 1), (9, 2), (9, 3), (9, 4), (9, 5),  --
			(10, 1), (10, 2), (10, 3), (10, 4), (10, 5),  --
			(11, 1), (11, 2), (11, 3), (11, 4), (11, 5),  --
			(12, 1), (12, 2), (12, 3), (12, 4), (12, 5),  -- TS Matcha
			(13, 1), (13, 2), (13, 3), (13, 4), (13, 5),  --
			(14, 1), (14, 2), (14, 3), (14, 4), (14, 5),  --
			(15, 1), (15, 2), (15, 3), (15, 4), (15, 5),  --
			(16, 1), (16, 2), (16, 3), (16, 4), (16, 5),  --
            (17, 4), (17, 5), --
			(18, 1), (18, 2), (18, 3), (18, 4), (18, 5),  --
            (19, 4), (19, 5), --
			(20, 1), (20, 2), (20, 3), (20, 4), (20, 5),  --
			(21, 1), (21, 2), (21, 3), (21, 4), (21, 5),  --
			(22, 1), (22, 2), (22, 3), (22, 4), (22, 5);  --
            
insert into ProductIce(product_id, ice_id) values
-- 1- Không đá mát, 2- 30%, 3-50%, 4-70%, 5- 100%, 6- Không đá, 7-Làm nóng
				-- Trà Sữa
						(1, 1), (1, 2), (1, 3), (1, 4), (1, 5), (1, 6), (1, 7), -- Matcha Đậu Đỏ
						(2, 1), (2, 2), (2, 3), (2, 4), (2, 5), (2, 6), (2, 7), -- Tiger Sugar
                        (3, 1), (3, 2), (3, 3), (3, 4), (3, 5), (3, 6), (3, 7), -- Khoai môn hoàng kim
                        (4, 5), -- Baby kem cafe
                        (5, 5), -- Ô long trân châu kem cafe
                        (6, 1), (6, 2), (6, 3), (6, 4), (6, 5), (6, 6), (6, 7), -- Trà Xanh
                        (7, 1), (7, 2), (7, 3), (7, 4), (7, 5), (7, 6), (7, 7), -- Caramel 130
                        (8, 1), (8, 2), (8, 3), (8, 4), (8, 5), (8, 6), (8, 7), -- KM Hoàng Kim
                        (9, 1), (9, 2), (9, 3), (9, 4), (9, 5), (9, 6), (9, 7), -- Ba Anh Em
                        (10, 1), (10, 2), (10, 3), (10, 4), (10, 5), (10, 6), (10, 7), -- Vị Nhài
                        (11, 1), (11, 2), (11, 3), (11, 4), (11, 5), (11, 6), (11, 7), -- Hạnh Phúc
                        (12, 1), (12, 2), (12, 3), (12, 4), (12, 5), (12, 6), (12, 7), -- 
                        (13, 1), (13, 2), (13, 3), (13, 4), (13, 5), (13, 6), (13, 7), -- 
                        (14, 1), (14, 2), (14, 3), (14, 4), (14, 5), (14, 6), (14, 7), -- 
                        (15, 1), (15, 2), (15, 3), (15, 4), (15, 5), (15, 6), (15, 7), -- 
                        (16, 1), (16, 2), (16, 3), (16, 4), (16, 5), (16, 6), (16, 7), -- 
                        (17, 1), (17, 2), (17, 3), (17, 4), (17, 5), (17, 6), (17, 7), -- 
                        (18, 1), (18, 2), (18, 3), (18, 4), (18, 5), (18, 6), (18, 7), -- 
                        (19, 1), (19, 2), (19, 3), (19, 4), (19, 5), (19, 6), (19, 7), -- 
                        (20, 1), (20, 2), (20, 3), (20, 4), (20, 5), (20, 6), (20, 7), -- 
                        (21, 1), (21, 2), (21, 3), (21, 4), (21, 5), (21, 6), (21, 7), -- 
                        (22, 1), (22, 2), (22, 3), (22, 4), (22, 5), (22, 6), (22, 7); -- 
                        
create table InvoiceDetail(
	invoice_no int not null,
    product_id int not null,
    quantity int not null,
    unit_price numeric not null,
    primary key(invoice_no, product_id),
    foreign key(invoice_no) references Invoice(invoice_no),
    foreign key(product_id) references Product(product_id)
);

create table ProductToppings(
	product_id int,
    topping_id int,
    primary key(product_id, topping_id),
    foreign key(product_id) references Product(product_id),
    foreign key(topping_id) references Topping(topping_id)
);
insert into ProductToppings(product_id, topping_id) value 
			(1, 1), (1, 2), (1, 3), (1, 4), (1, 5), (1, 6), (1, 7), (1, 8), (1, 9), (1, 10), (1, 11), (1, 12), (1, 13), (1, 14),  --
			(2, 1), (2, 2), (2, 3), (2, 4), (2, 5), (2, 6), (2, 7), (2, 8), (2, 9), (2, 10), (2, 11), (2, 12), (2, 13), (2, 14), --
			(3, 1), (3, 2), (3, 3), (3, 4), (3, 5), (3, 6), (3, 7), (3, 8), (3, 9), (3, 10), (3, 11), (3, 12), (3, 13), (3, 14),  --
			(4, 1), (4, 2), (4, 3), (4, 4), (4, 5), (4, 6), (4, 7), (4, 8), (4, 9), (4, 10), (4, 11), (4, 12), (4, 13), (4, 14),  --
			(5, 1), (5, 2), (5, 3), (5, 4), (5, 5), (5, 6), (5, 7), (5, 8), (5, 9), (5, 10), (5, 11), (5, 12), (5, 13), (5, 14),  --
			(6, 1), (6, 2), (6, 3), (6, 4), (6, 5), (6, 6), (6, 7), (6, 8), (6, 9), (6, 10), (6, 11), (6, 12), (6, 13), (6, 14),  --
			(7, 1), (7, 2), (7, 3), (7, 4), (7, 5), (7, 6), (7, 7), (7, 8), (7, 9), (7, 10), (7, 11), (7, 12), (7, 13), (7, 14),  --
			(8, 1), (8, 2), (8, 3), (8, 4), (8, 5), (8, 6), (8, 7), (8, 8), (8, 9), (8, 10), (8, 11), (8, 12), (8, 13), (8, 14),  --
			(9, 1), (9, 2), (9, 3), (9, 4), (9, 5), (9, 6), (9, 7), (9, 8), (9, 9), (9, 10), (9, 11), (9, 12), (9, 13), (9, 14),  --
			(10, 1), (10, 2), (10, 3), (10, 4), (10, 5), (10, 6), (10, 7), (10, 8), (10, 9), (10, 10), (10, 11), (10, 12), (10, 13), (10, 14),  --
			(11, 1), (11, 2), (11, 3), (11, 4), (11, 5), (11, 6), (11, 7), (11, 8), (11, 9), (11, 10), (11, 11), (11, 12), (11, 13), (11, 14),  --
			(12, 1), (12, 2), (12, 3), (12, 4), (12, 5), (12, 6), (12, 7), (12, 8), (12, 9), (12, 10), (12, 11), (12, 12), (12, 13), (12, 14),  --
			(13, 1), (13, 2), (13, 3), (13, 4), (13, 5), (13, 6), (13, 7), (13, 8), (13, 9), (13, 10), (13, 11), (13, 12), (13, 13), (13, 14),  --
			(14, 1), (14, 2), (14, 3), (14, 4), (14, 5), (14, 6), (14, 7), (14, 8), (14, 9), (14, 10), (14, 11), (14, 12), (14, 13), (14, 14),  --
			(15, 1), (15, 2), (15, 3), (15, 4), (15, 5), (15, 6), (15, 7), (15, 8), (15, 9), (15, 10), (15, 11), (15, 12), (15, 13), (15, 14),  --
			(16, 1), (16, 2), (16, 3), (16, 4), (16, 5), (16, 6), (16, 7), (16, 8), (16, 9), (16, 10), (16, 11), (16, 12), (16, 13), (16, 14),  --
			(17, 1), (17, 2), (17, 3), (17, 4), (17, 5), (17, 6), (17, 7), (17, 8), (17, 9), (17, 10), (17, 11), (17, 12), (17, 13), (17, 14),  --
			(18, 1), (18, 2), (18, 3), (18, 4), (18, 5), (18, 6), (18, 7), (18, 8), (18, 9), (18, 10), (18, 11), (18, 12), (18, 13), (18, 14),  --
			(19, 1), (19, 2), (19, 3), (19, 4), (19, 5), (19, 6), (19, 7), (19, 8), (19, 9), (19, 10), (19, 11), (19, 12), (19, 13), (19, 14), --
			(20, 1), (20, 2), (20, 3), (20, 4), (20, 5), (20, 6), (20, 7), (20, 8), (20, 9), (20, 10), (20, 11), (20, 12), (20, 13), (20, 14),  --
			(21, 1), (21, 2), (21, 3), (21, 4), (21, 5), (21, 6), (21, 7), (21, 8), (21, 9), (21, 10), (21, 11), (21, 12), (21, 13), (21, 14),  --
			(22, 1), (22, 2), (22, 3), (22, 4), (22, 5), (22, 6), (22, 7), (22, 8), (22, 9), (22, 10), (22, 11), (22, 12), (22, 13), (22, 14);  --
create table InvoiceDetailTopping(
	invoice_no int not null,
    product_id int not null,
    topping_id int not null,
    quantity int not null,
    unit_price numeric,
    primary key(invoice_no, product_id, topping_id),
    foreign key(invoice_no) references Invoice(invoice_no),
    foreign key(product_id) references Product(product_id),
    foreign key(topping_id) references Topping(topping_id)
);

select *from Product, Topping, ProductToppings where (ProductToppings.product_id = 3) and (ProductToppings.topping_id = Topping.topping_id) and (Product.product_id = ProductToppings.product_id);
select *from Product, Type, ProductTypes where (ProductTypes.product_id = 3) and (ProductTypes.type_id = Type.type_id) and (Product.product_id = ProductTypes.product_id);
select *from Product, Sugar, ProductSugar where (ProductSugar.product_id = 3) and (ProductSugar.sugar_id = Sugar.sugar_id) and (Product.product_id = ProductSugar.product_id);
select *from Product, Ice, ProductIce where (ProductIce.product_id = 3) and (ProductIce.ice_id = Ice.ice_id) and (Product.product_id = ProductIce.product_id);


-- create user 'PF13vtca'@'localhost' identified by 'tienvtca';
-- grant all privileges on *.* to 'PF13vtca'@'localhost';