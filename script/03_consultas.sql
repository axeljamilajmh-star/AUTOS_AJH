USE Autos_AJMH;
GO

-- Listar vehículos con marca y modelo
SELECT v.VehiculoID, m.Nombre AS Marca, mo.Nombre AS Modelo, v.VIN, v.Precio
FROM Vehiculo v
JOIN Modelo mo ON v.ModeloID = mo.ModeloID
JOIN Marca m ON mo.MarcaID = m.MarcaID;

-- Conteo por marca
SELECT m.Nombre, COUNT(*) AS NumVehiculos
FROM Vehiculo v
JOIN Modelo mo ON v.ModeloID = mo.ModeloID
JOIN Marca m ON mo.MarcaID = m.MarcaID
GROUP BY m.Nombre;

-- Ventas recientes
SELECT * FROM Venta ORDER BY Fecha DESC;
GO
