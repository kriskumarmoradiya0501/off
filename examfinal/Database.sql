CREATE TABLE t_department6 (
    c_departmentid SERIAL PRIMARY KEY,
    c_departmentname VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE t_admin6 (
    c_adminid SERIAL PRIMARY KEY,
    c_email VARCHAR(150) NOT NULL UNIQUE,
    c_password VARCHAR(100) NOT NULL
);

CREATE TABLE t_user6 (
    c_userid SERIAL PRIMARY KEY,
    c_adminid INT NOT NULL,
    c_name VARCHAR(100) NOT NULL,
    c_email VARCHAR(150) NOT NULL UNIQUE,
    c_mobile VARCHAR(15) NOT NULL,
    c_gender VARCHAR(20) NOT NULL,
    c_hobbies VARCHAR(200),
    c_departmentid INT NOT NULL,
    c_dob DATE NOT NULL,
    c_age INT CHECK (c_age BETWEEN 18 AND 60),
    c_address VARCHAR(500),
    c_image VARCHAR(200),
    c_status BOOLEAN DEFAULT TRUE,

    CONSTRAINT fk_department6 FOREIGN KEY (c_departmentid)
        REFERENCES t_department6(c_departmentid),

    CONSTRAINT fk_admin6 FOREIGN KEY (c_adminid)
        REFERENCES t_admin6(c_adminid)
);
