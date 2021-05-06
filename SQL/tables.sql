use Course_Project;

create table Admin
(
Id int,
Login nvarchar(50),
Password nvarchar(50)
)
drop table Admin
insert into Admin(Id,Login, Password)
	values (1,'Admin', 'Admin')

create table Customers
(
Customer_id int primary key identity(1,1),
Login nvarchar(50),
Password nvarchar(max),
Name nvarchar(20)
)

select * from Orders

select * from Customers

create table Masters
(
Master_id int primary key identity(1,1),
Name nvarchar(20),
Surname nvarchar(20),
Login nvarchar(30),
Password nvarchar(max),
NumberOfCompletedOrders int default 0,
Income int default 0
)

update Masters set NumberOfCompletedOrders=0, Income=0


select top(1) * from Masters order by Income desc

select Masters.Surname, TypesOfServices.Service, TypesOfServices.Price, Orders.Photo
from Orders inner join Masters
on Orders.Master_id=Masters.Master_id
inner join TypesOfServices
on TypesOfServices.Service_id=Orders.Services_id
where Orders.Customer_id=2

drop table Customers
drop table Orders
drop table Masters

insert into Masters(Name, Surname, Login, Password)
	values('Виктор','Петров','ttttt','123456')

create table Laptops
(
	Producer nvarchar(50) primary key
)

create table Models
(
	Producer nvarchar(50) foreign key
	references Laptops(Producer),
	Model nvarchar(50)
)

insert into Laptops(Producer)
	values('Acer'),
		  ('Apple'),
		  ('Asus'),
		  ('Dell'),
		  ('Dream Machines'),
		  ('HP'),
		  ('Huawei'),
		  ('Lenovo'),
		  ('Prestigio'),
		  ('Xiaomi')

drop table Models

insert into Models(Producer, Model)
	values('Acer','Aspire A315-21G-955U'),
		  ('Acer','Nitro 5 AN515-52-50NB'),
		  ('Acer','Aspire 5 A515-54-38HR'),
		  ('Acer','Aspire 5 A515-54G-30WF'),
		  ('Acer','Aspire 3 A317-51G-50AD'),
		  ('Acer','Nitro 5 AN515-54-79MM'),
		  ('Acer','Aspire A315-21-94QT'),
		  ('Acer','Acer Predator PH315-52-768W'),
		  ('Acer','Predator PH517-51-59A6'),
		  ('Acer','Aspire 3 A317-51G-35PU'),
		  ('Acer','A315-34-C6W0'),
		  ('Acer','Swift 5 SF514-52T-590S'),
		  ('Acer','Aspire A315-51-39TT'),
		  ('Acer','Aspire A515-54G-57LM'),
		  ('Apple','MacBook Air 13" 2020 512GB/MVH22'),
		  ('Apple','MacBook Air 13" 2019 128GB/MVFH2'),
		  ('Apple','MacBook Pro 13" Touch Bar 2019 128GB/MUHN2'),
		  ('Apple','MacBook Pro 16" Touch Bar 2019 512GB/MVVL2'),
		  ('Apple','MacBook Air 13" 2020 256GB/MWTK2'),
		  ('Apple','MacBook Pro 13" Touch Bar 2019 128GB/MUHQ2'),
		  ('Apple','MacBook Pro 13" 128GB/MPXQ2'),
		  ('Apple','MacBook Pro 15" Touch Bar/MR962'),
		  ('Asus','TUF Gaming FX505DY-BQ009'),
		  ('Asus','X407MA-BV088T'),
		  ('Asus','X509FB-EJ221'),
		  ('Asus','VivoBook X512FL-BQ287'),
		  ('Asus','TUF Gaming FX505DT-AL187'),
		  ('Asus','X571GT-BQ214'),
		  ('Asus','X509FB-EJ185'),
		  ('Asus','X509FJ-EJ014'),
		  ('Asus','ROG Strix G G731GT-AU004T'),
		  ('Asus','ROG Strix G G531GT-AL499'),
		  ('Asus','F509FJ-EJ392'),
		  ('Asus','ROG Zephyrus G15 GA502IU-AL051'),
		  ('Asus','VivoBook X505ZA-BR004'),
		  ('Asus','ROG Zephyrus S GX502GV-AZ035'),
		  ('Dell','Vostro 3581 (210-ARKV-273277506)'),
		  ('Dell','Inspiron 3582-9881'),
		  ('Dell','Inspiron 15 (3583-1940)'),
		  ('Dell','Inspiron G3 15 (3590-4888)'),
		  ('Dell','Vostro 5481 (210-AQZC-273227235)'),
		  ('Dell','G3 15 (3590-4830)'),
		  ('Dell','Inspiron 15 (3583-2877)'),
		  ('Dell','Vostro 3590 (210-ASVS-273284148)'),
		  ('Dell','Vostro 14 (5490-279955)'),
		  ('Dell','Dell G3 17 (3779-9123)'),
		  ('Dell','XPS 13 (9380-0150)'),
		  ('Dell','XPS 13 (9370-1695)'),
		  ('Dell','Vostro (3590-275493)'),
		  ('Dell','Inspiron 15 (3576-1480)'),
		  ('Dell','Inspiron 17 (3793-2928)'),
		  ('Dream Machines','G1660Ti-15BY45'),
		  ('Dream Machines','G1660Ti-15BY40'),
		  ('Dream Machines','G1660Ti-15BY41'),
		  ('Dream Machines','G1650-15BY40'),
		  ('Dream Machines','G1660Ti-15BY21'),
		  ('Dream Machines','G1650-15BY22'),
		  ('Dream Machines','G1650-15BY28'),
		  ('Dream Machines','G1660Ti-15BY20'),
		  ('Dream Machines','G1050-15BY51'),
		  ('Dream Machines','G1050Ti-15BY28'),
		  ('Dream Machines','G1050-15BY57'),
		  ('Dream Machines','G1050-17BY57'),
		  ('Dream Machines','G1050-17BY58'),
		  ('HP','15-da0482ur'),
		  ('HP','15-db0113ur'),
		  ('HP','15-db0441ur'),
		  ('HP','Pavilion Gaming 15-cx0124ur'),
		  ('HP','15-db0452ur'),
		  ('HP','250 G7(6MQ39EA)'),
		  ('HP','250 G7(6BP26EA)'),
		  ('HP','Pavilion 13-an0031ur'),
		  ('HP','HP Omen 15-dc0084ur'),
		  ('HP','15-db0429ur(7BW51EA)'),
		  ('HP','255 G7 (7DF20EA)'),
		  ('HP','HP Pavilion Gaming 15-ec0035ur'),
		  ('HP','15-db0450ur'),
		  ('HP','255 G7 (6BP86ES)'),
		  ('HP','ProBook 455 G6 (7QL74ES)'),
		  ('Huawei','MateBook X Pro MACH-W19'),
		  ('Lenovo','IdeaPad L340-15'),
		  ('Lenovo','IdeaPad S145-15'),
		  ('Lenovo','IdeaPad S145-15AST'),
		  ('Lenovo','IdeaPad S145-15IGM'),
		  ('Lenovo','IdeaPad S145-15API'),
		  ('Lenovo','IdeaPad S145-15IWL'),
		  ('Lenovo','IdeaPad 330-15ARR'),
		  ('Lenovo','IdeaPad L340-15IRH Gaming'),
		  ('Lenovo','IdeaPad L340-15API'),
		  ('Lenovo','IdeaPad 330-15AST'),
		  ('Lenovo','IdeaPad 330-15IKBR'),
		  ('Lenovo','Legion Y540-15IRH'),
		  ('Lenovo','Legion Y530-15ICH'),
		  ('Lenovo','ThinkBook 13s-IWL'),
		  ('Prestigio','SmartBook 141 C3/DB_CIS'),
		  ('Prestigio','SmartBook 141 C3/DG_CIS'),
		  ('Prestigio','SmartBook 141 C4/MG_CIS'),
		  ('Prestigio','SmartBook 141 C4/DG_CIS'),
		  ('Prestigio','Visconte Ecliptica'),
		  ('Xiaomi','Mi Notebook/JYU4140CN'),
		  ('Xiaomi','Mi Notebook Pro/JYU4057CN'),
		  ('Xiaomi','Mi Notebook Pro/JYU4147CN'),
		  ('Xiaomi','Mi Notebook Pro/JYU4035CN'),
		  ('Xiaomi','Mi Notebook/JYU4093CN'),
		  ('Xiaomi','Mi Notebook Air/JYU4149CN'),
		  ('Xiaomi','RedmiBook JYU4153CN')



select Laptops.Producer, Models.Model
from Laptops inner join Models
on Laptops.Producer=Models.Producer

drop table Masters

create table TypesOfServices
(
Service_id int primary key identity(1,1),
Service nvarchar(50),
Price int
)


drop table TypesOfServices

insert into TypesOfServices(Service, Price)
	values('Диагностика', 5),
	('Замена сетевой карты', 21),
	('Ремонт материнской платы', 73),
	('Ремонт блока питания', 26),
	('Ремонт кулера', 18),
	('Замена процессора', 48),
	('Замена ОЗУ', 14),
	('Установка/переустановка ПО', 25),
	('Подключение периф. устройств', 15),
	('Установка драйверов', 8),
	('Замена/ремонт видеокарты', 150),
	('Замена матрицы', 50),
	('Чистка аппаратов после попадания влаги', 45),
	('Замена/ремонт клавиатуры', 37),
	('Замена динамиков', 30),
	('Замена разъемов', 15),
	('Ремонт петель', 20),
	('Чистка системы охлаждения', 25),
	('Другое', 0)

	drop table Orders

	
create table Orders
(
Order_id int primary key identity(1,1),
Customer_id int,
Master_id int,
Services_id int,
Producer nvarchar(50),
Model nvarchar(50),
Description_of_problem nvarchar(max),
Photo varbinary(max),
Format_of_photo nvarchar(30),
State bit,
IsCheck bit default 0,
foreign key(Customer_id) references Customers(Customer_id),
foreign key(Master_id) references Masters(Master_id),
foreign key(Services_id) references TypesOfServices(Service_id)
)
													
select Masters.Surname, TypesOfServices.Service
from Masters inner join Orders
on Masters.Master_id=Orders.Master_id
inner join TypesOfServices
on Orders.Services_id=TypesOfServices.Service_id
where Orders.IsCheck=0



select * from Masters



