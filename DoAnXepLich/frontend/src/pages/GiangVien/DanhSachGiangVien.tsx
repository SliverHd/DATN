import { useEffect, useState } from 'react'
import { giangVienApi } from '../../api/giangVienApi'
import { boMonApi } from '../../api/boMonApi'
import Loading from '../../components/Loading'
import type { GiangVien, TaoGiangVienDto } from '../../types/giangVien'
import type { BoMon } from '../../types/boMon'

function DanhSachGiangVien() {
  const [danhSach, setDanhSach] = useState<GiangVien[]>([])
  const [danhSachBoMon, setDanhSachBoMon] = useState<BoMon[]>([])
  const [dangTai, setDangTai] = useState(true)
  const [loi, setLoi] = useState('')
  const [tuKhoa, setTuKhoa] = useState('')

  // State Modal Thêm / Sửa
  const [hienModal, setHienModal] = useState(false)
  const [dangSuaId, setDangSuaId] = useState<number | null>(null)
  const [formData, setFormData] = useState<TaoGiangVienDto>({
    hoTen: '',
    email: '',
    soDienThoai: '',
    chucDanh: 'Giảng viên',
    maBoMon: undefined,
    trangThai: 'DangLamViec',
  })
  const [dangLuu, setDangLuu] = useState(false)

  const taiDuLieu = async () => {
    try {
      setDangTai(true)
      const [resGV, resBM] = await Promise.all([
        giangVienApi.getAll(),
        boMonApi.getAll().catch(() => ({ data: [] as BoMon[] })),
      ])
      setDanhSach(resGV.data)
      setDanhSachBoMon(resBM.data)
      setLoi('')
    } catch {
      setLoi('Không tải được danh sách giảng viên.')
    } finally {
      setDangTai(false)
    }
  }

  useEffect(() => {
    void taiDuLieu()
  }, [])

  const handleMoModalTaoMoi = () => {
    setDangSuaId(null)
    setFormData({
      hoTen: '',
      email: '',
      soDienThoai: '',
      chucDanh: 'Giảng viên',
      maBoMon: danhSachBoMon.length > 0 ? danhSachBoMon[0].maBoMon : undefined,
      trangThai: 'DangLamViec',
    })
    setHienModal(true)
  }

  const handleMoModalSua = (gv: GiangVien) => {
    setDangSuaId(gv.maGiangVien)
    setFormData({
      hoTen: gv.hoTen,
      email: gv.email,
      soDienThoai: gv.soDienThoai || '',
      chucDanh: gv.chucDanh || 'Giảng viên',
      maBoMon: gv.maBoMon,
      trangThai: gv.trangThai || 'DangLamViec',
    })
    setHienModal(true)
  }

  const handleLuu = async (e: React.FormEvent) => {
    e.preventDefault()
    if (!formData.hoTen.trim() || !formData.email.trim()) {
      alert('Vui lòng nhập Họ tên và Email!')
      return
    }

    try {
      setDangLuu(true)
      if (dangSuaId) {
        await giangVienApi.update(dangSuaId, formData)
      } else {
        await giangVienApi.create(formData)
      }
      setHienModal(false)
      await taiDuLieu()
    } catch (err: unknown) {
      const errorMsg = err instanceof Error ? err.message : 'Lỗi khi lưu dữ liệu'
      alert('Không thể lưu thông tin giảng viên: ' + errorMsg)
    } finally {
      setDangLuu(false)
    }
  }

  const handleXoa = async (id: number, hoTen: string) => {
    if (!window.confirm(`Bạn có chắc chắn muốn xóa giảng viên "${hoTen}"?`)) {
      return
    }

    try {
      await giangVienApi.delete(id)
      await taiDuLieu()
    } catch {
      alert('Không thể xóa giảng viên này.')
    }
  }

  const danhSachLoc = danhSach.filter(
    (gv) =>
      gv.hoTen.toLowerCase().includes(tuKhoa.toLowerCase()) ||
      gv.email.toLowerCase().includes(tuKhoa.toLowerCase()) ||
      (gv.tenBoMon && gv.tenBoMon.toLowerCase().includes(tuKhoa.toLowerCase()))
  )

  return (
    <section>
      <div className="d-flex justify-content-between align-items-center mb-3">
        <div>
          <h2 className="h4 mb-1">Quản lý Giảng viên</h2>
          <p className="text-secondary small mb-0">Quản lý danh sách, bộ môn và thông tin giảng viên bộ môn</p>
        </div>
        <button className="btn btn-primary" onClick={handleMoModalTaoMoi}>
          + Thêm giảng viên
        </button>
      </div>

      <div className="card mb-3 shadow-sm">
        <div className="card-body py-2">
          <div className="row g-2 align-items-center">
            <div className="col-md-6">
              <input
                type="text"
                className="form-control"
                placeholder="Tìm kiếm theo tên, email, bộ môn..."
                value={tuKhoa}
                onChange={(e) => setTuKhoa(e.target.value)}
              />
            </div>
            <div className="col-md-6 text-end text-muted small">
              Tổng số: <strong>{danhSachLoc.length}</strong> giảng viên
            </div>
          </div>
        </div>
      </div>

      {dangTai && <Loading />}
      {loi && <div className="alert alert-warning">{loi}</div>}

      {!dangTai && !loi && (
        <div className="table-responsive bg-white rounded shadow-sm">
          <table className="table table-hover table-striped table-bordered align-middle mb-0">
            <thead className="table-light">
              <tr>
                <th style={{ width: '60px' }}>STT</th>
                <th>Họ tên</th>
                <th>Email</th>
                <th>Số điện thoại</th>
                <th>Chức danh</th>
                <th>Bộ môn</th>
                <th>Trạng thái</th>
                <th style={{ width: '130px' }} className="text-center">Thao tác</th>
              </tr>
            </thead>
            <tbody>
              {danhSachLoc.map((giangVien, index) => (
                <tr key={giangVien.maGiangVien}>
                  <td>{index + 1}</td>
                  <td className="fw-semibold">{giangVien.hoTen}</td>
                  <td>{giangVien.email}</td>
                  <td>{giangVien.soDienThoai || '-'}</td>
                  <td>{giangVien.chucDanh || 'Giảng viên'}</td>
                  <td>
                    <span className="badge bg-secondary">
                      {giangVien.tenBoMon || 'Chưa gán'}
                    </span>
                  </td>
                  <td>
                    <span
                      className={`badge ${
                        giangVien.trangThai === 'DangLamViec' || giangVien.trangThai === 'DangHoatDong'
                          ? 'bg-success'
                          : 'bg-warning text-dark'
                      }`}
                    >
                      {giangVien.trangThai === 'DangLamViec' || giangVien.trangThai === 'DangHoatDong'
                        ? 'Đang làm việc'
                        : 'Tạm nghỉ'}
                    </span>
                  </td>
                  <td className="text-center">
                    <button
                      className="btn btn-sm btn-outline-primary me-1"
                      onClick={() => handleMoModalSua(giangVien)}
                      title="Sửa"
                    >
                      Sửa
                    </button>
                    <button
                      className="btn btn-sm btn-outline-danger"
                      onClick={() => handleXoa(giangVien.maGiangVien, giangVien.hoTen)}
                      title="Xóa"
                    >
                      Xóa
                    </button>
                  </td>
                </tr>
              ))}
              {danhSachLoc.length === 0 && (
                <tr>
                  <td colSpan={8} className="text-center text-secondary py-4">
                    Chưa có giảng viên nào.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      )}

      {/* Modal Thêm / Sửa */}
      {hienModal && (
        <div className="modal d-block" style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
          <div className="modal-dialog modal-dialog-centered">
            <div className="modal-content shadow">
              <div className="modal-header">
                <h5 className="modal-title">
                  {dangSuaId ? 'Chỉnh sửa Giảng viên' : 'Thêm Giảng viên mới'}
                </h5>
                <button
                  type="button"
                  className="btn-close"
                  onClick={() => setHienModal(false)}
                ></button>
              </div>
              <form onSubmit={handleLuu}>
                <div className="modal-body">
                  <div className="mb-3">
                    <label className="form-label fw-semibold">Họ tên <span className="text-danger">*</span></label>
                    <input
                      type="text"
                      className="form-control"
                      required
                      value={formData.hoTen}
                      onChange={(e) => setFormData({ ...formData, hoTen: e.target.value })}
                      placeholder="Nhập họ tên đầy đủ"
                    />
                  </div>
                  <div className="mb-3">
                    <label className="form-label fw-semibold">Email <span className="text-danger">*</span></label>
                    <input
                      type="email"
                      className="form-control"
                      required
                      value={formData.email}
                      onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                      placeholder="example@nuce.edu.vn"
                    />
                  </div>
                  <div className="row">
                    <div className="col-md-6 mb-3">
                      <label className="form-label fw-semibold">Số điện thoại</label>
                      <input
                        type="text"
                        className="form-control"
                        value={formData.soDienThoai || ''}
                        onChange={(e) => setFormData({ ...formData, soDienThoai: e.target.value })}
                      />
                    </div>
                    <div className="col-md-6 mb-3">
                      <label className="form-label fw-semibold">Chức danh</label>
                      <input
                        type="text"
                        className="form-control"
                        value={formData.chucDanh || ''}
                        onChange={(e) => setFormData({ ...formData, chucDanh: e.target.value })}
                      />
                    </div>
                  </div>
                  <div className="mb-3">
                    <label className="form-label fw-semibold">Bộ môn</label>
                    <select
                      className="form-select"
                      value={formData.maBoMon || ''}
                      onChange={(e) => setFormData({ ...formData, maBoMon: e.target.value ? Number(e.target.value) : undefined })}
                    >
                      <option value="">-- Chọn bộ môn --</option>
                      {danhSachBoMon.map((bm) => (
                        <option key={bm.maBoMon} value={bm.maBoMon}>
                          {bm.tenBoMon}
                        </option>
                      ))}
                    </select>
                  </div>
                  <div className="mb-3">
                    <label className="form-label fw-semibold">Trạng thái</label>
                    <select
                      className="form-select"
                      value={formData.trangThai}
                      onChange={(e) => setFormData({ ...formData, trangThai: e.target.value })}
                    >
                      <option value="DangLamViec">Đang làm việc</option>
                      <option value="TamNghi">Tạm nghỉ</option>
                    </select>
                  </div>
                </div>
                <div className="modal-footer">
                  <button
                    type="button"
                    className="btn btn-secondary"
                    onClick={() => setHienModal(false)}
                  >
                    Hủy
                  </button>
                  <button type="submit" className="btn btn-primary" disabled={dangLuu}>
                    {dangLuu ? 'Đang lưu...' : 'Lưu thông tin'}
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      )}
    </section>
  )
}

export default DanhSachGiangVien
