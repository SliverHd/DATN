using Microsoft.EntityFrameworkCore;
using XepLichGiangDay.Api.Data;
using XepLichGiangDay.Api.DTOs.BoMon;
using XepLichGiangDay.Api.Models.Entities;

namespace XepLichGiangDay.Api.Services;

public class BoMonService(AppDbContext context)
{
    public async Task<List<BoMonDto>> GetAllAsync()
    {
        return await context.BoMon
            .Select(x => new BoMonDto
            {
                MaBoMon = x.MaBoMon,
                TenBoMon = x.TenBoMon,
                TenKhoa = x.TenKhoa
            })
            .ToListAsync();
    }

    public async Task<BoMonDto?> GetByIdAsync(int id)
    {
        var item = await context.BoMon.FindAsync(id);
        if (item is null) return null;

        return new BoMonDto
        {
            MaBoMon = item.MaBoMon,
            TenBoMon = item.TenBoMon,
            TenKhoa = item.TenKhoa
        };
    }

    public async Task<(bool Success, string? Error, BoMonDto? Data)> CreateAsync(TaoBoMonDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.TenBoMon))
        {
            return (false, "Ten bo mon khong duoc de trong.", null);
        }

        var tenBoMon = dto.TenBoMon.Trim();
        var exist = await context.BoMon.AnyAsync(x => x.TenBoMon.ToLower() == tenBoMon.ToLower());
        if (exist)
        {
            return (false, "Ten bo mon da ton tai.", null);
        }

        var item = new BoMon
        {
            TenBoMon = tenBoMon,
            TenKhoa = dto.TenKhoa?.Trim() ?? string.Empty
        };

        context.BoMon.Add(item);
        await context.SaveChangesAsync();

        return (true, null, new BoMonDto
        {
            MaBoMon = item.MaBoMon,
            TenBoMon = item.TenBoMon,
            TenKhoa = item.TenKhoa
        });
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(int id, TaoBoMonDto dto)
    {
        var item = await context.BoMon.FindAsync(id);
        if (item is null)
        {
            return (false, "Khong tim thay bo mon.");
        }

        if (string.IsNullOrWhiteSpace(dto.TenBoMon))
        {
            return (false, "Ten bo mon khong duoc de trong.");
        }

        var tenBoMon = dto.TenBoMon.Trim();
        var exist = await context.BoMon.AnyAsync(x => x.TenBoMon.ToLower() == tenBoMon.ToLower() && x.MaBoMon != id);
        if (exist)
        {
            return (false, "Ten bo mon da ton tai o bo mon khac.");
        }

        item.TenBoMon = tenBoMon;
        item.TenKhoa = dto.TenKhoa?.Trim() ?? string.Empty;

        await context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int id)
    {
        var item = await context.BoMon.FindAsync(id);
        if (item is null)
        {
            return (false, "Khong tim thay bo mon.");
        }

        var coGiangVien = await context.GiangVien.AnyAsync(x => x.MaBoMon == id);
        if (coGiangVien)
        {
            return (false, "Khong the xoa bo mon dang co giang vien truc thuoc.");
        }

        var coHocPhan = await context.HocPhan.AnyAsync(x => x.MaBoMon == id);
        if (coHocPhan)
        {
            return (false, "Khong the xoa bo mon dang co hoc phan truc thuoc.");
        }

        context.BoMon.Remove(item);
        await context.SaveChangesAsync();
        return (true, null);
    }
}
