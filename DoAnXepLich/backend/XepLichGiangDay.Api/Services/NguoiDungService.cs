using Microsoft.EntityFrameworkCore;
using XepLichGiangDay.Api.Data;
using XepLichGiangDay.Api.DTOs.NguoiDung;
using XepLichGiangDay.Api.Models.Entities;

namespace XepLichGiangDay.Api.Services;

public class NguoiDungService(AppDbContext context)
{
    public async Task<List<NguoiDungDto>> GetAllAsync()
    {
        var users = await context.NguoiDung.ToListAsync();
        var giangViens = await context.GiangVien.ToDictionaryAsync(x => x.MaGiangVien, x => x.HoTen);

        return users.Select(u => new NguoiDungDto
        {
            MaNguoiDung = u.MaNguoiDung,
            TenDangNhap = u.TenDangNhap,
            HoTen = u.HoTen,
            Email = u.Email,
            VaiTro = u.VaiTro,
            MaGiangVien = u.MaGiangVien,
            TenGiangVien = u.MaGiangVien.HasValue && giangViens.TryGetValue(u.MaGiangVien.Value, out var gv) ? gv : null,
            TrangThai = u.TrangThai
        }).ToList();
    }

    public async Task<NguoiDungDto?> GetByIdAsync(int id)
    {
        var u = await context.NguoiDung.FindAsync(id);
        if (u is null) return null;

        string? tenGv = null;
        if (u.MaGiangVien.HasValue)
        {
            var gv = await context.GiangVien.FindAsync(u.MaGiangVien.Value);
            tenGv = gv?.HoTen;
        }

        return new NguoiDungDto
        {
            MaNguoiDung = u.MaNguoiDung,
            TenDangNhap = u.TenDangNhap,
            HoTen = u.HoTen,
            Email = u.Email,
            VaiTro = u.VaiTro,
            MaGiangVien = u.MaGiangVien,
            TenGiangVien = tenGv,
            TrangThai = u.TrangThai
        };
    }

    public async Task<NguoiDungDto?> CreateAsync(TaoNguoiDungDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.TenDangNhap) || string.IsNullOrWhiteSpace(dto.MatKhau))
        {
            return null;
        }

        var tenDangNhap = dto.TenDangNhap.Trim();
        var exist = await context.NguoiDung.AnyAsync(x => x.TenDangNhap == tenDangNhap);
        if (exist) return null;

        var user = new NguoiDung
        {
            TenDangNhap = tenDangNhap,
            MatKhau = AuthService.BamMatKhau(dto.MatKhau.Trim()),
            HoTen = dto.HoTen.Trim(),
            Email = dto.Email?.Trim(),
            VaiTro = dto.VaiTro,
            MaGiangVien = dto.MaGiangVien,
            TrangThai = "KichHoat"
        };

        context.NguoiDung.Add(user);
        await context.SaveChangesAsync();

        return await GetByIdAsync(user.MaNguoiDung);
    }

    public async Task<bool> UpdateAsync(int id, CapNhatNguoiDungDto dto)
    {
        var u = await context.NguoiDung.FindAsync(id);
        if (u is null) return false;

        // Khong cho phep khoa hoac ha vai tro cua tai khoan admin goc
        if (u.TenDangNhap.Equals("admin", StringComparison.OrdinalIgnoreCase))
        {
            if (dto.TrangThai == "Khoa" || dto.VaiTro != "Admin")
            {
                return false;
            }
        }

        u.HoTen = dto.HoTen.Trim();
        u.Email = dto.Email?.Trim();
        u.VaiTro = dto.VaiTro;
        u.MaGiangVien = dto.MaGiangVien;
        u.TrangThai = dto.TrangThai;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ResetMatKhauAsync(int id, string matKhauMoi)
    {
        if (string.IsNullOrWhiteSpace(matKhauMoi)) return false;

        var u = await context.NguoiDung.FindAsync(id);
        if (u is null) return false;

        u.MatKhau = AuthService.BamMatKhau(matKhauMoi.Trim());
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var u = await context.NguoiDung.FindAsync(id);
        if (u is null) return false;

        // Khong cho phep xoa tai khoan admin goc
        if (u.TenDangNhap.Equals("admin", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        context.NguoiDung.Remove(u);
        await context.SaveChangesAsync();
        return true;
    }
}
