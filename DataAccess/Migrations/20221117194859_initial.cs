using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ESOEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Network = table.Column<string>(type: "text", nullable: false),
                    Obt_name = table.Column<string>(type: "text", nullable: false),
                    Obj_gv_type = table.Column<string>(type: "text", nullable: false),
                    Obj_number = table.Column<int>(type: "integer", nullable: true),
                    P_plus = table.Column<decimal>(type: "numeric", nullable: true),
                    PL_T = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    P_minus = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESOEntities", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ESOEntities");
        }
    }
}
