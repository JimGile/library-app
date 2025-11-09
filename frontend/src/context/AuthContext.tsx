import React, { createContext, useContext, useEffect, useState } from 'react'
import { getToken, logout as doLogout } from '../services/auth'
import api from '../services/api'

type AuthContextShape = {
  token: string | null
  logout: () => void
}

const AuthContext = createContext<AuthContextShape | undefined>(undefined)

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [token, setToken] = useState<string | null>(null)

  useEffect(() => {
    const t = getToken()
    if (t) {
      setToken(t)
      api.defaults.headers.common['Authorization'] = `Token ${t}`
    }
  }, [])

  const logout = () => {
    doLogout()
    setToken(null)
  }

  return <AuthContext.Provider value={{ token, logout }}>{children}</AuthContext.Provider>
}

export function useAuth() {
  const ctx = useContext(AuthContext)
  if (!ctx) throw new Error('useAuth must be used within AuthProvider')
  return ctx
}
