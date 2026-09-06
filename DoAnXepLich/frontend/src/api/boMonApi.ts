import axiosClient from './axiosClient'
import type { BoMon, TaoBoMonPayload } from '../types/boMon'

export const boMonApi = {
  getAll: () => axiosClient.get<BoMon[]>('/bo-mon'),
  getById: (id: number) => axiosClient.get<BoMon>(`/bo-mon/${id}`),
  create: (data: TaoBoMonPayload) => axiosClient.post<BoMon>('/bo-mon', data),
  update: (id: number, data: TaoBoMonPayload) => axiosClient.put(`/bo-mon/${id}`, data),
  delete: (id: number) => axiosClient.delete(`/bo-mon/${id}`),
}
