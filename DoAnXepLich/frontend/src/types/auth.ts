export interface DangNhapRequest {
  tenDangNhap: string
  matKhau: string
}

export interface ThongTinNguoiDung {
  maNguoiDung: number
  tenDangNhap: string
  hoTen: string
  email?: string
  vaiTro: 'Admin' | 'TruongBoMon' | 'GiangVien' | string
  maGiangVien?: number
  token: string
}

export interface DoiMatKhauRequest {
  maNguoiDung: number
  matKhauCu: string
  matKhauMoi: string
}
