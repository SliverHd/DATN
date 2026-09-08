export interface LopHocPhanTKB {
  maLopHocPhan: string;
  tenLopHocPhan: string;
  maMonHoc: string;
  maPhong: string;
  thuTrongTuan: number;
  tietBatDau: number;
  soTiet: number;
  caHoc?: string;
  maGVPhanCongTruoc?: string;
  tenGVPhanCongTruoc?: string;
}

export interface ChiTietPhanCong {
  maLopHocPhan: string;
  maGV: string;
  tenGV: string;
  daKhoaThuCong: boolean;
}

export interface KetQuaXepLich {
  tenThuatToan: string;
  khaThi: boolean;
  thoiGianChayMs: number;
  diemChatLuong: number;
  soViPhamCung: number;
  danhSachPhanCong: ChiTietPhanCong[];
}

export interface KetQuaDanhGiaRangBuocMem {
  tongDiemChatLuong: number;
  diemThoaManNguyenVong: number;
  diemPhatLechDinhMuc: number;
  diemPhatRaiLich: number;
}