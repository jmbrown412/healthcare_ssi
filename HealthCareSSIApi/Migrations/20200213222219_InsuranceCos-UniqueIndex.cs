using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthCareSSIApi.Migrations
{
    public partial class InsuranceCosUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicKey",
                table: "InsuranceCompanies");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "InsuranceCompanies",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceCompanies_Name",
                table: "InsuranceCompanies",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InsuranceCompanies_Name",
                table: "InsuranceCompanies");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "InsuranceCompanies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicKey",
                table: "InsuranceCompanies",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
