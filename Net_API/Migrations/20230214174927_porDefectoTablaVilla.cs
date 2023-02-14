using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Net_API.Migrations
{
    public partial class porDefectoTablaVilla : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Detalle", "Encanto", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "Inquilinos", "MetrosCuadrados", "Nombre", "Tarifa" },
                values: new object[] { 1, "Detalle de la Villa...", "", new DateTime(2023, 2, 14, 18, 49, 27, 675, DateTimeKind.Local).AddTicks(8846), new DateTime(2023, 2, 14, 18, 49, 27, 675, DateTimeKind.Local).AddTicks(8815), "", 5, 50, "Villa Real", 200.0 });

            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Detalle", "Encanto", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "Inquilinos", "MetrosCuadrados", "Nombre", "Tarifa" },
                values: new object[] { 2, "Detalle de la Villa...", "", new DateTime(2023, 2, 14, 18, 49, 27, 675, DateTimeKind.Local).AddTicks(8851), new DateTime(2023, 2, 14, 18, 49, 27, 675, DateTimeKind.Local).AddTicks(8849), "", 4, 80, "Premium Vistas Piscina", 400.0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
