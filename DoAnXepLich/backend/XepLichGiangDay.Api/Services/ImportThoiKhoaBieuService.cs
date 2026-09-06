using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using XepLichGiangDay.Api.Data;
using XepLichGiangDay.Api.DTOs.Import;
using XepLichGiangDay.Api.Models.Entities;

namespace XepLichGiangDay.Api.Services;

public class ImportThoiKhoaBieuService(AppDbContext context)
{
    public async Task<KetQuaPreviewImportDto> PreviewAsync(IFormFile file, int maHocKy, CancellationToken cancellationToken = default)
    {
        // 1. Kiểm tra học kỳ tồn tại
        var hocKyTonTai = await context.HocKy.AnyAsync(x => x.MaHocKy == maHocKy, cancellationToken);
        if (!hocKyTonTai)
        {
            throw new ArgumentException("Hoc ky khong ton tai trong he thong.");
        }

        // 2. Kiểm tra phần mở rộng file
        var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
        if (ext != ".xlsx" && ext != ".xls")
        {
            throw new ArgumentException("Dinh dang file khong hop le. Chi chap nhan file Excel (.xlsx, .xls).");
        }

        var ketQua = new KetQuaPreviewImportDto();
        var dsHocPhan = await context.HocPhan.AsNoTracking().ToListAsync(cancellationToken);

        try
        {
            using var stream = file.OpenReadStream();
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.FirstOrDefault();

            if (worksheet == null)
            {
                throw new ArgumentException("File Excel khong chua trang tinh (worksheet) hop le.");
            }

            var rows = worksheet.RowsUsed().Skip(1).ToList(); // Bỏ qua header
            if (rows.Count == 0)
            {
                throw new ArgumentException("File Excel khong chua du lieu lop hoc phan sau dong tieu de.");
            }

            int stt = 1;

            foreach (var row in rows)
            {
                var dto = new DongThoiKhoaBieuImportDto
                {
                    SoThuTu = stt++,
                    MaLopHocPhanTruong = row.Cell(1).GetString().Trim(),
                    TenLop = row.Cell(2).GetString().Trim(),
                    MaHocPhanTruong = row.Cell(3).GetString().Trim(),
                    TenHocPhan = row.Cell(4).GetString().Trim(),
                    SoLuongSinhVien = row.Cell(5).TryGetValue<int>(out var sl) ? sl : 40,
                    Thu = row.Cell(6).TryGetValue<int>(out var thu) ? thu : 0,
                    TietBatDau = row.Cell(7).TryGetValue<int>(out var tbd) ? tbd : 0,
                    TietKetThuc = row.Cell(8).TryGetValue<int>(out var tkt) ? tkt : 0,
                    PhongHoc = row.Cell(9).GetString().Trim(),
                    TuTuan = row.Cell(10).TryGetValue<int>(out var tt) ? tt : 1,
                    DenTuan = row.Cell(11).TryGetValue<int>(out var dt) ? dt : 15,
                };

                // Kiểm tra mã lớp học phần
                if (string.IsNullOrWhiteSpace(dto.MaLopHocPhanTruong))
                {
                    dto.DanhSachLoi.Add("Ma lop hoc phan khong duoc de trong.");
                }

                // Kiểm tra thứ trong tuần
                if (dto.Thu < 2 || dto.Thu > 8)
                {
                    dto.DanhSachLoi.Add("Thu phai tu 2 den 8 (8 la Chu nhat).");
                }

                // Kiểm tra tiết bắt đầu & kết thúc
                if (dto.TietBatDau < 1 || dto.TietBatDau > 12)
                {
                    dto.DanhSachLoi.Add("Tiet bat dau phai tu 1 den 12.");
                }

                if (dto.TietKetThuc < dto.TietBatDau || dto.TietKetThuc > 12)
                {
                    dto.DanhSachLoi.Add("Tiet ket thuc phai tu tiet bat dau den 12.");
                }

                // Kiểm tra tuần học
                if (dto.TuTuan < 1 || dto.TuTuan > 53)
                {
                    dto.DanhSachLoi.Add("Tu tuan phai tu 1 den 53.");
                }

                if (dto.DenTuan < dto.TuTuan || dto.DenTuan > 53)
                {
                    dto.DanhSachLoi.Add("Den tuan phai lon hon hoac bang tu tuan va toi da 53.");
                }

                // Kiểm tra số lượng sinh viên
                if (dto.SoLuongSinhVien < 0 || dto.SoLuongSinhVien > 500)
                {
                    dto.DanhSachLoi.Add("So luong sinh vien phai tu 0 den 500.");
                }

                // Khớp học phần trong hệ thống
                int.TryParse(dto.MaHocPhanTruong, out int maHpInt);
                var hp = dsHocPhan.FirstOrDefault(x =>
                    (maHpInt > 0 && x.MaHocPhan == maHpInt) ||
                    (!string.IsNullOrWhiteSpace(dto.TenHocPhan) && x.TenHocPhan.Equals(dto.TenHocPhan, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(dto.MaHocPhanTruong) && x.TenHocPhan.Equals(dto.MaHocPhanTruong, StringComparison.OrdinalIgnoreCase)));

                if (hp != null)
                {
                    dto.TenHocPhan = hp.TenHocPhan;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(dto.TenHocPhan))
                    {
                        dto.TenHocPhan = dto.MaHocPhanTruong;
                    }
                    if (string.IsNullOrWhiteSpace(dto.TenHocPhan))
                    {
                        dto.DanhSachLoi.Add("Khong co thong tin hoc phan hoac ma hoc phan.");
                    }
                }

                dto.HopLe = dto.DanhSachLoi.Count == 0;
                if (dto.HopLe)
                {
                    ketQua.SoDongHopLe++;
                }
                else
                {
                    ketQua.SoDongLoi++;
                }

                ketQua.ChiTiet.Add(dto);
            }

            ketQua.TongSoDong = ketQua.ChiTiet.Count;
            return ketQua;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Khong the doc file Excel: {ex.Message}");
        }
    }

    public async Task<int> XacNhanImportAsync(XacNhanImportRequest request, CancellationToken cancellationToken = default)
    {
        // 1. Kiểm tra học kỳ tồn tại
        var hocKyTonTai = await context.HocKy.AnyAsync(x => x.MaHocKy == request.MaHocKy, cancellationToken);
        if (!hocKyTonTai)
        {
            throw new ArgumentException("Hoc ky khong ton tai trong he thong.");
        }

        var dsHopLe = request.DanhSachDong.Where(x => x.HopLe).ToList();
        if (dsHopLe.Count == 0)
        {
            throw new ArgumentException("Khong co dong hop le nao de import.");
        }

        var dsHocPhan = await context.HocPhan.ToListAsync(cancellationToken);
        int soDongThanhCong = 0;

        using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            foreach (var dong in dsHopLe)
            {
                // Khớp học phần trong hệ thống (nếu chưa có thì tự động tạo mới khi import)
                int.TryParse(dong.MaHocPhanTruong, out int maHpInt);
                var hp = dsHocPhan.FirstOrDefault(x =>
                    (maHpInt > 0 && x.MaHocPhan == maHpInt) ||
                    (!string.IsNullOrWhiteSpace(dong.TenHocPhan) && x.TenHocPhan.Equals(dong.TenHocPhan, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(dong.MaHocPhanTruong) && x.TenHocPhan.Equals(dong.MaHocPhanTruong, StringComparison.OrdinalIgnoreCase)));

                if (hp == null)
                {
                    string tenHpMoi = !string.IsNullOrWhiteSpace(dong.TenHocPhan) ? dong.TenHocPhan : dong.MaHocPhanTruong;
                    if (string.IsNullOrWhiteSpace(tenHpMoi)) tenHpMoi = "Hoc phan " + dong.MaLopHocPhanTruong;

                    hp = new HocPhan
                    {
                        TenHocPhan = tenHpMoi,
                        SoTinChi = 3,
                        TrangThai = "DangApDung"
                    };
                    context.HocPhan.Add(hp);
                    await context.SaveChangesAsync(cancellationToken);
                    dsHocPhan.Add(hp);
                }

                int maHocPhan = hp.MaHocPhan;

                // Tìm hoặc tạo Lớp học phần (tránh nhân bản khi re-import)
                var lop = await context.LopHocPhan.FirstOrDefaultAsync(
                    x => x.MaHocKy == request.MaHocKy && x.MaLopHocPhanTruong == dong.MaLopHocPhanTruong,
                    cancellationToken);

                if (lop == null)
                {
                    lop = new LopHocPhan
                    {
                        MaLopHocPhanTruong = dong.MaLopHocPhanTruong,
                        MaHocPhan = maHocPhan,
                        MaHocKy = request.MaHocKy,
                        TenLop = string.IsNullOrWhiteSpace(dong.TenLop) ? dong.MaLopHocPhanTruong : dong.TenLop,
                        SoLuongSinhVien = dong.SoLuongSinhVien > 0 ? dong.SoLuongSinhVien : 40,
                        TrangThai = "DangMo"
                    };
                    context.LopHocPhan.Add(lop);
                    await context.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    // Cập nhật thông tin nếu có thay đổi
                    lop.MaHocPhan = maHocPhan;
                    if (!string.IsNullOrWhiteSpace(dong.TenLop)) lop.TenLop = dong.TenLop;
                    if (dong.SoLuongSinhVien > 0) lop.SoLuongSinhVien = dong.SoLuongSinhVien;
                    await context.SaveChangesAsync(cancellationToken);
                }

                // Thêm chi tiết thời khóa biểu nếu chưa có buổi học tương tự
                var daCoChiTiet = await context.ChiTietThoiKhoaBieu.AnyAsync(
                    x => x.MaLopHocPhan == lop.MaLopHocPhan &&
                         x.Thu == dong.Thu &&
                         x.TietBatDau == dong.TietBatDau &&
                         x.TietKetThuc == dong.TietKetThuc &&
                         x.TuTuan == dong.TuTuan &&
                         x.DenTuan == dong.DenTuan,
                    cancellationToken);

                if (!daCoChiTiet)
                {
                    var chiTiet = new ChiTietThoiKhoaBieu
                    {
                        MaLopHocPhan = lop.MaLopHocPhan,
                        Thu = dong.Thu,
                        TietBatDau = dong.TietBatDau,
                        TietKetThuc = dong.TietKetThuc,
                        PhongHoc = dong.PhongHoc,
                        TuTuan = dong.TuTuan,
                        DenTuan = dong.DenTuan
                    };
                    context.ChiTietThoiKhoaBieu.Add(chiTiet);
                    await context.SaveChangesAsync(cancellationToken);
                }

                soDongThanhCong++;
            }

            // Lưu lịch sử import
            var lichSu = new LichSuImport
            {
                MaHocKy = request.MaHocKy,
                TenFile = string.IsNullOrWhiteSpace(request.TenFile) ? "ThoiKhoaBieu.xlsx" : request.TenFile,
                NgayImport = DateTime.Now,
                TongSoDong = request.DanhSachDong.Count,
                SoDongThanhCong = soDongThanhCong,
                SoDongLoi = request.DanhSachDong.Count - soDongThanhCong,
                TrangThai = soDongThanhCong == request.DanhSachDong.Count ? "ThanhCong" : (soDongThanhCong > 0 ? "MotPhan" : "Loi"),
                GhiChu = $"Import thanh cong {soDongThanhCong}/{request.DanhSachDong.Count} dong."
            };

            context.LichSuImport.Add(lichSu);
            await context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            return soDongThanhCong;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<List<LichSuImportDto>> LayLichSuImportAsync(int maHocKy)
    {
        return await context.LichSuImport
            .Where(x => x.MaHocKy == maHocKy)
            .OrderByDescending(x => x.NgayImport)
            .Select(x => new LichSuImportDto
            {
                MaLichSu = x.MaLichSu,
                MaHocKy = x.MaHocKy,
                TenFile = x.TenFile,
                NgayImport = x.NgayImport,
                TongSoDong = x.TongSoDong,
                SoDongThanhCong = x.SoDongThanhCong,
                SoDongLoi = x.SoDongLoi,
                TrangThai = x.TrangThai,
                GhiChu = x.GhiChu
            })
            .ToListAsync();
    }

    public byte[] TaoFileMauExcel()
    {
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("ThoiKhoaBieu");

        // Tiêu đề cột
        ws.Cell(1, 1).Value = "Mã lớp HP";
        ws.Cell(1, 2).Value = "Tên lớp";
        ws.Cell(1, 3).Value = "Mã môn học";
        ws.Cell(1, 4).Value = "Tên môn học";
        ws.Cell(1, 5).Value = "Số lượng SV";
        ws.Cell(1, 6).Value = "Thứ";
        ws.Cell(1, 7).Value = "Tiết bắt đầu";
        ws.Cell(1, 8).Value = "Tiết kết thúc";
        ws.Cell(1, 9).Value = "Phòng học";
        ws.Cell(1, 10).Value = "Từ tuần";
        ws.Cell(1, 11).Value = "Đến tuần";

        // Dữ liệu mẫu dòng 1
        ws.Cell(2, 1).Value = "IT101_01";
        ws.Cell(2, 2).Value = "Lập trình C# - K20A";
        ws.Cell(2, 3).Value = "1";
        ws.Cell(2, 4).Value = "Nhập môn lập trình";
        ws.Cell(2, 5).Value = 45;
        ws.Cell(2, 6).Value = 2;
        ws.Cell(2, 7).Value = 1;
        ws.Cell(2, 8).Value = 3;
        ws.Cell(2, 9).Value = "A101";
        ws.Cell(2, 10).Value = 1;
        ws.Cell(2, 11).Value = 15;

        // Dữ liệu mẫu dòng 2
        ws.Cell(3, 1).Value = "IT102_01";
        ws.Cell(3, 2).Value = "Cơ sở dữ liệu - K20B";
        ws.Cell(3, 3).Value = "2";
        ws.Cell(3, 4).Value = "Cơ sở dữ liệu";
        ws.Cell(3, 5).Value = 40;
        ws.Cell(3, 6).Value = 4;
        ws.Cell(3, 7).Value = 4;
        ws.Cell(3, 8).Value = 6;
        ws.Cell(3, 9).Value = "B202";
        ws.Cell(3, 10).Value = 1;
        ws.Cell(3, 11).Value = 15;

        ws.Columns().AdjustToContents();

        using var memoryStream = new MemoryStream();
        workbook.SaveAs(memoryStream);
        return memoryStream.ToArray();
    }
}
