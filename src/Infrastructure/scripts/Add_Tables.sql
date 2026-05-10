USE DeviceManagementDb;
GO

-- Users
IF NOT EXISTS (
    SELECT 1
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME = 'Users' AND TABLE_SCHEMA = 'dbo'
)
BEGIN
    CREATE TABLE dbo.Users
    (
        Id INT IDENTITY(1,1) NOT NULL,
        Name NVARCHAR(200) NOT NULL,
        Role NVARCHAR(100) NOT NULL,
        Location NVARCHAR(200) NOT NULL,
        CreatedAt DATETIME2 NOT NULL,
        UpdatedAt DATETIME2 NOT NULL,
        DeletedAt DATETIME2 NULL,
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT PK_Users PRIMARY KEY (Id)
    );
    PRINT 'Table Users created.';
END
ELSE
    PRINT 'Table Users already exists — skipping.';
GO

-- Devices 
IF NOT EXISTS (
    SELECT 1
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME = 'Devices' AND TABLE_SCHEMA = 'dbo'
)
BEGIN
    CREATE TABLE dbo.Devices
    (
        Id INT IDENTITY(1,1) NOT NULL,
        Name NVARCHAR(200) NOT NULL,
        Manufacturer NVARCHAR(200) NOT NULL,
        Type INT NOT NULL,
        OperatingSystem NVARCHAR(100) NOT NULL,
        OsVersion NVARCHAR(50) NOT NULL,
        Processor NVARCHAR(200) NOT NULL,
        RamAmount INT NOT NULL,
        Description NVARCHAR(1000) NULL,
        CreatedAt DATETIME2 NOT NULL,
        UpdatedAt DATETIME2 NOT NULL,
        DeletedAt DATETIME2 NULL,
        IsDeleted BIT NOT NULL DEFAULT 0,
        AssignedUserId INT NULL,
        CONSTRAINT PK_Devices              PRIMARY KEY (Id),
        CONSTRAINT FK_Devices_Users        FOREIGN KEY (AssignedUserId)
            REFERENCES dbo.Users(Id)       ON DELETE SET NULL,
        CONSTRAINT CK_Devices_Type         CHECK (Type IN (1, 2))
    );

    CREATE INDEX IX_Devices_AssignedUserId
        ON dbo.Devices (AssignedUserId);

    PRINT 'Table Devices created.';
END
ELSE
    PRINT 'Table Devices already exists — skipping.';
GO

-- AuthUsers 
IF NOT EXISTS (
    SELECT 1
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME = 'AuthUsers' AND TABLE_SCHEMA = 'dbo'
)
BEGIN
    CREATE TABLE dbo.AuthUsers
    (
        Id INT IDENTITY(1,1) NOT NULL,
        Email NVARCHAR(256) NOT NULL,
        PasswordHash NVARCHAR(512) NOT NULL,
        Role NVARCHAR(50) NOT NULL DEFAULT 'Employee',
        CreatedAt DATETIME2 NOT NULL,
        LinkedUserId INT NULL,
        CONSTRAINT PK_AuthUsers       PRIMARY KEY (Id),
        CONSTRAINT UQ_AuthUsers_Email UNIQUE (Email),
        CONSTRAINT FK_AuthUsers_Users FOREIGN KEY (LinkedUserId)
            REFERENCES dbo.Users(Id)  ON DELETE SET NULL
    );

    CREATE INDEX IX_AuthUsers_LinkedUserId
        ON dbo.AuthUsers (LinkedUserId);

    PRINT 'Table AuthUsers created.';
END
ELSE
    PRINT 'Table AuthUsers already exists — skipping.';
GO

-- EF Core migrations history 
IF NOT EXISTS (
    SELECT 1
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME = '__EFMigrationsHistory' AND TABLE_SCHEMA = 'dbo'
)
BEGIN
    CREATE TABLE dbo.__EFMigrationsHistory
    (
        MigrationId NVARCHAR(150) NOT NULL,
        ProductVersion NVARCHAR(32) NOT NULL,
        CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY (MigrationId)
    );
    PRINT 'Table __EFMigrationsHistory created.';
END
GO

IF NOT EXISTS (
    SELECT 1
FROM dbo.__EFMigrationsHistory
WHERE MigrationId = '20240101000000_InitialCreate'
)
BEGIN
    INSERT INTO dbo.__EFMigrationsHistory
        (MigrationId, ProductVersion)
    VALUES
        ('20240101000000_InitialCreate', '10.0.0');
    PRINT 'Initial migration registered.';
END
GO

PRINT '== Table creation complete ==';
GO
