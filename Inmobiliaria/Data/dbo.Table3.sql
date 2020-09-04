CREATE TABLE [dbo].Inmuebles
(
	[Id] INT NOT NULL IDENTITY PRIMARY  KEY, 
    [Direccion] VARCHAR(50) NULL, 
    [Uso] VARCHAR(50) NULL, 
    [Tipo] VARCHAR(50) NULL, 
    [Ambientes] INT NULL, 
    [Precio] DECIMAL NULL, 
    [IdPropietario] INT NOT NULL FOREIGN KEY REFERENCES Propietarios(Id)
)
