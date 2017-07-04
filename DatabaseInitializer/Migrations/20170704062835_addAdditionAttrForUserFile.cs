using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseInitializer.Migrations
{
    public partial class addAdditionAttrForUserFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "UserFiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserFiles",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFiles_AspNetUsers_UserId",
                table: "UserFiles");

            migrationBuilder.DropIndex(
                name: "IX_UserFiles_UserId",
                table: "UserFiles");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "UserFiles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserFiles");
        }
    }
}
