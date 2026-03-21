using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SitioMVC.Migrations
{
    /// <inheritdoc />
    public partial class Foto_FechaNacimiento_Tabla_Personas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaNacimiento",
                table: "Personas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FotoUrl",
                table: "Personas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaNacimiento",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "FotoUrl",
                table: "Personas");
        }
    }
}
