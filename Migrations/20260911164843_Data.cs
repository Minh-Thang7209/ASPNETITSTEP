using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPNETITSTEP.Migrations
{
    /// <inheritdoc />
    public partial class Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderInPrice",
                table: "ProductGroups",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderInPrice",
                table: "ProductGroups");
        }
    }
}
