export interface DongThoiKhoaBieuImport {
  soThuTu: number
  maLopHocPhanTruong: string
  tenLop: string
  maHocPhanTruong: string
  tenHocPhan: string
  soLuongSinhVien: number
  soTinChi: number
  thu: number
  tietBatDau: number
  tietKetThuc: number
  phongHoc: string
  tuTuan: number
  denTuan: number
  hopLe: boolean
  danhSachLoi: string[]
}

export interface KetQuaPreviewImport {
  tongSoDong: number
  soDongHopLe: number
  soDongLoi: number
  chiTiet: DongThoiKhoaBieuImport[]
}

export interface LichSuImport {
  maLichSu: number
  maHocKy: number
  tenFile: string
  ngayImport: string
  tongSoDong: number
  soDongThanhCong: number
  soDongLoi: number
  trangThai: string
  ghiChu?: string
}
