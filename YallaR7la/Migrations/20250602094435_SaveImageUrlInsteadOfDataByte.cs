using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YallaR7la.Migrations
{
    /// <inheritdoc />
    public partial class SaveImageUrlInsteadOfDataByte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "DestinationImages");

            migrationBuilder.DropColumn(
                name: "UniqeImageId",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "UniqeImageId",
                table: "DestinationImages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
        }
    }
}
