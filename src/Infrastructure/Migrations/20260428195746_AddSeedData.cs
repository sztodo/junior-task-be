using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                            table: "Users",
                            columns: new[] { "Id", "Name", "Role", "Location", "CreatedAt", "UpdatedAt", "IsDeleted" },
                            values: new object[,]
                            {
                    { 1, "Alice Johnson", "Software Engineer", "New York, USA", DateTime.UtcNow, DateTime.UtcNow, false },
                    { 2, "Bob Smith", "QA Engineer", "London, UK", DateTime.UtcNow, DateTime.UtcNow, false },
                    { 3, "Carol White", "Product Manager", "Berlin, Germany", DateTime.UtcNow, DateTime.UtcNow, false },
                    { 4, "David Lee", "DevOps Engineer", "Toronto, Canada", DateTime.UtcNow, DateTime.UtcNow, false },
                    { 5, "Eva Martinez", "UX Designer", "Madrid, Spain", DateTime.UtcNow, DateTime.UtcNow, false }
                            });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "Name", "Manufacturer", "Type", "OperatingSystem", "OsVersion", "Processor", "RamAmount", "Description", "AssignedUserId", "CreatedAt", "UpdatedAt", "IsDeleted" },
                values: new object[,]
                {
                    { 1, "iPhone 15 Pro", "Apple", 1, "iOS", "17.2", "Apple A17 Pro", 8, "Company flagship iOS phone", 1, DateTime.UtcNow, DateTime.UtcNow, false },
                    { 2, "Galaxy S24 Ultra", "Samsung", 1, "Android", "14", "Snapdragon 8 Gen 3", 12, "High-end Android device", 2, DateTime.UtcNow, DateTime.UtcNow, false },
                    { 3, "iPad Pro 12.9\" M2", "Apple", 2, "iPadOS", "17.2", "Apple M2", 16, "Design team tablet", 5, DateTime.UtcNow, DateTime.UtcNow, false },
                    { 4, "Pixel 8 Pro", "Google", 1, "Android", "14", "Google Tensor G3", 12, "Testing device for Android", 3, DateTime.UtcNow, DateTime.UtcNow, false },
                    { 5, "Galaxy Tab S9 Ultra", "Samsung", 2, "Android", "14", "Snapdragon 8 Gen 2", 12, "Large-screen tablet for demos", 4, DateTime.UtcNow, DateTime.UtcNow, false },
                    { 6, "iPhone SE (3rd Gen)", "Apple", 1, "iOS", "17.2", "Apple A15 Bionic", 4, "Budget iOS test device", null, DateTime.UtcNow, DateTime.UtcNow, false },
                    { 7, "OnePlus 12", "OnePlus", 1, "Android", "14", "Snapdragon 8 Gen 3", 12, "Developer testing Android phone", null, DateTime.UtcNow, DateTime.UtcNow, false },
                    { 8, "iPad mini 6", "Apple", 2, "iPadOS", "17.2", "Apple A15 Bionic", 4, "Compact tablet for field use", null, DateTime.UtcNow, DateTime.UtcNow, false },
                    { 9, "Xperia 1 V", "Sony", 1, "Android", "14", "Snapdragon 8 Gen 2", 8, "Media-focused test device", null, DateTime.UtcNow, DateTime.UtcNow, false },
                    { 10, "Surface Duo 2", "Microsoft", 2, "Android", "12", "Snapdragon 888", 8, "Dual-screen experimental device", null, DateTime.UtcNow, DateTime.UtcNow, false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "Devices", keyColumn: "Id", keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            migrationBuilder.DeleteData(table: "Users", keyColumn: "Id", keyValues: new object[] { 1, 2, 3, 4, 5 });
        }
    }
}
