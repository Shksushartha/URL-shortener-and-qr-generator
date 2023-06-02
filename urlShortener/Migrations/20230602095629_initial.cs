using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace urlShortener.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "urls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    urlIdentifier = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    originalUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_urls", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_urls_urlIdentifier",
                table: "urls",
                column: "urlIdentifier",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "urls");
        }
    }
}
