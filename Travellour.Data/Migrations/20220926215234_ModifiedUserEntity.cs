using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travellour.Data.Migrations
{
    public partial class ModifiedUserEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Images_ImageId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsCoverImage",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "IsProfileImage",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "ImageId",
                table: "AspNetUsers",
                newName: "ProfileImageId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_ImageId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_ProfileImageId");

            migrationBuilder.AddColumn<int>(
                name: "CoverImageId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CoverImageId",
                table: "AspNetUsers",
                column: "CoverImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Images_CoverImageId",
                table: "AspNetUsers",
                column: "CoverImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Images_ProfileImageId",
                table: "AspNetUsers",
                column: "ProfileImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Images_CoverImageId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Images_ProfileImageId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CoverImageId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CoverImageId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ProfileImageId",
                table: "AspNetUsers",
                newName: "ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_ProfileImageId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_ImageId");

            migrationBuilder.AddColumn<bool>(
                name: "IsCoverImage",
                table: "Images",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsProfileImage",
                table: "Images",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Images_ImageId",
                table: "AspNetUsers",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }
    }
}
