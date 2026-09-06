import { useEffect, useState } from 'react'
import { giangVienApi } from '../../api/giangVienApi'
import Loading from '../../components/Loading'
import type { GiangVien } from '../../types/giangVien'

function DanhSachGiangVien() {
  const [danhSach, setDanhSach] = useState<GiangVien[]>([])
  const [dangTai, setDangTai] = useState(true)
  const [loi, setLoi] = useState('')

  useEffect(() => {
    const taiDuLieu = async () => {
      try {
        const response = await giangVienApi.getAll()
        setDanhSach(response.data)
      } catch {
        setLoi('Khong tai duoc danh sach giang vien.')
      } finally {
        setDangTai(false)
      }
    }

    void taiDuLieu()
  }, [])

  return (
    <section>
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h2 className="h4 mb-0">Giang vien</h2>
      </div>

      {dangTai && <Loading />}
      {loi && <div className="alert alert-warning">{loi}</div>}

      {!dangTai && !loi && (
        <div className="table-responsive">
          <table className="table table-striped table-bordered align-middle">
            <thead>
              <tr>
                <th>Ho ten</th>
                <th>Email</th>
                <th>Trang thai</th>
              </tr>
            </thead>
            <tbody>
              {danhSach.map((giangVien) => (
                <tr key={giangVien.maGiangVien}>
                  <td>{giangVien.hoTen}</td>
                  <td>{giangVien.email}</td>
                  <td>{giangVien.trangThai}</td>
                </tr>
              ))}
              {danhSach.length === 0 && (
                <tr>
                  <td colSpan={3} className="text-center text-secondary">
                    Chua co giang vien.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      )}
    </section>
  )
}

export default DanhSachGiangVien
