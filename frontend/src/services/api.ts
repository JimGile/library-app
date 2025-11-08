import axios from 'axios'

const api = axios.create({
  baseURL: '/api',
})

export async function fetchBooks(page = 1, pageSize = 10, search = '', category = '') {
  const params: any = { page, page_size: pageSize }
  if (search) params.search = search
  if (category) params.category = category
  const resp = await api.get('/books/', { params })
  return resp.data
}

export async function fetchBook(id: number) {
  const resp = await api.get(`/books/${id}/`)
  return resp.data
}

export default api
