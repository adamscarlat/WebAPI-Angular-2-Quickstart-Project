using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HeroAPI.Migrations
{
    public partial class TokenStoreAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TokenStore",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    ExpirationTime = table.Column<ulong>(nullable: false),
                    IsValid = table.Column<bool>(nullable: false),
                    Token = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenStore", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TokenStore_Token",
                table: "TokenStore",
                column: "Token");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TokenStore");
        }
    }
}
