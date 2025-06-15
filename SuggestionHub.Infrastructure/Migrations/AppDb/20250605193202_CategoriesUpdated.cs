using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuggestionHub.Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class CategoriesUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Suggestions_CategoryId",
                table: "Suggestions",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestions_Categories_CategoryId",
                table: "Suggestions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suggestions_Categories_CategoryId",
                table: "Suggestions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Suggestions_CategoryId",
                table: "Suggestions");
        }
    }
}
