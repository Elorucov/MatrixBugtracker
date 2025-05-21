using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatrixBugtracker.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixProductMemberNav3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "YRmAYV699TIJGpBSGwjcG+qPXiNe8T2EjiE+VaV/U1OUHDusYtDQ3mSH2QpjiT2S");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "ri3zn9eayzMC3aaDQg3uKhdVZ3nfJdX3XIuFk82d5g3VqBKRLnlyJwlI40ht6MQw");
        }
    }
}
