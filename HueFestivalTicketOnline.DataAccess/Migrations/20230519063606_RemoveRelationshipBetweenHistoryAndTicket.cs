using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HueFestivalTicketOnline.DataAccess.Migrations
{
    public partial class RemoveRelationshipBetweenHistoryAndTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoryChecks_Tickets_TicketId",
                table: "HistoryChecks");

            migrationBuilder.RenameColumn(
                name: "TicketId",
                table: "HistoryChecks",
                newName: "FesProgramId");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryChecks_TicketId",
                table: "HistoryChecks",
                newName: "IX_HistoryChecks_FesProgramId");

            migrationBuilder.AlterColumn<string>(
                name: "TicketCode",
                table: "Tickets",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryChecks_FesPrograms_FesProgramId",
                table: "HistoryChecks",
                column: "FesProgramId",
                principalTable: "FesPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoryChecks_FesPrograms_FesProgramId",
                table: "HistoryChecks");

            migrationBuilder.RenameColumn(
                name: "FesProgramId",
                table: "HistoryChecks",
                newName: "TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryChecks_FesProgramId",
                table: "HistoryChecks",
                newName: "IX_HistoryChecks_TicketId");

            migrationBuilder.AlterColumn<string>(
                name: "TicketCode",
                table: "Tickets",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryChecks_Tickets_TicketId",
                table: "HistoryChecks",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
