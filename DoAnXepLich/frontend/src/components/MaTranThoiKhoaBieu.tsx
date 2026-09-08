import React, { useState } from 'react';
import type { LopHocPhanTKB, ChiTietPhanCong } from '../types/kieuDuLieuXepLich';

interface Props {
  danhSachLop: LopHocPhanTKB[];
  danhSachPhanCong: ChiTietPhanCong[];
  onKhoaGiangVien: (maLop: string, maGV: string, tenGV: string) => void;
}

export const MaTranThoiKhoaBieu: React.FC<Props> = ({
  danhSachLop,
  danhSachPhanCong,
  onKhoaGiangVien
}) => {
  const cacNgayTrongTuan = [2, 3, 4, 5, 6, 7];
  const cacCaTiet = [1, 4, 7, 10];
  const [lopDuocChon, setLopDuocChon] = useState<LopHocPhanTKB | null>(null);
  const [tenGVTam, setTenGVTam] = useState('');

  const layPhanCongCuaLop = (maLop: string) =>
    danhSachPhanCong.find(a => a.maLopHocPhan === maLop);

  return (
    <div style={{ marginTop: '20px' }}>
      <h3>📅 MA TRẬN THỜI KHÓA BIỂU PHÂN CÔNG GIẢNG VIÊN</h3>
      <table
        style={{
          width: '100%',
          borderCollapse: 'collapse',
          textAlign: 'center',
          border: '1px solid #d9d9d9'
        }}
      >
        <thead style={{ backgroundColor: '#fafafa' }}>
          <tr>
            <th style={{ padding: '10px', border: '1px solid #d9d9d9' }}>Tiết / Ca</th>
            {cacNgayTrongTuan.map(ngay => (
              <th key={ngay} style={{ padding: '10px', border: '1px solid #d9d9d9' }}>
                Thứ {ngay}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {cacCaTiet.map(tiet => (
            <tr key={tiet}>
              <td
                style={{
                  fontWeight: 'bold',
                  backgroundColor: '#f0f0f0',
                  padding: '10px',
                  border: '1px solid #d9d9d9'
                }}
              >
                Tiết {tiet} - {tiet + 2}
              </td>
              {cacNgayTrongTuan.map(ngay => {
                const lopTrongKhungGio = danhSachLop.find(
                  c => c.thuTrongTuan === ngay && c.tietBatDau === tiet
                );

                if (!lopTrongKhungGio) {
                  return (
                    <td
                      key={ngay}
                      style={{
                        backgroundColor: '#fff',
                        padding: '10px',
                        border: '1px solid #d9d9d9'
                      }}
                    >
                      -
                    </td>
                  );
                }

                const phanCong = layPhanCongCuaLop(lopTrongKhungGio.maLopHocPhan);

                return (
                  <td
                    key={ngay}
                    style={{
                      backgroundColor: phanCong?.daKhoaThuCong ? '#fffbe6' : '#e6f7ff',
                      padding: '10px',
                      border: '1px solid #d9d9d9'
                    }}
                  >
                    <div style={{ fontWeight: 'bold' }}>{lopTrongKhungGio.tenLopHocPhan}</div>
                    <div style={{ fontSize: '12px', color: '#666' }}>
                      Phòng: {lopTrongKhungGio.maPhong}
                    </div>
                    <div style={{ marginTop: '6px', color: '#1890ff', fontWeight: 'bold' }}>
                      👨‍🏫 {phanCong ? phanCong.tenGV : 'Chưa xếp'}{' '}
                      {phanCong?.daKhoaThuCong && '🔒'}
                    </div>
                    <button
                      onClick={() => {
                        setLopDuocChon(lopTrongKhungGio);
                        setTenGVTam(phanCong?.tenGV || '');
                      }}
                      style={{
                        marginTop: '6px',
                        fontSize: '11px',
                        cursor: 'pointer',
                        padding: '3px 8px',
                        borderRadius: '4px',
                        border: '1px solid #1890ff',
                        backgroundColor: '#fff',
                        color: '#1890ff'
                      }}
                    >
                      Đổi GV & Khóa
                    </button>
                  </td>
                );
              })}
            </tr>
          ))}
        </tbody>
      </table>

      {/* Popup Đổi Giảng viên Bán tự động */}
      {lopDuocChon && (
        <div
          style={{
            position: 'fixed',
            top: '30%',
            left: '35%',
            backgroundColor: '#fff',
            padding: '20px',
            border: '2px solid #1890ff',
            borderRadius: '8px',
            boxShadow: '0 4px 12px rgba(0,0,0,0.15)',
            zIndex: 1000
          }}
        >
          <h4 style={{ marginTop: 0 }}>Can thiệp bán tự động: {lopDuocChon.tenLopHocPhan}</h4>
          <p style={{ fontSize: '13px', margin: '8px 0' }}>Nhập tên GV muốn chỉ định:</p>
          <input
            type="text"
            value={tenGVTam}
            onChange={e => setTenGVTam(e.target.value)}
            style={{ padding: '6px', width: '90%', marginBottom: '12px' }}
          />
          <br />
          <button
            onClick={() => {
              onKhoaGiangVien(lopDuocChon.maLopHocPhan, 'GV_TU_CHON', tenGVTam);
              setLopDuocChon(null);
            }}
            style={{
              padding: '6px 14px',
              backgroundColor: '#52c41a',
              color: '#fff',
              border: 'none',
              borderRadius: '4px',
              cursor: 'pointer',
              fontWeight: 'bold'
            }}
          >
            🔒 Xác nhận & Khóa
          </button>
          <button
            onClick={() => setLopDuocChon(null)}
            style={{
              marginLeft: '10px',
              padding: '6px 14px',
              borderRadius: '4px',
              border: '1px solid #ccc',
              cursor: 'pointer'
            }}
          >
            Đóng
          </button>
        </div>
      )}
    </div>
  );
};