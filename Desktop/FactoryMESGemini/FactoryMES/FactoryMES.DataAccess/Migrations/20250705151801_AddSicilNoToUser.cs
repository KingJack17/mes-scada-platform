using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoryMES.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSicilNoToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SicilNo",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SicilNo",
                table: "Users");
        }
    }
}
