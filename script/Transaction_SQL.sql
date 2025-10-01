-- Transaction_SQL.sql
USE [Autos_AJMH]; -- CAMBIA si tu base no se llama así
GO
SET NOCOUNT ON;
SET XACT_ABORT ON;

-- TRANSACCIÓN 1
PRINT '--- Transacción 1: Insertar vehículo ---';
BEGIN TRANSACTION;
BEGIN TRY
    INSERT INTO dbo.Vehiculo (Marca, Modelo, Anio, Precio, Estado)
    VALUES ('Toyota', 'Corolla', 2022, 18500, 'Disponible');

    COMMIT TRANSACTION;
    PRINT 'Transacción 1: COMMIT';
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    PRINT CONCAT('Transacción 1: ROLLBACK -> ', ERROR_MESSAGE());
END CATCH;
GO

-- TRANSACCIÓN 2
PRINT '--- Transacción 2: Registrar venta ---';
BEGIN TRANSACTION;
BEGIN TRY
    DECLARE @IdCliente INT = 1;
    DECLARE @IdVehiculo INT = 1;
    DECLARE @Total DECIMAL(18,2);

    SELECT @Total = Precio FROM dbo.Vehiculo WHERE IdVehiculo = @IdVehiculo;
    IF @Total IS NULL
        THROW 51000, 'Vehículo no encontrado.', 1;

    INSERT INTO dbo.Venta (IdCliente, IdVehiculo, Fecha, Total)
    VALUES (@IdCliente, @IdVehiculo, SYSDATETIME(), @Total);

    UPDATE dbo.Vehiculo SET Estado = 'Vendido' WHERE IdVehiculo = @IdVehiculo;

    COMMIT TRANSACTION;
    PRINT 'Transacción 2: COMMIT';
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    PRINT CONCAT('Transacción 2: ROLLBACK -> ', ERROR_MESSAGE());
END CATCH;
GO

-- TRANSACCIÓN 3 (rollback forzado)
PRINT '--- Transacción 3: Actualizar cliente (demo rollback) ---';
BEGIN TRANSACTION;
BEGIN TRY
    UPDATE dbo.Cliente SET Telefono = '88889999' WHERE IdCliente = 1;

    THROW 52001, 'Error forzado para demostrar ROLLBACK.', 1;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    PRINT CONCAT('Transacción 3: ROLLBACK -> ', ERROR_MESSAGE());
END CATCH;
GO
SELECT TOP 10 * FROM dbo.Vehiculo ORDER BY IdVehiculo DESC;
SELECT TOP 10 * FROM dbo.Venta ORDER BY IdVenta DESC;
SELECT * FROM dbo.Cliente WHERE IdCliente = 1;
