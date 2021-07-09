using Microsoft.EntityFrameworkCore.Migrations;

namespace Service.FrontendKeyValue.Postgres.Migrations
{
    public partial class ver_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "frontend_key_value");

            migrationBuilder.CreateTable(
                name: "key_value",
                schema: "frontend_key_value",
                columns: table => new
                {
                    Key = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    ClientId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_key_value", x => new { x.ClientId, x.Key });
                });

            migrationBuilder.CreateIndex(
                name: "IX_key_value_ClientId",
                schema: "frontend_key_value",
                table: "key_value",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "key_value",
                schema: "frontend_key_value");
        }
    }
}
