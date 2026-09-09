import axiosClient from './axiosClient'
import type { NguyenVong, TaoNguyenVongPayload } from '../types/nguyenVong'

export const nguyenVongApi = {
  getAll: (maHocKy?: number, maGiangVien?: number) =>
    axiosClient.get<NguyenVong[]>('/nguyen-vong', {
      params: { maHocKy, maGiangVien },
    }),
  getById: (id: number) => axiosClient.get<NguyenVong>(`/nguyen-vong/${id}`),
  create: (data: TaoNguyenVongPayload) => axiosClient.post<NguyenVong>('/nguyen-vong', data),
  update: (id: number, data: TaoNguyenVongPayload) => axiosClient.put(`/nguyen-vong/${id}`, data),
  delete: (id: number) => axiosClient.delete(`/nguyen-vong/${id}`),
}
