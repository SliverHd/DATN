using Microsoft.EntityFrameworkCore;
using XepLichGiangDay.Api.Data;
using XepLichGiangDay.Api.DTOs.NguyenVong;
using XepLichGiangDay.Api.Models.Entities;

namespace XepLichGiangDay.Api.Services;

public class NguyenVongService(AppDbContext context)
{
    public async Task<List<NguyenVongDto>> GetAllAsync(int? maHocKy = null, int? maGiangVien = null)
    {
        var query = context.NguyenVongGiangVien.AsQueryable();

        if (maHocKy.HasValue && maHocKy.Value > 0)
        {
            query = query.Where(x => x.MaHocKy == maHocKy.Value);
        }

        if (maGiangVien.HasValue && maGiangVien.Value > 0)
        {
            query = query.Where(x => x.MaGiangVien == maGiangVien.Value);
        }

        var giangViens = await context.GiangVien.ToDictionaryAsync(g => g.MaGiangVien, g => g.HoTen);

        var list = await query.ToListAsync();

        return list.Select(x => new NguyenVongDto
        {
            MaNguyenVong = x.MaNguyenVong,
            MaGiangVien = x.MaGiangVien,
            TenGiangVien = giangViens.TryGetValue(x.MaGiangVien, out var name) ? name : $"GV-{x.MaGiangVien}",
            MaHocKy = x.MaHocKy,
            LoaiNguyenVong = x.LoaiNguyenVong,
            MucDo = x.MucDo,
            Thu = x.Thu,
            TietBatDau = x.TietBatDau,
            TietKetThuc = x.TietKetThuc,
            TrongSoPhat = x.TrongSoPhat,
            NoiDungGoc = x.NoiDungGoc
        }).ToList();
    }

    public async Task<NguyenVongDto?> GetByIdAsync(int id)
    {
        var item = await context.NguyenVongGiangVien.FindAsync(id);
        if (item == null) return null;

        var gv = await context.GiangVien.FindAsync(item.MaGiangVien);

        return new NguyenVongDto
        {
            MaNguyenVong = item.MaNguyenVong,
            MaGiangVien = item.MaGiangVien,
            TenGiangVien = gv?.HoTen ?? $"GV-{item.MaGiangVien}",
            MaHocKy = item.MaHocKy,
            LoaiNguyenVong = item.LoaiNguyenVong,
            MucDo = item.MucDo,
            Thu = item.Thu,
            TietBatDau = item.TietBatDau,
            TietKetThuc = item.TietKetThuc,
            TrongSoPhat = item.TrongSoPhat,
            NoiDungGoc = item.NoiDungGoc
        };
    }

    public async Task<NguyenVongDto> CreateAsync(TaoNguyenVongDto dto)
    {
        var item = new NguyenVongGiangVien
        {
            MaGiangVien = dto.MaGiangVien,
            MaHocKy = dto.MaHocKy,
            LoaiNguyenVong = string.IsNullOrWhiteSpace(dto.LoaiNguyenVong) ? "ThoiGianDay" : dto.LoaiNguyenVong,
            MucDo = dto.MucDo,
            Thu = dto.Thu,
            TietBatDau = dto.TietBatDau,
            TietKetThuc = dto.TietKetThuc,
            TrongSoPhat = dto.TrongSoPhat > 0 ? dto.TrongSoPhat : 10.0,
            NoiDungGoc = dto.NoiDungGoc
        };

        context.NguyenVongGiangVien.Add(item);
        await context.SaveChangesAsync();

        var gv = await context.GiangVien.FindAsync(item.MaGiangVien);

        return new NguyenVongDto
        {
            MaNguyenVong = item.MaNguyenVong,
            MaGiangVien = item.MaGiangVien,
            TenGiangVien = gv?.HoTen ?? $"GV-{item.MaGiangVien}",
            MaHocKy = item.MaHocKy,
            LoaiNguyenVong = item.LoaiNguyenVong,
            MucDo = item.MucDo,
            Thu = item.Thu,
            TietBatDau = item.TietBatDau,
            TietKetThuc = item.TietKetThuc,
            TrongSoPhat = item.TrongSoPhat,
            NoiDungGoc = item.NoiDungGoc
        };
    }

    public async Task<bool> UpdateAsync(int id, TaoNguyenVongDto dto)
    {
        var item = await context.NguyenVongGiangVien.FindAsync(id);
        if (item == null) return false;

        item.MaGiangVien = dto.MaGiangVien;
        item.MaHocKy = dto.MaHocKy;
        item.LoaiNguyenVong = dto.LoaiNguyenVong;
        item.MucDo = dto.MucDo;
        item.Thu = dto.Thu;
        item.TietBatDau = dto.TietBatDau;
        item.TietKetThuc = dto.TietKetThuc;
        item.TrongSoPhat = dto.TrongSoPhat;
        item.NoiDungGoc = dto.NoiDungGoc;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await context.NguyenVongGiangVien.FindAsync(id);
        if (item == null) return false;

        context.NguyenVongGiangVien.Remove(item);
        await context.SaveChangesAsync();
        return true;
    }
}
