import axiosClient from './axiosClient'

export const nguyenVongApi = {
  getAll: () => axiosClient.get<unknown[]>('/nguyen-vong'),
}
