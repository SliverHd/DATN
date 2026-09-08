import type { KetQuaXepLich, KetQuaDanhGiaRangBuocMem } from '../types/kieuDuLieuXepLich';

const API_BASE_URL = 'http://localhost:5005/api/DieuPhoiXepLich';

export interface PhanHoiXepLich {
  maHocKi: string;
  thuatToanSuDung: string;
  ketQua: KetQuaXepLich;
  danhGiaRangBuocMem: KetQuaDanhGiaRangBuocMem;
}

export const dichVuXepLich = {
  // 1. Gọi API xếp lịch theo học kỳ
  chayTheoHocKy: async (maHocKi: string): Promise<PhanHoiXepLich> => {
    const res = await fetch(`${API_BASE_URL}/chay-theo-hoc-ky?maHocKi=${encodeURIComponent(maHocKi)}`, {
      method: 'POST'
    });
    if (!res.ok) {
      throw new Error(`Lỗi từ máy chủ: ${res.statusText}`);
    }
    return res.json();
  },

  // 2. Gọi API khóa giảng viên thủ công (Bán tự động)
  khoaGiangVien: async (maHocKi: string, maLop: string, maGV: string, tenGV: string) => {
    const res = await fetch(`${API_BASE_URL}/khoa-giang-vien-thu-cong`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        maHocKi,
        maLopHocPhan: maLop,
        maGVMoi: maGV,
        tenGVMoi: tenGV,
        daKhoa: true
      })
    });
    return res.json();
  }
};