using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TombolaGame.Migrations
{
    /// <inheritdoc />
    public partial class AddModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Awards_Tombolas_TombolaId",
                table: "Awards");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Tombolas_TombolaId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_TombolaId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "TombolaId",
                table: "Players");

            migrationBuilder.AddColumn<int>(
                name: "MaximumAwards",
                table: "Tombolas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaximumPlayers",
                table: "Tombolas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinimumAwards",
                table: "Tombolas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinimumPlayers",
                table: "Tombolas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StrategyType",
                table: "Tombolas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "TombolaId",
                table: "Awards",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "PlayerTombola",
                columns: table => new
                {
                    PlayersId = table.Column<int>(type: "integer", nullable: false),
                    TombolasId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerTombola", x => new { x.PlayersId, x.TombolasId });
                    table.ForeignKey(
                        name: "FK_PlayerTombola_Players_PlayersId",
                        column: x => x.PlayersId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerTombola_Tombolas_TombolasId",
                        column: x => x.TombolasId,
                        principalTable: "Tombolas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TombolaWinner",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TombolaId = table.Column<int>(type: "integer", nullable: false),
                    PlayerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TombolaWinner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TombolaWinner_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TombolaWinner_Tombolas_TombolaId",
                        column: x => x.TombolaId,
                        principalTable: "Tombolas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTombola_TombolasId",
                table: "PlayerTombola",
                column: "TombolasId");

            migrationBuilder.CreateIndex(
                name: "IX_TombolaWinner_PlayerId",
                table: "TombolaWinner",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_TombolaWinner_TombolaId",
                table: "TombolaWinner",
                column: "TombolaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Awards_Tombolas_TombolaId",
                table: "Awards",
                column: "TombolaId",
                principalTable: "Tombolas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Awards_Tombolas_TombolaId",
                table: "Awards");

            migrationBuilder.DropTable(
                name: "PlayerTombola");

            migrationBuilder.DropTable(
                name: "TombolaWinner");

            migrationBuilder.DropColumn(
                name: "MaximumAwards",
                table: "Tombolas");

            migrationBuilder.DropColumn(
                name: "MaximumPlayers",
                table: "Tombolas");

            migrationBuilder.DropColumn(
                name: "MinimumAwards",
                table: "Tombolas");

            migrationBuilder.DropColumn(
                name: "MinimumPlayers",
                table: "Tombolas");

            migrationBuilder.DropColumn(
                name: "StrategyType",
                table: "Tombolas");

            migrationBuilder.AddColumn<int>(
                name: "TombolaId",
                table: "Players",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TombolaId",
                table: "Awards",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_TombolaId",
                table: "Players",
                column: "TombolaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Awards_Tombolas_TombolaId",
                table: "Awards",
                column: "TombolaId",
                principalTable: "Tombolas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Tombolas_TombolaId",
                table: "Players",
                column: "TombolaId",
                principalTable: "Tombolas",
                principalColumn: "Id");
        }
    }
}
