using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travellour.Data.Migrations
{
    public partial class ModifiedPostEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Groups_GroupId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_GroupId",
                table: "Posts");

            migrationBuilder.AlterColumn<string>(
                name: "GroupId",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupId1",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_GroupId1",
                table: "Posts",
                column: "GroupId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Groups_GroupId1",
                table: "Posts",
                column: "GroupId1",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Groups_GroupId1",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_GroupId1",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "GroupId1",
                table: "Posts");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Posts",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_GroupId",
                table: "Posts",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Groups_GroupId",
                table: "Posts",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
