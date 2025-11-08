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

  if (loading) return <div>Loading books…</div>

  return (
    <div>
      <h2>Books</h2>
      <div style={{ marginBottom: 12 }}>
        <input
          placeholder="Search title or author"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          onKeyDown={(e) => {
            if (e.key === 'Enter') load(1, (e.target as HTMLInputElement).value, pageSize)
          }}
          style={{ marginRight: 8 }}
        />
        <button onClick={() => load(1, search, pageSize)}>Search</button>
        <label style={{ marginLeft: 12 }}>
          Page size:
          <select value={pageSize} onChange={(e) => setPageSize(Number(e.target.value))} style={{ marginLeft: 6 }}>
            <option value={5}>5</option>
            <option value={10}>10</option>
            <option value={20}>20</option>
          </select>
        </label>
      </div>

      <ul>
        {books.map((b) => (
          <li key={b.id}>
            <Link to={`/books/${b.id}`}>{b.title}</Link> — {b.author} ({b.category})
          </li>
        ))}
      </ul>

      <div style={{ marginTop: 12 }}>
        <button onClick={() => previous && load(page - 1, search, pageSize)} disabled={!previous}>
          Previous
        </button>
        <span style={{ margin: '0 8px' }}>
          Page {page} — {count} results
        </span>
        <button onClick={() => next && load(page + 1, search, pageSize)} disabled={!next}>
          Next
        </button>
      </div>
    </div>
  )
}
