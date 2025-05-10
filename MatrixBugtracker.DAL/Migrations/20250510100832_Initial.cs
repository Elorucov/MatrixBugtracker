using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatrixBugtracker.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "confirmations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    code = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    kind = table.Column<byte>(type: "tinyint", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by_user_id = table.Column<int>(type: "int", nullable: false),
                    deletion_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    creator_id = table.Column<int>(type: "int", nullable: false),
                    creation_time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_confirmations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "comment_attachments",
                columns: table => new
                {
                    comment_id = table.Column<int>(type: "int", nullable: false),
                    file_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("K_CommentAttachment", x => new { x.comment_id, x.file_id });
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    report_id = table.Column<int>(type: "int", nullable: false),
                    update_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    text = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    new_severity = table.Column<byte>(type: "tinyint", nullable: false),
                    new_status = table.Column<byte>(type: "tinyint", nullable: false),
                    is_attachments_private = table.Column<bool>(type: "bit", nullable: false),
                    as_moderator = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by_user_id = table.Column<int>(type: "int", nullable: false),
                    deletion_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    creator_id = table.Column<int>(type: "int", nullable: false),
                    creation_time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "files",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    original_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    path = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    mime_type = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by_user_id = table.Column<int>(type: "int", nullable: false),
                    deletion_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    creator_id = table.Column<int>(type: "int", nullable: false),
                    creation_time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_files", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by_user_id = table.Column<int>(type: "int", nullable: false),
                    deletion_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_email_confirmed = table.Column<bool>(type: "bit", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    role = table.Column<byte>(type: "tinyint", nullable: false),
                    photo_file_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserPhoto",
                        column: x => x.photo_file_id,
                        principalTable: "files",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "moderators",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by_user_id = table.Column<int>(type: "int", nullable: false),
                    deletion_time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_moderators", x => x.id);
                    table.ForeignKey(
                        name: "FK_Moder_Id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    photo_file_id = table.Column<int>(type: "int", nullable: true),
                    access_level = table.Column<byte>(type: "tinyint", nullable: false),
                    type = table.Column<byte>(type: "tinyint", nullable: false),
                    is_over = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by_user_id = table.Column<int>(type: "int", nullable: false),
                    deletion_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    creator_id = table.Column<int>(type: "int", nullable: false),
                    creation_time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_ProductPhoto",
                        column: x => x.photo_file_id,
                        principalTable: "files",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Product_Creator",
                        column: x => x.creator_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    is_archived = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by_user_id = table.Column<int>(type: "int", nullable: false),
                    deletion_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    creator_id = table.Column<int>(type: "int", nullable: false),
                    creation_time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.id);
                    table.ForeignKey(
                        name: "FK_Tag_Creator",
                        column: x => x.creator_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "product_members",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "int", nullable: false),
                    member_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("K_ProductMember", x => new { x.product_id, x.member_id });
                    table.ForeignKey(
                        name: "FK_PU_Member",
                        column: x => x.member_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_PU_Product",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "product_moderators",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "int", nullable: false),
                    moderator_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("K_ProductModerator", x => new { x.product_id, x.moderator_id });
                    table.ForeignKey(
                        name: "FK_PM_Moderator",
                        column: x => x.moderator_id,
                        principalTable: "moderators",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_PM_Product",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "report_attachments",
                columns: table => new
                {
                    report_id = table.Column<int>(type: "int", nullable: false),
                    file_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("K_ReportAttachment", x => new { x.report_id, x.file_id });
                    table.ForeignKey(
                        name: "FK_RA_Attachment",
                        column: x => x.file_id,
                        principalTable: "files",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_RA_Report",
                        column: x => x.report_id,
                        principalTable: "products",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "report_reproduces",
                columns: table => new
                {
                    report_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("K_ReportReproduce", x => new { x.report_id, x.user_id });
                    table.ForeignKey(
                        name: "FK_RR_Report",
                        column: x => x.report_id,
                        principalTable: "products",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_RR_User",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "reports",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    update_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    title = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    steps = table.Column<string>(type: "text", nullable: false),
                    actual = table.Column<string>(type: "text", nullable: false),
                    supposed = table.Column<string>(type: "text", nullable: false),
                    severity = table.Column<byte>(type: "tinyint", nullable: false),
                    problem_type = table.Column<byte>(type: "tinyint", nullable: false),
                    status = table.Column<byte>(type: "tinyint", nullable: false),
                    is_files_private = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by_user_id = table.Column<int>(type: "int", nullable: false),
                    deletion_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    creator_id = table.Column<int>(type: "int", nullable: false),
                    creation_time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reports", x => x.id);
                    table.ForeignKey(
                        name: "FK_Report_Creator",
                        column: x => x.creator_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Report_Product",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "report_tags",
                columns: table => new
                {
                    report_id = table.Column<int>(type: "int", nullable: false),
                    tag_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("K_ReportTag", x => new { x.report_id, x.tag_id });
                    table.ForeignKey(
                        name: "FK_RT_Report",
                        column: x => x.report_id,
                        principalTable: "products",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_RT_Tag",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "deleted_by_user_id", "deletion_time", "email", "first_name", "is_deleted", "is_email_confirmed", "last_name", "password", "photo_file_id", "role" },
                values: new object[] { 1, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@example.com", "John", false, true, "Doe", "JF5Oh/zsiK7RXJTNi4UB2/jR3Knd6whpyKxFNv6u4U5J+z7ZO1K5l/Gfl/2rCCBa", null, (byte)1 });

            migrationBuilder.InsertData(
                table: "moderators",
                columns: new[] { "id", "deleted_by_user_id", "deletion_time", "is_deleted", "user_id" },
                values: new object[] { 1, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_comment_attachments_file_id",
                table: "comment_attachments",
                column: "file_id");

            migrationBuilder.CreateIndex(
                name: "UQ_CommentAttachment",
                table: "comment_attachments",
                columns: new[] { "comment_id", "file_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_comments_creator_id",
                table: "comments",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_comments_report_id",
                table: "comments",
                column: "report_id");

            migrationBuilder.CreateIndex(
                name: "UQ_EmailConfirm",
                table: "confirmations",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_files_creator_id",
                table: "files",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "UQ_FilePath",
                table: "files",
                column: "path",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Moder",
                table: "moderators",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_members_member_id",
                table: "product_members",
                column: "member_id");

            migrationBuilder.CreateIndex(
                name: "UQ_ProductMember",
                table: "product_members",
                columns: new[] { "product_id", "member_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_moderators_moderator_id",
                table: "product_moderators",
                column: "moderator_id");

            migrationBuilder.CreateIndex(
                name: "UQ_ProductModer",
                table: "product_moderators",
                columns: new[] { "product_id", "moderator_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_products_creator_id",
                table: "products",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_photo_file_id",
                table: "products",
                column: "photo_file_id",
                unique: true,
                filter: "[photo_file_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ_ProductName",
                table: "products",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_report_attachments_file_id",
                table: "report_attachments",
                column: "file_id");

            migrationBuilder.CreateIndex(
                name: "UQ_ReportAttachment",
                table: "report_attachments",
                columns: new[] { "report_id", "file_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_report_reproduces_user_id",
                table: "report_reproduces",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UQ_ReportReproduce",
                table: "report_reproduces",
                columns: new[] { "report_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_report_tags_tag_id",
                table: "report_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "UQ_ReportTag",
                table: "report_tags",
                columns: new[] { "report_id", "tag_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reports_creator_id",
                table: "reports",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_reports_product_id",
                table: "reports",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_tags_creator_id",
                table: "tags",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "UQ_TagName",
                table: "tags",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_photo_file_id",
                table: "users",
                column: "photo_file_id",
                unique: true,
                filter: "[photo_file_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ_Email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CA_Attachment",
                table: "comment_attachments",
                column: "file_id",
                principalTable: "files",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CA_Comment",
                table: "comment_attachments",
                column: "comment_id",
                principalTable: "comments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Creator",
                table: "comments",
                column: "creator_id",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Report",
                table: "comments",
                column: "report_id",
                principalTable: "reports",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Creator",
                table: "files",
                column: "creator_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPhoto",
                table: "users");

            migrationBuilder.DropTable(
                name: "comment_attachments");

            migrationBuilder.DropTable(
                name: "confirmations");

            migrationBuilder.DropTable(
                name: "product_members");

            migrationBuilder.DropTable(
                name: "product_moderators");

            migrationBuilder.DropTable(
                name: "report_attachments");

            migrationBuilder.DropTable(
                name: "report_reproduces");

            migrationBuilder.DropTable(
                name: "report_tags");

            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "moderators");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "reports");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "files");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
