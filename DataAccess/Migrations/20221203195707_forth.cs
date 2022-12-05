using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class forth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskEntities");

            migrationBuilder.RenameColumn(
                name: "P_plus",
                table: "ESOEntities",
                newName: "PPlus");

            migrationBuilder.RenameColumn(
                name: "P_minus",
                table: "ESOEntities",
                newName: "PMinus");

            migrationBuilder.RenameColumn(
                name: "PL_T",
                table: "ESOEntities",
                newName: "PlT");

            migrationBuilder.RenameColumn(
                name: "Obt_name",
                table: "ESOEntities",
                newName: "ObtName");

            migrationBuilder.RenameColumn(
                name: "Obj_number",
                table: "ESOEntities",
                newName: "ObjNumber");

            migrationBuilder.RenameColumn(
                name: "Obj_gv_type",
                table: "ESOEntities",
                newName: "ObjGvType");

            migrationBuilder.AlterColumn<string>(
                name: "Network",
                table: "ESOEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlT",
                table: "ESOEntities",
                newName: "PL_T");

            migrationBuilder.RenameColumn(
                name: "PPlus",
                table: "ESOEntities",
                newName: "P_plus");

            migrationBuilder.RenameColumn(
                name: "PMinus",
                table: "ESOEntities",
                newName: "P_minus");

            migrationBuilder.RenameColumn(
                name: "ObtName",
                table: "ESOEntities",
                newName: "Obt_name");

            migrationBuilder.RenameColumn(
                name: "ObjNumber",
                table: "ESOEntities",
                newName: "Obj_number");

            migrationBuilder.RenameColumn(
                name: "ObjGvType",
                table: "ESOEntities",
                newName: "Obj_gv_type");

            migrationBuilder.AlterColumn<string>(
                name: "Network",
                table: "ESOEntities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TaskEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRunning = table.Column<bool>(type: "boolean", nullable: false),
                    TaskName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskEntities", x => x.Id);
                });
        }
    }
}
