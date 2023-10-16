using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace AuthorizationOperation.Infrastructure.EF.Migrations
{
    public partial class AuthorizationDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "authorization_status",
                columns: table => new
                {
                    id = table.Column<ushort>(type: "smallint unsigned", nullable: false),
                    name = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_authorization_status_id", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "char(36)", nullable: false),
                    user_name = table.Column<string>(type: "longtext", nullable: false),
                    password = table.Column<string>(type: "longtext", nullable: false),
                    email = table.Column<string>(type: "longtext", nullable: false),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_uuid", x => x.uuid);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "authorization",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    customer = table.Column<string>(type: "longtext", nullable: false),
                    status_id = table.Column<ushort>(type: "smallint unsigned", nullable: false),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_authorization_id", x => x.id);
                    table.ForeignKey(
                        name: "fk_authorization_authorization_status_status_id",
                        column: x => x.status_id,
                        principalTable: "authorization_status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "authorization_status",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { (ushort)1, "WAITING_FOR_SIGNERS" },
                    { (ushort)2, "AUTHORIZED" },
                    { (ushort)3, "EXPIRED" },
                    { (ushort)4, "CANCELLED" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_authorization_status_id",
                table: "authorization",
                column: "status_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "authorization");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "authorization_status");
        }
    }
}
