using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoeStoreASP.Migrations
{
    public partial class payment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceId);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceShoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    ShoeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceShoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceShoes_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceShoes_Shoes_ShoeId",
                        column: x => x.ShoeId,
                        principalTable: "Shoes",
                        principalColumn: "ShoeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceShoes_InvoiceId",
                table: "InvoiceShoes",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceShoes_ShoeId",
                table: "InvoiceShoes",
                column: "ShoeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceShoes");

            migrationBuilder.DropTable(
                name: "Invoices");
        }
    }
}
