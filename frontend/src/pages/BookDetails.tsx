import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import { fetchBook } from '../services/api'

export default function BookDetails() {
  const { id } = useParams()
  const [book, setBook] = useState<any | null>(null)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    let mounted = true
    if (!id) return
    setLoading(true)
    fetchBook(Number(id))
      .then((data) => mounted && setBook(data))
      .finally(() => mounted && setLoading(false))
    return () => {
      mounted = false
    }
  }, [id])

  if (loading) return <div>Loading…</div>
  if (!book) return <div>Book not found</div>
  return (
    <div className="card">
      <div className="card-body">
        <h2 className="card-title">{book.title}</h2>
        <h6 className="card-subtitle mb-2 text-muted">{book.author}</h6>
        <p className="mb-1">
          <strong>Category:</strong> {book.category?.name}
        </p>
        <p className="card-text">{book.description}</p>
        <p>
          <span className={`badge ${book.is_available ? 'bg-success' : 'bg-secondary'}`}>{book.is_available ? 'Available' : 'Not available'}</span>
        </p>
      </div>
    </div>
  )
}
