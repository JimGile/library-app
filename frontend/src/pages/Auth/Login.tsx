import React, { useState } from 'react'
import { login } from '../../services/auth'
import { useNavigate } from 'react-router-dom'

export default function Login() {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState<string | null>(null)
  const nav = useNavigate()

  const submit = async (e: React.FormEvent) => {
    e.preventDefault()
    try {
      await login(username, password)
      nav('/')
    } catch (err: any) {
      setError(err?.response?.data || 'Login failed')
    }
  }

  return (
    <div className="col-12 col-md-6 mx-auto">
      <div className="card">
        <div className="card-body">
          <h2 className="card-title mb-3">Login</h2>
          <form onSubmit={submit}>
            <div className="mb-3">
              <input className="form-control" placeholder="Username" value={username} onChange={(e) => setUsername(e.target.value)} />
            </div>
            <div className="mb-3">
              <input className="form-control" placeholder="Password" type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
            </div>
            <button className="btn btn-primary" type="submit">Login</button>
          </form>
          {error && <div className="mt-3 text-danger">{String(error)}</div>}
        </div>
      </div>
    </div>
  )
}
