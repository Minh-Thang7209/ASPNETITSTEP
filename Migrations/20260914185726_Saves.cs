using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPNETITSTEP.Migrations
{
    /// <inheritdoc />
    public partial class Saves : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccesses_UsersRoles_RoleId",
                table: "UserAccesses");

            migrationBuilder.DropTable(
                name: "AuthJournals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersRoles",
                table: "UsersRoles");

            migrationBuilder.RenameTable(
                name: "UsersRoles",
                newName: "UserRoles");

            migrationBuilder.RenameColumn(
                name: "BirthDate",
                table: "UsersData",
                newName: "Birthdate");

            migrationBuilder.AddColumn<int>(
                name: "OrderInPrice",
                table: "ProductVersions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "OrderInPrice",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "UserAccesses",
                keyColumn: "Id",
                keyValue: new Guid("96dcbbba-9aee-44a2-8835-72dfe4e1a710"),
                column: "RoleId",
                value: new Guid("acb35324-7b84-4e3b-9a26-00aad72a600c"));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: new Guid("702d05c2-fcd0-4c1d-b0bb-2aeb4b98f91a"),
                column: "Description",
                value: "Самозареєстрований користувач");

            migrationBuilder.UpdateData(
                table: "UsersData",
                keyColumn: "Id",
                keyValue: new Guid("190052ca-f844-498a-a05f-1d4ba2adc0e8"),
                column: "FullName",
                value: "Адміністратор Системи");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccesses_UserRoles_RoleId",
                table: "UserAccesses",
                column: "RoleId",
                principalTable: "UserRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccesses_UserRoles_RoleId",
                table: "UserAccesses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "OrderInPrice",
                table: "ProductVersions");

            migrationBuilder.DropColumn(
                name: "OrderInPrice",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UsersRoles");

            migrationBuilder.RenameColumn(
                name: "Birthdate",
                table: "UsersData",
                newName: "BirthDate");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersRoles",
                table: "UsersRoles",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AuthJournals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Dk = table.Column<string>(type: "TEXT", nullable: false),
                    IsOk = table.Column<bool>(type: "INTEGER", nullable: false),
                    Login = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthJournals", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "UserAccesses",
                keyColumn: "Id",
                keyValue: new Guid("96dcbbba-9aee-44a2-8835-72dfe4e1a710"),
                column: "RoleId",
                value: new Guid("702d05c2-fcd0-4c1d-b0bb-2aeb4b98f91a"));

            migrationBuilder.UpdateData(
                table: "UsersData",
                keyColumn: "Id",
                keyValue: new Guid("190052ca-f844-498a-a05f-1d4ba2adc0e8"),
                column: "FullName",
                value: "Адміністратор системи");

            migrationBuilder.UpdateData(
                table: "UsersRoles",
                keyColumn: "Id",
                keyValue: new Guid("702d05c2-fcd0-4c1d-b0bb-2aeb4b98f91a"),
                column: "Description",
                value: "Самозаєстрований користувач");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccesses_UsersRoles_RoleId",
                table: "UserAccesses",
                column: "RoleId",
                principalTable: "UsersRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
