using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuggestionHub.Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class LikeEntityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Suggestions_SuggestionAggregateId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_SuggestionAggregateId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "SuggestionAggregateId",
                table: "Likes");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_SuggestionId",
                table: "Likes",
                column: "SuggestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Suggestions_SuggestionId",
                table: "Likes",
                column: "SuggestionId",
                principalTable: "Suggestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Suggestions_SuggestionId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_SuggestionId",
                table: "Likes");

            migrationBuilder.AddColumn<int>(
                name: "SuggestionAggregateId",
                table: "Likes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Likes_SuggestionAggregateId",
                table: "Likes",
                column: "SuggestionAggregateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Suggestions_SuggestionAggregateId",
                table: "Likes",
                column: "SuggestionAggregateId",
                principalTable: "Suggestions",
                principalColumn: "Id");
        }
    }
}
