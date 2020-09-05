CREATE TABLE [dbo].Contratos
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FechaInicio] DATETIME NULL, 
    [FechaFin] DATETIME NULL, 
    [MontoAlquiler] DECIMAL NULL, 
    [IdInquilino] INT NOT NULL FOREIGN KEY REFERENCES Inquilinos(Id),
    [IdInmueble] INT NOT NULL FOREIGN KEY REFERENCES Inmuebles(Id)
)
