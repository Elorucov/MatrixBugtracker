using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatrixBugtracker.DAL.Migrations
{
    /// <inheritdoc />
    public partial class PlatformNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "platform_notifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    kind = table.Column<byte>(type: "tinyint", nullable: false),
                    text = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by_user_id = table.Column<int>(type: "int", nullable: false),
                    deletion_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    creator_id = table.Column<int>(type: "int", nullable: false),
                    creation_time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_platform_notifications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "platform_notification_read_users",
                columns: table => new
                {
                    notification_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("K_PlatNotifUser", x => new { x.user_id, x.notification_id });
                    table.ForeignKey(
                        name: "FK_PNU_Notification",
                        column: x => x.notification_id,
                        principalTable: "platform_notifications",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_PNU_User",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "KH+NeBnIMREz3VnEsmqjddRu3wRqBYemhiCan1mmy6c5cTE6YMKaCMDtAx2vG0/b");

            migrationBuilder.CreateIndex(
                name: "IX_platform_notification_read_users_notification_id",
                table: "platform_notification_read_users",
                column: "notification_id");

            migrationBuilder.CreateIndex(
                name: "UQ_PlatNotifUser",
                table: "platform_notification_read_users",
                columns: new[] { "user_id", "notification_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "platform_notification_read_users");

            migrationBuilder.DropTable(
                name: "platform_notifications");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "pwYGDZsywAomLNVJWCQJMPGj2aw177pOHAoVtSivmK7uZAN428lRVopCB7fzQWPq");
        }
    }
}
