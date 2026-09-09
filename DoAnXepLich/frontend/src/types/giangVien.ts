export interface GiangVien {
  maGiangVien: number
  hoTen: string
  email: string
  soDienThoai?: string
  chucDanh?: string
  maBoMon?: number
  tenBoMon?: string
  trangThai: string
}

export interface TaoGiangVienDto {
  hoTen: string
  email: string
  soDienThoai?: string
  chucDanh?: string
  maBoMon?: number
  trangThai: string
}
