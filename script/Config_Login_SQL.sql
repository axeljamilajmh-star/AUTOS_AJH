-- Config_Login_SQL.sql
-- Ejecutar conectado como administrador (Windows Authentication)

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

USE [Autos_AJMH]; -- CAMBIA si tu base no se llama así
GO

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
