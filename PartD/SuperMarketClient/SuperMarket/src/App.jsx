import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import MainPage from './Components/Features/Global/MainPage'
import SupplierPage from './Components/Features/Supplier/SupplierPage'
import Login from './Components/Features/Global/Login'
import OrderList from './Components/Features/Global/OrdersList'
import HomePage from './Components/Features/Global/HomePage'
import { HeadphonesSharp } from '@mui/icons-material'
import Register from './Components/Features/Supplier/Register'
import StepsOrder from './Components/Features/Grocer/StepsOrder'
function App() {

  return (
    <>
    <Router>
      <Routes>
        <Route path="/" element={<MainPage />} />
        <Route path="/supplier" element={<SupplierPage />} />
        <Route path="/orderList" element={<OrderList />} />
        <Route path="/register" element={<Register />} />
        <Route path="/login" element={<Login />} />
        <Route path="/home" element={<HomePage/>} />
        <Route path="/create-order" element={<StepsOrder/>} />
      </Routes>
    </Router>
    </>
  )
}

export default App
