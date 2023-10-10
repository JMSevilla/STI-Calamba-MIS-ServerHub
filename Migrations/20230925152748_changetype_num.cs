using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sti_sys_backend.Migrations
{
    /// <inheritdoc />
    public partial class changetype_num : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ticketing_accounts_requesterId",
                table: "ticketing");

            migrationBuilder.DropIndex(
                name: "IX_ticketing_requesterId",
                table: "ticketing");

            migrationBuilder.AlterColumn<int>(
                name: "specificAssignee",
                table: "ticketing",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "specificAssignee",
                table: "ticketing",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ticketing_requesterId",
                table: "ticketing",
                column: "requesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ticketing_accounts_requesterId",
                table: "ticketing",
                column: "requesterId",
                principalTable: "accounts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
