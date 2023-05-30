using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryInventory.Migrations
{
    public partial class InitializeDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "autores",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                              .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    apellidos = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_autores", author => author.id);
                });

            migrationBuilder.CreateTable(
                name: "editoriales",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                              .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    sede = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_editoriales", publisher => publisher.id);
                });

            migrationBuilder.CreateTable(
                name: "libros",
                columns: table => new
                {
                    ISBN = table.Column<int>(type: "int", nullable: false),
                    editoriales_id = table.Column<int>(type: "int", nullable: false),
                    titulo = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    sinopsis = table.Column<string>(type: "ntext", nullable: true),
                    n_paginas = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_libros", book => book.ISBN);
                    table.ForeignKey(
                        name: "FK_libros_editoriales_editoriales_id",
                        column: x => x.editoriales_id,
                        principalTable: "editoriales",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "autores_has_libros",
                columns: table => new
                {
                    autores_id = table.Column<int>(type: "int", nullable: false),
                    libros_ISBN = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_autores_has_libros", x => new { x.autores_id, x.libros_ISBN });
                    table.ForeignKey(
                        name: "FK_autores_has_libros_autores_autores_id",
                        column: author_book => author_book.autores_id,
                        principalTable: "autores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_autores_has_libros_libros_libros_ISBN",
                        column: author_book => author_book.libros_ISBN,
                        principalTable: "libros",
                        principalColumn: "ISBN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_autores_has_libros_libros_ISBN",
                table: "autores_has_libros",
                column: "libros_ISBN");

            migrationBuilder.CreateIndex(
                name: "IX_libros_editoriales_id",
                table: "libros",
                column: "editoriales_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "autores_has_libros");

            migrationBuilder.DropTable(name: "autores");

            migrationBuilder.DropTable(name: "libros");

            migrationBuilder.DropTable(name: "editoriales");
        }
    }
}