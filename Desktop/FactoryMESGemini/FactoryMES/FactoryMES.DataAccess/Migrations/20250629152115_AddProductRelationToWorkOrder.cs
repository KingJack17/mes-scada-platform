using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoryMES.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddProductRelationToWorkOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "WorkOrders");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "WorkOrders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_ProductId",
                table: "WorkOrders",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_Products_ProductId",
                table: "WorkOrders",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_Products_ProductId",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_ProductId",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "WorkOrders");

            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "WorkOrders",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
