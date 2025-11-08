import { defineStore } from 'pinia'
import agenteService from '@/services/agenteService'

function getAuthCookie() {
  const isProd = process.env.NODE_ENV === 'production'
  let domainOpt
  if (isProd && process.client) {
    const host = window.location.hostname
    if (host.endsWith('homesguatemala.com')) {
      domainOpt = '.homesguatemala.com'
    } else {
      domainOpt = host
    }
  }
  return useCookie('jwt-token', {
    sameSite: 'lax',
    secure: isProd,
    path: '/',
    ...(domainOpt ? { domain: domainOpt } : {})
  })
}

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: process.client ? localStorage.getItem('jwt-token') : null,
    user: null,
    status: 'idle',
  }),
  getters: {
    isAuthenticated: (s) => !!s.token && !!s.user,
    hasRole: (s) => (role) => s.user?.roles?.includes(String(role).toLowerCase()) || false,
    hasAnyRole: (s) => (roles) => roles.some(r => s.user?.roles?.includes(String(r).toLowerCase())),
  },
  actions: {
    async login(email, password) {
      this.status = 'loading'
      try {
        const data = await agenteService.login(email, password)
        const token = data?.token
        if (!token) throw new Error('Token no recibido')

        const cookie = getAuthCookie()
        cookie.value = token
        if (process.client) localStorage.setItem('jwt-token', token)
        this.token = token

        const me = await agenteService.getCurrentUser()
        const roleRaw = me.rol || me.role || 'user'
        const role = String(roleRaw).toLowerCase()
        this.user = {
          id: me.id,
          email: me.email,
          name: me.nombre || me.userName || '',
          dni: me.dni,
          roles: [role],
          raw: me,
        }
        this.status = 'authenticated'
      } catch (e) {
        this.$reset()
        const cookie = getAuthCookie()
        cookie.value = null
        if (process.client) localStorage.removeItem('jwt-token')
        throw e
      }
    },

    async hydrateFromToken() {
      if (!process.client) return
      const token = localStorage.getItem('jwt-token')
      if (!token || this.user) return
      this.status = 'loading'
      try {
        const me = await agenteService.getCurrentUser()
        const roleRaw = me.rol || me.role || 'user'
        const role = String(roleRaw).toLowerCase()
        this.token = token
        this.user = {
          id: me.id,
          email: me.email,
          name: me.nombre || me.userName || '',
          dni: me.dni,
          roles: [role],
          raw: me,
        }
        this.status = 'authenticated'
      } catch {
        this.$reset()
        localStorage.removeItem('jwt-token')
        const cookie = getAuthCookie()
        cookie.value = null
      }
    },

    logout() {
      try { agenteService.logout?.() } catch {}
      this.$reset()
      if (process.client) localStorage.removeItem('jwt-token')
      const cookie = getAuthCookie()
      cookie.value = null
    },
  },
})
