using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YallaR7la.Migrations
{
    /// <inheritdoc />
    public partial class ReBuildDatabaseagain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "DestinationImages");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "BusinessOwners");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Admins");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "DestinationImages",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "BusinessOwners",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<Guid>(
                name: "UniqueIdImage",
                table: "BusinessOwners",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Admins",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<Guid>(
                name: "UniqeImageId",
                table: "Admins",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Bookeds",
                columns: table => new
                {
                    BookedId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DestinationId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookeds", x => x.BookedId);
                    table.ForeignKey(
                        name: "FK_Bookeds_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookeds_Destinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Destinations",
                        principalColumn: "DestinationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookeds_DestinationId",
                table: "Bookeds",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookeds_UserId",
                table: "Bookeds",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookeds");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "DestinationImages");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "BusinessOwners");

            migrationBuilder.DropColumn(
                name: "UniqueIdImage",
                table: "BusinessOwners");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "UniqeImageId",
                table: "Admins");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "DestinationImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "BusinessOwners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
