using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EmpolyeeTrailMonitor.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmpolyeeTrail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GPSX = table.Column<float>(nullable: true),
                    GPSY = table.Column<float>(nullable: true),
                    BmapLap = table.Column<float>(nullable: true),
                    BmapLng = table.Column<float>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    CreateUser = table.Column<string>(nullable: true),
                    IsCar = table.Column<bool>(nullable: true),
                    Distance = table.Column<int>(nullable: true),
                    DistanceSecond = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpolyeeTrail", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmpolyeeTrail");
        }
    }
}
