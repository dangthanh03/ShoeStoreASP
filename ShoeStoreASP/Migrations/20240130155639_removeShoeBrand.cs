using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoeStoreASP.Migrations
{
    public partial class removeShoeBrand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoeBrands");

            migrationBuilder.CreateIndex(
                name: "IX_Shoes_BrandId",
                table: "Shoes",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shoes_Brands_BrandId",
                table: "Shoes",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "BrandId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shoes_Brands_BrandId",
                table: "Shoes");

            migrationBuilder.DropIndex(
                name: "IX_Shoes_BrandId",
                table: "Shoes");

            migrationBuilder.CreateTable(
                name: "ShoeBrands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    ShoeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoeBrands", x => x.Id);
                });
        }
    }
}
