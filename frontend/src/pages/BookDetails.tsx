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
    <div>
      <h2>{book.title}</h2>
      <p>
        <strong>Author:</strong> {book.author}
      </p>
      <p>
        <strong>Category:</strong> {book.category?.name}
      </p>
      <p>{book.description}</p>
      <p>
        <em>{book.is_available ? 'Available' : 'Not available'}</em>
      </p>
    </div>
  )
}
