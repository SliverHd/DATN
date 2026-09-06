import axiosClient from './axiosClient'
import type { LopHocPhan } from '../types/lopHocPhan'

export const thoiKhoaBieuApi = {
  getAll: (maHocKy?: number) =>
    axiosClient.get<LopHocPhan[]>('/thoi-khoa-bieu', {
      params: { maHocKy: maHocKy && maHocKy > 0 ? maHocKy : undefined },
    }),
  delete: (id: number) => axiosClient.delete(`/thoi-khoa-bieu/${id}`),
}
