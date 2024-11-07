using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WiseCrackCollector.Migrations
{
    /// <inheritdoc />
    public partial class create_permission_managemembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ManageMembers",
                table: "UserGroupPermissions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManageMembers",
                table: "UserGroupPermissions");
        }
    }
}
