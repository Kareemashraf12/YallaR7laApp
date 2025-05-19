using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YallaR7la.Migrations
{
    /// <inheritdoc />
    public partial class EditeTableFeedBack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReviewed",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "SentimentScore",
                table: "Feedbacks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReviewed",
                table: "Feedbacks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SentimentScore",
                table: "Feedbacks",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
