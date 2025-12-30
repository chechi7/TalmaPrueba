using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppiEjercicio.Migrations
{
    /// <inheritdoc />
    public partial class PrimeraMigración : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InfoVuelos",
                columns: table => new
                {
                    IdVuelo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CiudadOrigen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaVuelo = table.Column<DateOnly>(type: "date", nullable: false),
                    Aerolinea = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NumeroVuelo = table.Column<int>(type: "int", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoVuelos", x => x.IdVuelo);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InfoVuelos");
        }
    }
}
