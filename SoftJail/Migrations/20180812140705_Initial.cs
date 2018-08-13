using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftJail.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cell",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CellNumber = table.Column<int>(nullable: false),
                    HasWindow = table.Column<bool>(nullable: false),
                    DepartmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cell", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cell_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Officer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(maxLength: 30, nullable: false),
                    Salary = table.Column<decimal>(nullable: false),
                    Position = table.Column<int>(nullable: false),
                    Weapon = table.Column<int>(nullable: false),
                    DepartmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Officer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Officer_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prisoner",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(maxLength: 20, nullable: false),
                    Nickname = table.Column<string>(nullable: false),
                    Age = table.Column<int>(nullable: false),
                    IncarcerationDate = table.Column<int>(nullable: false),
                    ReleaseDate = table.Column<int>(nullable: false),
                    Bail = table.Column<decimal>(nullable: false),
                    CellId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prisoner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prisoner_Cell_CellId",
                        column: x => x.CellId,
                        principalTable: "Cell",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: false),
                    Sender = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    PrisonerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mail_Prisoner_PrisonerId",
                        column: x => x.PrisonerId,
                        principalTable: "Prisoner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfficerPrisoner",
                columns: table => new
                {
                    PrisonerId = table.Column<int>(nullable: false),
                    OfficerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficerPrisoner", x => new { x.OfficerId, x.PrisonerId });
                    table.ForeignKey(
                        name: "FK_OfficerPrisoner_Officer_OfficerId",
                        column: x => x.OfficerId,
                        principalTable: "Officer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfficerPrisoner_Prisoner_PrisonerId",
                        column: x => x.PrisonerId,
                        principalTable: "Prisoner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cell_DepartmentId",
                table: "Cell",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Mail_PrisonerId",
                table: "Mail",
                column: "PrisonerId");

            migrationBuilder.CreateIndex(
                name: "IX_Officer_DepartmentId",
                table: "Officer",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficerPrisoner_PrisonerId",
                table: "OfficerPrisoner",
                column: "PrisonerId");

            migrationBuilder.CreateIndex(
                name: "IX_Prisoner_CellId",
                table: "Prisoner",
                column: "CellId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mail");

            migrationBuilder.DropTable(
                name: "OfficerPrisoner");

            migrationBuilder.DropTable(
                name: "Officer");

            migrationBuilder.DropTable(
                name: "Prisoner");

            migrationBuilder.DropTable(
                name: "Cell");

            migrationBuilder.DropTable(
                name: "Department");
        }
    }
}
