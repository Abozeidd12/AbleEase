using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbleEaseInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class type_message : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "messageType",
                table: "SentMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "messageType",
                table: "ReceivedMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "messageType",
                table: "SentMessages");

            migrationBuilder.DropColumn(
                name: "messageType",
                table: "ReceivedMessages");
        }
    }
}
