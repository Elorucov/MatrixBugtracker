using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatrixBugtracker.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ReportRelationFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RR_Report",
                table: "report_reproduces");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "IGXbF4r5OSXSQgu98XmXgYqOMl4T1qt1WjR2scb/LHllmVEUxNF/hOidXGlSNLlD");

            migrationBuilder.AddForeignKey(
                name: "FK_RR_Report",
                table: "report_reproduces",
                column: "report_id",
                principalTable: "reports",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RR_Report",
                table: "report_reproduces");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "MictMqs835fPY4bkD4Q5ym+uamCUSKR0SA/lkp5v0/l5mCtwsWFcA/dPRpGwUFee");

            migrationBuilder.AddForeignKey(
                name: "FK_RR_Report",
                table: "report_reproduces",
                column: "report_id",
                principalTable: "products",
                principalColumn: "id");
        }
    }
}
