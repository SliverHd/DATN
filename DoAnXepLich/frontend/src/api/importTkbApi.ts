import axiosClient from './axiosClient'
import type { KetQuaPreviewImport, LichSuImport, DongThoiKhoaBieuImport } from '../types/importTkb'

export const importTkbApi = {
  preview: (file: File, maHocKy: number) => {
    const formData = new FormData()
    formData.append('file', file)
    formData.append('maHocKy', maHocKy.toString())
    return axiosClient.post<KetQuaPreviewImport>('/import-thoi-khoa-bieu/preview', formData, {
      headers: { 'Content-Type': 'multipart/form-data' },
    })
  },

  xacNhan: (maHocKy: number, tenFile: string, danhSachDong: DongThoiKhoaBieuImport[]) => {
    return axiosClient.post<{ soDongThanhCong: number }>('/import-thoi-khoa-bieu/xac-nhan', {
      maHocKy,
      tenFile,
      danhSachDong,
    })
  },

  getLichSu: (maHocKy: number) => {
    return axiosClient.get<LichSuImport[]>(`/import-thoi-khoa-bieu/lich-su/${maHocKy}`)
  },

  downloadFileMauUrl: '/api/import-thoi-khoa-bieu/file-mau',
}
