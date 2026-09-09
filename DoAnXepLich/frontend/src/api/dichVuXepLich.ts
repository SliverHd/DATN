import axiosClient from './axiosClient';
import type { KetQuaXepLich, KetQuaDanhGiaRangBuocMem } from '../types/kieuDuLieuXepLich';

export interface PhanHoiXepLich {
  maHocKi: string;
  thuatToanSuDung: string;
  ketQua: KetQuaXepLich;
  danhGiaRangBuocMem: KetQuaDanhGiaRangBuocMem;
}

export const dichVuXepLich = {
  // 1. Gọi API xếp lịch theo học kỳ
  chayTheoHocKy: async (maHocKi: string): Promise<PhanHoiXepLich> => {
    const res = await axiosClient.post<PhanHoiXepLich>(
      `/DieuPhoiXepLich/chay-theo-hoc-ky?maHocKi=${encodeURIComponent(maHocKi)}`
    );
    return res.data;
  },

  // 2. Gọi API khóa giảng viên thủ công (Bán tự động)
  khoaGiangVien: async (maHocKi: string, maLop: string, maGV: string, tenGV: string) => {
    const res = await axiosClient.post('/DieuPhoiXepLich/khoa-giang-vien-thu-cong', {
      maHocKi,
      maLopHocPhan: maLop,
      maGVMoi: maGV,
      tenGVMoi: tenGV,
      daKhoa: true,
    });
    return res.data;
  },
};