USE Autos_AJMH;
GO

CREATE TABLE Marca (
    MarcaID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL
);

CREATE TABLE Modelo (
    ModeloID INT IDENTITY(1,1) PRIMARY KEY,
    MarcaID INT NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Ano INT,
    CONSTRAINT FK_Modelo_Marca FOREIGN KEY (MarcaID) REFERENCES Marca(MarcaID)
);

CREATE TABLE Propietario (
    PropietarioID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(200) NOT NULL,
    Telefono NVARCHAR(20),
    Email NVARCHAR(100)
);

CREATE TABLE Vehiculo (
    VehiculoID INT IDENTITY(1,1) PRIMARY KEY,
    ModeloID INT NOT NULL,
    VIN NVARCHAR(50) UNIQUE NOT NULL,
    Color NVARCHAR(50),
    Precio DECIMAL(12,2),
    Ano INT,
    PropietarioID INT NULL,
    CONSTRAINT FK_Vehiculo_Modelo FOREIGN KEY (ModeloID) REFERENCES Modelo(ModeloID),
    CONSTRAINT FK_Vehiculo_Propietario FOREIGN KEY (PropietarioID) REFERENCES Propietario(PropietarioID)
);

CREATE TABLE Venta (
    VentaID INT IDENTITY(1,1) PRIMARY KEY,
    VehiculoID INT NOT NULL,
    Fecha DATE NOT NULL,
    PrecioVenta DECIMAL(12,2) NOT NULL,
    Comprador NVARCHAR(200),
    CONSTRAINT FK_Venta_Vehiculo FOREIGN KEY (VehiculoID) REFERENCES Vehiculo(VehiculoID)
);
GO
