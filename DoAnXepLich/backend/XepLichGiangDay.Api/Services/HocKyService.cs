using Microsoft.EntityFrameworkCore;
using XepLichGiangDay.Api.Data;
using XepLichGiangDay.Api.DTOs.HocKy;
using XepLichGiangDay.Api.Models.Entities;

namespace XepLichGiangDay.Api.Services;

public class HocKyService(AppDbContext context)
{
    public async Task<List<HocKyDto>> GetAllAsync()
    {
        var namHocs = await context.NamHoc.ToDictionaryAsync(x => x.MaNamHoc, x => x.TenNamHoc);
        var list = await context.HocKy.ToListAsync();

        return list.Select(x => new HocKyDto
        {
            MaHocKy = x.MaHocKy,
            MaNamHoc = x.MaNamHoc,
            TenNamHoc = namHocs.TryGetValue(x.MaNamHoc, out var tenNh) ? tenNh : null,
            TenHocKy = x.TenHocKy,
            NgayBatDau = x.NgayBatDau,
            NgayKetThuc = x.NgayKetThuc,
            TrangThai = x.TrangThai
        }).ToList();
    }

    public async Task<HocKyDto?> GetByIdAsync(int id)
    {
        var item = await context.HocKy.FindAsync(id);
        if (item is null) return null;

        var namHoc = await context.NamHoc.FindAsync(item.MaNamHoc);

        return new HocKyDto
        {
            MaHocKy = item.MaHocKy,
            MaNamHoc = item.MaNamHoc,
            TenNamHoc = namHoc?.TenNamHoc,
            TenHocKy = item.TenHocKy,
            NgayBatDau = item.NgayBatDau,
            NgayKetThuc = item.NgayKetThuc,
            TrangThai = item.TrangThai
        };
    }

    public async Task<(bool Success, string? Error, HocKyDto? Data)> CreateAsync(TaoHocKyDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.TenHocKy))
        {
            return (false, "Ten hoc ky khong duoc de trong.", null);
        }

        var namHoc = await context.NamHoc.FindAsync(dto.MaNamHoc);
        if (namHoc is null)
        {
            return (false, "Nam hoc khong ton tai trong he thong.", null);
        }

        var tenHocKy = dto.TenHocKy.Trim();
        var exist = await context.HocKy.AnyAsync(x => x.MaNamHoc == dto.MaNamHoc && x.TenHocKy.ToLower() == tenHocKy.ToLower());
        if (exist)
        {
            return (false, "Ten hoc ky da ton tai trong nam hoc nay.", null);
        }

        if (dto.NgayBatDau >= dto.NgayKetThuc)
        {
            return (false, "Ngay bat dau phai truoc ngay ket thuc.", null);
        }

        var item = new HocKy
        {
            MaNamHoc = dto.MaNamHoc,
            TenHocKy = tenHocKy,
            NgayBatDau = dto.NgayBatDau,
            NgayKetThuc = dto.NgayKetThuc,
            TrangThai = string.IsNullOrWhiteSpace(dto.TrangThai) ? "DangApDung" : dto.TrangThai
        };

        context.HocKy.Add(item);
        await context.SaveChangesAsync();

        return (true, null, new HocKyDto
        {
            MaHocKy = item.MaHocKy,
            MaNamHoc = item.MaNamHoc,
            TenNamHoc = namHoc.TenNamHoc,
            TenHocKy = item.TenHocKy,
            NgayBatDau = item.NgayBatDau,
            NgayKetThuc = item.NgayKetThuc,
            TrangThai = item.TrangThai
        });
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(int id, TaoHocKyDto dto)
    {
        var item = await context.HocKy.FindAsync(id);
        if (item is null)
        {
            return (false, "Khong tim thay hoc ky.");
        }

        if (string.IsNullOrWhiteSpace(dto.TenHocKy))
        {
            return (false, "Ten hoc ky khong duoc de trong.");
        }

        var namHocTonTai = await context.NamHoc.AnyAsync(x => x.MaNamHoc == dto.MaNamHoc);
        if (!namHocTonTai)
        {
            return (false, "Nam hoc khong ton tai trong he thong.");
        }

        var tenHocKy = dto.TenHocKy.Trim();
        var exist = await context.HocKy.AnyAsync(x => x.MaNamHoc == dto.MaNamHoc && x.TenHocKy.ToLower() == tenHocKy.ToLower() && x.MaHocKy != id);
        if (exist)
        {
            return (false, "Ten hoc ky da ton tai trong nam hoc nay.");
        }

        if (dto.NgayBatDau >= dto.NgayKetThuc)
        {
            return (false, "Ngay bat dau phai truoc ngay ket thuc.");
        }

        item.MaNamHoc = dto.MaNamHoc;
        item.TenHocKy = tenHocKy;
        item.NgayBatDau = dto.NgayBatDau;
        item.NgayKetThuc = dto.NgayKetThuc;
        item.TrangThai = string.IsNullOrWhiteSpace(dto.TrangThai) ? "DangApDung" : dto.TrangThai;

        await context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int id)
    {
        var item = await context.HocKy.FindAsync(id);
        if (item is null)
        {
            return (false, "Khong tim thay hoc ky.");
        }

        var coLopHocPhan = await context.LopHocPhan.AnyAsync(x => x.MaHocKy == id);
        if (coLopHocPhan)
        {
            return (false, "Khong the xoa hoc ky dang co lop hoc phan truc thuoc.");
        }

        var coPhanCong = await context.PhanCongGiangDay.AnyAsync(x => x.MaHocKy == id);
        if (coPhanCong)
        {
            return (false, "Khong the xoa hoc ky dang co du lieu phan cong giang day.");
        }

        var coKetQua = await context.KetQuaXepLich.AnyAsync(x => x.MaHocKy == id);
        if (coKetQua)
        {
            return (false, "Khong the xoa hoc ky dang co ket qua xep lich.");
        }

        context.HocKy.Remove(item);
        await context.SaveChangesAsync();
        return (true, null);
    }
}
