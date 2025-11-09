import api from './api'

const TOKEN_KEY = 'library_token'

export async function register(username: string, email: string, password: string) {
  const resp = await api.post('/auth/register/', { username, email, password })
  const token = resp.data.token
  if (token) {
    localStorage.setItem(TOKEN_KEY, token)
    api.defaults.headers.common['Authorization'] = `Token ${token}`
  }
  return resp.data
}

export async function login(username: string, password: string) {
  const resp = await api.post('/auth/login/', { username, password })
  const token = resp.data.token
  if (token) {
    localStorage.setItem(TOKEN_KEY, token)
    api.defaults.headers.common['Authorization'] = `Token ${token}`
  }
  return resp.data
}

export function logout() {
  localStorage.removeItem(TOKEN_KEY)
  delete api.defaults.headers.common['Authorization']
}

export function getToken() {
  return localStorage.getItem(TOKEN_KEY)
}
