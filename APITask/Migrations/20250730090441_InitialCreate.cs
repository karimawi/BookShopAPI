using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace APITask.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "MasterSchema");

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "MasterSchema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CatOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "MasterSchema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Author = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BookPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "MasterSchema",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "MasterSchema",
                table: "Categories",
                columns: new[] { "Id", "CatName", "CatOrder" },
                values: new object[,]
                {
                    { 1, "Fiction", 1 },
                    { 2, "Science", 2 },
                    { 3, "Technology", 3 },
                    { 4, "Biography", 4 },
                    { 5, "History", 5 }
                });

            migrationBuilder.InsertData(
                schema: "MasterSchema",
                table: "Products",
                columns: new[] { "Id", "Author", "BookPrice", "CategoryId", "Description", "Title" },
                values: new object[,]
                {
                    { 1, "John Smith", 29.99m, 1, "An exciting fiction novel", "The Great Adventure" },
                    { 2, "Dr. Sarah Wilson", 45.50m, 2, "Basic principles of physics", "Physics Fundamentals" },
                    { 3, "Mike Johnson", 55.00m, 3, "Complete guide to web technologies", "Modern Web Development" },
                    { 4, "Walter Isaacson", 35.99m, 4, "Life story of Apple founder", "Steve Jobs Biography" },
                    { 5, "Robert Miller", 42.75m, 5, "Comprehensive history of WWII", "World War II Chronicles" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CatName",
                schema: "MasterSchema",
                table: "Categories",
                column: "CatName");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                schema: "MasterSchema",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products",
                schema: "MasterSchema");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "MasterSchema");
        }
    }
}
