using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SitioMVC.Migrations
{
    /// <inheritdoc />
    public partial class TablaPersonas_ColumnaCorreo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "Personas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Correo",
                table: "Personas");
        }
    }
}
