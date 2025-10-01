USE Autos_AJMH;
GO

INSERT INTO Marca (Nombre) VALUES ('Toyota'), ('Ford'), ('Honda');

INSERT INTO Modelo (MarcaID, Nombre, Ano)
VALUES (1, 'Corolla', 2020), (2, 'Focus', 2018), (3, 'Civic', 2019);

INSERT INTO Propietario (Nombre, Telefono, Email)
VALUES ('Juan Perez','555-1111','juan@mail.com');

INSERT INTO Vehiculo (ModeloID, VIN, Color, Precio, Ano, PropietarioID)
VALUES (1,'VIN0001','Rojo',15000,2020,1);

INSERT INTO Venta (VehiculoID, Fecha, PrecioVenta, Comprador)
VALUES (1, '2024-05-15', 14000, 'Cliente A');
GO
