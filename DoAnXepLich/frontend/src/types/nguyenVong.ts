export interface NguyenVong {
  maNguyenVong: number
  maGiangVien: number
  tenGiangVien: string
  maHocKy: number
  loaiNguyenVong: string
  mucDo: number
  thu?: number
  tietBatDau?: number
  tietKetThuc?: number
  trongSoPhat: number
  noiDungGoc?: string
}

export interface TaoNguyenVongPayload {
  maGiangVien: number
  maHocKy: number
  loaiNguyenVong: string
  mucDo: number
  thu?: number
  tietBatDau?: number
  tietKetThuc?: number
  trongSoPhat?: number
  noiDungGoc?: string
}
