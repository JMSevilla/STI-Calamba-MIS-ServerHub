using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sti_sys_backend.Migrations
{
    /// <inheritdoc />
    public partial class add_joined_accounts_ticketing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Accountsid",
                table: "ticketing",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ticketing_Accountsid",
                table: "ticketing",
                column: "Accountsid");

            migrationBuilder.AddForeignKey(
                name: "FK_ticketing_accounts_Accountsid",
                table: "ticketing",
                column: "Accountsid",
                principalTable: "accounts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ticketing_accounts_Accountsid",
                table: "ticketing");

            migrationBuilder.DropIndex(
                name: "IX_ticketing_Accountsid",
                table: "ticketing");

            migrationBuilder.DropColumn(
                name: "Accountsid",
                table: "ticketing");
        }
    }
}
