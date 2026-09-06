import axiosClient from './axiosClient'
import type { DangNhapRequest, DoiMatKhauRequest, ThongTinNguoiDung } from '../types/auth'

export const authApi = {
  login: (data: DangNhapRequest) =>
    axiosClient.post<ThongTinNguoiDung>('/auth/login', data),

  doiMatKhau: (data: DoiMatKhauRequest) =>
    axiosClient.post('/auth/doi-mat-khau', data),
}
