using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETickets.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCHair : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfChairs",
                table: "Cinemas");

            migrationBuilder.AddColumn<bool>(
                name: "IsWorking",
                table: "Chairs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWorking",
                table: "Chairs");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfChairs",
                table: "Cinemas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
