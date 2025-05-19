using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YallaR7la.Migrations
{
    /// <inheritdoc />
    public partial class EditTheAdminIdToNulable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Chat_ChatId",
                table: "Message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Message",
                table: "Message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chat",
                table: "Chat");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Chat");

            migrationBuilder.AlterColumn<string>(
                name: "ChatId",
                table: "Message",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "MessageId",
                table: "Message",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "AdminId",
                table: "Chat",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ChatId",
                table: "Chat",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Message",
                table: "Message",
                column: "MessageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chat",
                table: "Chat",
                column: "ChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Chat_ChatId",
                table: "Message",
                column: "ChatId",
                principalTable: "Chat",
                principalColumn: "ChatId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Chat_ChatId",
                table: "Message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Message",
                table: "Message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chat",
                table: "Chat");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Chat");

            migrationBuilder.AlterColumn<int>(
                name: "ChatId",
                table: "Message",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Message",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "AdminId",
                table: "Chat",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Chat",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Message",
                table: "Message",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chat",
                table: "Chat",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Chat_ChatId",
                table: "Message",
                column: "ChatId",
                principalTable: "Chat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
