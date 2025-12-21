using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SitioMVC.Migrations
{
    /// <inheritdoc />
    public partial class TablaReportesImagenesEnParalelo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportesProcesoParaleloImagenes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorreoUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportesProcesoParaleloImagenes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImagenProcesoParalelo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReporteProcesoParaleloImagenesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EstadoImagenProcesoParalelo = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagenProcesoParalelo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImagenProcesoParalelo_ReportesProcesoParaleloImagenes_ReporteProcesoParaleloImagenesId",
                        column: x => x.ReporteProcesoParaleloImagenesId,
                        principalTable: "ReportesProcesoParaleloImagenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImagenProcesoParalelo_ReporteProcesoParaleloImagenesId",
                table: "ImagenProcesoParalelo",
                column: "ReporteProcesoParaleloImagenesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImagenProcesoParalelo");

            migrationBuilder.DropTable(
                name: "ReportesProcesoParaleloImagenes");
        }
    }
}
