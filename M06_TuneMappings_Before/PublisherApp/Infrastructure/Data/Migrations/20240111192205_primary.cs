using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class primary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FinalVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Completed = table.Column<bool>(type: "bit", nullable: false),
                    CompletedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Fullfilled = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContractVersion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkingTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateSentToAuthors = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AcceptanceDeadline = table.Column<DateOnly>(type: "date", nullable: false),
                    ModificationDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModificationReason = table.Column<int>(type: "int", nullable: false),
                    Accepted = table.Column<bool>(type: "bit", nullable: false),
                    Specs_AdvanceAmountUSD = table.Column<int>(type: "int", nullable: false),
                    Specs_AuthorAvailableForPR = table.Column<bool>(type: "bit", nullable: false),
                    Specs_DigitalRoyaltyPct = table.Column<int>(type: "int", nullable: false),
                    Specs_HardCoverRoyaltyPct = table.Column<int>(type: "int", nullable: false),
                    Specs_PriceForAddlAuthorCopiesUSD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Specs_PromoCopiesForAuthor = table.Column<int>(type: "int", nullable: false),
                    Specs_PublicityProvided = table.Column<bool>(type: "bit", nullable: false),
                    Specs_SoftCoverRoyaltyPct = table.Column<int>(type: "int", nullable: false),
                    Specs_TranslationRoyaltyUSD = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractVersion_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Author",
                columns: table => new
                {
                    ContractVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name_LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Signed = table.Column<bool>(type: "bit", nullable: false),
                    SignedAuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Author", x => new { x.ContractVersionId, x.Id });
                    table.ForeignKey(
                        name: "FK_Author_ContractVersion_ContractVersionId",
                        column: x => x.ContractVersionId,
                        principalTable: "ContractVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractVersion_ContractId",
                table: "ContractVersion",
                column: "ContractId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Author");

            migrationBuilder.DropTable(
                name: "ContractVersion");

            migrationBuilder.DropTable(
                name: "Contracts");
        }
    }
}
