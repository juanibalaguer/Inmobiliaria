CREATE TABLE [dbo].Inquilinos
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY, 
    [DNI] INT NULL, 
    [Nombre] VARCHAR(50) NULL, 
    [Apellido] VARCHAR(50) NULL, 
    [Email] VARCHAR(50) NULL, 
    [Telefono] VARCHAR(50) NULL, 
    [NombreGarante] VARCHAR(50) NULL, 
    [TelefonoGarante] VARCHAR(50) NULL
)
