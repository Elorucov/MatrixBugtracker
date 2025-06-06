using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatrixBugtracker.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ConfirmatiosTablenRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ_EmailConfirm",
                table: "confirmations");

            migrationBuilder.DropColumn(
                name: "email",
                table: "confirmations");

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "confirmations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "vfQX91Bkm0ydSzEa+SxS6AJXQCBhPSFy65CDuZnjXTDnxmWBh9yn04sNtV/8v4dc");

            migrationBuilder.CreateIndex(
                name: "IX_confirmations_user_id",
                table: "confirmations",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Confirmation_User",
                table: "confirmations",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Confirmation_User",
                table: "confirmations");

            migrationBuilder.DropIndex(
                name: "IX_confirmations_user_id",
                table: "confirmations");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "confirmations");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "confirmations",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "KH+NeBnIMREz3VnEsmqjddRu3wRqBYemhiCan1mmy6c5cTE6YMKaCMDtAx2vG0/b");

            migrationBuilder.CreateIndex(
                name: "UQ_EmailConfirm",
                table: "confirmations",
                column: "email",
                unique: true);
        }
    }
}
