using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthorizationOperation.Infrastructure.EF.Migrations
{
    public partial class AuthorizationAddUUID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "uuid",
                table: "authorization",
                type: "varchar(36)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "uuid",
                table: "authorization");
        }
    }
}
