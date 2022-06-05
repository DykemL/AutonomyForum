using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutonomyForum.Migrations
{
    public partial class AddAvatar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AvatarFileId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AvatarFileId",
                table: "AspNetUsers",
                column: "AvatarFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Files_AvatarFileId",
                table: "AspNetUsers",
                column: "AvatarFileId",
                principalTable: "Files",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Files_AvatarFileId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AvatarFileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AvatarFileId",
                table: "AspNetUsers");
        }
    }
}
