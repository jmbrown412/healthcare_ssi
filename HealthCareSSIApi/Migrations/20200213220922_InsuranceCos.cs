using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthCareSSIApi.Migrations
{
    public partial class InsuranceCos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Hospital",
                table: "Hospital");

            migrationBuilder.RenameTable(
                name: "Hospital",
                newName: "Hospitals");

            migrationBuilder.RenameIndex(
                name: "IX_Hospital_Name",
                table: "Hospitals",
                newName: "IX_Hospitals_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Hospitals",
                table: "Hospitals",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "InsuranceCompanies",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    PublicKey = table.Column<string>(nullable: true),
                    CreatedAtUTC = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceCompanies", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InsuranceCompanies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Hospitals",
                table: "Hospitals");

            migrationBuilder.RenameTable(
                name: "Hospitals",
                newName: "Hospital");

            migrationBuilder.RenameIndex(
                name: "IX_Hospitals_Name",
                table: "Hospital",
                newName: "IX_Hospital_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Hospital",
                table: "Hospital",
                column: "Id");
        }
    }
}
