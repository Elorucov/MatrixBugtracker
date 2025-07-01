using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatrixBugtracker.DAL.Migrations
{
    /// <inheritdoc />
    public partial class TextForUserNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "text",
                table: "user_notifications",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "IOzmEm+FF+LIXZu92+uMCi4sEBMDLm7+162zgsJzFNNon/P/88nkC0DNmts6wVhn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "text",
                table: "user_notifications");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "Ul+9VClJlyg1axVBgtI6tbTbCd8cYIwUwKkVolPQXK1ew23Q6amkHAmGTVfiyBRQ");
        }
    }
}
