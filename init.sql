IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'FaleMaisDB')
BEGIN
    CREATE DATABASE FaleMaisDB;
END
GO

USE FaleMaisDB;
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Tarifas')
BEGIN
    CREATE TABLE Tarifas (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Origem NVARCHAR(10) NOT NULL,
        Destino NVARCHAR(10) NOT NULL,
        Valor DECIMAL(10,2) NOT NULL
    );
END
GO

INSERT INTO Tarifas (Origem, Destino, Valor) VALUES ('011', '016', 1.90);
INSERT INTO Tarifas (Origem, Destino, Valor) VALUES ('016', '011', 2.90);
INSERT INTO Tarifas (Origem, Destino, Valor) VALUES ('011', '017', 1.70);
INSERT INTO Tarifas (Origem, Destino, Valor) VALUES ('017', '011', 2.70);
INSERT INTO Tarifas (Origem, Destino, Valor) VALUES ('011', '018', 0.90);
INSERT INTO Tarifas (Origem, Destino, Valor) VALUES ('018', '011', 1.90);

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Planos')
BEGIN
    CREATE TABLE Planos (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Nome NVARCHAR(50) NOT NULL,
        MinutosGratis INT NOT NULL
    );
END
GO

INSERT INTO Planos (Nome, MinutosGratis) VALUES ('FaleMais 30', 30);
INSERT INTO Planos (Nome, MinutosGratis) VALUES ('FaleMais 60', 60);
INSERT INTO Planos (Nome, MinutosGratis) VALUES ('FaleMais 120', 120);
