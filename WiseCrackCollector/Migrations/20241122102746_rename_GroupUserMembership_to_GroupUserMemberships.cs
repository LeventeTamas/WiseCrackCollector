using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WiseCrackCollector.Migrations
{
    /// <inheritdoc />
    public partial class rename_GroupUserMembership_to_GroupUserMemberships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupUserMembership");

            migrationBuilder.CreateTable(
                name: "GroupUserMemberships",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    GroupId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Read = table.Column<bool>(type: "bit", nullable: false),
                    Add = table.Column<bool>(type: "bit", nullable: false),
                    Update = table.Column<bool>(type: "bit", nullable: false),
                    Delete = table.Column<bool>(type: "bit", nullable: false),
                    ManageMembers = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUserMemberships", x => new { x.UserId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_GroupUserMemberships_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupUserMemberships_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupUserMemberships_GroupId",
                table: "GroupUserMemberships",
                column: "GroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupUserMemberships");

            migrationBuilder.CreateTable(
                name: "GroupUserMembership",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    GroupId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Add = table.Column<bool>(type: "bit", nullable: false),
                    Delete = table.Column<bool>(type: "bit", nullable: false),
                    ManageMembers = table.Column<bool>(type: "bit", nullable: false),
                    Read = table.Column<bool>(type: "bit", nullable: false),
                    Update = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUserMembership", x => new { x.UserId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_GroupUserMembership_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupUserMembership_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupUserMembership_GroupId",
                table: "GroupUserMembership",
                column: "GroupId");
        }
    }
}
