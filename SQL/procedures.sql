create procedure addCustomer @login nvarchar(30), @password nvarchar(30), @name nvarchar(30)
as 
begin
insert into Customers(Login, Password, Name)
	values(@login, @password, @name)
end

create procedure addMaster @login nvarchar(30), @password nvarchar(30), @name nvarchar(30), @surname nvarchar(30)
as
begin
insert into Masters(Name, Surname, Login, Password)
	values(@name, @surname, @login, @password)
end

select * from Masters

create procedure getCustomers
as
select Login from Customers

create procedure getMasters
as
select Login from Masters

create procedure LoginAccount
as 
select Masters.Login, Masters.Password, Customers.Login, Customers.Password, Admin.Login, Admin.Password
from Customers left join Masters
on Masters.Master_id=Customers.Customer_id 
left join Admin
on Masters.Master_id=Admin.Id

select Masters.Login, Masters.Password, Customers.Login, Customers.Password, Admin.Login, Admin.Password
from Customers,Admin, Masters

select Login, Password from Admin
drop procedure LoginAccount


create procedure editPassword @oldPassword nvarchar(30), @newPassword nvarchar(30), @login nvarchar(30)
as
begin
update Customers set Password=@newPassword where Login=@login and Password=@oldPassword
end

create procedure checkOldPasswordCustomer 
as 
select Login, Password from Customers

create procedure CustomerName
as
select Customer_id,Login, Name from Customers

drop procedure CustomerName

create procedure CheckCustomerId
as
select Login, Customer_id from Customers


create procedure MasterName
as
select Master_id ,Login, Name, Surname from Masters where Master_id>0

drop procedure MasterName



drop procedure addInAcceptedOrders

create procedure addInAcceptedOrders @order_id int, @master_id int
as
begin
	update Orders set Master_id=@master_id where Order_id=@order_id
end
create procedure addInAcceptedOrders @order_id int, @master_id int
as
begin
	update Orders set Master_id=@master_id where Order_id=@order_id
end

select * from Orders

create procedure addInReadyOrders @order_id int
as
begin
	update Orders set State=1 where Order_id=@order_id
end

create procedure editPasswordMaster @oldPassword nvarchar(30), @newPassword nvarchar(30), @login nvarchar(30)
as
begin
update Masters set Password=@newPassword where Login=@login and Password=@oldPassword
end

create procedure checkOldPasswordMaster
as 
select Login, Password from Masters

create procedure activeOrders
as
select  Orders.Order_id, Masters.Surname, TypesOfServices.Service, TypesOfServices.Price , Customers.Login
from Masters inner join Orders
on Masters.Master_id=Orders.Master_id
inner join TypesOfServices on Orders.Services_id=TypesOfServices.Service_id
inner join Customers on Orders.Customer_id=Customers.Customer_id
where Orders.State=0 and Orders.Order_id is not null


create procedure fillingFields
as
select Service, Price from TypesOfServices


create procedure fillingFields1
as
select Models.Producer, Models.Model from Models inner join Laptops
on Laptops.Producer=Models.Producer where Models.Producer='Acer'

create procedure fillProd
as
select Producer from Laptops

drop procedure activeOrders

create procedure notAcceptedOrders 
as
select Orders.Order_id, Orders.Master_id, Customers.Name, TypesOfServices.Price, TypesOfServices.Service, Orders.Producer, Orders.Model, Orders.Description_of_problem, Orders.Photo, Orders.State
from Customers Inner Join Orders
on Customers.Customer_id=Orders.Customer_id
inner join TypesOfServices on Orders.Services_id=TypesOfServices.Service_id
where Orders.Master_id is null

drop procedure notAcceptedOrders

create procedure acceptedOrders 
as
select Orders.Order_id, Orders.Master_id, Customers.Name, TypesOfServices.Price, TypesOfServices.Service, Orders.Producer, Orders.Model, Orders.Description_of_problem, Orders.Photo, Orders.State
from Customers Inner Join Orders
on Customers.Customer_id=Orders.Customer_id
inner join TypesOfServices on Orders.Services_id=TypesOfServices.Service_id
where Orders.Master_id is not null and State=0 and Orders.Master_id>0

drop procedure readyOrders

create procedure readyOrders 
as
select Orders.Order_id, Orders.Master_id, Customers.Name, TypesOfServices.Price, TypesOfServices.Service, Orders.Producer, Orders.Model, Orders.Description_of_problem, Orders.Photo, Orders.State
from Customers Inner Join Orders
on Customers.Customer_id=Orders.Customer_id
inner join TypesOfServices on Orders.Services_id=TypesOfServices.Service_id
where Orders.State=1 and Orders.Master_id is not null and Orders.Master_id>0

create procedure Pick @order_id int,@service_id int, @master_id int 
as
begin
	update Orders set Services_id=@service_id, Master_id=@master_id where Order_id=@order_id 
end



select Orders.Order_id, Orders.Master_id, Customers.Name, TypesOfServices.Price, TypesOfServices.Service, Orders.Producer, Orders.Model, Orders.Description_of_problem, Orders.State
from  Orders Inner Join  Customers
on Customers.Customer_id=Orders.Customer_id
inner join TypesOfServices 
on Orders.Services_id=TypesOfServices.Service_id
where Orders.Master_id is null

select Orders.Order_id, Orders.Master_id, Masters.Name, Masters.Surname, Customers.Name, TypesOfServices.Price, TypesOfServices.Service, Orders.Producer, Orders.Model, Orders.Description_of_problem, Orders.State
from  Orders Inner Join  Customers
on Customers.Customer_id=Orders.Customer_id
inner join TypesOfServices 
on Orders.Services_id=TypesOfServices.Service_id
inner join Masters
on Orders.Master_id=Masters.Master_id
where Orders.Master_id is not null


select Orders.Order_id, Orders.Master_id, Masters.Name, Masters.Surname, Customers.Name, TypesOfServices.Price, TypesOfServices.Service, Orders.Producer, 
Orders.Model, Orders.Description_of_problem from Orders Inner Join  Customers on Customers.Customer_id = Orders.Customer_id 
inner join TypesOfServices on Orders.Services_id = TypesOfServices.Service_id inner join Masters 
on Orders.Master_id = Masters.Master_id where Orders.Master_id is not null and Orders.State=1

select Orders.Order_id, Customers.Name, TypesOfServices.Price, TypesOfServices.Service, Orders.Producer, 
Orders.Model, Orders.Description_of_problem from Orders Inner Join  Customers on Customers.Customer_id = Orders.Customer_id 
inner join TypesOfServices on Orders.Services_id = TypesOfServices.Service_id where Orders.Master_id is null

select * from Orders
select * from sys.tables
--exec addUser 'admin','admin','tolik'