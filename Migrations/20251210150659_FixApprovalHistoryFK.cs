using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reimbursement_API.Migrations
{
    /// <inheritdoc />
    public partial class FixApprovalHistoryFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReimburstmentId",
                table: "ApprovalHistories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReimburstmentId",
                table: "ApprovalHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
