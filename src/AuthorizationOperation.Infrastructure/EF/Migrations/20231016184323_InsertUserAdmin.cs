using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace AuthorizationOperation.Infrastructure.EF.Migrations
{
    public partial class InsertUserAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
              table: "user",
              columns: new[] { "uuid", "user_name", "password", "email", "created" },
              values: new object[] {
                        "680639e9-df5b-11eb-87b7-1c1b0d97eb3a",
                        "admin",
                        "admin",
                        "admin@prueba.com",
                        DateTime.Now
              });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "authorization_status",
                keyColumn: "uuid",
                keyValue: "680639e9-df5b-11eb-87b7-1c1b0d97eb3a"
            );
        }
    }
}
