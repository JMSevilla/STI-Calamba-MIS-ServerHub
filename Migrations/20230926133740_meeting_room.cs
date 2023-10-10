using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sti_sys_backend.Migrations
{
    /// <inheritdoc />
    public partial class meeting_room : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "meeting_room",
                columns: table => new
                {
                    room_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    room_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    room_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    numbers_of_joiners = table.Column<int>(type: "int", nullable: false),
                    comlabId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    room_link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    room_status = table.Column<int>(type: "int", nullable: false),
                    room_creator = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meeting_room", x => x.room_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "meeting_room");
        }
    }
}
