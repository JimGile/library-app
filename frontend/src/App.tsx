import React from 'react'
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom'
import BookList from './pages/BookList'
import BookDetails from './pages/BookDetails'
import Register from './pages/Auth/Register'
import Login from './pages/Auth/Login'
import { AuthProvider } from './context/AuthContext'

export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <div>
          <nav className="navbar navbar-expand-lg navbar-dark bg-dark mb-4">
            <div className="container">
              <Link className="navbar-brand" to="/">
                Library
              </Link>
              <button
                className="navbar-toggler"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target="#navbars"
                aria-controls="navbars"
                aria-expanded="false"
                aria-label="Toggle navigation"
              >
                <span className="navbar-toggler-icon" />
              </button>

              <div className="collapse navbar-collapse" id="navbars">
                <ul className="navbar-nav ms-auto">
                  <li className="nav-item">
                    <Link className="nav-link" to="/register">
                      Register
                    </Link>
                  </li>
                  <li className="nav-item">
                    <Link className="nav-link" to="/login">
                      Login
                    </Link>
                  </li>
                </ul>
              </div>
            </div>
          </nav>

          <main className="container">
            <Routes>
              <Route path="/" element={<BookList />} />
              <Route path="/books/:id" element={<BookDetails />} />
              <Route path="/register" element={<Register />} />
              <Route path="/login" element={<Login />} />
            </Routes>
          </main>
        </div>
      </AuthProvider>
    </BrowserRouter>
  )
}
