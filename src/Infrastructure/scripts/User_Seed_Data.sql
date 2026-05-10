
USE DeviceManagementDb;
GO
IF NOT EXISTS (SELECT 1
FROM dbo.Users
WHERE Id = 1)
BEGIN
    SET IDENTITY_INSERT dbo.Users ON;

    INSERT INTO dbo.Users
        (Id, Name, Role, Location, CreatedAt, UpdatedAt)
    VALUES
        (1, 'Alice Johnson', 'Software Engineer', 'New York, USA', GETUTCDATE(), GETUTCDATE()),
        (2, 'Bob Smith', 'QA Engineer', 'London, UK', GETUTCDATE(), GETUTCDATE()),
        (3, 'Carol White', 'Product Manager', 'Berlin, Germany', GETUTCDATE(), GETUTCDATE()),
        (4, 'David Lee', 'DevOps Engineer', 'Toronto, Canada', GETUTCDATE(), GETUTCDATE()),
        (5, 'Eva Martinez', 'UX Designer', 'Madrid, Spain', GETUTCDATE(), GETUTCDATE());

    SET IDENTITY_INSERT dbo.Users OFF;
    PRINT 'Users seeded.';
END
ELSE
    PRINT 'Users already seeded — skipping.';
GO
PRINT '== Seed data complete ==';
GO