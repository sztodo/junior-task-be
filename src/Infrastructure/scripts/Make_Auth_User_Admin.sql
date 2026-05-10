USE DeviceManagementDb;
GO

-- Configure: set the email of the account to promote 
DECLARE @TargetEmail NVARCHAR(256) = 'your-email@example.com';

IF NOT EXISTS (
    SELECT 1
FROM dbo.AuthUsers
WHERE Email = @TargetEmail
)
BEGIN
    PRINT 'ERROR: No account found with email ' + @TargetEmail;
    PRINT 'Register the account via the app first, then re-run this script.';
END
ELSE IF EXISTS (
    SELECT 1
FROM dbo.AuthUsers
WHERE Email = @TargetEmail AND Role = 'Admin'
)
BEGIN
    PRINT @TargetEmail + ' is already an Admin — skipping.';
END
ELSE
BEGIN
    UPDATE dbo.AuthUsers
    SET Role = 'Admin'
    WHERE Email = @TargetEmail;

    PRINT 'SUCCESS: ' + @TargetEmail + ' has been promoted to Admin.';
    PRINT 'They will need to log out and log back in for the new role to take effect.';
END
GO