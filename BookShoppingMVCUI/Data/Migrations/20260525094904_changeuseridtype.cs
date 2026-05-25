using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShoppingMVCUI.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeuseridtype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "ShoppingCart",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "userId",
                table: "ShoppingCart",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
