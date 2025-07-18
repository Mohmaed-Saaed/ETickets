using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETickets.Migrations
{
    /// <inheritdoc />
    public partial class AddCinemaToCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CinemaId",
                table: "Carts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CinemaId",
                table: "Carts",
                column: "CinemaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Cinemas_CinemaId",
                table: "Carts",
                column: "CinemaId",
                principalTable: "Cinemas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Cinemas_CinemaId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_CinemaId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "CinemaId",
                table: "Carts");
        }
    }
}
