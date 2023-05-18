using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HueFestivalTicketOnline.DataAccess.Migrations
{
    public partial class DeleteDetailInvoiceTableFromDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetailInvoices");

            migrationBuilder.AddColumn<int>(
                name: "FesTypeTicketId",
                table: "InvoiceTickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "InvoiceTickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTickets_FesTypeTicketId",
                table: "InvoiceTickets",
                column: "FesTypeTicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceTickets_FesTypeTickets_FesTypeTicketId",
                table: "InvoiceTickets",
                column: "FesTypeTicketId",
                principalTable: "FesTypeTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceTickets_FesTypeTickets_FesTypeTicketId",
                table: "InvoiceTickets");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceTickets_FesTypeTicketId",
                table: "InvoiceTickets");

            migrationBuilder.DropColumn(
                name: "FesTypeTicketId",
                table: "InvoiceTickets");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "InvoiceTickets");

            migrationBuilder.CreateTable(
                name: "DetailInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FesTypeTicketId = table.Column<int>(type: "int", nullable: false),
                    InvoiceTicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetailInvoices_FesTypeTickets_FesTypeTicketId",
                        column: x => x.FesTypeTicketId,
                        principalTable: "FesTypeTickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetailInvoices_InvoiceTickets_InvoiceTicketId",
                        column: x => x.InvoiceTicketId,
                        principalTable: "InvoiceTickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetailInvoices_FesTypeTicketId",
                table: "DetailInvoices",
                column: "FesTypeTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailInvoices_InvoiceTicketId",
                table: "DetailInvoices",
                column: "InvoiceTicketId");
        }
    }
}
