using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShopProject.Migrations
{
    /// <inheritdoc />
    public partial class newcategoryfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLeafCategory",
                table: "Categories",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLeafCategory",
                table: "Categories");
        }
    }
}
