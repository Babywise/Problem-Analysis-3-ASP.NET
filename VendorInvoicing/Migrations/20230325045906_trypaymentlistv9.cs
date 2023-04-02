using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendorInvoicing.Migrations
{
    /// <inheritdoc />
    public partial class trypaymentlistv9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoicePaymentTerms");

            migrationBuilder.AddColumn<int>(
                name: "InvoiceId",
                table: "PaymentTerms",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PaymentTerms",
                keyColumn: "PaymentTermsId",
                keyValue: 1,
                column: "InvoiceId",
                value: null);

            migrationBuilder.UpdateData(
                table: "PaymentTerms",
                keyColumn: "PaymentTermsId",
                keyValue: 2,
                column: "InvoiceId",
                value: null);

            migrationBuilder.UpdateData(
                table: "PaymentTerms",
                keyColumn: "PaymentTermsId",
                keyValue: 3,
                column: "InvoiceId",
                value: null);

            migrationBuilder.UpdateData(
                table: "PaymentTerms",
                keyColumn: "PaymentTermsId",
                keyValue: 4,
                column: "InvoiceId",
                value: null);

            migrationBuilder.UpdateData(
                table: "PaymentTerms",
                keyColumn: "PaymentTermsId",
                keyValue: 5,
                column: "InvoiceId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTerms_InvoiceId",
                table: "PaymentTerms",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTerms_Invoices_InvoiceId",
                table: "PaymentTerms",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "InvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTerms_Invoices_InvoiceId",
                table: "PaymentTerms");

            migrationBuilder.DropIndex(
                name: "IX_PaymentTerms_InvoiceId",
                table: "PaymentTerms");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "PaymentTerms");

            migrationBuilder.CreateTable(
                name: "InvoicePaymentTerms",
                columns: table => new
                {
                    InvoicesInvoiceId = table.Column<int>(type: "int", nullable: false),
                    PaymentTermsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoicePaymentTerms", x => new { x.InvoicesInvoiceId, x.PaymentTermsId });
                    table.ForeignKey(
                        name: "FK_InvoicePaymentTerms_Invoices_InvoicesInvoiceId",
                        column: x => x.InvoicesInvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoicePaymentTerms_PaymentTerms_PaymentTermsId",
                        column: x => x.PaymentTermsId,
                        principalTable: "PaymentTerms",
                        principalColumn: "PaymentTermsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePaymentTerms_PaymentTermsId",
                table: "InvoicePaymentTerms",
                column: "PaymentTermsId");
        }
    }
}
