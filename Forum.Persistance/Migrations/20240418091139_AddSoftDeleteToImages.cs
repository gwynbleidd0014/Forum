﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum.Persistance.Migrations
{
    public partial class AddSoftDeleteToImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Images",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Images");
        }
    }
}
