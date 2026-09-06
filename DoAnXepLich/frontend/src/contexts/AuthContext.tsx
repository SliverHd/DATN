import { createContext, useContext, useEffect, useState, type ReactNode } from 'react'
import { authApi } from '../api/authApi'
import type { DangNhapRequest, ThongTinNguoiDung } from '../types/auth'

interface AuthContextType {
  user: ThongTinNguoiDung | null
  dangTai: boolean
  dangNhap: (data: DangNhapRequest) => Promise<void>
  dangXuat: () => void
  doiMatKhau: (matKhauCu: string, matKhauMoi: string) => Promise<void>
}

const AuthContext = createContext<AuthContextType | undefined>(undefined)

const STORAGE_KEY = 'xep_lich_user'

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<ThongTinNguoiDung | null>(null)
  const [dangTai, setDangTai] = useState(true)

  useEffect(() => {
    const raw = localStorage.getItem(STORAGE_KEY)
    if (raw) {
      try {
        setUser(JSON.parse(raw))
      } catch {
        localStorage.removeItem(STORAGE_KEY)
      }
    }
    setDangTai(false)
  }, [])

  const dangNhap = async (data: DangNhapRequest) => {
    const response = await authApi.login(data)
    setUser(response.data)
    localStorage.setItem(STORAGE_KEY, JSON.stringify(response.data))
  }

  const dangXuat = () => {
    setUser(null)
    localStorage.removeItem(STORAGE_KEY)
  }

  const doiMatKhau = async (matKhauCu: string, matKhauMoi: string) => {
    if (!user) throw new Error('Chua dang nhap.')
    await authApi.doiMatKhau({
      maNguoiDung: user.maNguoiDung,
      matKhauCu,
      matKhauMoi,
    })
  }

  return (
    <AuthContext.Provider value={{ user, dangTai, dangNhap, dangXuat, doiMatKhau }}>
      {children}
    </AuthContext.Provider>
  )
}

export function useAuth() {
  const context = useContext(AuthContext)
  if (!context) {
    throw new Error('useAuth phai duoc su dung trong AuthProvider.')
  }
  return context
}
