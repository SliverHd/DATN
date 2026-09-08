import React, { useState } from 'react';

interface Props {
  onChayXepLich: (maHocKi: string, thuatToan: string) => void;
  dangXuLy: boolean;
  thoiGianChayMs?: number;
}

export const BangDieuKhienXepLich: React.FC<Props> = ({
  onChayXepLich,
  dangXuLy,
  thoiGianChayMs
}) => {
  const [maHocKi, setMaHocKi] = useState('HK1-2026-2027');
  const [thuatToan, setThuatToan] = useState('CPSAT');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onChayXepLich(maHocKi, thuatToan);
  };

  return (
    <div
      style={{
        padding: '16px',
        backgroundColor: '#f8f9fa',
        border: '1px solid #dee2e6',
        borderRadius: '8px',
        marginBottom: '20px'
      }}
    >
      <h3 style={{ margin: '0 0 12px 0', fontSize: '16px', color: '#333' }}>
        ⚙️ BẢNG ĐIỀU KHIỂN XẾP THỜI KHÓA BIỂU
      </h3>

      <form
        onSubmit={handleSubmit}
        style={{ display: 'flex', flexWrap: 'wrap', gap: '20px', alignItems: 'flex-end' }}
      >
        {/* 1. Chọn học kỳ */}
        <div>
          <label style={{ display: 'block', fontSize: '13px', fontWeight: 'bold', marginBottom: '4px' }}>
            Học kỳ:
          </label>
          <select
            value={maHocKi}
            onChange={e => setMaHocKi(e.target.value)}
            style={{ padding: '6px 12px', borderRadius: '4px', border: '1px solid #ccc' }}
          >
            <option value="HK1-2026-2027">Học kỳ 1 (2026 - 2027)</option>
            <option value="HK2-2026-2027">Học kỳ 2 (2026 - 2027)</option>
          </select>
        </div>

        {/* 2. Chọn thuật toán */}
        <div>
          <label style={{ display: 'block', fontSize: '13px', fontWeight: 'bold', marginBottom: '4px' }}>
            Thuật toán:
          </label>
          <select
            value={thuatToan}
            onChange={e => setThuatToan(e.target.value)}
            style={{ padding: '6px 12px', borderRadius: '4px', border: '1px solid #1890ff', fontWeight: 'bold' }}
          >
            <option value="CPSAT">Google CP-SAT (SV2 - Mặc định)</option>
            <option value="GA">Genetic Algorithm (SV1)</option>
            <option value="GREEDY">Greedy (Đối chứng)</option>
          </select>
        </div>

        {/* 3. Đo lường thời gian chạy thực tế */}
        <div>
          <label style={{ display: 'block', fontSize: '13px', fontWeight: 'bold', marginBottom: '4px' }}>
            Thời gian thuật toán chạy:
          </label>
          <div
            style={{
              padding: '6px 14px',
              backgroundColor: '#fff',
              border: '1px solid #d9d9d9',
              borderRadius: '4px',
              fontWeight: 'bold',
              color: thoiGianChayMs !== undefined ? '#389e0d' : '#888',
              minWidth: '110px'
            }}
          >
            {thoiGianChayMs !== undefined ? `⚡ ${thoiGianChayMs} ms` : 'Chưa chạy'}
          </div>
        </div>

        {/* Nút bấm kích hoạt */}
        <div>
          <button
            type="submit"
            disabled={dangXuLy}
            style={{
              padding: '8px 24px',
              backgroundColor: dangXuLy ? '#bfbfbf' : '#1890ff',
              color: '#fff',
              border: 'none',
              borderRadius: '4px',
              fontWeight: 'bold',
              cursor: dangXuLy ? 'not-allowed' : 'pointer'
            }}
          >
            {dangXuLy ? 'Đang giải...' : '🚀 Bắt đầu xếp lịch'}
          </button>
        </div>
      </form>
    </div>
  );
};