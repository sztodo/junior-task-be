-- DeviceType: 1 = Phone, 2 = Tablet
IF NOT EXISTS (SELECT 1
FROM dbo.Devices
WHERE Id = 1)
BEGIN
    SET IDENTITY_INSERT dbo.Devices ON;

    INSERT INTO dbo.Devices
        (Id, Name, Manufacturer, Type, OperatingSystem, OsVersion, Processor, RamAmount, Description, AssignedUserId, CreatedAt, UpdatedAt)
    VALUES
        (1, 'iPhone 15 Pro', 'Apple', 1, 'iOS', '17.2', 'Apple A17 Pro', 8, 'Company flagship iOS phone', 1, GETUTCDATE(), GETUTCDATE()),
        (2, 'Galaxy S24 Ultra', 'Samsung', 1, 'Android', '14', 'Snapdragon 8 Gen 3', 12, 'High-end Android device', 2, GETUTCDATE(), GETUTCDATE()),
        (3, 'iPad Pro 12.9" M2', 'Apple', 2, 'iPadOS', '17.2', 'Apple M2', 16, 'Design team tablet', 5, GETUTCDATE(), GETUTCDATE()),
        (4, 'Pixel 8 Pro', 'Google', 1, 'Android', '14', 'Google Tensor G3', 12, 'Testing device for Android', 3, GETUTCDATE(), GETUTCDATE()),
        (5, 'Galaxy Tab S9 Ultra', 'Samsung', 2, 'Android', '14', 'Snapdragon 8 Gen 2', 12, 'Large-screen tablet for demos', 4, GETUTCDATE(), GETUTCDATE()),
        (6, 'iPhone SE (3rd Gen)', 'Apple', 1, 'iOS', '17.2', 'Apple A15 Bionic', 4, 'Budget iOS test device', NULL, GETUTCDATE(), GETUTCDATE()),
        (7, 'OnePlus 12', 'OnePlus', 1, 'Android', '14', 'Snapdragon 8 Gen 3', 12, 'Developer testing Android phone', NULL, GETUTCDATE(), GETUTCDATE()),
        (8, 'iPad mini 6', 'Apple', 2, 'iPadOS', '17.2', 'Apple A15 Bionic', 4, 'Compact tablet for field use', NULL, GETUTCDATE(), GETUTCDATE()),
        (9, 'Xperia 1 V', 'Sony', 1, 'Android', '14', 'Snapdragon 8 Gen 2', 8, 'Media-focused test device', NULL, GETUTCDATE(), GETUTCDATE()),
        (10, 'Surface Duo 2', 'Microsoft', 2, 'Android', '12', 'Snapdragon 888', 8, 'Dual-screen experimental device', NULL, GETUTCDATE(), GETUTCDATE());

    SET IDENTITY_INSERT dbo.Devices OFF;
    PRINT 'Devices seeded.';
END
ELSE
    PRINT 'Devices already seeded — skipping.';
GO

PRINT '== Seed data complete ==';
GO