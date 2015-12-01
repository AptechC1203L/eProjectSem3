﻿use master
create database doctor_web_forum
go
use doctor_web_forum
go
begin transaction
create table users(
	id integer identity primary key,
	full_name text,
	email nvarchar(50),
	user_address text,
	tel varchar(20),
	hash_password text,
	role_name varchar(10),
	is_private bit
)

create table message_threads(
	id integer identity primary key,
	room_id integer,
	creator_id integer,
	created_at datetime,
	title ntext
)

create table rooms(
	id integer identity primary key,
	name ntext,
	created_at datetime,
	number_of_thread integer
)

create table message_table(
	id integer identity primary key,
	parrent_message_id integer,
	creator_id integer,
	content ntext,
	thread_id integer,
	created_at datetime
)

alter table message_threads add constraint msg_thread_room foreign key (room_id) references rooms(id)
alter table message_threads add constraint msg_thread_user foreign key (creator_id) references users(id)
alter table message_table add constraint msg_table_thread foreign key (thread_id) references message_threads(id)
alter table message_table add constraint msg_table_user foreign key (creator_id) references users(id)

commit transaction

--Insert data

insert into users values ('Admin','admin@mail.com', 'Ha Noi', '093876222', '7c222fb2927d828af22f592134e8932480637c0d', 'admin', 1)
insert into users values ('Doctor No 1','doctorno1@mail.com', 'Da Nang', '093374222', '7c222fb2927d828af22f592134e8932480637c0d', 'doctor', 0)
insert into users values ('Doctor No 2','doctorno2@mail.com', 'Nghe An', '093374222', '7c222fb2927d828af22f592134e8932480637c0d', 'doctor', 0)
insert into users values ('Doctor No 3','doctorno3@mail.com', 'Thai Binh', '093374222', '7c222fb2927d828af22f592134e8932480637c0d', 'doctor', 0)
insert into users values ('Doctor No 4','doctorno4@mail.com', 'TP Ho Chi Minh', '0933742874', '7c222fb2927d828af22f592134e8932480637c0d', 'doctor', 0)
-- password 12345678 - > sha1

insert into rooms values ('Benh tim mach', '', 5)
insert into rooms values ('Bệnh lão khoa', '', 5)
insert into rooms values ('Khoa hô hấp', '', 4)

insert into message_threads values (1, 2, '', 'Các món ăn tốt cho người bệnh tim mạch')
insert into message_threads values (1, 2, '', 'Dấu hiệu nhận biết sớm bệnh tim mạch')
insert into message_threads values (1, 2, '', 'Bệnh tim mạch nên kiêng gì')
insert into message_threads values (2, 3, '', 'Món ăn tốt cho người cao tuổi')
insert into message_threads values (2, 3, '', 'Nguy cơ tai biến ở người cao tuổi')
insert into message_threads values (2, 3, '', 'Người cao tuổi và cuộc sống')