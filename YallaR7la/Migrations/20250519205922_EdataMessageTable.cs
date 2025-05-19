using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YallaR7la.Migrations
{
    /// <inheritdoc />
    public partial class EdataMessageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Admins_SenderId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_AspNetUsers_SenderId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_BusinessOwners_SenderId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_SenderId",
                table: "Message");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Message",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "Message",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "BusinessOwnerId",
                table: "Message",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdminId",
                table: "Message",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Message_AdminId",
                table: "Message",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_BusinessOwnerId",
                table: "Message",
                column: "BusinessOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserId",
                table: "Message",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Admins_AdminId",
                table: "Message",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "AdminId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_AspNetUsers_UserId",
                table: "Message",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_BusinessOwners_BusinessOwnerId",
                table: "Message",
                column: "BusinessOwnerId",
                principalTable: "BusinessOwners",
                principalColumn: "BusinessOwnerId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Admins_AdminId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_AspNetUsers_UserId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_BusinessOwners_BusinessOwnerId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_AdminId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_BusinessOwnerId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_UserId",
                table: "Message");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Message",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "Message",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "BusinessOwnerId",
                table: "Message",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdminId",
                table: "Message",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Message_SenderId",
                table: "Message",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Admins_SenderId",
                table: "Message",
                column: "SenderId",
                principalTable: "Admins",
                principalColumn: "AdminId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_AspNetUsers_SenderId",
                table: "Message",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_BusinessOwners_SenderId",
                table: "Message",
                column: "SenderId",
                principalTable: "BusinessOwners",
                principalColumn: "BusinessOwnerId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
