CREATE TABLE PermissionTypes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Description NVARCHAR(255) NOT NULL
);

CREATE TABLE Permissions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeForeName NVARCHAR(100) NOT NULL,
    EmployeeSurName NVARCHAR(100) NOT NULL,
    PermissionTypeId INT NOT NULL,
    PermissionDate DATETIME2 NOT NULL,
    CONSTRAINT FK_Permissions_PermissionTypes FOREIGN KEY (PermissionTypeId)
        REFERENCES PermissionTypes(Id)
);

-- Insertar tipos de permiso
INSERT INTO PermissionTypes (Description) VALUES 
('Sick Leave'),
('Vacation'),
('Personal Leave'),
('Remote Work');

