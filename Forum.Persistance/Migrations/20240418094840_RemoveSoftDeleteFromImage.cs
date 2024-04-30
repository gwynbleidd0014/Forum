using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum.Persistance.Migrations
{
    public partial class RemoveSoftDeleteFromImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Images");

            migrationBuilder.AddColumn<string>(
                name: "AbsolutePath",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AbsolutePath",
                table: "Images");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Images",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
