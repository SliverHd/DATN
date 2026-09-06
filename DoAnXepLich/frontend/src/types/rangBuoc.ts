export interface RangBuocHocKy {
  maRangBuoc: number
  maHocKy: number
  tenRangBuoc: string
  loaiRangBuoc: string
  trongSoPhat: number
  moTa?: string
}

export interface DinhMucGiangVien {
  maDinhMuc: number
  maGiangVien: number
  tenGiangVien: string
  email: string
  maHocKy: number
  soTietToiThieu: number
  soTietToiDa: number
}

export interface CapNhatDinhMucPayload {
  maGiangVien: number
  maHocKy: number
  soTietToiThieu: number
  soTietToiDa: number
}

export interface ApDungDinhMucChungPayload {
  maHocKy: number
  soTietToiThieu: number
  soTietToiDa: number
}

export interface KetQuaKiemTraRangBuoc {
  hopLe: boolean
  tongSoLop: number
  soLopDaPhanCong: number
  soLopChuaPhanCong: number
  soViPhamChuyenMon: number
  soViPhamTrungLich: number
  soViPhamLichBan: number
  danhSachViPham: string[]
}
