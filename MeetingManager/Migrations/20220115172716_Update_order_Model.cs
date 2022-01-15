using Microsoft.EntityFrameworkCore.Migrations;

namespace MeetingManager.Migrations
{
    public partial class Update_order_Model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Offer_OfferId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_OfferId",
                table: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "OfferId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OfferId",
                table: "Order",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Order_OfferId",
                table: "Order",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Offer_OfferId",
                table: "Order",
                column: "OfferId",
                principalTable: "Offer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
