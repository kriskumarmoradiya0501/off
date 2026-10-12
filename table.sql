CREATE TABLE t_user (
    c_userid SERIAL PRIMARY KEY,
    c_name VARCHAR(100) NOT NULL,
    c_email VARCHAR(150) NOT NULL UNIQUE,
    c_password VARCHAR(255) NOT NULL,
    c_mobile VARCHAR(20),
    c_gender VARCHAR(20),
    c_role VARCHAR(20) DEFAULT 'User'
);

CREATE TABLE t_event (
    c_eventid SERIAL PRIMARY KEY,
    c_eventname VARCHAR(100) NOT NULL,
    c_eventdate DATE NOT NULL,
    c_eventtime TIME NOT NULL,
    c_venue VARCHAR(100) NOT NULL,
    c_totalseats INT NOT NULL CHECK (c_totalseats >= 0)
);

CREATE TABLE t_registration (
    c_registrationid SERIAL PRIMARY KEY,
    c_userid INT NOT NULL,
    c_eventid INT NOT NULL,
    c_registrationdate DATE NOT NULL,
    CONSTRAINT fk_registration_user
        FOREIGN KEY (c_userid) REFERENCES t_user(c_userid)
        ON DELETE CASCADE,
    CONSTRAINT fk_registration_event
        FOREIGN KEY (c_eventid) REFERENCES t_event(c_eventid)
        ON DELETE CASCADE,
    CONSTRAINT uq_registration UNIQUE (c_userid, c_eventid)
);




-- Insert Users
INSERT INTO t_user (c_name, c_email, c_password, c_mobile, c_gender, c_role) VALUES
('Admin User', 'admin@event.com', 'Admin@123', '9876543210', 'Male', 'User'),
('Rahul Patel', 'rahul@gmail.com', 'Rahul@123', '9876543211', 'Male', 'User'),
('Priya Shah', 'priya@gmail.com', 'Priya@123', '9876543212', 'Female', 'User'),
('Amit Mehta', 'amit@gmail.com', 'Amit@123', '9876543213', 'Male', 'User'),
('Neha Patel', 'neha@gmail.com', 'Neha@123', '9876543214', 'Female', 'User');

-- Insert Events
INSERT INTO t_event (c_eventname, c_eventdate, c_eventtime, c_venue, c_totalseats) VALUES
('Tech Conference', '2026-10-20', '10:00:00', 'Grand Hall', 100),
('AI Workshop', '2026-10-25', '14:00:00', 'Innovation Hub', 50),
('Music Concert', '2026-11-05', '19:00:00', 'Open Arena', 500),
('.NET Developer Meet', '2026-10-11', '11:00:00', 'Tech Center', 75),
('Database Workshop', '2026-11-15', '10:00:00', 'Training Hall', 40);

-- Insert Registrations
INSERT INTO t_registration (c_userid, c_eventid, c_registrationdate) VALUES
(2, 1, '2026-09-20'),  -- Rahul Patel → Tech Conference
(3, 1, '2026-09-21'),  -- Priya Shah → Tech Conference
(4, 2, '2026-09-22'),  -- Amit Mehta → AI Workshop
(5, 2, '2026-09-23'),  -- Neha Patel → AI Workshop
(2, 3, '2026-09-24'),  -- Rahul Patel → Music Concert
(3, 3, '2026-09-25'),  -- Priya Shah → Music Concert
(4, 3, '2026-09-26'),  -- Amit Mehta → Music Concert
(5, 4, '2026-09-27');  -- Neha Patel → .NET Developer Meet

UPDATE t_user
SET c_role = 'Admin'
WHERE c_email = 'admin@event.com';




