using Microsoft.EntityFrameworkCore;
using XepLichGiangDay.Api.Data;
using XepLichGiangDay.Api.DTOs.NamHoc;
using XepLichGiangDay.Api.Models.Entities;

namespace XepLichGiangDay.Api.Services;

public class NamHocService(AppDbContext context)
{
    public async Task<List<NamHocDto>> GetAllAsync()
    {
        return await context.NamHoc
            .Select(x => new NamHocDto
            {
                MaNamHoc = x.MaNamHoc,
                TenNamHoc = x.TenNamHoc,
                NgayBatDau = x.NgayBatDau,
                NgayKetThuc = x.NgayKetThuc,
                TrangThai = x.TrangThai
            })
            .ToListAsync();
    }

    public async Task<NamHocDto?> GetByIdAsync(int id)
    {
        var item = await context.NamHoc.FindAsync(id);
        if (item is null) return null;

        return new NamHocDto
        {
            MaNamHoc = item.MaNamHoc,
            TenNamHoc = item.TenNamHoc,
            NgayBatDau = item.NgayBatDau,
            NgayKetThuc = item.NgayKetThuc,
            TrangThai = item.TrangThai
        };
    }

    public async Task<(bool Success, string? Error, NamHocDto? Data)> CreateAsync(TaoNamHocDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.TenNamHoc))
        {
            return (false, "Ten nam hoc khong duoc de trong.", null);
        }

        var tenNamHoc = dto.TenNamHoc.Trim();
        var exist = await context.NamHoc.AnyAsync(x => x.TenNamHoc.ToLower() == tenNamHoc.ToLower());
        if (exist)
        {
            return (false, "Ten nam hoc da ton tai.", null);
        }

        if (dto.NgayBatDau >= dto.NgayKetThuc)
        {
            return (false, "Ngay bat dau phai truoc ngay ket thuc.", null);
        }

        var item = new NamHoc
        {
            TenNamHoc = tenNamHoc,
            NgayBatDau = dto.NgayBatDau,
            NgayKetThuc = dto.NgayKetThuc,
            TrangThai = string.IsNullOrWhiteSpace(dto.TrangThai) ? "DangApDung" : dto.TrangThai
        };

        context.NamHoc.Add(item);
        await context.SaveChangesAsync();

        return (true, null, new NamHocDto
        {
            MaNamHoc = item.MaNamHoc,
            TenNamHoc = item.TenNamHoc,
            NgayBatDau = item.NgayBatDau,
            NgayKetThuc = item.NgayKetThuc,
            TrangThai = item.TrangThai
        });
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(int id, TaoNamHocDto dto)
    {
        var item = await context.NamHoc.FindAsync(id);
        if (item is null)
        {
            return (false, "Khong tim thay nam hoc.");
        }

        if (string.IsNullOrWhiteSpace(dto.TenNamHoc))
        {
            return (false, "Ten nam hoc khong duoc de trong.");
        }

        var tenNamHoc = dto.TenNamHoc.Trim();
        var exist = await context.NamHoc.AnyAsync(x => x.TenNamHoc.ToLower() == tenNamHoc.ToLower() && x.MaNamHoc != id);
        if (exist)
        {
            return (false, "Ten nam hoc da ton tai o nam hoc khac.");
        }

        if (dto.NgayBatDau >= dto.NgayKetThuc)
        {
            return (false, "Ngay bat dau phai truoc ngay ket thuc.");
        }

        item.TenNamHoc = tenNamHoc;
        item.NgayBatDau = dto.NgayBatDau;
        item.NgayKetThuc = dto.NgayKetThuc;
        item.TrangThai = string.IsNullOrWhiteSpace(dto.TrangThai) ? "DangApDung" : dto.TrangThai;

        await context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int id)
    {
        var item = await context.NamHoc.FindAsync(id);
        if (item is null)
        {
            return (false, "Khong tim thay nam hoc.");
        }

        var coHocKy = await context.HocKy.AnyAsync(x => x.MaNamHoc == id);
        if (coHocKy)
        {
            return (false, "Khong the xoa nam hoc dang co hoc ky truc thuoc.");
        }

        context.NamHoc.Remove(item);
        await context.SaveChangesAsync();
        return (true, null);
    }
}
