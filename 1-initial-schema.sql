CREATE TABLE Users (
	user_id INTEGER PRIMARY KEY SERIAL,
	nickname TEXT UNIQUE,
	display_name TEXT,
	password_hash TEXT,
	region TEXT,
	country TEXT,
	is_private_profile BOOLEAN DEFAULT FALSE,
);


CREATE TABLE Roles (
	role_id INTEGER PRIMARY SERIAL,
	name TEXT
);


CREATE TABLE UserRoles (
	user_id INTEGER REFERENCES Users (user_id),
	role_id INTEGER REFERENCES Roles (role_id),
	CONSTRAINT PRIMARY KEY (user_id, role_id),
);


CREATE TABLE UserLogins (
	user_id INTEGER REFERENCES Users (user_id)
);


CREATE TABLE Rooms (
	room_id INTEGER PRIMARY KEY,
	name TEXT,
);

CREATE TABLE Topics (
	room_id INTEGER REFERENCES Rooms (room_id),
	title TEXT,
);

CREATE TABLE Messages (
	topic_id,
	author_id,
	content,
);
