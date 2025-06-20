using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETickets.Migrations
{
    /// <inheritdoc />
    public partial class addmigrationx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Movies__MovieDisplayStateId__44FFC1QA",
                table: "Movies");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_MovieDisplayStates_MovieDisplayStateId",
                table: "Movies",
                column: "MovieDisplayStateId",
                principalTable: "MovieDisplayStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_MovieDisplayStates_MovieDisplayStateId",
                table: "Movies");

            migrationBuilder.AddForeignKey(
                name: "FK__Movies__MovieDisplayStateId__44FFC1QA",
                table: "Movies",
                column: "MovieDisplayStateId",
                principalTable: "MovieDisplayStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
