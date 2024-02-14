using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoeStoreASP.Migrations
{
    public partial class foreign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Shoes");

            migrationBuilder.CreateIndex(
                name: "IX_ShoeTypes_ShoeId",
                table: "ShoeTypes",
                column: "ShoeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoeTypes_TypeId",
                table: "ShoeTypes",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoeTypes_Shoes_ShoeId",
                table: "ShoeTypes",
                column: "ShoeId",
                principalTable: "Shoes",
                principalColumn: "ShoeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoeTypes_Types_TypeId",
                table: "ShoeTypes",
                column: "TypeId",
                principalTable: "Types",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoeTypes_Shoes_ShoeId",
                table: "ShoeTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoeTypes_Types_TypeId",
                table: "ShoeTypes");

            migrationBuilder.DropIndex(
                name: "IX_ShoeTypes_ShoeId",
                table: "ShoeTypes");

            migrationBuilder.DropIndex(
                name: "IX_ShoeTypes_TypeId",
                table: "ShoeTypes");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Shoes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
