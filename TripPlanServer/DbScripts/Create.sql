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
PlacePicURL nvarchar(1000) not null,
PlaceName nvarchar(50) not null,
CategoryId int foreign key References Categories(CategoryId),
PlaceDescription nvarchar(300) not null,
Xcoor float,
Ycoor float,
GooglePlaceId nvarchar(100)
)

create table PlanPlace
(
PlaceId int foreign key References Places(PlaceId),
PlanId int foreign key References PlanGroup(PlanId),
PlaceDate DateTime,
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



-- Insert Categories [ADDED]
INSERT INTO Categories (CategoryName) VALUES ('Parks'), ('Landmarks'); -- [ADDED]

-- Insert Users
INSERT INTO Users (FirstName, LastName, Email, Passwd, PhoneNumber, IsManager)
VALUES 
    ('admin', 'Traitel', 'admin@gmail.com', '1234a', '0559394845', 1),
    ('Yaron', 'Traitel', 'yaron.traitel@gmail.com', '1234a', '0559394844', 0),
    ('Yosef', 'Benzo', 'yosef@gmail.com', '1234a', '0559698849', 0),
    ('John', 'Doe', 'john.doe@gmail.com', 'password123', '1234567890', 0),
    ('Jane', 'Smith', 'jane.smith@gmail.com', 'password123', '0987654321', 0);

-- Insert PlanGroup
INSERT INTO PlanGroup (GroupName, UserId, IsPublished, GroupDescription, StartDate, EndDate)
VALUES 
    ('Family', 2, 0, 'The group of the family!', '2024-01-17', '2024-01-21'),
    ('ADADAD', 2, 0, 'The group of the family!', '2024-01-14', '2024-01-17'),
    ('OferFamily', 3, 1, 'The group of the family!', '2024-01-17', '2024-02-21'),
    ('BibiGroup', 3, 1, 'The group of the family!', '2024-01-17', '2024-02-21'),
    ('FriendsTrip', 4, 1, 'Trip with friends!', '2024-03-01', '2024-03-10'),
    ('WorkRetreat', 5, 0, 'Work retreat for team building.', '2024-04-15', '2024-04-20');

-- Insert Places
INSERT INTO Places (PlaceId, PlacePicURL, PlaceName, CategoryId, PlaceDescription, Xcoor, Ycoor, GooglePlaceId)
VALUES 
    (1, 'https://i.natgeofe.com/n/15ec8dec-df7c-45af-a0ae-08d4e906a134/belvedere-castle.jpg?w=2880&h=2160', 'Central Park', 1, 'A large public park in New York City.', 40.785091, -73.968285, 'ChIJ4zGFAZpYwokRGUGph3Mf37k'),
    (2, 'https://assets.editorial.aetnd.com/uploads/2015/02/topic-golden-gate-bridge-gettyimages-177770941.jpg', 'Golden Gate Bridge', 2, 'A suspension bridge spanning the Golden Gate', 37.819929, -122.478255, 'ChIJiQHsW0mAhYARkZsGmKZXCt0'), 
    (3, 'https://www.worldatlas.com/upload/f4/d8/7b/shutterstock-1397031029.jpg', 'Statue of Liberty', 2, 'A colossal neoclassical sculpture', 40.689247, -74.044502, 'ChIJK3u3rEMZwokR1Usnnf7dBzA'),
    (4, 'https://upload.wikimedia.org/wikipedia/commons/4/47/New_york_times_square-terabass.jpg', 'Times Square', 2, 'A major commercial intersection, tourist destination, entertainment center, and neighborhood in Midtown Manhattan, New York City.', 40.758896, -73.985130, 'ChIJmQJIxlVYwokRLgeuocVOGVU'),
    (5, 'https://dynamic-media-cdn.tripadvisor.com/media/photo-o/26/92/e4/97/disneyland-paris.jpg?w=900&h=500&s=1', 'Disneyland Park', 2, 'An amusement park in Anaheim, California.', 33.812092, -117.918974, 'ChIJZQ5ISa6s3IAR1zivl6wW7i8'),

    -- Places in France for Plan 2
    (6, 'https://www.planetware.com/photos-large/F/eiffel-tower.jpg', 'Eiffel Tower', 2, 'A wrought-iron lattice tower in Paris, France.', 48.858844, 2.294351, 'ChIJA8Z0Y5Jv5kcRjQbNq0G7aAA'),
    (7, 'https://www.placestotravel.com/wp-content/uploads/2024/02/louvre-tips.jpg', 'Louvre Museum', 2, 'The worlds largest art museum and a historic monument in Paris, France.', 48.860611, 2.337644, 'ChIJb8Jg9pFx5kcRjQbNq0G7aAA'),
    (8, 'https://cdn.britannica.com/29/255529-050-63A22A3C/notre-dame-de-paris-cathedral-paris-france.jpg', 'Notre-Dame Cathedral', 2, 'A medieval Catholic cathedral', 48.852968, 2.349902, 'ChIJRVY_et5x5kcRjQbNq0G7aAA'),
    (9, 'https://cdn-imgix.headout.com/mircobrands-content/image/127af66e56d2cd2fc27f65502e0f091a-AdobeStock_70930062.jpeg?auto=format&w=1069.6000000000001&h=687.6&q=90&fit=crop&ar=14%3A9&crop=faces', 'Mont Saint-Michel', 2, 'An island and mainland commune in Normandy, France.', 48.636063, -1.511579, 'ChIJd6kh67QdE0gRjQbNq0G7aAA'),
    (10, 'https://cdn-imgix.headout.com/media/images/00a87210d0b9efdc10c1230b916c105f-269-paris-paris--palace-of-versailles-01.jpg?auto=format&w=1051.2&h=540&q=90&fit=fit', 'Palace of Versailles', 2, 'A former royal residence located in Versailles, France.', 48.804865, 2.120355, 'ChIJd6kh67QdE0gRjQbNq0G7aAA');


-- Insert PlanPlace
INSERT INTO PlanPlace (PlaceId, PlanId, PlaceDate)
VALUES 
    -- Plan 1 (United States)
    (1, 1, '2024-01-17T10:00:00'),
    (2, 1, '2024-01-17T14:15:00'),
    (3, 1, '2024-01-18T13:40:00'),
    (4, 1, '2024-01-18T09:30:00'),
    (5, 1, '2024-01-19T21:55:00'),

    -- Plan 2 (France)
    (6, 2, '2024-01-14T20:30:00'),
    (7, 2, '2024-01-14T14:10:00'),
    (8, 2, '2024-01-14T11:20:00'),
    (9, 2, '2024-01-14T12:35:00'),
    (10, 2, '2024-01-14T10:30:00');

-- Select data for verification
SELECT * FROM Users;
SELECT * FROM PlanGroup;
SELECT * FROM PlanPlace;
SELECT * FROM Places;

SELECT * FROM Reviews
SELECT * FROM Categories

-- Add users to the PlanGroup 'ADADAD'
INSERT INTO UserGroup (PlanId, UserId)
VALUES 
    (2, 4), -- Adding user with UserId 4
    (2, 5); -- Adding user with UserId 5

-- Verify the insertion
SELECT * FROM UserGroup WHERE PlanId = 2;

-- Insert Reviews for BibiGroup
INSERT INTO Reviews (Title, PlanId, UserId, Stars, ReviewText, ReviewDate)
VALUES 
    ('Amazing Trip!', 4, 3, 5, 'This was an amazing trip with the family. Highly recommend!', '2024-02-22'),
    ('Good Experience', 4, 4, 4, 'Had a good time, but some places were too crowded.', '2024-02-23'),
    ('Not Bad', 4, 5, 3, 'The trip was okay, but could have been better organized.', '2024-02-24');

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
