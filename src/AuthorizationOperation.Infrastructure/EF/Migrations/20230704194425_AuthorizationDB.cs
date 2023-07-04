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
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_authorization_status", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Authorizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UUID = table.Column<string>(type: "varchar(36)", nullable: false),
                    Customer = table.Column<string>(type: "longtext", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authorizations_authorization_status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "authorization_status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "authorization_status",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "WAITING_FOR_SIGNERS" },
                    { 1, "AUTHORIZED" },
                    { 2, "EXPIRED" },
                    { 3, "CANCELLED" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_StatusId",
                table: "Authorizations",
                column: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Authorizations");

            migrationBuilder.DropTable(
                name: "authorization_status");
        }
    }
}
