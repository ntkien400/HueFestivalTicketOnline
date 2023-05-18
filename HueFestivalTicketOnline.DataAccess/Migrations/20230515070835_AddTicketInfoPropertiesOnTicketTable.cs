using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HueFestivalTicketOnline.DataAccess.Migrations
{
    public partial class AddTicketInfoPropertiesOnTicketTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TicketInfo",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketInfo",
                table: "Tickets");
        }
    }
}
