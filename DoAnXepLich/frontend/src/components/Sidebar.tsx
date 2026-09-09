import { NavLink } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext'

const adminMenuItems = [
  { path: '/tai-khoan', label: 'Quản lý tài khoản' },
  { path: '/nam-hoc', label: 'Quản lý năm học' },
  { path: '/hoc-ky', label: 'Quản lý học kỳ' },
  { path: '/bo-mon', label: 'Quản lý bộ môn' },
  { path: '/giang-vien', label: 'Quản lý giảng viên' },
  { path: '/hoc-phan', label: 'Quản lý học phần' },
  { path: '/thoi-khoa-bieu', label: 'Thời khóa biểu (Import)' },
  { path: '/nguyen-vong', label: 'Quản lý nguyện vọng' },
  { path: '/rang-buoc', label: 'Cấu hình ràng buộc' },
  { path: '/xep-lich', label: 'Xếp lịch tự động' },
]

const truongBoMonMenuItems = [
  { path: '/thoi-khoa-bieu', label: 'Thời khóa biểu' },
  { path: '/giang-vien', label: 'Quản lý giảng viên' },
  { path: '/hoc-phan', label: 'Quản lý học phần' },
  { path: '/nguyen-vong', label: 'Nguyện vọng giảng viên' },
  { path: '/rang-buoc', label: 'Cấu hình ràng buộc' },
  { path: '/xep-lich', label: 'Xếp lịch tự động' },
]

const giangVienMenuItems = [
  { path: '/thoi-khoa-bieu', label: 'Thời khóa biểu' },
  { path: '/nguyen-vong', label: 'Đăng ký nguyện vọng' },
  { path: '/xep-lich', label: 'Xem lịch phân công' },
]

function Sidebar() {
  const { user } = useAuth()
  let items = truongBoMonMenuItems
  if (user?.vaiTro === 'Admin') {
    items = adminMenuItems
  } else if (user?.vaiTro === 'GiangVien') {
    items = giangVienMenuItems
  }

  return (
    <aside className="sidebar border-end bg-light p-3" style={{ minWidth: '220px' }}>
      <div className="fw-semibold mb-3 fs-5 text-primary">Đồ Án Xếp Lịch</div>
      <nav className="nav nav-pills flex-column gap-1">
        {items.map((item) => (
          <NavLink
            key={item.path}
            to={item.path}
            className={({ isActive }) =>
              `nav-link ${isActive ? 'active' : 'text-dark'}`
            }
          >
            {item.label}
          </NavLink>
        ))}
      </nav>
    </aside>
  )
}

export default Sidebar
