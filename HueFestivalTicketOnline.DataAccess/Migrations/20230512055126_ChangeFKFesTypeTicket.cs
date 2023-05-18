using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HueFestivalTicketOnline.DataAccess.Migrations
{
    public partial class ChangeFKFesTypeTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FesTypeTickets_FesPrograms_ProgramId",
                table: "FesTypeTickets");

            migrationBuilder.DropIndex(
                name: "IX_FesTypeTickets_ProgramId",
                table: "FesTypeTickets");

            migrationBuilder.DropColumn(
                name: "ProgramId",
                table: "FesTypeTickets");

            migrationBuilder.CreateIndex(
                name: "IX_FesTypeTickets_FesProgramId",
                table: "FesTypeTickets",
                column: "FesProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_FesTypeTickets_FesPrograms_FesProgramId",
                table: "FesTypeTickets",
                column: "FesProgramId",
                principalTable: "FesPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FesTypeTickets_FesPrograms_FesProgramId",
                table: "FesTypeTickets");

            migrationBuilder.DropIndex(
                name: "IX_FesTypeTickets_FesProgramId",
                table: "FesTypeTickets");

            migrationBuilder.AddColumn<int>(
                name: "ProgramId",
                table: "FesTypeTickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FesTypeTickets_ProgramId",
                table: "FesTypeTickets",
                column: "ProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_FesTypeTickets_FesPrograms_ProgramId",
                table: "FesTypeTickets",
                column: "ProgramId",
                principalTable: "FesPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
