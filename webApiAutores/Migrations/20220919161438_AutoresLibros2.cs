using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webApiAutores.Migrations
{
    public partial class AutoresLibros2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutoresLibros_Libros_Libroid",
                table: "AutoresLibros");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AutoresLibros",
                table: "AutoresLibros");

            migrationBuilder.DropColumn(
                name: "LiborId",
                table: "AutoresLibros");

            migrationBuilder.RenameColumn(
                name: "Libroid",
                table: "AutoresLibros",
                newName: "LibroId");

            migrationBuilder.RenameIndex(
                name: "IX_AutoresLibros_Libroid",
                table: "AutoresLibros",
                newName: "IX_AutoresLibros_LibroId");

            migrationBuilder.AlterColumn<int>(
                name: "LibroId",
                table: "AutoresLibros",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AutoresLibros",
                table: "AutoresLibros",
                columns: new[] { "AutorId", "LibroId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AutoresLibros_Libros_LibroId",
                table: "AutoresLibros",
                column: "LibroId",
                principalTable: "Libros",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutoresLibros_Libros_LibroId",
                table: "AutoresLibros");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AutoresLibros",
                table: "AutoresLibros");

            migrationBuilder.RenameColumn(
                name: "LibroId",
                table: "AutoresLibros",
                newName: "Libroid");

            migrationBuilder.RenameIndex(
                name: "IX_AutoresLibros_LibroId",
                table: "AutoresLibros",
                newName: "IX_AutoresLibros_Libroid");

            migrationBuilder.AlterColumn<int>(
                name: "Libroid",
                table: "AutoresLibros",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "LiborId",
                table: "AutoresLibros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AutoresLibros",
                table: "AutoresLibros",
                columns: new[] { "AutorId", "LiborId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AutoresLibros_Libros_Libroid",
                table: "AutoresLibros",
                column: "Libroid",
                principalTable: "Libros",
                principalColumn: "id");
        }
    }
}
