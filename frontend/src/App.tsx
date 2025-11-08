import React from 'react'
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom'
import BookList from './pages/BookList'
import BookDetails from './pages/BookDetails'

export default function App() {
  return (
    <BrowserRouter>
      <div>
        <header>
          <h1>
            <Link to="/">Library</Link>
          </h1>
        </header>
        <main>
          <Routes>
            <Route path="/" element={<BookList />} />
            <Route path="/books/:id" element={<BookDetails />} />
          </Routes>
        </main>
      </div>
    </BrowserRouter>
  )
}
