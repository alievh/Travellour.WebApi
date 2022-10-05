using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travellour.Data.Migrations
{
    public partial class UpdatedUserGroupRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Groups");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
