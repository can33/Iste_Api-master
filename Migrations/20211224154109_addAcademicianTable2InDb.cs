using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Iste_Api.Migrations
{
    public partial class addAcademicianTable2InDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Academicians",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenEndDate",
                table: "Academicians",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Academicians");

            migrationBuilder.DropColumn(
                name: "RefreshTokenEndDate",
                table: "Academicians");
        }
    }
}
