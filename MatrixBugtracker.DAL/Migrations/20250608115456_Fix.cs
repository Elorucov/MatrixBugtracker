using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatrixBugtracker.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RT_Report",
                table: "report_tags");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "HjI3BtyEQuzp51C8qQt3O3T2bwm2ydjY5Xl2u5mmWgYJJltOwB9RIE3yr+GBFoL0");

            migrationBuilder.AddForeignKey(
                name: "FK_RT_Report",
                table: "report_tags",
                column: "report_id",
                principalTable: "reports",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RT_Report",
                table: "report_tags");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "vfQX91Bkm0ydSzEa+SxS6AJXQCBhPSFy65CDuZnjXTDnxmWBh9yn04sNtV/8v4dc");

            migrationBuilder.AddForeignKey(
                name: "FK_RT_Report",
                table: "report_tags",
                column: "report_id",
                principalTable: "products",
                principalColumn: "id");
        }
    }
}
