using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatrixBugtracker.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ReportNewColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_files_private",
                table: "reports",
                newName: "is_severity_set_by_moderator");

            migrationBuilder.AddColumn<bool>(
                name: "is_attachments_private",
                table: "reports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "NJjEmoD2ezljTxFm61z1PmbVV3dwqN2XtqiTnpKrw6mb/YU+H1cyR42ZV65KFO8a");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_attachments_private",
                table: "reports");

            migrationBuilder.RenameColumn(
                name: "is_severity_set_by_moderator",
                table: "reports",
                newName: "is_files_private");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "IGXbF4r5OSXSQgu98XmXgYqOMl4T1qt1WjR2scb/LHllmVEUxNF/hOidXGlSNLlD");
        }
    }
}
