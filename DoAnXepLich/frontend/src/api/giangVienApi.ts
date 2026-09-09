import axiosClient from './axiosClient'
import type { GiangVien, TaoGiangVienDto } from '../types/giangVien'

export const giangVienApi = {
  getAll: () => axiosClient.get<GiangVien[]>('/giang-vien'),
  getById: (id: number) => axiosClient.get<GiangVien>(`/giang-vien/${id}`),
  create: (data: TaoGiangVienDto) => axiosClient.post<GiangVien>('/giang-vien', data),
  update: (id: number, data: TaoGiangVienDto) => axiosClient.put(`/giang-vien/${id}`, data),
  delete: (id: number) => axiosClient.delete(`/giang-vien/${id}`),
}
