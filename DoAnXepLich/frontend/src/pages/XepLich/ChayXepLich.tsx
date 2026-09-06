import { useState, type FormEvent } from 'react'
import { xepLichApi } from '../../api/xepLichApi'
import type { KetQuaXepLich, YeuCauXepLich } from '../../types/xepLich'

function ChayXepLich() {
  const [form, setForm] = useState<YeuCauXepLich>({
    maHocKy: 1,
    thuatToan: 'GA',
    kichThuocQuanThe: 50,
    soTheHe: 100,
    tyLeLaiGhep: 0.8,
    tyLeDotBien: 0.05,
  })
  const [ketQua, setKetQua] = useState<KetQuaXepLich | null>(null)
  const [dangChay, setDangChay] = useState(false)
  const [loi, setLoi] = useState('')

  const capNhatSo = (name: keyof YeuCauXepLich, value: string) => {
    setForm((current) => ({
      ...current,
      [name]: value === '' ? undefined : Number(value),
    }))
  }

  const chayXepLich = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    setDangChay(true)
    setLoi('')
    setKetQua(null)

    const payload =
      form.thuatToan === 'CP_SAT'
        ? { maHocKy: form.maHocKy, thuatToan: form.thuatToan }
        : form

    try {
      const response = await xepLichApi.chayXepLich(payload)
      setKetQua(response.data)
    } catch {
      setLoi('Khong chay duoc thuat toan xep lich.')
    } finally {
      setDangChay(false)
    }
  }

  return (
    <section>
      <h2 className="h4 mb-3">Chay xep lich</h2>

      <form className="row g-3" onSubmit={chayXepLich}>
        <div className="col-md-4">
          <label className="form-label" htmlFor="maHocKy">
            Hoc ky
          </label>
          <input
            id="maHocKy"
            type="number"
            min={1}
            className="form-control"
            value={form.maHocKy}
            onChange={(event) => capNhatSo('maHocKy', event.target.value)}
          />
        </div>

        <div className="col-md-4">
          <label className="form-label" htmlFor="thuatToan">
            Thuat toan
          </label>
          <select
            id="thuatToan"
            className="form-select"
            value={form.thuatToan}
            onChange={(event) =>
              setForm((current) => ({
                ...current,
                thuatToan: event.target.value as YeuCauXepLich['thuatToan'],
              }))
            }
          >
            <option value="GA">Genetic Algorithm</option>
            <option value="CP_SAT">CP-SAT</option>
          </select>
        </div>

        {form.thuatToan === 'GA' && (
          <>
            <div className="col-md-4">
              <label className="form-label" htmlFor="kichThuocQuanThe">
                Kich thuoc quan the
              </label>
              <input
                id="kichThuocQuanThe"
                type="number"
                min={1}
                className="form-control"
                value={form.kichThuocQuanThe ?? ''}
                onChange={(event) =>
                  capNhatSo('kichThuocQuanThe', event.target.value)
                }
              />
            </div>

            <div className="col-md-4">
              <label className="form-label" htmlFor="soTheHe">
                So the he
              </label>
              <input
                id="soTheHe"
                type="number"
                min={1}
                className="form-control"
                value={form.soTheHe ?? ''}
                onChange={(event) => capNhatSo('soTheHe', event.target.value)}
              />
            </div>

            <div className="col-md-4">
              <label className="form-label" htmlFor="tyLeLaiGhep">
                Ty le lai ghep
              </label>
              <input
                id="tyLeLaiGhep"
                type="number"
                min={0}
                max={1}
                step={0.01}
                className="form-control"
                value={form.tyLeLaiGhep ?? ''}
                onChange={(event) =>
                  capNhatSo('tyLeLaiGhep', event.target.value)
                }
              />
            </div>

            <div className="col-md-4">
              <label className="form-label" htmlFor="tyLeDotBien">
                Ty le dot bien
              </label>
              <input
                id="tyLeDotBien"
                type="number"
                min={0}
                max={1}
                step={0.01}
                className="form-control"
                value={form.tyLeDotBien ?? ''}
                onChange={(event) =>
                  capNhatSo('tyLeDotBien', event.target.value)
                }
              />
            </div>
          </>
        )}

        <div className="col-12">
          <button type="submit" className="btn btn-primary" disabled={dangChay}>
            {dangChay ? 'Dang chay...' : 'Chay xep lich'}
          </button>
        </div>
      </form>

      {loi && <div className="alert alert-warning mt-3">{loi}</div>}

      {ketQua && (
        <div className="border rounded p-3 mt-4">
          <h3 className="h5">Ket qua tam</h3>
          <p className="mb-1">Thuat toan: {ketQua.thuatToan}</p>
          <p className="mb-1">Tong diem phat: {ketQua.tongDiemPhat}</p>
          <p className="mb-0">Trang thai: {ketQua.trangThai}</p>
        </div>
      )}
    </section>
  )
}

export default ChayXepLich
