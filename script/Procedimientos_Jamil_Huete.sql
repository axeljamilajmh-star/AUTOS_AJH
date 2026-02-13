USE Autos_AJMH;
GO
-- Procedimientos almacenados - Autor: Jamil Huete
SET NOCOUNT ON;
GO

-- Lista todas las marcas
CREATE OR ALTER PROCEDURE sp_ListarMarcas
AS
BEGIN
    SELECT MarcaID, Nombre
    FROM Marca;
END;
GO

-- Buscar marcas por nombre
CREATE OR ALTER PROCEDURE sp_BuscarMarcaPorNombre
    @Nombre NVARCHAR(100)
AS
BEGIN
    SELECT MarcaID, Nombre
    FROM Marca
    WHERE Nombre LIKE '%' + @Nombre + '%';
END;
GO

-- Actualizar nombre de marca
CREATE OR ALTER PROCEDURE sp_ActualizarMarca
    @MarcaID INT,
    @NuevoNombre NVARCHAR(100)
AS
BEGIN
    UPDATE Marca
    SET Nombre = @NuevoNombre
    WHERE MarcaID = @MarcaID;
END;
GO

-- Lista todos los modelos con su marca
CREATE OR ALTER PROCEDURE sp_ListarModelos
AS
BEGIN
    SELECT mo.ModeloID, mo.Nombre AS Modelo, mo.Ano, m.Nombre AS Marca
    FROM Modelo mo
    JOIN Marca m ON mo.MarcaID = m.MarcaID;
END;
GO

-- Filtrar modelos por año
CREATE OR ALTER PROCEDURE sp_FiltrarModelosPorAno
    @Ano INT
AS
BEGIN
    SELECT ModeloID, Nombre, Ano
    FROM Modelo
    WHERE Ano = @Ano;
END;
GO

-- Actualizar nombre de modelo
CREATE OR ALTER PROCEDURE sp_ActualizarModelo
    @ModeloID INT,
    @NuevoNombre NVARCHAR(100)
AS
BEGIN
    UPDATE Modelo
    SET Nombre = @NuevoNombre
    WHERE ModeloID = @ModeloID;
END;
GO


-- Lista todos los propietarios
CREATE OR ALTER PROCEDURE sp_ListarPropietarios
AS
BEGIN
    SELECT PropietarioID, Nombre, Telefono, Email
    FROM Propietario;
END;
GO

-- Buscar propietario por nombre
CREATE OR ALTER PROCEDURE sp_BuscarPropietarioPorNombre
    @Nombre NVARCHAR(200)
AS
BEGIN
    SELECT PropietarioID, Nombre, Telefono, Email
    FROM Propietario
    WHERE Nombre LIKE '%' + @Nombre + '%';
END;
GO

-- Actualizar teléfono del propietario
CREATE OR ALTER PROCEDURE sp_ActualizarTelefonoPropietario
    @PropietarioID INT,
    @NuevoTelefono NVARCHAR(20)
AS
BEGIN
    UPDATE Propietario
    SET Telefono = @NuevoTelefono
    WHERE PropietarioID = @PropietarioID;
END;
GO

-- Lista todos los vehículos con marca y modelo
CREATE OR ALTER PROCEDURE sp_ListarVehiculos
AS
BEGIN
    SELECT v.VehiculoID, m.Nombre AS Marca, mo.Nombre AS Modelo,
           v.VIN, v.Color, v.Precio, v.Ano, v.Estado
    FROM Vehiculo v
    JOIN Modelo mo ON v.ModeloID = mo.ModeloID
    JOIN Marca m ON mo.MarcaID = m.MarcaID;
END;
GO

-- Filtro de vehículos por rango de precios
CREATE OR ALTER PROCEDURE sp_FiltrarVehiculosPorPrecio
    @MinPrecio DECIMAL(12,2),
    @MaxPrecio DECIMAL(12,2)
AS
BEGIN
    SELECT VehiculoID, VIN, Color, Precio, Ano
    FROM Vehiculo
    WHERE Precio BETWEEN @MinPrecio AND @MaxPrecio;
END;
GO

-- Actualizar estado de un vehículo
CREATE OR ALTER PROCEDURE sp_ActualizarEstadoVehiculo
    @VehiculoID INT,
    @NuevoEstado NVARCHAR(50)
AS
BEGIN
    UPDATE Vehiculo
    SET Estado = @NuevoEstado
    WHERE VehiculoID = @VehiculoID;
END;
GO

-- Lista todas las ventas con detalle
CREATE OR ALTER PROCEDURE sp_ListarVentas
AS
BEGIN
    SELECT v.VentaID, ve.VIN, mo.Nombre AS Modelo, ma.Nombre AS Marca,
           v.Fecha, v.PrecioVenta, v.Comprador
    FROM Venta v
    JOIN Vehiculo ve ON v.VehiculoID = ve.VehiculoID
    JOIN Modelo mo ON ve.ModeloID = mo.ModeloID
    JOIN Marca ma ON mo.MarcaID = ma.MarcaID;
END;
GO

-- Filtro de ventas por fecha
CREATE OR ALTER PROCEDURE sp_FiltrarVentasPorFecha
    @FechaInicio DATE,
    @FechaFin DATE
AS
BEGIN
    SELECT VentaID, VehiculoID, Fecha, PrecioVenta, Comprador
    FROM Venta
    WHERE Fecha BETWEEN @FechaInicio AND @FechaFin;
END;
GO

-- Actualizar comprador de una venta
CREATE OR ALTER PROCEDURE sp_ActualizarComprador
    @VentaID INT,
    @NuevoComprador NVARCHAR(200)
AS
BEGIN
    UPDATE Venta
    SET Comprador = @NuevoComprador
    WHERE VentaID = @VentaID;
END;
GO
-- Total de vehículos por marca
CREATE OR ALTER PROCEDURE sp_TotalVehiculosPorMarca
AS
BEGIN
    SELECT m.Nombre AS Marca, COUNT(v.VehiculoID) AS TotalVehiculos
    FROM Vehiculo v
    JOIN Modelo mo ON v.ModeloID = mo.ModeloID
    JOIN Marca m ON mo.MarcaID = m.MarcaID
    GROUP BY m.Nombre;
END;
GO

-- Promedio de precios de vehículos
CREATE OR ALTER PROCEDURE sp_PromedioPrecioVehiculos
AS
BEGIN
    SELECT AVG(Precio) AS PromedioPrecioVehiculos FROM Vehiculo;
END;
GO

-- Valor total vendido
CREATE OR ALTER PROCEDURE sp_TotalVentas
AS
BEGIN
    SELECT SUM(PrecioVenta) AS TotalVentas FROM Venta;
END;
GO

-- Vehículo más caro
CREATE OR ALTER PROCEDURE sp_VehiculoMasCaro
AS
BEGIN
    SELECT TOP 1 VehiculoID, VIN, Precio
    FROM Vehiculo
    ORDER BY Precio DESC;
END;
GO

-- Vehículo más barato
CREATE OR ALTER PROCEDURE sp_VehiculoMasBarato
AS
BEGIN
    SELECT TOP 1 VehiculoID, VIN, Precio
    FROM Vehiculo
    ORDER BY Precio ASC;
END;
GO
