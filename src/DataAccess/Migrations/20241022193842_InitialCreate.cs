using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ident = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LatitudeDeg = table.Column<float>(type: "real", nullable: false),
                    LongitudeDeg = table.Column<float>(type: "real", nullable: false),
                    ElevationFt = table.Column<int>(type: "int", nullable: true),
                    Continent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsoCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsoRegion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Municipality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScheduledService = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GpsCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IataCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WikipediaLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Keywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Airports");
        }
    }
}
