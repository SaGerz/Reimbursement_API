using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reimbursement_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNamingConvenBankAccModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "bankaccounts",
                newName: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "bankaccounts",
                newName: "isActive");
        }
    }
}
