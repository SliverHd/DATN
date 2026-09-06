import { useState, type FormEvent } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext'

function Header() {
  const { user, dangXuat, doiMatKhau } = useAuth()
  const navigate = useNavigate()

  const [hienModalDoiMk, setHienModalDoiMk] = useState(false)
  const [matKhauCu, setMatKhauCu] = useState('')
  const [matKhauMoi, setMatKhauMoi] = useState('')
  const [loi, setLoi] = useState('')
  const [thongBao, setThongBao] = useState('')

  const handleDangXuat = () => {
    dangXuat()
    navigate('/login')
  }

  const handleDoiMatKhau = async (e: FormEvent) => {
    e.preventDefault()
    setLoi('')
    setThongBao('')

    if (!matKhauCu || !matKhauMoi) {
      setLoi('Vui lòng nhập đầy đủ mật khẩu cũ và mới.')
      return
    }

    try {
      await doiMatKhau(matKhauCu, matKhauMoi)
      setThongBao('Đổi mật khẩu thành công!')
      setMatKhauCu('')
      setMatKhauMoi('')
      setTimeout(() => {
        setHienModalDoiMk(false)
        setThongBao('')
      }, 1200)
    } catch {
      setLoi('Mật khẩu cũ không đúng.')
    }
  }

  const hienThiHoTen = (ten?: string) => {
    if (!ten) return ''
    if (ten === 'Quan tri vien he thong') return 'Quản trị viên hệ thống'
    return ten
  }

  return (
    <header className="border-bottom bg-white px-4 py-2 d-flex justify-content-between align-items-center">
      <h1 className="h6 mb-0 text-secondary">Hệ thống phân công giảng viên vào thời khóa biểu</h1>

      <div className="d-flex align-items-center gap-2">
        {user ? (
          <>
            <span className="small text-muted">Xin chào:</span>
            <span className="small fw-semibold">{hienThiHoTen(user.hoTen)}</span>
            <span className="badge bg-secondary small">{user.vaiTro === 'Admin' ? 'Quản trị viên' : user.vaiTro}</span>

            <div className="btn-group ms-2">
              <button
                className="btn btn-outline-secondary btn-sm"
                onClick={() => {
                  setLoi('')
                  setThongBao('')
                  setHienModalDoiMk(true)
                }}
              >
                Đổi mật khẩu
              </button>
              <button
                className="btn btn-outline-danger btn-sm"
                onClick={handleDangXuat}
              >
                Đăng xuất
              </button>
            </div>
          </>
        ) : (
          <button
            className="btn btn-primary btn-sm"
            onClick={() => navigate('/login')}
          >
            Đăng nhập
          </button>
        )}
      </div>

      {/* Modal Doi Mat Khau */}
      {hienModalDoiMk && (
        <div
          className="modal show d-block"
          style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}
        >
          <div className="modal-dialog modal-sm">
            <div className="modal-content">
              <form onSubmit={handleDoiMatKhau}>
                <div className="modal-header py-2">
                  <h5 className="modal-title h6">Đổi mật khẩu</h5>
                  <button
                    type="button"
                    className="btn-close"
                    onClick={() => setHienModalDoiMk(false)}
                  />
                </div>
                <div className="modal-body">
                  {loi && <div className="alert alert-danger py-1 small">{loi}</div>}
                  {thongBao && <div className="alert alert-success py-1 small">{thongBao}</div>}

                  <div className="mb-2">
                    <label className="form-label small fw-semibold">Mật khẩu hiện tại</label>
                    <input
                      type="password"
                      className="form-control form-control-sm"
                      value={matKhauCu}
                      onChange={(e) => setMatKhauCu(e.target.value)}
                      required
                    />
                  </div>
                  <div className="mb-2">
                    <label className="form-label small fw-semibold">Mật khẩu mới</label>
                    <input
                      type="password"
                      className="form-control form-control-sm"
                      value={matKhauMoi}
                      onChange={(e) => setMatKhauMoi(e.target.value)}
                      required
                    />
                  </div>
                </div>
                <div className="modal-footer py-2">
                  <button
                    type="button"
                    className="btn btn-secondary btn-sm"
                    onClick={() => setHienModalDoiMk(false)}
                  >
                    Hủy
                  </button>
                  <button type="submit" className="btn btn-primary btn-sm">
                    Lưu mật khẩu
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      )}
    </header>
  )
}

export default Header
