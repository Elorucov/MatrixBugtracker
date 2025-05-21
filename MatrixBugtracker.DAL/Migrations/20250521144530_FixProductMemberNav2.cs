using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatrixBugtracker.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixProductMemberNav2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_product_members_products_ProductId1",
                table: "product_members");

            migrationBuilder.DropIndex(
                name: "IX_product_members_ProductId1",
                table: "product_members");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "product_members");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "ri3zn9eayzMC3aaDQg3uKhdVZ3nfJdX3XIuFk82d5g3VqBKRLnlyJwlI40ht6MQw");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId1",
                table: "product_members",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "SzwC4sHWEOcLe0qzQf0WPKcvkLTRVia2jXV91pmeiwfmTWMBQfIubkwDIIA5plns");

            migrationBuilder.CreateIndex(
                name: "IX_product_members_ProductId1",
                table: "product_members",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_product_members_products_ProductId1",
                table: "product_members",
                column: "ProductId1",
                principalTable: "products",
                principalColumn: "id");
        }
    }
}
