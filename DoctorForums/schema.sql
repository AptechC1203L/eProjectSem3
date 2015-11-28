
drop table Messages, Topics, Rooms, Users;

begin


create table Users (
	Id int identity primary key,
	Email text not null,
	DisplayName text not null,
	PasswordHash text not null,
	Region text,
	Country text
);

create table Rooms (
	Id int identity primary key,
	Title text not null,
	CreatedAt datetime default getutcdate()
);

create table Topics (
	Id int identity primary key,
	Title text not null,
	AuthorId int references Users (Id),
	RoomId int references Rooms (Id),
	CreatedAt datetime default getutcdate()
);

create table Messages (
	Id int identity primary key,
	Content text not null,
	AuthorId int references Users (Id),
	TopicId int references Topics (Id),
	CreatedAt datetime default getutcdate()
);


insert into Users values ('ndtrung4419@gmail.com', 'Trung Ngo', '123456', 'Hanoi', 'Vietnam');
insert into Users values ('linhmai@gmail.com', 'Linh Mai', '123456', 'Hanoi', 'Vietnam');

insert into Rooms values ('Tai mui hong', getutcdate()), ('Nhan khoa', getutcdate());

insert into Topics values
	('Cho em hoi mot chut', 1, 1, getutcdate()),
	('Cho em hoi mot cau', 1, 1, getutcdate()),
	('Ca nha oi!', 2, 2, getutcdate()),
	('Mot so van de nho', 2, 2, getutcdate());

insert into Messages values 
	('hahhahahaha', 1, 1, getutcdate()),
	('hahhahahaha', 2, 1, getutcdate()),
	('hahhahahaha', 2, 2, getutcdate()),
	('hahhahahaha', 1, 2, getutcdate()),
	('hahhahahaha', 1, 3, getutcdate()),
	('hahhahahaha', 1, 4, getutcdate());

end