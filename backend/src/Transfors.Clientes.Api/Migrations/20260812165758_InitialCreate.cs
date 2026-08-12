using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transfors.Clientes.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoDocumento = table.Column<int>(type: "int", nullable: false),
                    NumeroDocumento = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nombres = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaNacimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    Genero = table.Column<int>(type: "int", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CorreoElectronico = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Ciudad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_CorreoElectronico",
                table: "Clientes",
                column: "CorreoElectronico");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_TipoDocumento_NumeroDocumento",
                table: "Clientes",
                columns: new[] { "TipoDocumento", "NumeroDocumento" },
                unique: true);

            // Stored procedure para el listado/búsqueda de clientes.
            // Demuestra el uso de procedimientos almacenados en SQL Server (valorado en la oferta):
            //   @Search: texto libre que filtra por nombres, apellidos, documento, correo o ciudad.
            //   @Estado: filtra por estado (1 = Activo, 0 = Inactivo). NULL = todos.
            migrationBuilder.Sql(@"
CREATE OR ALTER PROCEDURE dbo.usp_Clientes_Listar
    @Search NVARCHAR(200) = NULL,
    @Estado BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        TipoDocumento,
        NumeroDocumento,
        Nombres,
        Apellidos,
        FechaNacimiento,
        Genero,
        Telefono,
        CorreoElectronico,
        Direccion,
        Ciudad,
        Estado,
        FechaCreacion,
        FechaModificacion
    FROM dbo.Clientes
    WHERE
        (@Estado IS NULL OR Estado = @Estado)
        AND (
            @Search IS NULL OR LTRIM(RTRIM(@Search)) = ''
            OR Nombres          LIKE '%' + @Search + '%'
            OR Apellidos        LIKE '%' + @Search + '%'
            OR NumeroDocumento  LIKE '%' + @Search + '%'
            OR CorreoElectronico LIKE '%' + @Search + '%'
            OR Ciudad           LIKE '%' + @Search + '%'
        )
    ORDER BY Apellidos, Nombres;
END;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.usp_Clientes_Listar;");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
