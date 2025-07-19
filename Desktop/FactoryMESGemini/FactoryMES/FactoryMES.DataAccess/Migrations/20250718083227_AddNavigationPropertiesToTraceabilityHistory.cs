using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoryMES.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddNavigationPropertiesToTraceabilityHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TraceableUnitHistories_MachineId",
                table: "TraceableUnitHistories",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_TraceableUnitHistories_OperatorId",
                table: "TraceableUnitHistories",
                column: "OperatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_TraceableUnitHistories_Machines_MachineId",
                table: "TraceableUnitHistories",
                column: "MachineId",
                principalTable: "Machines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TraceableUnitHistories_Users_OperatorId",
                table: "TraceableUnitHistories",
                column: "OperatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TraceableUnitHistories_Machines_MachineId",
                table: "TraceableUnitHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TraceableUnitHistories_Users_OperatorId",
                table: "TraceableUnitHistories");

            migrationBuilder.DropIndex(
                name: "IX_TraceableUnitHistories_MachineId",
                table: "TraceableUnitHistories");

            migrationBuilder.DropIndex(
                name: "IX_TraceableUnitHistories_OperatorId",
                table: "TraceableUnitHistories");
        }
    }
}
