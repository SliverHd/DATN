import axiosClient from './axiosClient'
import type { GiangVien } from '../types/giangVien'

export const giangVienApi = {
  getAll: () => axiosClient.get<GiangVien[]>('/giang-vien'),
}
