using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TombolaGame.Migrations
{
    /// <inheritdoc />
    public partial class AddEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "StrategyTypeTemp",
                table: "Tombolas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            // Обновяване на временната колона с правилните стойности според string-а
            migrationBuilder.Sql(@"
                UPDATE ""Tombolas"" SET ""StrategyTypeTemp"" =
                    CASE ""StrategyType""
                        WHEN 'OnePrizePerPlayer' THEN 0
                        WHEN 'Random' THEN 1
                        WHEN 'Weighted' THEN 2
                        ELSE 0
                    END;
            ");

            // Добавяне на временна колона за State
            migrationBuilder.AddColumn<int>(
                name: "StateTemp",
                table: "Tombolas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            // Обновяване на временната State колона
            migrationBuilder.Sql(@"
                UPDATE ""Tombolas"" SET ""StateTemp"" =
                    CASE ""State""
                        WHEN 'Waiting' THEN 0
                        WHEN 'InProgress' THEN 1
                        WHEN 'Finished' THEN 2
                        ELSE 0
                    END;
            ");

            // Изтриване на старите string колони
            migrationBuilder.DropColumn(
                name: "StrategyType",
                table: "Tombolas");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Tombolas");

            // Преименуване на временните колони към оригиналните имена
            migrationBuilder.RenameColumn(
                name: "StrategyTypeTemp",
                table: "Tombolas",
                newName: "StrategyType");

            migrationBuilder.RenameColumn(
                name: "StateTemp",
                table: "Tombolas",
                newName: "State");

            migrationBuilder.AlterColumn<int>(
                name: "StrategyType",
                table: "Tombolas",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "State",
                table: "Tombolas",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StrategyTypeTemp",
                table: "Tombolas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
                UPDATE ""Tombolas"" SET ""StrategyTypeTemp"" =
                    CASE ""StrategyType""
                        WHEN 0 THEN 'OnePrizePerPlayer'
                        WHEN 1 THEN 'Random'
                        WHEN 2 THEN 'Weighted'
                        ELSE 'OnePrizePerPlayer'
                    END;
            ");

            migrationBuilder.AddColumn<string>(
                name: "StateTemp",
                table: "Tombolas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
                UPDATE ""Tombolas"" SET ""StateTemp"" =
                    CASE ""State""
                        WHEN 0 THEN 'Waiting'
                        WHEN 1 THEN 'InProgress'
                        WHEN 2 THEN 'Finished'
                        ELSE 'Waiting'
                    END;
            ");

            migrationBuilder.DropColumn(
                name: "StrategyType",
                table: "Tombolas");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Tombolas");

            migrationBuilder.RenameColumn(
                name: "StrategyTypeTemp",
                table: "Tombolas",
                newName: "StrategyType");

            migrationBuilder.RenameColumn(
                name: "StateTemp",
                table: "Tombolas",
                newName: "State");

            migrationBuilder.AlterColumn<string>(
                name: "StrategyType",
                table: "Tombolas",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Tombolas",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
