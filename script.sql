DROP DATABASE IF EXISTS antonio_d3_avaliacao;
CREATE DATABASE antonio_d3_avaliacao;
use antonio_d3_avaliacao;

CREATE TABLE login (
	id int NOT NULL AUTO_INCREMENT PRIMARY KEY,
	nome char(30),
    email char(30),
    senha char(30)
);

INSERT INTO login (nome, email, senha) VALUES ('admin', 'admin@email.com', 'admin123');