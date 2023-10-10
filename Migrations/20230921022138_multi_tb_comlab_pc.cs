using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sti_sys_backend.Migrations
{
    /// <inheritdoc />
    public partial class multi_tb_comlab_pc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "comLab",
                table: "ticketing",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "com_laboratory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    comlabName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    totalComputers = table.Column<int>(type: "int", nullable: false),
                    totalWorkingComputers = table.Column<int>(type: "int", nullable: false),
                    totalNotWorkingComputers = table.Column<int>(type: "int", nullable: false),
                    totalNoNetworkComputers = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_com_laboratory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "pc",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    operatingSystem = table.Column<int>(type: "int", nullable: false),
                    computerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    comLab = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    comlabId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    computerStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pc", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "com_laboratory");

            migrationBuilder.DropTable(
                name: "pc");

            migrationBuilder.DropColumn(
                name: "comLab",
                table: "ticketing");
        }
    }
}
