import axiosClient from './axiosClient'
import type { HocKy, TaoHocKyPayload } from '../types/hocKy'

export const hocKyApi = {
  getAll: () => axiosClient.get<HocKy[]>('/hoc-ky'),
  getById: (id: number) => axiosClient.get<HocKy>(`/hoc-ky/${id}`),
  create: (data: TaoHocKyPayload) => axiosClient.post<HocKy>('/hoc-ky', data),
  update: (id: number, data: TaoHocKyPayload) => axiosClient.put(`/hoc-ky/${id}`, data),
  delete: (id: number) => axiosClient.delete(`/hoc-ky/${id}`),
}
