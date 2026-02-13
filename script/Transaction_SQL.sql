USE [Autos_AJMH];
GO
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

    -- Obtener precio del vehículo
    SELECT @Precio = Precio FROM dbo.Vehiculo WHERE VehiculoID = @VehiculoID;
    IF @Precio IS NULL
        THROW 51000, 'Vehículo no encontrado.', 1;

    -- Registrar venta
    INSERT INTO dbo.Venta (VehiculoID, Fecha, PrecioVenta, Comprador)
    VALUES (@VehiculoID, SYSDATETIME(), @Precio, 'Cliente A');

    -- Actualizar estado del vehículo
    UPDATE dbo.Vehiculo SET Estado = 'Vendido' WHERE VehiculoID = @VehiculoID;

    COMMIT TRANSACTION;
    PRINT 'Transacción 2: COMMIT';
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    PRINT CONCAT('Transacción 2: ROLLBACK -> ', ERROR_MESSAGE());
END CATCH;
GO

-- TRANSACCIÓN 3: Actualizar propietario (demostración de rollback)
PRINT '--- Transacción 3: Actualizar propietario (demo rollback) ---';
BEGIN TRANSACTION;
BEGIN TRY
    UPDATE dbo.Propietario
    SET Telefono = '88889999'
    WHERE PropietarioID = 1;

    -- Forzar error para demostrar ROLLBACK
    THROW 52001, 'Error forzado para demostrar ROLLBACK.', 1;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    PRINT CONCAT('Transacción 3: ROLLBACK -> ', ERROR_MESSAGE());
END CATCH;
GO

-- CONSULTAS FINALES
-- Mostrar últimos vehículos insertados
SELECT TOP 10 * FROM dbo.Vehiculo ORDER BY VehiculoID DESC;

-- Mostrar últimas ventas
SELECT TOP 10 * FROM dbo.Venta ORDER BY VentaID DESC;

-- Mostrar información del propietario 1
SELECT * FROM dbo.Propietario WHERE PropietarioID = 1;
GO