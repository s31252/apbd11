using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apbd11.Migrations
{
    /// <inheritdoc />
    public partial class Initooo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Medicaments",
                columns: new[] { "IdMedicament", "Description", "Name", "Type" },
                values: new object[] { 2, "Some desc...", "BBB", "BBB" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Medicaments",
                keyColumn: "IdMedicament",
                keyValue: 2);
        }
    }
}
