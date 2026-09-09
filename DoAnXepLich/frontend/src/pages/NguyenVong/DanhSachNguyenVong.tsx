import { useEffect, useState } from 'react'
import { nguyenVongApi } from '../../api/nguyenVongApi'
import { giangVienApi } from '../../api/giangVienApi'
import { hocKyApi } from '../../api/hocKyApi'
import Loading from '../../components/Loading'
import type { NguyenVong, TaoNguyenVongPayload } from '../../types/nguyenVong'
import type { GiangVien } from '../../types/giangVien'
import type { HocKy } from '../../types/hocKy'

function DanhSachNguyenVong() {
  const [danhSach, setDanhSach] = useState<NguyenVong[]>([])
  const [danhSachGiangVien, setDanhSachGiangVien] = useState<GiangVien[]>([])
  const [danhSachHocKy, setDanhSachHocKy] = useState<HocKy[]>([])
  const [hocKyChon, setHocKyChon] = useState<number | undefined>(undefined)
  const [dangTai, setDangTai] = useState(true)
  const [loi, setLoi] = useState('')

  // State Modal Thêm mới
  const [hienModal, setHienModal] = useState(false)
  const [formData, setFormData] = useState<TaoNguyenVongPayload>({
    maGiangVien: 0,
    maHocKy: 0,
    loaiNguyenVong: 'ThoiGianDay',
    mucDo: 3,
    thu: 2,
    tietBatDau: 1,
    tietKetThuc: 3,
    trongSoPhat: 10,
    noiDungGoc: '',
  })
  const [dangLuu, setDangLuu] = useState(false)

  const taiDuLieu = async () => {
    try {
      setDangTai(true)
      const [resGV, resHK] = await Promise.all([
        giangVienApi.getAll(),
        hocKyApi.getAll().catch(() => ({ data: [] as HocKy[] })),
      ])
      setDanhSachGiangVien(resGV.data)
      setDanhSachHocKy(resHK.data)

      const defaultHk = resHK.data.length > 0 ? resHK.data[0].maHocKy : undefined
      const selectedHk = hocKyChon || defaultHk
      setHocKyChon(selectedHk)

      const resNV = await nguyenVongApi.getAll(selectedHk)
      setDanhSach(resNV.data)
      setLoi('')
    } catch {
      setLoi('Không tải được danh sách nguyện vọng giảng viên.')
    } finally {
      setDangTai(false)
    }
  }

  useEffect(() => {
    void taiDuLieu()
  }, [hocKyChon])

  const handleMoModalTaoMoi = () => {
    setFormData({
      maGiangVien: danhSachGiangVien.length > 0 ? danhSachGiangVien[0].maGiangVien : 0,
      maHocKy: hocKyChon || (danhSachHocKy.length > 0 ? danhSachHocKy[0].maHocKy : 1),
      loaiNguyenVong: 'ThoiGianDay',
      mucDo: 3,
      thu: 2,
      tietBatDau: 1,
      tietKetThuc: 3,
      trongSoPhat: 10,
      noiDungGoc: 'Ưu tiên xếp lớp ca sáng',
    })
    setHienModal(true)
  }

  const handleLuu = async (e: React.FormEvent) => {
    e.preventDefault()
    if (!formData.maGiangVien || !formData.maHocKy) {
      alert('Vui lòng chọn Giảng viên và Học kỳ!')
      return
    }

    try {
      setDangLuu(true)
      await nguyenVongApi.create(formData)
      setHienModal(false)
      const res = await nguyenVongApi.getAll(hocKyChon)
      setDanhSach(res.data)
    } catch (err: unknown) {
      const msg = err instanceof Error ? err.message : 'Lỗi không xác định'
      alert('Không thể đăng ký nguyện vọng: ' + msg)
    } finally {
      setDangLuu(false)
    }
  }

  const handleXoa = async (id: number) => {
    if (!window.confirm('Bạn có chắc muốn xóa nguyện vọng này?')) return
    try {
      await nguyenVongApi.delete(id)
      const res = await nguyenVongApi.getAll(hocKyChon)
      setDanhSach(res.data)
    } catch {
      alert('Không thể xóa nguyện vọng.')
    }
  }

  return (
    <section>
      <div className="d-flex justify-content-between align-items-center mb-3">
        <div>
          <h2 className="h4 mb-1">Nguyện vọng Giảng viên</h2>
          <p className="text-secondary small mb-0">
            Quản lý và đăng ký nguyện vọng thời gian giảng dạy, ca học mong muốn theo Học kỳ
          </p>
        </div>
        <button className="btn btn-primary" onClick={handleMoModalTaoMoi}>
          + Đăng ký nguyện vọng
        </button>
      </div>

      <div className="card mb-3 shadow-sm">
        <div className="card-body py-2">
          <div className="row g-2 align-items-center">
            <div className="col-md-4">
              <label className="form-label small fw-semibold me-2 mb-0">Học kỳ:</label>
              <select
                className="form-select form-select-sm d-inline-block w-auto"
                value={hocKyChon || ''}
                onChange={(e) => setHocKyChon(Number(e.target.value))}
              >
                {danhSachHocKy.map((hk) => (
                  <option key={hk.maHocKy} value={hk.maHocKy}>
                    {hk.tenHocKy} ({hk.tenNamHoc || 'Năm học'})
                  </option>
                ))}
              </select>
            </div>
            <div className="col-md-8 text-end text-muted small">
              Tổng cộng: <strong>{danhSach.length}</strong> nguyện vọng đã đăng ký
            </div>
          </div>
        </div>
      </div>

      {dangTai && <Loading />}
      {loi && <div className="alert alert-warning">{loi}</div>}

      {!dangTai && !loi && (
        <div className="table-responsive bg-white rounded shadow-sm">
          <table className="table table-hover table-bordered align-middle mb-0">
            <thead className="table-light">
              <tr>
                <th style={{ width: '50px' }}>STT</th>
                <th>Giảng viên</th>
                <th>Loại nguyện vọng</th>
                <th>Thứ</th>
                <th>Khung tiết</th>
                <th>Mức ưu tiên</th>
                <th>Ghi chú / Nội dung</th>
                <th style={{ width: '90px' }} className="text-center">Thao tác</th>
              </tr>
            </thead>
            <tbody>
              {danhSach.map((item, idx) => (
                <tr key={item.maNguyenVong}>
                  <td>{idx + 1}</td>
                  <td className="fw-semibold">{item.tenGiangVien}</td>
                  <td>
                    <span className="badge bg-info text-dark">
                      {item.loaiNguyenVong === 'ThoiGianDay' ? 'Thời gian dạy' : 'Môn giảng dạy'}
                    </span>
                  </td>
                  <td>{item.thu ? `Thứ ${item.thu}` : 'Tất cả'}</td>
                  <td>
                    {item.tietBatDau && item.tietKetThuc
                      ? `Tiết ${item.tietBatDau} - ${item.tietKetThuc}`
                      : '-'}
                  </td>
                  <td>
                    <span className="badge bg-success">
                      Level {item.mucDo} / 5
                    </span>
                  </td>
                  <td className="small text-secondary">{item.noiDungGoc || '-'}</td>
                  <td className="text-center">
                    <button
                      className="btn btn-sm btn-outline-danger"
                      onClick={() => handleXoa(item.maNguyenVong)}
                      title="Xóa"
                    >
                      Xóa
                    </button>
                  </td>
                </tr>
              ))}
              {danhSach.length === 0 && (
                <tr>
                  <td colSpan={8} className="text-center text-secondary py-4">
                    Chưa có nguyện vọng nào được đăng ký trong học kỳ này.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      )}

      {/* Modal đăng ký nguyện vọng */}
      {hienModal && (
        <div className="modal d-block" style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
          <div className="modal-dialog modal-dialog-centered">
            <div className="modal-content shadow">
              <div className="modal-header">
                <h5 className="modal-title">Đăng ký Nguyện vọng Giảng dạy</h5>
                <button type="button" className="btn-close" onClick={() => setHienModal(false)}></button>
              </div>
              <form onSubmit={handleLuu}>
                <div className="modal-body">
                  <div className="mb-3">
                    <label className="form-label fw-semibold">Giảng viên <span className="text-danger">*</span></label>
                    <select
                      className="form-select"
                      value={formData.maGiangVien}
                      onChange={(e) => setFormData({ ...formData, maGiangVien: Number(e.target.value) })}
                      required
                    >
                      <option value="">-- Chọn giảng viên --</option>
                      {danhSachGiangVien.map((gv) => (
                        <option key={gv.maGiangVien} value={gv.maGiangVien}>
                          {gv.hoTen} ({gv.email})
                        </option>
                      ))}
                    </select>
                  </div>
                  <div className="row">
                    <div className="col-md-6 mb-3">
                      <label className="form-label fw-semibold">Thứ mong muốn</label>
                      <select
                        className="form-select"
                        value={formData.thu || 2}
                        onChange={(e) => setFormData({ ...formData, thu: Number(e.target.value) })}
                      >
                        <option value={2}>Thứ 2</option>
                        <option value={3}>Thứ 3</option>
                        <option value={4}>Thứ 4</option>
                        <option value={5}>Thứ 5</option>
                        <option value={6}>Thứ 6</option>
                        <option value={7}>Thứ 7</option>
                      </select>
                    </div>
                    <div className="col-md-6 mb-3">
                      <label className="form-label fw-semibold">Mức độ ưu tiên (1-5)</label>
                      <select
                        className="form-select"
                        value={formData.mucDo}
                        onChange={(e) => setFormData({ ...formData, mucDo: Number(e.target.value) })}
                      >
                        <option value={1}>1 - Thấp</option>
                        <option value={2}>2 - Trung bình thấp</option>
                        <option value={3}>3 - Bình thường</option>
                        <option value={4}>4 - Cao</option>
                        <option value={5}>5 - Rất cao (Rất mong muốn)</option>
                      </select>
                    </div>
                  </div>
                  <div className="row">
                    <div className="col-md-6 mb-3">
                      <label className="form-label fw-semibold">Tiết bắt đầu</label>
                      <input
                        type="number"
                        className="form-control"
                        min={1}
                        max={16}
                        value={formData.tietBatDau || 1}
                        onChange={(e) => setFormData({ ...formData, tietBatDau: Number(e.target.value) })}
                      />
                    </div>
                    <div className="col-md-6 mb-3">
                      <label className="form-label fw-semibold">Tiết kết thúc</label>
                      <input
                        type="number"
                        className="form-control"
                        min={1}
                        max={16}
                        value={formData.tietKetThuc || 3}
                        onChange={(e) => setFormData({ ...formData, tietKetThuc: Number(e.target.value) })}
                      />
                    </div>
                  </div>
                  <div className="mb-3">
                    <label className="form-label fw-semibold">Nội dung ghi chú</label>
                    <textarea
                      className="form-control"
                      rows={2}
                      value={formData.noiDungGoc || ''}
                      onChange={(e) => setFormData({ ...formData, noiDungGoc: e.target.value })}
                      placeholder="Ví dụ: Ưu tiên dạy buổi sáng thứ 2 và thứ 4"
                    />
                  </div>
                </div>
                <div className="modal-footer">
                  <button type="button" className="btn btn-secondary" onClick={() => setHienModal(false)}>
                    Hủy
                  </button>
                  <button type="submit" className="btn btn-primary" disabled={dangLuu}>
                    {dangLuu ? 'Đang lưu...' : 'Đăng ký nguyện vọng'}
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

export default DanhSachNguyenVong
