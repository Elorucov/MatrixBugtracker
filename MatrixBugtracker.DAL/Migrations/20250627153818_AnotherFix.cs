using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatrixBugtracker.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AnotherFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "deleted_by_user_id",
                table: "users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "creation_time",
                table: "users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "creator_id",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "update_time",
                table: "users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "update_user_id",
                table: "users",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "deleted_by_user_id",
                table: "user_notifications",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "update_time",
                table: "user_notifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "update_user_id",
                table: "user_notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "deleted_by_user_id",
                table: "tags",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "update_time",
                table: "tags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "update_user_id",
                table: "tags",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "deleted_by_user_id",
                table: "reports",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "update_user_id",
                table: "reports",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeletedByUserId",
                table: "refresh_tokens",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "refresh_tokens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdateUserId",
                table: "refresh_tokens",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "deleted_by_user_id",
                table: "products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "update_time",
                table: "products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "update_user_id",
                table: "products",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "deleted_by_user_id",
                table: "platform_notifications",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "update_time",
                table: "platform_notifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "update_user_id",
                table: "platform_notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "deleted_by_user_id",
                table: "files",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "update_time",
                table: "files",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "update_user_id",
                table: "files",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "deleted_by_user_id",
                table: "confirmations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "update_time",
                table: "confirmations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "update_user_id",
                table: "confirmations",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "deleted_by_user_id",
                table: "comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "update_user_id",
                table: "comments",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "creation_time", "creator_id", "deleted_by_user_id", "password", "UpdateDate", "update_time", "update_user_id" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, null, "i7910PfJ+r6/jME4fExLWargdKchpDYPHUeAfqNO9wdUYVq66DpEdB337Sw+mIWn", null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "users");

            migrationBuilder.DropColumn(
                name: "creation_time",
                table: "users");

            migrationBuilder.DropColumn(
                name: "creator_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "update_time",
                table: "users");

            migrationBuilder.DropColumn(
                name: "update_user_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "update_time",
                table: "user_notifications");

            migrationBuilder.DropColumn(
                name: "update_user_id",
                table: "user_notifications");

            migrationBuilder.DropColumn(
                name: "update_time",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "update_user_id",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "update_user_id",
                table: "reports");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "UpdateUserId",
                table: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "update_time",
                table: "products");

            migrationBuilder.DropColumn(
                name: "update_user_id",
                table: "products");

            migrationBuilder.DropColumn(
                name: "update_time",
                table: "platform_notifications");

            migrationBuilder.DropColumn(
                name: "update_user_id",
                table: "platform_notifications");

            migrationBuilder.DropColumn(
                name: "update_time",
                table: "files");

            migrationBuilder.DropColumn(
                name: "update_user_id",
                table: "files");

            migrationBuilder.DropColumn(
                name: "update_time",
                table: "confirmations");

            migrationBuilder.DropColumn(
                name: "update_user_id",
                table: "confirmations");

            migrationBuilder.DropColumn(
                name: "update_user_id",
                table: "comments");

            migrationBuilder.AlterColumn<int>(
                name: "deleted_by_user_id",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "deleted_by_user_id",
                table: "user_notifications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "deleted_by_user_id",
                table: "tags",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "deleted_by_user_id",
                table: "reports",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeletedByUserId",
                table: "refresh_tokens",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "deleted_by_user_id",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "deleted_by_user_id",
                table: "platform_notifications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "deleted_by_user_id",
                table: "files",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "deleted_by_user_id",
                table: "confirmations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "deleted_by_user_id",
                table: "comments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "deleted_by_user_id", "password" },
                values: new object[] { 0, "XhropSoXhYua3eVs77GcBvNJKd6P3MLOYp+/wz5TXGDfPK9wCZnNTdz2nvAGDuA/" });
        }
    }
}
