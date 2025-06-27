using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETickets.Migrations
{
    /// <inheritdoc />
    public partial class xd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieImage_Movies_MovieId",
                table: "MovieImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieImage",
                table: "MovieImage");

            migrationBuilder.RenameTable(
                name: "MovieImage",
                newName: "MovieImages");

            migrationBuilder.RenameIndex(
                name: "IX_MovieImage_MovieId",
                table: "MovieImages",
                newName: "IX_MovieImages_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieImages",
                table: "MovieImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieImages_Movies_MovieId",
                table: "MovieImages",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieImages_Movies_MovieId",
                table: "MovieImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieImages",
                table: "MovieImages");

            migrationBuilder.RenameTable(
                name: "MovieImages",
                newName: "MovieImage");

            migrationBuilder.RenameIndex(
                name: "IX_MovieImages_MovieId",
                table: "MovieImage",
                newName: "IX_MovieImage_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieImage",
                table: "MovieImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieImage_Movies_MovieId",
                table: "MovieImage",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
