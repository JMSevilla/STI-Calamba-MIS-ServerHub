using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sti_sys_backend.Migrations
{
    /// <inheritdoc />
    public partial class localmechanisms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mail_gun_secured",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    domain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _apistatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mail_gun_secured", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mail_gun_secured");
        }
    }
}
