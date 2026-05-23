using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShoppingMVCUI.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedstatusid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "OrderStatus",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "StatusID",
                table: "OrderStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusID",
                table: "OrderStatus");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "OrderStatus",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);
        }
    }
}
