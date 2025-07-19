using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoryMES.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddProcessAndMachineRelationsToRoute_Nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MachineId",
                table: "Routes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProcessId",
                table: "Machines",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_MachineId",
                table: "Routes",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_ProcessId",
                table: "Machines",
                column: "ProcessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Processes_ProcessId",
                table: "Machines",
                column: "ProcessId",
                principalTable: "Processes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Machines_MachineId",
                table: "Routes",
                column: "MachineId",
                principalTable: "Machines",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Processes_ProcessId",
                table: "Machines");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Machines_MachineId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_MachineId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Machines_ProcessId",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "MachineId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "ProcessId",
                table: "Machines");
        }
    }
}
