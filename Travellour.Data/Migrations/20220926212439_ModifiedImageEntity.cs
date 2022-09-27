using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travellour.Data.Migrations
{
    public partial class ModifiedImageEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCoverImage",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "IsProfileImage",
                table: "Images");
        }
    }
}
