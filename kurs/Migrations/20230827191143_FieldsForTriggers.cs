using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kurs.Migrations
{
    /// <inheritdoc />
    public partial class FieldsForTriggers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductCount",
                table: "Categories",
                newName: "ProductsCount");

            migrationBuilder.AddColumn<int>(
                name: "ProductsCount",
                table: "Tags",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReviewsCount",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductsCount",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductsCount",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "ReviewsCount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductsCount",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "ProductsCount",
                table: "Categories",
                newName: "ProductCount");
        }
    }
}
