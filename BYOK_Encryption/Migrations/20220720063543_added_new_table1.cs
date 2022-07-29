using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BYOK_Encryption.Migrations
{
    public partial class added_new_table1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bYOK_Enabled_Tenants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Table_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEntireDbEncrypted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bYOK_Enabled_Tenants", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bYOK_Enabled_Tenants");
        }
    }
}
