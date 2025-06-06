using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatrixBugtracker.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UserNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_notifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    kind = table.Column<byte>(type: "tinyint", nullable: false),
                    target_user_id = table.Column<int>(type: "int", nullable: false),
                    linked_entity_type = table.Column<byte>(type: "tinyint", nullable: true),
                    linked_entity_id = table.Column<int>(type: "int", nullable: true),
                    viewed = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by_user_id = table.Column<int>(type: "int", nullable: false),
                    deletion_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    creator_id = table.Column<int>(type: "int", nullable: false),
                    creation_time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_notifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_Notifications_TargetUser",
                        column: x => x.target_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "pwYGDZsywAomLNVJWCQJMPGj2aw177pOHAoVtSivmK7uZAN428lRVopCB7fzQWPq");

            migrationBuilder.CreateIndex(
                name: "IX_user_notifications_target_user_id",
                table: "user_notifications",
                column: "target_user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_notifications");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "YRmAYV699TIJGpBSGwjcG+qPXiNe8T2EjiE+VaV/U1OUHDusYtDQ3mSH2QpjiT2S");
        }
    }
}
