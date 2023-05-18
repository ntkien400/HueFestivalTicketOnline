using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HueFestivalTicketOnline.DataAccess.Migrations
{
    public partial class ChangeDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_FesPrograms_FesProgramId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_InvoiceTickets_InvoiceId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TypeTickets_TypeTicketId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "ProgramTypeTickets");

            migrationBuilder.DropTable(
                name: "TypeTickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_FesProgramId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "FesProgramId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "TypeTicketId",
                table: "Tickets",
                newName: "FesTypeTicketId");

            migrationBuilder.RenameColumn(
                name: "InvoiceId",
                table: "Tickets",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_TypeTicketId",
                table: "Tickets",
                newName: "IX_Tickets_FesTypeTicketId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_InvoiceId",
                table: "Tickets",
                newName: "IX_Tickets_UserId");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "InvoiceTickets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "FesTypeTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quanity = table.Column<int>(type: "int", nullable: false),
                    FesProgramId = table.Column<int>(type: "int", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FesTypeTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FesTypeTickets_FesPrograms_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "FesPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetailInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoiceTicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FesTypeTicketId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_FesTypeTickets_ProgramId",
                table: "FesTypeTickets",
                column: "ProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_FesTypeTickets_FesTypeTicketId",
                table: "Tickets",
                column: "FesTypeTicketId",
                principalTable: "FesTypeTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_UserId",
                table: "Tickets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_FesTypeTickets_FesTypeTicketId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_UserId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "DetailInvoices");

            migrationBuilder.DropTable(
                name: "FesTypeTickets");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "InvoiceTickets");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Tickets",
                newName: "InvoiceId");

            migrationBuilder.RenameColumn(
                name: "FesTypeTicketId",
                table: "Tickets",
                newName: "TypeTicketId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_UserId",
                table: "Tickets",
                newName: "IX_Tickets_InvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_FesTypeTicketId",
                table: "Tickets",
                newName: "IX_Tickets_TypeTicketId");

            migrationBuilder.AddColumn<int>(
                name: "FesProgramId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TypeTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeTickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProgramTypeTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    TypeTicketId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quanity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramTypeTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramTypeTickets_FesPrograms_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "FesPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProgramTypeTickets_TypeTickets_TypeTicketId",
                        column: x => x.TypeTicketId,
                        principalTable: "TypeTickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_FesProgramId",
                table: "Tickets",
                column: "FesProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramTypeTickets_ProgramId",
                table: "ProgramTypeTickets",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramTypeTickets_TypeTicketId",
                table: "ProgramTypeTickets",
                column: "TypeTicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_FesPrograms_FesProgramId",
                table: "Tickets",
                column: "FesProgramId",
                principalTable: "FesPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_InvoiceTickets_InvoiceId",
                table: "Tickets",
                column: "InvoiceId",
                principalTable: "InvoiceTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TypeTickets_TypeTicketId",
                table: "Tickets",
                column: "TypeTicketId",
                principalTable: "TypeTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
