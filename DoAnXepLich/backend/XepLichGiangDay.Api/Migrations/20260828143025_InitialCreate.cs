using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XepLichGiangDay.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bo_mon",
                columns: table => new
                {
                    ma_bo_mon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten_bo_mon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ten_khoa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bo_mon", x => x.ma_bo_mon);
                });

            migrationBuilder.CreateTable(
                name: "nam_hoc",
                columns: table => new
                {
                    ma_nam_hoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten_nam_hoc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ngay_bat_dau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_ket_thuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    trang_thai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nam_hoc", x => x.ma_nam_hoc);
                });

            migrationBuilder.CreateTable(
                name: "giang_vien",
                columns: table => new
                {
                    ma_giang_vien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ho_ten = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    so_dien_thoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    chuc_danh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ma_bo_mon = table.Column<int>(type: "int", nullable: true),
                    trang_thai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_giang_vien", x => x.ma_giang_vien);
                    table.ForeignKey(
                        name: "FK_giang_vien_bo_mon_ma_bo_mon",
                        column: x => x.ma_bo_mon,
                        principalTable: "bo_mon",
                        principalColumn: "ma_bo_mon",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "hoc_phan",
                columns: table => new
                {
                    ma_hoc_phan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten_hoc_phan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    so_tin_chi = table.Column<int>(type: "int", nullable: false),
                    ma_bo_mon = table.Column<int>(type: "int", nullable: true),
                    trang_thai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hoc_phan", x => x.ma_hoc_phan);
                    table.ForeignKey(
                        name: "FK_hoc_phan_bo_mon_ma_bo_mon",
                        column: x => x.ma_bo_mon,
                        principalTable: "bo_mon",
                        principalColumn: "ma_bo_mon",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "hoc_ky",
                columns: table => new
                {
                    ma_hoc_ky = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_nam_hoc = table.Column<int>(type: "int", nullable: false),
                    ten_hoc_ky = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ngay_bat_dau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_ket_thuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    trang_thai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hoc_ky", x => x.ma_hoc_ky);
                    table.ForeignKey(
                        name: "FK_hoc_ky_nam_hoc_ma_nam_hoc",
                        column: x => x.ma_nam_hoc,
                        principalTable: "nam_hoc",
                        principalColumn: "ma_nam_hoc",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "nang_luc_giang_day",
                columns: table => new
                {
                    ma_nang_luc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_giang_vien = table.Column<int>(type: "int", nullable: false),
                    ma_hoc_phan = table.Column<int>(type: "int", nullable: false),
                    muc_do_chuyen_mon = table.Column<int>(type: "int", nullable: false),
                    trang_thai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nang_luc_giang_day", x => x.ma_nang_luc);
                    table.ForeignKey(
                        name: "FK_nang_luc_giang_day_giang_vien_ma_giang_vien",
                        column: x => x.ma_giang_vien,
                        principalTable: "giang_vien",
                        principalColumn: "ma_giang_vien",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_nang_luc_giang_day_hoc_phan_ma_hoc_phan",
                        column: x => x.ma_hoc_phan,
                        principalTable: "hoc_phan",
                        principalColumn: "ma_hoc_phan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dinh_muc_giang_vien",
                columns: table => new
                {
                    ma_dinh_muc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_giang_vien = table.Column<int>(type: "int", nullable: false),
                    ma_hoc_ky = table.Column<int>(type: "int", nullable: false),
                    so_tiet_toi_thieu = table.Column<int>(type: "int", nullable: false),
                    so_tiet_toi_da = table.Column<int>(type: "int", nullable: false),
                    trang_thai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dinh_muc_giang_vien", x => x.ma_dinh_muc);
                    table.ForeignKey(
                        name: "FK_dinh_muc_giang_vien_giang_vien_ma_giang_vien",
                        column: x => x.ma_giang_vien,
                        principalTable: "giang_vien",
                        principalColumn: "ma_giang_vien",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dinh_muc_giang_vien_hoc_ky_ma_hoc_ky",
                        column: x => x.ma_hoc_ky,
                        principalTable: "hoc_ky",
                        principalColumn: "ma_hoc_ky",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "lich_ban_giang_vien",
                columns: table => new
                {
                    ma_lich_ban = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_giang_vien = table.Column<int>(type: "int", nullable: false),
                    ma_hoc_ky = table.Column<int>(type: "int", nullable: false),
                    thu = table.Column<int>(type: "int", nullable: false),
                    tiet_bat_dau = table.Column<int>(type: "int", nullable: false),
                    tiet_ket_thuc = table.Column<int>(type: "int", nullable: false),
                    tu_ngay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    den_ngay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ly_do = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lich_ban_giang_vien", x => x.ma_lich_ban);
                    table.ForeignKey(
                        name: "FK_lich_ban_giang_vien_giang_vien_ma_giang_vien",
                        column: x => x.ma_giang_vien,
                        principalTable: "giang_vien",
                        principalColumn: "ma_giang_vien",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lich_ban_giang_vien_hoc_ky_ma_hoc_ky",
                        column: x => x.ma_hoc_ky,
                        principalTable: "hoc_ky",
                        principalColumn: "ma_hoc_ky",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "lop_hoc_phan",
                columns: table => new
                {
                    ma_lop_hoc_phan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_lop_hoc_phan_truong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ma_hoc_phan = table.Column<int>(type: "int", nullable: false),
                    ma_hoc_ky = table.Column<int>(type: "int", nullable: false),
                    ten_lop = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    so_luong_sinh_vien = table.Column<int>(type: "int", nullable: false),
                    trang_thai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lop_hoc_phan", x => x.ma_lop_hoc_phan);
                    table.ForeignKey(
                        name: "FK_lop_hoc_phan_hoc_ky_ma_hoc_ky",
                        column: x => x.ma_hoc_ky,
                        principalTable: "hoc_ky",
                        principalColumn: "ma_hoc_ky",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_lop_hoc_phan_hoc_phan_ma_hoc_phan",
                        column: x => x.ma_hoc_phan,
                        principalTable: "hoc_phan",
                        principalColumn: "ma_hoc_phan",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "nguyen_vong_giang_vien",
                columns: table => new
                {
                    ma_nguyen_vong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_giang_vien = table.Column<int>(type: "int", nullable: false),
                    ma_hoc_ky = table.Column<int>(type: "int", nullable: false),
                    loai_nguyen_vong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    muc_do = table.Column<int>(type: "int", nullable: false),
                    thu = table.Column<int>(type: "int", nullable: true),
                    tiet_bat_dau = table.Column<int>(type: "int", nullable: true),
                    tiet_ket_thuc = table.Column<int>(type: "int", nullable: true),
                    trong_so_phat = table.Column<double>(type: "float", nullable: false),
                    noi_dung_goc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nguyen_vong_giang_vien", x => x.ma_nguyen_vong);
                    table.ForeignKey(
                        name: "FK_nguyen_vong_giang_vien_giang_vien_ma_giang_vien",
                        column: x => x.ma_giang_vien,
                        principalTable: "giang_vien",
                        principalColumn: "ma_giang_vien",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_nguyen_vong_giang_vien_hoc_ky_ma_hoc_ky",
                        column: x => x.ma_hoc_ky,
                        principalTable: "hoc_ky",
                        principalColumn: "ma_hoc_ky",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "rang_buoc_hoc_ky",
                columns: table => new
                {
                    ma_rang_buoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_hoc_ky = table.Column<int>(type: "int", nullable: false),
                    ten_rang_buoc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    loai_rang_buoc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    trong_so_phat = table.Column<double>(type: "float", nullable: false),
                    mo_ta = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rang_buoc_hoc_ky", x => x.ma_rang_buoc);
                    table.ForeignKey(
                        name: "FK_rang_buoc_hoc_ky_hoc_ky_ma_hoc_ky",
                        column: x => x.ma_hoc_ky,
                        principalTable: "hoc_ky",
                        principalColumn: "ma_hoc_ky",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chi_tiet_thoi_khoa_bieu",
                columns: table => new
                {
                    ma_chi_tiet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_lop_hoc_phan = table.Column<int>(type: "int", nullable: false),
                    thu = table.Column<int>(type: "int", nullable: false),
                    tiet_bat_dau = table.Column<int>(type: "int", nullable: false),
                    tiet_ket_thuc = table.Column<int>(type: "int", nullable: false),
                    phong_hoc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tu_tuan = table.Column<int>(type: "int", nullable: false),
                    den_tuan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chi_tiet_thoi_khoa_bieu", x => x.ma_chi_tiet);
                    table.ForeignKey(
                        name: "FK_chi_tiet_thoi_khoa_bieu_lop_hoc_phan_ma_lop_hoc_phan",
                        column: x => x.ma_lop_hoc_phan,
                        principalTable: "lop_hoc_phan",
                        principalColumn: "ma_lop_hoc_phan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ket_qua_xep_lich",
                columns: table => new
                {
                    ma_ket_qua = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_hoc_ky = table.Column<int>(type: "int", nullable: false),
                    ma_lop_hoc_phan = table.Column<int>(type: "int", nullable: false),
                    ma_giang_vien = table.Column<int>(type: "int", nullable: false),
                    diem_phat = table.Column<double>(type: "float", nullable: false),
                    ghi_chu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ket_qua_xep_lich", x => x.ma_ket_qua);
                    table.ForeignKey(
                        name: "FK_ket_qua_xep_lich_giang_vien_ma_giang_vien",
                        column: x => x.ma_giang_vien,
                        principalTable: "giang_vien",
                        principalColumn: "ma_giang_vien",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ket_qua_xep_lich_hoc_ky_ma_hoc_ky",
                        column: x => x.ma_hoc_ky,
                        principalTable: "hoc_ky",
                        principalColumn: "ma_hoc_ky",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ket_qua_xep_lich_lop_hoc_phan_ma_lop_hoc_phan",
                        column: x => x.ma_lop_hoc_phan,
                        principalTable: "lop_hoc_phan",
                        principalColumn: "ma_lop_hoc_phan",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "phan_cong_giang_day",
                columns: table => new
                {
                    ma_phan_cong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_lop_hoc_phan = table.Column<int>(type: "int", nullable: false),
                    ma_giang_vien = table.Column<int>(type: "int", nullable: false),
                    ma_hoc_ky = table.Column<int>(type: "int", nullable: false),
                    nguon_phan_cong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    diem_phat = table.Column<double>(type: "float", nullable: false),
                    trang_thai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phan_cong_giang_day", x => x.ma_phan_cong);
                    table.ForeignKey(
                        name: "FK_phan_cong_giang_day_giang_vien_ma_giang_vien",
                        column: x => x.ma_giang_vien,
                        principalTable: "giang_vien",
                        principalColumn: "ma_giang_vien",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_phan_cong_giang_day_hoc_ky_ma_hoc_ky",
                        column: x => x.ma_hoc_ky,
                        principalTable: "hoc_ky",
                        principalColumn: "ma_hoc_ky",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_phan_cong_giang_day_lop_hoc_phan_ma_lop_hoc_phan",
                        column: x => x.ma_lop_hoc_phan,
                        principalTable: "lop_hoc_phan",
                        principalColumn: "ma_lop_hoc_phan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_chi_tiet_thoi_khoa_bieu_ma_lop_hoc_phan",
                table: "chi_tiet_thoi_khoa_bieu",
                column: "ma_lop_hoc_phan");

            migrationBuilder.CreateIndex(
                name: "IX_dinh_muc_giang_vien_ma_giang_vien",
                table: "dinh_muc_giang_vien",
                column: "ma_giang_vien");

            migrationBuilder.CreateIndex(
                name: "IX_dinh_muc_giang_vien_ma_hoc_ky",
                table: "dinh_muc_giang_vien",
                column: "ma_hoc_ky");

            migrationBuilder.CreateIndex(
                name: "IX_giang_vien_ma_bo_mon",
                table: "giang_vien",
                column: "ma_bo_mon");

            migrationBuilder.CreateIndex(
                name: "IX_hoc_ky_ma_nam_hoc",
                table: "hoc_ky",
                column: "ma_nam_hoc");

            migrationBuilder.CreateIndex(
                name: "IX_hoc_phan_ma_bo_mon",
                table: "hoc_phan",
                column: "ma_bo_mon");

            migrationBuilder.CreateIndex(
                name: "IX_ket_qua_xep_lich_ma_giang_vien",
                table: "ket_qua_xep_lich",
                column: "ma_giang_vien");

            migrationBuilder.CreateIndex(
                name: "IX_ket_qua_xep_lich_ma_hoc_ky",
                table: "ket_qua_xep_lich",
                column: "ma_hoc_ky");

            migrationBuilder.CreateIndex(
                name: "IX_ket_qua_xep_lich_ma_lop_hoc_phan",
                table: "ket_qua_xep_lich",
                column: "ma_lop_hoc_phan");

            migrationBuilder.CreateIndex(
                name: "IX_lich_ban_giang_vien_ma_giang_vien",
                table: "lich_ban_giang_vien",
                column: "ma_giang_vien");

            migrationBuilder.CreateIndex(
                name: "IX_lich_ban_giang_vien_ma_hoc_ky",
                table: "lich_ban_giang_vien",
                column: "ma_hoc_ky");

            migrationBuilder.CreateIndex(
                name: "IX_lop_hoc_phan_ma_hoc_ky",
                table: "lop_hoc_phan",
                column: "ma_hoc_ky");

            migrationBuilder.CreateIndex(
                name: "IX_lop_hoc_phan_ma_hoc_phan",
                table: "lop_hoc_phan",
                column: "ma_hoc_phan");

            migrationBuilder.CreateIndex(
                name: "IX_nang_luc_giang_day_ma_giang_vien",
                table: "nang_luc_giang_day",
                column: "ma_giang_vien");

            migrationBuilder.CreateIndex(
                name: "IX_nang_luc_giang_day_ma_hoc_phan",
                table: "nang_luc_giang_day",
                column: "ma_hoc_phan");

            migrationBuilder.CreateIndex(
                name: "IX_nguyen_vong_giang_vien_ma_giang_vien",
                table: "nguyen_vong_giang_vien",
                column: "ma_giang_vien");

            migrationBuilder.CreateIndex(
                name: "IX_nguyen_vong_giang_vien_ma_hoc_ky",
                table: "nguyen_vong_giang_vien",
                column: "ma_hoc_ky");

            migrationBuilder.CreateIndex(
                name: "IX_phan_cong_giang_day_ma_giang_vien",
                table: "phan_cong_giang_day",
                column: "ma_giang_vien");

            migrationBuilder.CreateIndex(
                name: "IX_phan_cong_giang_day_ma_hoc_ky",
                table: "phan_cong_giang_day",
                column: "ma_hoc_ky");

            migrationBuilder.CreateIndex(
                name: "IX_phan_cong_giang_day_ma_lop_hoc_phan",
                table: "phan_cong_giang_day",
                column: "ma_lop_hoc_phan");

            migrationBuilder.CreateIndex(
                name: "IX_rang_buoc_hoc_ky_ma_hoc_ky",
                table: "rang_buoc_hoc_ky",
                column: "ma_hoc_ky");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chi_tiet_thoi_khoa_bieu");

            migrationBuilder.DropTable(
                name: "dinh_muc_giang_vien");

            migrationBuilder.DropTable(
                name: "ket_qua_xep_lich");

            migrationBuilder.DropTable(
                name: "lich_ban_giang_vien");

            migrationBuilder.DropTable(
                name: "nang_luc_giang_day");

            migrationBuilder.DropTable(
                name: "nguyen_vong_giang_vien");

            migrationBuilder.DropTable(
                name: "phan_cong_giang_day");

            migrationBuilder.DropTable(
                name: "rang_buoc_hoc_ky");

            migrationBuilder.DropTable(
                name: "giang_vien");

            migrationBuilder.DropTable(
                name: "lop_hoc_phan");

            migrationBuilder.DropTable(
                name: "hoc_ky");

            migrationBuilder.DropTable(
                name: "hoc_phan");

            migrationBuilder.DropTable(
                name: "nam_hoc");

            migrationBuilder.DropTable(
                name: "bo_mon");
        }
    }
}
