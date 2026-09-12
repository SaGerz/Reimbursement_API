using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reimbursement_API.Migrations
{
    /// <inheritdoc />
    public partial class FixRefreshTokenTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refreshTokens_users_UserId1",
                table: "refreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_refreshTokens_UserId1",
                table: "refreshTokens");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "refreshTokens");

            migrationBuilder.RenameColumn(
                name: "isRevoce",
                table: "refreshTokens",
                newName: "isRevoked");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "refreshTokens",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_refreshTokens_UserId",
                table: "refreshTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_refreshTokens_users_UserId",
                table: "refreshTokens",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refreshTokens_users_UserId",
                table: "refreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_refreshTokens_UserId",
                table: "refreshTokens");

            migrationBuilder.RenameColumn(
                name: "isRevoked",
                table: "refreshTokens",
                newName: "isRevoce");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "refreshTokens",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "refreshTokens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_refreshTokens_UserId1",
                table: "refreshTokens",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_refreshTokens_users_UserId1",
                table: "refreshTokens",
                column: "UserId1",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
