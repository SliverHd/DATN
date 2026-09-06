export interface YeuCauXepLich {
  maHocKy: number
  thuatToan: 'GA' | 'CP_SAT'
  kichThuocQuanThe?: number
  soTheHe?: number
  tyLeLaiGhep?: number
  tyLeDotBien?: number
}

export interface PhanCong {
  maLopHocPhan: number
  maGiangVien: number
  diemPhat: number
  ghiChu: string
}

export interface KetQuaXepLich {
  thuatToan: string
  tongDiemPhat: number
  trangThai: string
  danhSachPhanCong: PhanCong[]
}
