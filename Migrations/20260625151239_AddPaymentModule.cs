using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reimbursement_API.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bankaccounts",
                columns: table => new
                {
                    BankAccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BankCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Accountnumber = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountHolderName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bankaccounts", x => x.BankAccountId);
                    table.ForeignKey(
                        name: "FK_bankaccounts_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "paymenttransactions",
                columns: table => new
                {
                    PaymentTransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReimburstmentId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Provider = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderRefrence = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FailureReason = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CompleteAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paymenttransactions", x => x.PaymentTransactionId);
                    table.ForeignKey(
                        name: "FK_paymenttransactions_reimburstments_ReimburstmentId",
                        column: x => x.ReimburstmentId,
                        principalTable: "reimburstments",
                        principalColumn: "ReimbursementId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_bankaccounts_UserId",
                table: "bankaccounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_paymenttransactions_ReimburstmentId",
                table: "paymenttransactions",
                column: "ReimburstmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bankaccounts");

            migrationBuilder.DropTable(
                name: "paymenttransactions");
        }
    }
}
