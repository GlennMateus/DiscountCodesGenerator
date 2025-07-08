using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscountCodesGenerator.Migrations
{
    /// <inheritdoc />
    public partial class DiscountCodeEntityAdjusts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "DiscountCodes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "TimesUsed",
                table: "DiscountCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodes_Code",
                table: "DiscountCodes",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DiscountCodes_Code",
                table: "DiscountCodes");

            migrationBuilder.DropColumn(
                name: "TimesUsed",
                table: "DiscountCodes");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "DiscountCodes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
