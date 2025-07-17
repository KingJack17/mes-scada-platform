using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoryMES.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCycleTimeForOeeCalculation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ActualCycleTimeSeconds",
                table: "ProductionData",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "IdealCycleTimeSeconds",
                table: "Machines",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualCycleTimeSeconds",
                table: "ProductionData");

            migrationBuilder.DropColumn(
                name: "IdealCycleTimeSeconds",
                table: "Machines");
        }
    }
}
