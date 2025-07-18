﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoryMES.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedToMachines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Machines",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Machines");
        }
    }
}
