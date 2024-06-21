using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class testsToProblem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tests_ProblemId",
                schema: "omiiasi",
                table: "Tests",
                column: "ProblemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Problems_ProblemId",
                schema: "omiiasi",
                table: "Tests",
                column: "ProblemId",
                principalSchema: "omiiasi",
                principalTable: "Problems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Problems_ProblemId",
                schema: "omiiasi",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_ProblemId",
                schema: "omiiasi",
                table: "Tests");
        }
    }
}
