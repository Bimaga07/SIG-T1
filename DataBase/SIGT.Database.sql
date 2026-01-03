USE SIGT;
GO

CREATE TABLE Usuarios (
    ID INT IDENTITY PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE
);
GO

CREATE TABLE Tareas (
    ID INT IDENTITY PRIMARY KEY,
    Titulo NVARCHAR(200) NOT NULL,
    Descripcion NVARCHAR(500),
    Estado NVARCHAR(50) NOT NULL,
    UsuarioID INT NOT NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Tareas_Usuarios
        FOREIGN KEY (UsuarioID) REFERENCES Usuarios(ID)
);
GO

CREATE TABLE RegistroDeActividad (
    ID INT IDENTITY PRIMARY KEY,
    TareaID INT NOT NULL,
    Accion NVARCHAR(200) NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT GETDATE()
);
GO



CREATE PROCEDURE sp_Tarea_Create
    @Titulo NVARCHAR(200),
    @Descripcion NVARCHAR(500),
    @Estado NVARCHAR(50),
    @UsuarioID INT
AS
BEGIN
    INSERT INTO Tareas (Titulo, Descripcion, Estado, UsuarioID)
    VALUES (@Titulo, @Descripcion, @Estado, @UsuarioID);
END;
GO

CREATE PROCEDURE sp_Tarea_Update
    @ID INT,
    @Titulo NVARCHAR(200),
    @Descripcion NVARCHAR(500),
    @Estado NVARCHAR(50)
AS
BEGIN
    UPDATE Tareas
    SET Titulo = @Titulo,
        Descripcion = @Descripcion,
        Estado = @Estado
    WHERE ID = @ID;
END;
GO

CREATE TRIGGER trg_Tareas_AfterInsert
ON Tareas
AFTER INSERT
AS
BEGIN
    INSERT INTO RegistroDeActividad (TareaID, Accion)
    SELECT ID, 'Tarea creada'
    FROM inserted;
END;
GO

CREATE VIEW vw_TareasConUsuario
AS
SELECT 
    t.ID,
    t.Titulo,
    t.Estado,
    u.Nombre + ' ' + u.Apellido AS UsuarioAsignado,
    t.FechaCreacion
FROM Tareas t
INNER JOIN Usuarios u ON t.UsuarioID = u.ID;
GO

INSERT INTO Usuarios (Nombre, Apellido, Email)
VALUES ('Ana', 'Lopez', 'ana@test.com');

EXEC sp_Tarea_Create
    @Titulo = 'Tarea de prueba',
    @Descripcion = 'Probando SP',
    @Estado = 'Pendiente',
    @UsuarioID = 1;

SELECT * FROM RegistroDeActividad;
