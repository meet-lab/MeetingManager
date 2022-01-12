using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeetingManager.Migrations
{
    public partial class Update_Cart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Cart_CartId",
                table: "Offer");

            migrationBuilder.DropIndex(
                name: "IX_Offer_CartId",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "EditDate",
                table: "Cart");

            migrationBuilder.CreateTable(
                name: "CartLineItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OfferId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartLineItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartLineItem_Cart_CartId",
                        column: x => x.CartId,
                        principalTable: "Cart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartLineItem_Offer_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartLineItem_CartId",
                table: "CartLineItem",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartLineItem_OfferId",
                table: "CartLineItem",
                column: "OfferId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartLineItem");

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "Offer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Cart",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EditDate",
                table: "Cart",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Offer_CartId",
                table: "Offer",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Cart_CartId",
                table: "Offer",
                column: "CartId",
                principalTable: "Cart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
