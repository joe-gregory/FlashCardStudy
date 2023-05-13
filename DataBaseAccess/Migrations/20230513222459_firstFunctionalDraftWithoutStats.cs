using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataBaseAccess.Migrations
{
    /// <inheritdoc />
    public partial class firstFunctionalDraftWithoutStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RightScores",
                table: "StudySession",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Score",
                table: "StudySession",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Turns",
                table: "StudySession",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WrongScores",
                table: "StudySession",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "FlashCard",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Turn",
                table: "CardStudySessionScore",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RightScores",
                table: "StudySession");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "StudySession");

            migrationBuilder.DropColumn(
                name: "Turns",
                table: "StudySession");

            migrationBuilder.DropColumn(
                name: "WrongScores",
                table: "StudySession");

            migrationBuilder.DropColumn(
                name: "Turn",
                table: "CardStudySessionScore");

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "FlashCard",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
