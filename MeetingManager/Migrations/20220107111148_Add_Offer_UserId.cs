using Microsoft.EntityFrameworkCore.Migrations;

namespace MeetingManager.Migrations
{
    public partial class Add_Offer_UserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offer_User_UserForeignKey",
                table: "Offer");

            migrationBuilder.DropIndex(
                name: "IX_Offer_UserForeignKey",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "UserForeignKey",
                table: "Offer");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Offer",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Offer",
                type: "decimal(10,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Offer",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offer_UserId",
                table: "Offer",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_User_UserId",
                table: "Offer",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offer_User_UserId",
                table: "Offer");

            migrationBuilder.DropIndex(
                name: "IX_Offer_UserId",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Offer");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Offer",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Offer",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,4)");

            migrationBuilder.AddColumn<int>(
                name: "UserForeignKey",
                table: "Offer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Offer_UserForeignKey",
                table: "Offer",
                column: "UserForeignKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_User_UserForeignKey",
                table: "Offer",
                column: "UserForeignKey",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
