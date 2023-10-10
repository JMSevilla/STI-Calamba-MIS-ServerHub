﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sti_sys_backend.Migrations
{
    /// <inheritdoc />
    public partial class joinedparticipantstb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "joined_participants",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    accountId = table.Column<int>(type: "int", nullable: false),
                    room_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    comlabId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    _joinedStatus = table.Column<int>(type: "int", nullable: false),
                    date_joined = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_left = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_joined_participants", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "joined_participants");

            
        }
    }
}
