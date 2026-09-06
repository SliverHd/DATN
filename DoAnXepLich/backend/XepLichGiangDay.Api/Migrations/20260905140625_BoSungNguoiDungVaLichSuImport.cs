using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XepLichGiangDay.Api.Migrations
{
    /// <inheritdoc />
    public partial class BoSungNguoiDungVaLichSuImport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "lich_su_import",
                columns: table => new
                {
                    ma_lich_su = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_hoc_ky = table.Column<int>(type: "int", nullable: false),
                    ten_file = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ngay_import = table.Column<DateTime>(type: "datetime2", nullable: false),
                    tong_so_dong = table.Column<int>(type: "int", nullable: false),
                    so_dong_thanh_cong = table.Column<int>(type: "int", nullable: false),
                    so_dong_loi = table.Column<int>(type: "int", nullable: false),
                    trang_thai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ghi_chu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lich_su_import", x => x.ma_lich_su);
                    table.ForeignKey(
                        name: "FK_lich_su_import_hoc_ky_ma_hoc_ky",
                        column: x => x.ma_hoc_ky,
                        principalTable: "hoc_ky",
                        principalColumn: "ma_hoc_ky",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "nguoi_dung",
                columns: table => new
                {
                    ma_nguoi_dung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten_dang_nhap = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    mat_khau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ho_ten = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vai_tro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ma_giang_vien = table.Column<int>(type: "int", nullable: true),
                    trang_thai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nguoi_dung", x => x.ma_nguoi_dung);
                    table.ForeignKey(
                        name: "FK_nguoi_dung_giang_vien_ma_giang_vien",
                        column: x => x.ma_giang_vien,
                        principalTable: "giang_vien",
                        principalColumn: "ma_giang_vien",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_lich_su_import_ma_hoc_ky",
                table: "lich_su_import",
                column: "ma_hoc_ky");

            migrationBuilder.CreateIndex(
                name: "IX_nguoi_dung_ma_giang_vien",
                table: "nguoi_dung",
                column: "ma_giang_vien");

            migrationBuilder.CreateIndex(
                name: "IX_nguoi_dung_ten_dang_nhap",
                table: "nguoi_dung",
                column: "ten_dang_nhap",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lich_su_import");

            migrationBuilder.DropTable(
                name: "nguoi_dung");
        }
    }
}
