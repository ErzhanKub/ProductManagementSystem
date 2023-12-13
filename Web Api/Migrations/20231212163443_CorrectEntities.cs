using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Api.Migrations
{
    /// <inheritdoc />
    public partial class CorrectEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalFields",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalFields",
                table: "Products");
        }
    }
}
