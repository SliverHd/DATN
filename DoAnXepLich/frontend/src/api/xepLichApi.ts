import axiosClient from './axiosClient'
import type { KetQuaXepLich, YeuCauXepLich } from '../types/xepLich'

export const xepLichApi = {
  chayXepLich: (data: YeuCauXepLich) =>
    axiosClient.post<KetQuaXepLich>('/xep-lich', data),
}
