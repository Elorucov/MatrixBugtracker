using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatrixBugtracker.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Fix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RA_Report",
                table: "report_attachments");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "MictMqs835fPY4bkD4Q5ym+uamCUSKR0SA/lkp5v0/l5mCtwsWFcA/dPRpGwUFee");

            migrationBuilder.AddForeignKey(
                name: "FK_RA_Report",
                table: "report_attachments",
                column: "report_id",
                principalTable: "reports",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RA_Report",
                table: "report_attachments");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "HjI3BtyEQuzp51C8qQt3O3T2bwm2ydjY5Xl2u5mmWgYJJltOwB9RIE3yr+GBFoL0");

            migrationBuilder.AddForeignKey(
                name: "FK_RA_Report",
                table: "report_attachments",
                column: "report_id",
                principalTable: "products",
                principalColumn: "id");
        }
    }
}
