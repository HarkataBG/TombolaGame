using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TombolaGame.Migrations
{
    /// <inheritdoc />
    public partial class AddAwardId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AwardId",
                table: "TombolaWinner",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TombolaWinner_AwardId",
                table: "TombolaWinner",
                column: "AwardId");

            migrationBuilder.AddForeignKey(
                name: "FK_TombolaWinner_Awards_AwardId",
                table: "TombolaWinner",
                column: "AwardId",
                principalTable: "Awards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TombolaWinner_Awards_AwardId",
                table: "TombolaWinner");

            migrationBuilder.DropIndex(
                name: "IX_TombolaWinner_AwardId",
                table: "TombolaWinner");

            migrationBuilder.DropColumn(
                name: "AwardId",
                table: "TombolaWinner");
        }
    }
}
