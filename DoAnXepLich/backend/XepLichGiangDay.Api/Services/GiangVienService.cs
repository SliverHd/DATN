using Microsoft.EntityFrameworkCore;
using XepLichGiangDay.Api.Data;
using XepLichGiangDay.Api.DTOs.GiangVien;
using XepLichGiangDay.Api.Models.Entities;

namespace XepLichGiangDay.Api.Services;

public class GiangVienService(AppDbContext context)
{
    public async Task<List<GiangVienDto>> GetAllAsync()
    {
        return await context.GiangVien
            .Select(x => new GiangVienDto
            {
                MaGiangVien = x.MaGiangVien,
                HoTen = x.HoTen,
                Email = x.Email,
                SoDienThoai = x.SoDienThoai,
                ChucDanh = x.ChucDanh,
                TrangThai = x.TrangThai
            })
            .ToListAsync();
    }

    public async Task<GiangVienDto?> GetByIdAsync(int id)
    {
        var item = await context.GiangVien.FindAsync(id);
        if (item is null) return null;

        return new GiangVienDto
        {
            MaGiangVien = item.MaGiangVien,
            HoTen = item.HoTen,
            Email = item.Email,
            SoDienThoai = item.SoDienThoai,
            ChucDanh = item.ChucDanh,
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
            TrangThai = dto.TrangThai
        };

        context.GiangVien.Add(item);
        await context.SaveChangesAsync();

        return new GiangVienDto
        {
            MaGiangVien = item.MaGiangVien,
            HoTen = item.HoTen,
            Email = item.Email,
            SoDienThoai = item.SoDienThoai,
            ChucDanh = item.ChucDanh,
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
