using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbleEaseInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addprogramprice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "price",
                table: "Programs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "price",
                table: "Programs");
        }
    }
}
