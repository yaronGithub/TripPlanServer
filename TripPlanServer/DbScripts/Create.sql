Use master
Go
IF EXISTS (SELECT * FROM sys.databases WHERE name = N'TripPlanDB')
BEGIN
	DROP DATABASE TripPlanDB;
END
Go

create database TripPlanDB
go
use TripPlanDB
go

create table Users
(
UserId int primary key Identity,
FirstName nvarchar(50) not null,
LastName nvarchar(50) not null,
Email nvarchar(50) unique not null,
Passwd nvarchar(50) not null,
PhoneNumber nvarchar(50) not null,
IsManager bit Not Null Default 0
)

create table PlanGroup
(
PlanId int primary key Identity,
GroupName nvarchar(50) not null,
UserId int foreign key References Users(UserId),
IsPublished bit not null default 0,
GroupDescription nvarchar(100) not null,
StartDate Date,
EndDate Date
)

create table UserGroup
(
PlanId int foreign key references PlanGroup(PlanId),
UserId int foreign key references Users(UserId),
CONSTRAINT PK_UserGroup PRIMARY KEY (PlanId,UserId)
)

create table Reviews
(
ReviewId int primary key identity,
Title nvarchar(50) not null,
PlanId int foreign key References PlanGroup(PlanId),
UserId int foreign key References Users(UserId),
Stars int,
ReviewText nvarchar(100) not null,
ReviewDate Date
)

create table Categories
(
CategoryId int primary key identity,
CategoryName nvarchar(50) not null,
)

create table Places
(
PlaceId int primary key,
PlacePicURL nvarchar(70) not null,
PlaceName nvarchar(50) not null,
CategoryId int foreign key References Categories(CategoryId),
PlaceDescription nvarchar(100) not null,
Xcoor float,
Ycoor float
)

create table PlanPlace
(
PlaceId int foreign key References Places(PlaceId),
PlanId int foreign key References PlanGroup(PlanId),
PlaceDate Date,
CONSTRAINT PK_PlanPlace PRIMARY KEY (PlanId,PlaceId)

)

create table Pictures
(
PicId int primary key identity,
PlanId int foreign key references PlanGroup(PlanId),
PlaceId int foreign key references Places(PlaceId),
PicExt nvarchar(400) not null,
CONSTRAINT FK_PicturesPlan FOREIGN KEY (PlanId,PlaceId) REFERENCES PlanPlace (PlanId,PlaceId)
)

create table Favorites
(
PlanId int foreign key References PlanGroup(PlanId),
UserId int foreign key References Users(UserId),
CONSTRAINT PK_FavoritesPlan PRIMARY KEY (PlanId,UserId),
)

alter table Users
add PicId int foreign key references Pictures(PicId)

-- Insert a user into the users table
INSERT INTO Users(FirstName, LastName, Email, Passwd, PhoneNumber, IsManager)
VALUES ('admin', 'Traitel', 'admin@gmail.com', '1234a', '0559394845', 1);

INSERT INTO Users(FirstName, LastName, Email, Passwd, PhoneNumber, IsManager)
VALUES ('Yaron', 'Traitel', 'yaron@gmail.com', '1234a', '0559394844', 0);

INSERT INTO Users(FirstName, LastName, Email, Passwd, PhoneNumber, IsManager)
VALUES ('yosef', 'benzo', 'yosef@gmail.com', '1234a', '0559698849', 0);

INSERT INTO PlanGroup(GroupName, UserId, IsPublished, GroupDescription, StartDate, EndDate)
VALUES ('Family', 2, 0, 'The group of the family!', '2024-1-17', '2024-2-21');

INSERT INTO PlanGroup(GroupName, UserId, IsPublished, GroupDescription, StartDate, EndDate)
VALUES ('ADADAD', 2, 0, 'The group of the family!', '2024-1-17', '2024-2-21');


INSERT INTO PlanGroup(GroupName, UserId, IsPublished, GroupDescription, StartDate, EndDate)
VALUES ('OferFamily', 3, 1, 'The group of the family!', '2024-1-17', '2024-2-21');

INSERT INTO PlanGroup(GroupName, UserId, IsPublished, GroupDescription, StartDate, EndDate)
VALUES ('BibiGroup', 3, 1, 'The group of the family!', '2024-1-17', '2024-2-21');

select * from Users
select * from PlanGroup
select * from PlanPlace


-- Create a login for the admin user

CREATE LOGIN [TripPlanAdminLogin] WITH PASSWORD =
'kukuPassword';

Go

-- Create a user in the TasksManagementDB database for the login

CREATE USER [TripPlanAdminUser] FOR LOGIN
[TripPlanAdminLogin];

Go

-- Add the user to the db_owner role to grant admin privileges

ALTER ROLE db_owner ADD MEMBER [TripPlanAdminUser];

Go

/*
scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=TripPlanDB;User ID=TripPlanAdminLogin;Password=kukuPassword;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context TripPlanDbContext -DataAnnotations -force
*/