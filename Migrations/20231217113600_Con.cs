using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketSystemApplication.Migrations
{
    /// <inheritdoc />
    public partial class Con : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "13c0d132-cc9e-416d-944d-83e95e855f3y",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "72eaa430-e1ed-4da6-b855-956880a7161d", "AQAAAAIAAYagAAAAEKIJ4okNDxwX/SJFiNXcTDslKG0WPK4gXkQYGXa7KKRp3D1Qvfp74xdr8fe+7s/iyg==", "bf8bf1fc-2fd0-4716-bb73-2b7e0b695a9e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "13c0d132-cc9e-416d-944d-83e95e855f3y",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d62ee455-b174-4583-a09f-63d5731f4641", "AQAAAAIAAYagAAAAEBv+swFQH6Ny3QyUvFs1J8l4Mo4mFz8jx68P+tXuOldYeHtkQdhXYs4eCyxQJWETWA==", "1f59e6e5-48b5-41eb-88f7-77ad883f2af1" });
        }
    }
}
