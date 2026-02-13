============================================================
--  CREACIÓN DEL LOGIN A NIVEL SERVIDOR Y USUARIO EN LA BD
-- ============================================================

USE master;
GO

IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = N'autos_narla')
BEGIN
    CREATE LOGIN [autos_narla]
    WITH PASSWORD = N'Autos_NarlaPG25',
         CHECK_POLICY = OFF,
         CHECK_EXPIRATION = OFF;
    PRINT 'LOGIN autos_narla creado a nivel servidor.';
END
ELSE
    PRINT 'LOGIN autos_narla ya existe a nivel servidor.';
GO

--  CREACIÓN DE LA BASE DE DATOS Y TABLAS

IF DB_ID('Autos_AJMH') IS NULL
    CREATE DATABASE Autos_AJMH;
GO

USE Autos_AJMH;
GO

-- Crear usuario en la BD y asignar rol db_owner
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = N'autos_narla')
BEGIN
    CREATE USER [autos_narla] FOR LOGIN [autos_narla];
    PRINT 'USUARIO autos_narla creado en la BD.';
END
ELSE
    PRINT 'USUARIO autos_narla ya existe en la BD.';
GO

EXEC sp_addrolemember 'db_owner', 'autos_narla';
PRINT 'USUARIO autos_narla agregado a db_owner.';
GO

-- Tabla Marca
CREATE TABLE IF NOT EXISTS Marca (
    MarcaID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL
);

-- Tabla Modelo
CREATE TABLE IF NOT EXISTS Modelo (
    ModeloID INT IDENTITY(1,1) PRIMARY KEY,
    MarcaID INT NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Ano INT,
    CONSTRAINT FK_Modelo_Marca FOREIGN KEY (MarcaID) REFERENCES Marca(MarcaID)
);

-- Tabla Propietario
CREATE TABLE IF NOT EXISTS Propietario (
    PropietarioID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(200) NOT NULL,
    Telefono NVARCHAR(20),
    Email NVARCHAR(100)
);

-- Tabla Vehiculo
CREATE TABLE IF NOT EXISTS Vehiculo (
    VehiculoID INT IDENTITY(1,1) PRIMARY KEY,
    ModeloID INT NOT NULL,
    VIN NVARCHAR(50) UNIQUE NOT NULL,
    Color NVARCHAR(50),
    Precio DECIMAL(12,2),
    Ano INT,
    PropietarioID INT NULL,
    Estado NVARCHAR(50) DEFAULT 'Disponible',
    CONSTRAINT FK_Vehiculo_Modelo FOREIGN KEY (ModeloID) REFERENCES Modelo(ModeloID),
    CONSTRAINT FK_Vehiculo_Propietario FOREIGN KEY (PropietarioID) REFERENCES Propietario(PropietarioID)
);

-- Tabla Venta
CREATE TABLE IF NOT EXISTS Venta (
    VentaID INT IDENTITY(1,1) PRIMARY KEY,
    VehiculoID INT NOT NULL,
    Fecha DATE NOT NULL,
    PrecioVenta DECIMAL(12,2) NOT NULL,
    Comprador NVARCHAR(200),
    CONSTRAINT FK_Venta_Vehiculo FOREIGN KEY (VehiculoID) REFERENCES Vehiculo(VehiculoID)
);
GO

--  DE DATOS INICIALES

INSERT INTO Marca (Nombre) VALUES ('Toyota'), ('Ford'), ('Honda');

INSERT INTO Modelo (MarcaID, Nombre, Ano)
VALUES (1, 'Corolla', 2020),
       (2, 'Focus', 2018),
       (3, 'Civic', 2019);

INSERT INTO Propietario (Nombre, Telefono, Email)
VALUES ('Juan Perez','555-1111','juan@mail.com');

INSERT INTO Vehiculo (ModeloID, VIN, Color, Precio, Ano, PropietarioID)
VALUES (1,'VIN0001','Rojo',15000,2020,1);

INSERT INTO Venta (VehiculoID, Fecha, PrecioVenta, Comprador)
VALUES (1, '2024-05-15', 14000, 'Cliente A');
GO

--  TRANSACCIONES CORREGIDAS


SET NOCOUNT ON;
SET XACT_ABORT ON;

-- TRANSACCIÓN 1: Insertar nuevo vehículo
PRINT '--- Transacción 1: Insertar vehículo ---';
BEGIN TRANSACTION;
BEGIN TRY
    INSERT INTO dbo.Vehiculo (ModeloID, VIN, Color, Precio, Ano, PropietarioID, Estado)
    VALUES (1, 'VIN0002', 'Azul', 18500, 2022, 1, 'Disponible');

    COMMIT TRANSACTION;
    PRINT 'Transacción 1: COMMIT';
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    PRINT CONCAT('Transacción 1: ROLLBACK -> ', ERROR_MESSAGE());
END CATCH;
GO

-- TRANSACCIÓN 2: Registrar venta
PRINT '--- Transacción 2: Registrar venta ---';
BEGIN TRANSACTION;
BEGIN TRY
    DECLARE @VehiculoID INT = 1;
    DECLARE @Precio DECIMAL(12,2);

    SELECT @Precio = Precio FROM dbo.Vehiculo WHERE VehiculoID = @VehiculoID;
    IF @Precio IS NULL
        THROW 51000, 'Vehículo no encontrado.', 1;

    INSERT INTO dbo.Venta (VehiculoID, Fecha, PrecioVenta, Comprador)
    VALUES (@VehiculoID, SYSDATETIME(), @Precio, 'Cliente A');

    UPDATE dbo.Vehiculo SET Estado = 'Vendido' WHERE VehiculoID = @VehiculoID;

    COMMIT TRANSACTION;
    PRINT 'Transacción 2: COMMIT';
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    PRINT CONCAT('Transacción 2: ROLLBACK -> ', ERROR_MESSAGE());
END CATCH;
GO

-- TRANSACCIÓN 3: Actualizar propietario
PRINT '--- Transacción 3: Actualizar propietario (demo rollback) ---';
BEGIN TRANSACTION;
BEGIN TRY
    UPDATE dbo.Propietario
    SET Telefono = '88889999'
    WHERE PropietarioID = 1;

    THROW 52001, 'Error forzado para demostrar ROLLBACK.', 1;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    PRINT CONCAT('Transacción 3: ROLLBACK -> ', ERROR_MESSAGE());
END CATCH;
GO

--  CONSULTAS FINALES


SELECT TOP 10 * FROM dbo.Vehiculo ORDER BY VehiculoID DESC;
SELECT TOP 10 * FROM dbo.Venta ORDER BY VentaID DESC;
SELECT * FROM dbo.Propietario WHERE PropietarioID = 1;
GO