using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETickets.Migrations
{
    /// <inheritdoc />
    public partial class fixEr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movieDays_Days_DayId",
                table: "movieDays");

            migrationBuilder.DropForeignKey(
                name: "FK_movieDays_Movies_MovieId",
                table: "movieDays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_movieDays",
                table: "movieDays");

            migrationBuilder.RenameTable(
                name: "movieDays",
                newName: "MovieDays");

            migrationBuilder.RenameIndex(
                name: "IX_movieDays_MovieId",
                table: "MovieDays",
                newName: "IX_MovieDays_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_movieDays_DayId",
                table: "MovieDays",
                newName: "IX_MovieDays_DayId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieDays",
                table: "MovieDays",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieDays_Days_DayId",
                table: "MovieDays",
                column: "DayId",
                principalTable: "Days",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieDays_Movies_MovieId",
                table: "MovieDays",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieDays_Days_DayId",
                table: "MovieDays");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieDays_Movies_MovieId",
                table: "MovieDays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieDays",
                table: "MovieDays");

            migrationBuilder.RenameTable(
                name: "MovieDays",
                newName: "movieDays");

            migrationBuilder.RenameIndex(
                name: "IX_MovieDays_MovieId",
                table: "movieDays",
                newName: "IX_movieDays_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieDays_DayId",
                table: "movieDays",
                newName: "IX_movieDays_DayId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_movieDays",
                table: "movieDays",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_movieDays_Days_DayId",
                table: "movieDays",
                column: "DayId",
                principalTable: "Days",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_movieDays_Movies_MovieId",
                table: "movieDays",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
