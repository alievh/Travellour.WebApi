using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travellour.Data.Migrations
{
    public partial class UpdatedGroupEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Images_ImageId",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "ImageId",
                table: "Groups",
                newName: "ProfileImageId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_ImageId",
                table: "Groups",
                newName: "IX_Groups_ProfileImageId");

            migrationBuilder.AddColumn<int>(
                name: "CoverImageId",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_CoverImageId",
                table: "Groups",
                column: "CoverImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Images_CoverImageId",
                table: "Groups",
                column: "CoverImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Images_ProfileImageId",
                table: "Groups",
                column: "ProfileImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Images_CoverImageId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Images_ProfileImageId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_CoverImageId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "CoverImageId",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "ProfileImageId",
                table: "Groups",
                newName: "ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_ProfileImageId",
                table: "Groups",
                newName: "IX_Groups_ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Images_ImageId",
                table: "Groups",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }
    }
}
