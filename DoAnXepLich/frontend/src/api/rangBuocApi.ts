import axiosClient from './axiosClient'
import type {
  RangBuocHocKy,
  DinhMucGiangVien,
  CapNhatDinhMucPayload,
  ApDungDinhMucChungPayload,
  KetQuaKiemTraRangBuoc,
} from '../types/rangBuoc'

export const rangBuocApi = {
  getRangBuocByHocKy: (maHocKy: number) =>
    axiosClient.get<RangBuocHocKy[]>('/rang-buoc', { params: { maHocKy } }),

  capNhatTrongSo: (id: number, trongSoPhat: number) =>
    axiosClient.put(`/rang-buoc/${id}`, { trongSoPhat }),

  getDinhMucByHocKy: (maHocKy: number) =>
    axiosClient.get<DinhMucGiangVien[]>('/rang-buoc/dinh-muc', { params: { maHocKy } }),

  capNhatDinhMuc: (payload: CapNhatDinhMucPayload) =>
    axiosClient.post('/rang-buoc/dinh-muc', payload),

  apDungDinhMucChung: (payload: ApDungDinhMucChungPayload) =>
    axiosClient.post('/rang-buoc/dinh-muc/ap-dung-chung', payload),

  kiemTraRangBuocCung: (maHocKy: number) =>
    axiosClient.get<KetQuaKiemTraRangBuoc>(`/rang-buoc/kiem-tra-cung/${maHocKy}`),
}
