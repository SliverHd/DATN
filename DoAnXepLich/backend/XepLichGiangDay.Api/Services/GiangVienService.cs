using Microsoft.EntityFrameworkCore;
using XepLichGiangDay.Api.Data;
using XepLichGiangDay.Api.DTOs.GiangVien;
using XepLichGiangDay.Api.Models.Entities;

namespace XepLichGiangDay.Api.Services;

public class GiangVienService(AppDbContext context)
{
    public async Task<List<GiangVienDto>> GetAllAsync()
    {
        var boMons = await context.BoMon.ToDictionaryAsync(b => b.MaBoMon, b => b.TenBoMon);
        var list = await context.GiangVien.ToListAsync();

        return list.Select(x => new GiangVienDto
        {
            MaGiangVien = x.MaGiangVien,
            HoTen = x.HoTen,
            Email = x.Email,
            SoDienThoai = x.SoDienThoai,
            ChucDanh = x.ChucDanh,
            MaBoMon = x.MaBoMon,
            TenBoMon = x.MaBoMon.HasValue && boMons.TryGetValue(x.MaBoMon.Value, out var ten) ? ten : null,
            TrangThai = x.TrangThai
        }).ToList();
    }

    public async Task<GiangVienDto?> GetByIdAsync(int id)
    {
        var item = await context.GiangVien.FindAsync(id);
        if (item is null) return null;

        string? tenBoMon = null;
        if (item.MaBoMon.HasValue)
        {
            var bm = await context.BoMon.FindAsync(item.MaBoMon.Value);
            tenBoMon = bm?.TenBoMon;
        }

        return new GiangVienDto
        {
            MaGiangVien = item.MaGiangVien,
            HoTen = item.HoTen,
            Email = item.Email,
            SoDienThoai = item.SoDienThoai,
            ChucDanh = item.ChucDanh,
            MaBoMon = item.MaBoMon,
            TenBoMon = tenBoMon,
            TrangThai = item.TrangThai
        };
    }

    public async Task<GiangVienDto> CreateAsync(TaoGiangVienDto dto)
    {
        var item = new GiangVien
        {
            HoTen = dto.HoTen,
            Email = dto.Email,
            SoDienThoai = dto.SoDienThoai,
            ChucDanh = dto.ChucDanh,
            MaBoMon = dto.MaBoMon,
            TrangThai = string.IsNullOrWhiteSpace(dto.TrangThai) ? "DangLamViec" : dto.TrangThai
        };

        context.GiangVien.Add(item);
        await context.SaveChangesAsync();

        string? tenBoMon = null;
        if (item.MaBoMon.HasValue)
        {
            var bm = await context.BoMon.FindAsync(item.MaBoMon.Value);
            tenBoMon = bm?.TenBoMon;
        }

        return new GiangVienDto
        {
            MaGiangVien = item.MaGiangVien,
            HoTen = item.HoTen,
            Email = item.Email,
            SoDienThoai = item.SoDienThoai,
            ChucDanh = item.ChucDanh,
            MaBoMon = item.MaBoMon,
            TenBoMon = tenBoMon,
            TrangThai = item.TrangThai
        };
    }

    public async Task<bool> UpdateAsync(int id, TaoGiangVienDto dto)
    {
        var item = await context.GiangVien.FindAsync(id);
        if (item is null) return false;

        item.HoTen = dto.HoTen;
        item.Email = dto.Email;
        item.SoDienThoai = dto.SoDienThoai;
        item.ChucDanh = dto.ChucDanh;
        item.MaBoMon = dto.MaBoMon;
        item.TrangThai = dto.TrangThai;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await context.GiangVien.FindAsync(id);
        if (item is null) return false;

        context.GiangVien.Remove(item);
        await context.SaveChangesAsync();
        return true;
    }
}
