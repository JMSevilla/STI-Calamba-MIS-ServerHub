using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sti_sys_backend.Migrations
{
    /// <inheritdoc />
    public partial class jitsi_private_key : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "jitsi_private_key",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrivateKey = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    active = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jitsi_private_key", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "jitsi_private_key");
        }
    }
}
