import React, { useEffect, useState } from 'react'
import { fetchBooks } from '../services/api'
import { Link } from 'react-router-dom'

export default function BookList() {
  const [books, setBooks] = useState<any[]>([])
  const [loading, setLoading] = useState(true)
  const [page, setPage] = useState(1)
  const [pageSize, setPageSize] = useState(10)
  const [count, setCount] = useState(0)
  const [next, setNext] = useState<string | null>(null)
  const [previous, setPrevious] = useState<string | null>(null)
  const [search, setSearch] = useState('')

  const load = (p = page, s = search, ps = pageSize) => {
    setLoading(true)
    fetchBooks(p, ps, s)
      .then((data) => {
        setBooks(data.results || [])
        setCount(data.count || 0)
        setNext(data.next || null)
        setPrevious(data.previous || null)
        setPage(p)
      })
      .finally(() => setLoading(false))
  }

  useEffect(() => {
    load(1, search, pageSize)
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [pageSize])

  return (
    <div>
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h2 className="mb-0">Books</h2>
        <div className="text-muted">{count} results</div>
      </div>

      <div className="card mb-3">
        <div className="card-body">
          <div className="row g-2">
            <div className="col-md-6">
              <input
                className="form-control"
                placeholder="Search title or author"
                value={search}
                onChange={(e) => setSearch(e.target.value)}
                onKeyDown={(e) => {
                  if (e.key === 'Enter') load(1, (e.target as HTMLInputElement).value, pageSize)
                }}
              />
            </div>
            <div className="col-auto">
              <button className="btn btn-primary" onClick={() => load(1, search, pageSize)}>
                Search
              </button>
            </div>
            <div className="col-auto ms-auto">
              <label htmlFor="pageSizeSelect" className="form-label me-2 mb-0">Page size:</label>
              <select id="pageSizeSelect" className="form-select d-inline-block w-auto" value={pageSize} onChange={(e) => setPageSize(Number(e.target.value))} aria-label="Page size">
                <option value={5}>5</option>
                <option value={10}>10</option>
                <option value={20}>20</option>
              </select>
            </div>
          </div>
        </div>
      </div>

      {loading ? (
        <div className="text-center py-5">
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Loading...</span>
          </div>
        </div>
      ) : (
        <div className="row row-cols-1 row-cols-md-2 g-3">
          {books.map((b) => (
            <div key={b.id} className="col">
              <div className="card h-100">
                <div className="card-body">
                  <h5 className="card-title">
                    <Link to={`/books/${b.id}`} className="stretched-link text-decoration-none">
                      {b.title}
                    </Link>
                  </h5>
                  <h6 className="card-subtitle mb-2 text-muted">{b.author}</h6>
                  <p className="card-text text-truncate">{b.description}</p>
                </div>
                <div className="card-footer d-flex justify-content-between align-items-center">
                  <small className="text-muted">{b.category?.name}</small>
                  <span className={`badge ${b.is_available ? 'bg-success' : 'bg-secondary'}`}>{b.is_available ? 'Available' : 'Unavailable'}</span>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

      <div className="d-flex justify-content-between align-items-center mt-4">
        <button className="btn btn-outline-secondary" onClick={() => previous && load(page - 1, search, pageSize)} disabled={!previous}>
          Previous
        </button>
        <div>
          Page {page} — {count} results
        </div>
        <button className="btn btn-outline-secondary" onClick={() => next && load(page + 1, search, pageSize)} disabled={!next}>
          Next
        </button>
      </div>
    </div>
  )
}
