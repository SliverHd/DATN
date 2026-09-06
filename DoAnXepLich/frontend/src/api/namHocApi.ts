import axiosClient from './axiosClient'
import type { NamHoc, TaoNamHocPayload } from '../types/namHoc'

export const namHocApi = {
  getAll: () => axiosClient.get<NamHoc[]>('/nam-hoc'),
  getById: (id: number) => axiosClient.get<NamHoc>(`/nam-hoc/${id}`),
  create: (data: TaoNamHocPayload) => axiosClient.post<NamHoc>('/nam-hoc', data),
  update: (id: number, data: TaoNamHocPayload) => axiosClient.put(`/nam-hoc/${id}`, data),
  delete: (id: number) => axiosClient.delete(`/nam-hoc/${id}`),
}
