using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class additemmmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Itemms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isDelete = table.Column<bool>(nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(nullable: false),
                    UpdateDate = table.Column<DateTimeOffset>(nullable: true),
                    DeleteDate = table.Column<DateTimeOffset>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    stock = table.Column<int>(nullable: false),
                    price = table.Column<int>(nullable: false),
                    SuppId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itemms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Itemms_Supps_SuppId",
                        column: x => x.SuppId,
                        principalTable: "Supps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Itemms_SuppId",
                table: "Itemms",
                column: "SuppId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Itemms");
        }
    }
}
