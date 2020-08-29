CREATE TABLE [dbo].Propietarios

(
	[Id] INT NOT NULL PRIMARY KEY, 
    [DNI] INT NULL, 
    [Nombre] VARCHAR(50) NULL, 
    [Apellido] VARCHAR(50) NULL, 
    [Email] VARCHAR(50) NULL, 
    [Telefono] VARCHAR(50) NULL 
)
