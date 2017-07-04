using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseInitializer.Migrations
{
    public partial class IdentityNumberForUserAsClassLabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFiles_AspNetUsers_UserId",
                table: "UserFiles");

            migrationBuilder.DropIndex(
                name: "IX_UserFiles_UserId",
                table: "UserFiles");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserFiles",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdentityNumber",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserFiles",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFiles_UserId",
                table: "UserFiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFiles_AspNetUsers_UserId",
                table: "UserFiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
