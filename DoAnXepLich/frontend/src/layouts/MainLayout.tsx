import { Outlet } from 'react-router-dom'
import Header from '../components/Header'
import Sidebar from '../components/Sidebar'

function MainLayout() {
  return (
    <div className="app-shell">
      <Sidebar />
      <div className="main-panel">
        <Header />
        <main className="p-4">
          <Outlet />
        </main>
      </div>
    </div>
  )
}

export default MainLayout
