using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthCareSSIApi.Migrations
{
    public partial class DocumentTablesAddSignedMessageToDocSignedMessageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SignedMesage",
                table: "DocumentSignedMessages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignedMesage",
                table: "DocumentSignedMessages");
        }
    }
}
