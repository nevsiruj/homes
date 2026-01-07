-- Script para crear la tabla Articulos
-- Basado en la migración AddArticulosBlog

-- Verificar si la tabla ya existe
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Articulos]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Articulos] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Titulo] NVARCHAR(MAX) NOT NULL,
        [Contenido] NVARCHAR(MAX) NOT NULL,
        [Slug] NVARCHAR(MAX) NOT NULL,
        [Permalink] NVARCHAR(MAX) NOT NULL,
        [ImagenUrl] NVARCHAR(MAX) NOT NULL,
        [Categoria] NVARCHAR(MAX) NOT NULL,
        [FechaCreacion] DATETIME2 NOT NULL,
        [FechaActualizacion] DATETIME2 NULL,
        [Activo] BIT NOT NULL,
        [Orden] INT NOT NULL,
        CONSTRAINT [PK_Articulos] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    
    PRINT 'Tabla Articulos creada exitosamente';
END
ELSE
BEGIN
    PRINT 'La tabla Articulos ya existe';
END
GO

-- Insertar registro en __EFMigrationsHistory si no existe
IF NOT EXISTS (SELECT * FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = N'20260107060045_AddArticulosBlog')
BEGIN
    INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260107060045_AddArticulosBlog', N'6.0.0');
    
    PRINT 'Migración registrada en __EFMigrationsHistory';
END
ELSE
BEGIN
    PRINT 'La migración ya está registrada';
END
GO
