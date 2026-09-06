import axiosClient from './axiosClient'
import type { HocPhan } from '../types/hocPhan'

export const hocPhanApi = {
  getAll: () => axiosClient.get<HocPhan[]>('/hoc-phan'),
}
