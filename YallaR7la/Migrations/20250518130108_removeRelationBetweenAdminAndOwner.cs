using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YallaR7la.Migrations
{
    /// <inheritdoc />
    public partial class removeRelationBetweenAdminAndOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessOwners_Admins_AdminId",
                table: "BusinessOwners");

            migrationBuilder.DropIndex(
                name: "IX_BusinessOwners_AdminId",
                table: "BusinessOwners");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "BusinessOwners");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "BusinessOwners",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessOwners_AdminId",
                table: "BusinessOwners",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessOwners_Admins_AdminId",
                table: "BusinessOwners",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "AdminId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
