using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orders.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "countries",
                newName: "name");

            migrationBuilder.RenameIndex(
                name: "IX_countries_Name",
                table: "countries",
                newName: "IX_countries_name");

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_categories_Name",
                table: "categories",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "countries",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_countries_name",
                table: "countries",
                newName: "IX_countries_Name");
        }
    }
}
