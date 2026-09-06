import axiosClient from './axiosClient'
import type {
  CapNhatNguoiDungRequest,
  NguoiDung,
  TaoNguoiDungRequest,
} from '../types/nguoiDung'

export const nguoiDungApi = {
  getAll: () => axiosClient.get<NguoiDung[]>('/nguoi-dung'),

  getById: (id: number) => axiosClient.get<NguoiDung>(`/nguoi-dung/${id}`),

  create: (data: TaoNguoiDungRequest) =>
    axiosClient.post<NguoiDung>('/nguoi-dung', data),

  update: (id: number, data: CapNhatNguoiDungRequest) =>
    axiosClient.put(`/nguoi-dung/${id}`, data),

  resetMatKhau: (id: number, matKhauMoi: string) =>
    axiosClient.post(`/nguoi-dung/${id}/reset-mat-khau`, { matKhauMoi }),

  delete: (id: number) => axiosClient.delete(`/nguoi-dung/${id}`),
}
