using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WiseCrackCollector.Migrations
{
    /// <inheritdoc />
    public partial class rename_UserGroupPermission_to_GroupUserMembership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGroupPermissions_AspNetUsers_UserId",
                table: "UserGroupPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroupPermissions_Groups_GroupId",
                table: "UserGroupPermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGroupPermissions",
                table: "UserGroupPermissions");

            migrationBuilder.RenameTable(
                name: "UserGroupPermissions",
                newName: "GroupUserMembership");

            migrationBuilder.RenameIndex(
                name: "IX_UserGroupPermissions_GroupId",
                table: "GroupUserMembership",
                newName: "IX_GroupUserMembership_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupUserMembership",
                table: "GroupUserMembership",
                columns: new[] { "UserId", "GroupId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GroupUserMembership_AspNetUsers_UserId",
                table: "GroupUserMembership",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupUserMembership_Groups_GroupId",
                table: "GroupUserMembership",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupUserMembership_AspNetUsers_UserId",
                table: "GroupUserMembership");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupUserMembership_Groups_GroupId",
                table: "GroupUserMembership");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupUserMembership",
                table: "GroupUserMembership");

            migrationBuilder.RenameTable(
                name: "GroupUserMembership",
                newName: "UserGroupPermissions");

            migrationBuilder.RenameIndex(
                name: "IX_GroupUserMembership_GroupId",
                table: "UserGroupPermissions",
                newName: "IX_UserGroupPermissions_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGroupPermissions",
                table: "UserGroupPermissions",
                columns: new[] { "UserId", "GroupId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroupPermissions_AspNetUsers_UserId",
                table: "UserGroupPermissions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroupPermissions_Groups_GroupId",
                table: "UserGroupPermissions",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
