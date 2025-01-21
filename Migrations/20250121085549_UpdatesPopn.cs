using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace test.Migrations
{
    /// <inheritdoc />
    public partial class UpdatesPopn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryID",
                table: "Populations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Populations_CountryID",
                table: "Populations",
                column: "CountryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Populations_Countries_CountryID",
                table: "Populations",
                column: "CountryID",
                principalTable: "Countries",
                principalColumn: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Populations_Countries_CountryID",
                table: "Populations");

            migrationBuilder.DropIndex(
                name: "IX_Populations_CountryID",
                table: "Populations");

            migrationBuilder.DropColumn(
                name: "CountryID",
                table: "Populations");
        }
    }
}
